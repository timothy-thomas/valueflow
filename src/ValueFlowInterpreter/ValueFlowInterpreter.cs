using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using GME.CSharp;
using GME;
using GME.MGA;
using GME.MGA.Core;
using VF = ISIS.GME.Dsml.ValueFlow.Interfaces;
using VFClasses = ISIS.GME.Dsml.ValueFlow.Classes;
using System.Windows.Forms;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using System.Collections;

namespace ValueFlowInterpreter
{
    /// <summary>
    /// This class implements the necessary COM interfaces for a GME interpreter component.
    /// </summary>
    [Guid(ComponentConfig.guid),
    ProgId(ComponentConfig.progID),
    ClassInterface(ClassInterfaceType.AutoDual)]
    [ComVisible(true)]
    public class ValueFlowInterpreter : IMgaComponentEx, IGMEVersionInfo
    {
        /// <summary>
        /// Contains information about the GUI event that initiated the invocation.
        /// </summary>
        public enum ComponentStartMode
        {
            GME_MAIN_START = 0, 		// Not used by GME
            GME_BROWSER_START = 1,      // Right click in the GME Tree Browser window
            GME_CONTEXT_START = 2,		// Using the context menu by right clicking a model element in the GME modeling window
            GME_EMBEDDED_START = 3,		// Not used by GME
            GME_MENU_START = 16,		// Clicking on the toolbar icon, or using the main menu
            GME_BGCONTEXT_START = 18,	// Using the context menu by right clicking the background of the GME modeling window
            GME_ICON_START = 32,		// Not used by GME
            GME_SILENT_MODE = 128 		// Not used by GME, available to testers not using GME
        }

        /// <summary>
        /// This function is called for each interpreter invocation before Main.
        /// Don't perform MGA operations here unless you open a tansaction.
        /// </summary>
        /// <param name="project">The handle of the project opened in GME, for which the interpreter was called.</param>
        public void Initialize(MgaProject project)
        {
            MgaGateway = new MgaGateway(project);
        }

        abstract class ValueFlowElement
        {
            public string name;
            public System.Guid guid;
            public bool constant;
            public string value;
            public List<System.Guid> dependencies;
        }
        
        class Parameter : ValueFlowElement
        {
            public Parameter(string nm, System.Guid id, string val)
            {
                name = nm;
                guid = id;
                constant = true;
                value = val;
            }

            public Parameter(string nm, System.Guid id, System.Guid dep)
            {
                name = nm;
                guid = id;
                constant = false;
                dependencies = new List<System.Guid> { dep };
            }
        }

        class Function : ValueFlowElement
        {
            public enum FunctionType
            {
                SIMPLE = 0,
                COMPLEX = 1,
                PYTHON = 2
            }
            public FunctionType type;
            public string simpleType;
            public Dictionary<System.Guid, string> complexMapping;
            public string expression;
            public string python_filename;
            public List<System.Guid> outputs;

            public Function(string nm, System.Guid id, List<System.Guid> deps, FunctionType ty, string func)
            {
                constant = false;
                name = nm;
                guid = id;
                dependencies = deps;
                type = ty;
                if (type == FunctionType.SIMPLE)
                {
                    simpleType = func;
                }
                else
                {
                    expression = func;
                }
            }

            public static Dictionary<string, string> simpleFunctionTransform = new Dictionary<string, string>
            {
                {"Addition", "add"},
                {"Multiplication", "mult"},
                {"AritmeticMean", "mean"},
                {"GeometricMean", "gmean"},
                {"Maximum", "max"},
                {"Minimum", "min"}
            };

            public Function(string nm, System.Guid id, List<System.Guid> deps, FunctionType ty, string fn, List<System.Guid> op)
            {
                constant = false;
                name = nm;
                guid = id;
                type = ty;
                python_filename = fn;
                dependencies = deps;
                outputs = op;
            }
        }

        void BuildLists(string parents, VF.Component component, List<string> components, List<Parameter> parameters, List<Function> functions)
        {
            components.Add(parents + component.Name);
            parents = parents + component.Name + ".";
            foreach (var p in component.Children.ParameterCollection) {
                if (!p.SrcConnections.ValueFlowCollection.Any()) // No incoming ValueFlow connections
                {
                    // Value should already be assigned
                    var value = p.Attributes.Value;
                    if (value == null || value == "") // Looks like it wasn't assigned, after all
                    {
                        value = "0";
                    }
                    else if (value.Contains('/')) // To support single fraction inputs
                    {
                        value = "float(" + value.Replace("/", ")/");
                    }
                    parameters.Add(new Parameter(parents + p.Name, p.Guid, value));
                }
                else
                {
                    var dep = p.SrcConnections.ValueFlowCollection.First().SrcEnd.Guid;
                    parameters.Add(new Parameter(parents + p.Name, p.Guid, dep));
                }
            }

            foreach (var f in component.Children.SimpleFormulaCollection)
            {
                var deps = new List<System.Guid>();
                foreach (var flow in f.SrcConnections.ValueFlowCollection)
                {
                    deps.Add(flow.SrcEnd.Guid);
                }
                functions.Add(new Function(parents + f.Name, f.Guid, deps, Function.FunctionType.SIMPLE, f.Attributes.Method.ToString()));
            }

            foreach (var f in component.Children.ComplexFormulaCollection)
            {
                // Translate the MuParser expression to a python expression
                AntlrInputStream input = new AntlrInputStream(f.Attributes.Expression.ToString());
                MuParserLexer lexer = new MuParserLexer(input);
                CommonTokenStream tokens = new CommonTokenStream(lexer);
                MuParserParser parser = new MuParserParser(tokens);
                IParseTree tree = parser.expr();
                MuParserToPythonVisitor visitor = new MuParserToPythonVisitor();
                var pythonExpression = visitor.Visit(tree);

                // Get all unique ID's referenced in complexFormula
                ParseTreeWalker walker = new ParseTreeWalker();
                IDListener listener = new IDListener();
                walker.Walk(listener, tree);
                var ids = listener.GetIDs().OrderByDescending(id => id.Length);

                // Get mapping from IDs to Guids
                var guids = new Dictionary<string, System.Guid>();
                foreach (var flow in f.SrcConnections.ValueFlowCollection)
                {
                    if (flow.Attributes.Name == null || flow.Attributes.Name == "")
                    {
                        guids[flow.SrcEnd.Name] = flow.SrcEnd.Guid;
                    }
                    else
                    {
                        guids[flow.Attributes.Name] = flow.SrcEnd.Guid;
                    }
                }

                // Save ordered Guid list of dependencies
                var deps = ids.Select(id => guids[id]).ToList();
                
                // Substitute template strings into pythonExpression to create templateExpression
                var template = new StringBuilder(pythonExpression);
                var i = 0;
                foreach (var id in ids)
                    template.Replace(id, "$" + i++);
                var templateExpression = template.ToString();
                
                functions.Add(new Function(parents + f.Name, f.Guid, deps, Function.FunctionType.COMPLEX, templateExpression));
            }

            foreach (var f in component.Children.PythonCollection)
            {
                var filename = System.IO.Path.GetFileName(f.Attributes.Filename);
                System.IO.File.Copy(f.Attributes.Filename, filename, true);
                string pythonBody = System.IO.File.ReadAllText(filename);
                
                // Sort Dependencies and Outputs based on order in function.
                var deps = new List<System.Guid>();
                var outputs = new List<System.Guid>();

                List<string> pyArgs = new Regex(System.IO.Path.GetFileNameWithoutExtension(filename)+@"\((.*)\)").Match(pythonBody).Groups[1].Captures[0].ToString().Split(',').Select(p => p.Trim()).ToList();
                List<string> pyReturnVals = new Regex(@"return\s+\[(.*)\]").Match(pythonBody).Groups[1].Captures[0].ToString().Split(',').Select(p => p.Trim()).ToList();

                foreach (var arg in pyArgs)
                {
                    if (f.Children.InputCollection.Select(x => x.Name).Contains(arg))
                    {
                        var input = f.Children.InputCollection.First(x => x.Name == arg);
                        var dep = input.SrcConnections.ValueFlowCollection.First().SrcEnd.Guid;
                        deps.Add(dep);
                    }
                }
                foreach (var returnVal in pyReturnVals)
                {
                    if(f.Children.OutputCollection.Select(x => x.Name).Contains(returnVal))
                    {
                        var output = f.Children.OutputCollection.First(x => x.Name == returnVal);
                        outputs.Add(output.Guid);
                    }
                }
                functions.Add(new Function(parents + f.Name, f.Guid, deps, Function.FunctionType.PYTHON, f.Attributes.Filename, outputs));
            }

            foreach (VF.Component c in component.Children.ComponentCollection) {
                BuildLists(parents, c, components, parameters, functions);
            }

        }
        
        /// <summary>
        /// The main entry point of the interpreter. A transaction is already open,
        /// GMEConsole is available. A general try-catch block catches all the exceptions
        /// coming from this function, you don't need to add it. For more information, see InvokeEx.
        /// </summary>
        /// <param name="project">The handle of the project opened in GME, for which the interpreter was called.</param>
        /// <param name="currentobj">The model open in the active tab in GME. Its value is null if no model is open (no GME modeling windows open). </param>
        /// <param name="selectedobjs">
        /// A collection for the selected model elements. It is never null.
        /// If the interpreter is invoked by the context menu of the GME Tree Browser, then the selected items in the tree browser. Folders
        /// are never passed (they are not FCOs).
        /// If the interpreter is invoked by clicking on the toolbar icon or the context menu of the modeling window, then the selected items 
        /// in the active GME modeling window. If nothing is selected, the collection is empty (contains zero elements).
        /// </param>
        /// <param name="startMode">Contains information about the GUI event that initiated the invocation.</param>
        [ComVisible(false)]
        public void Main(MgaProject project, MgaFCO currentobj, MgaFCOs selectedobjs, ComponentStartMode startMode)
        {
            // TODO: Add your interpreter code
			
			// Get RootFolder
			IMgaFolder rootFolder = project.RootFolder;
            
            // To use the domain-specific API:
            //  Create another project with the same name as the paradigm name
            //  Copy the paradigm .mga file to the directory containing the new project
            //  In the new project, install the GME DSMLGenerator NuGet package (search for DSMLGenerator)
            //  Add a Reference in this project to the other project
            //  Add "using [ParadigmName] = ISIS.GME.Dsml.[ParadigmName].Classes.Interfaces;" to the top of this file
            // if (currentobj.Meta.Name == "KindName")
            // [ParadigmName].[KindName] dsCurrentObj = ISIS.GME.Dsml.[ParadigmName].Classes.[KindName].Cast(currentobj);			
            
            if (currentobj.Meta.Name == "Component")
            {
                VF.Component dsCurrentObj = VFClasses.Component.Cast(currentobj);

                // List of components for which we need dictionaries
                var components = new List<string>();

                // constants and function references list
                var parameters = new List<Parameter>();

                // functions
                var functions = new List<Function>();

                // Build the list of all constants and functions
                BuildLists("", dsCurrentObj, components, parameters, functions);

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"output.py"))
                {
                    file.WriteLine("import json");
                    file.WriteLine("");
                    file.WriteLine("parameters = dict()");
                    file.WriteLine("");
                    file.WriteLine("def add(x, *args):");
                    file.WriteLine("  for arg in args:");
                    file.WriteLine("    x = x + arg");
                    file.WriteLine("  return x");
                    file.WriteLine("");
                    file.WriteLine("def mult(x, *args):");
                    file.WriteLine("  for arg in args:");
                    file.WriteLine("    x = x * arg");
                    file.WriteLine("  return x");
                    file.WriteLine("");
                    file.WriteLine("def max (x, *args):");
                    file.WriteLine("  for arg in args:");
                    file.WriteLine("    if arg > x:");
                    file.WriteLine("      x = arg");
                    file.WriteLine("  return x");
                    file.WriteLine("");

                    file.WriteLine("simpleResults = list()");
                    file.WriteLine("complexResults = list()");
                    file.WriteLine("pythonResults = list()");
                    file.WriteLine("");

                    foreach (var c in components)
                    {
                        file.WriteLine("parameters[\"" + c.Replace(".", "\"][\"") + "\"] = dict()");
                    }

                    var knownElements = new List<System.Guid>();
                    var newKnownElements = new List<System.Guid>();
                    var values = new Dictionary<System.Guid, string>();
                    int simpleResultsCount = 0;
                    int complexResultsCount = 0;
                    int pythonResultsCount = 0;
                    int passIndex = 0;

                    int lastCount = -1;
                    int count = 0;
                    while (count > lastCount)
                    {
                        file.WriteLine("\n#----------------- Pass " + passIndex++ + " ------------------");
                        lastCount = count;
                        foreach (var p in parameters.Where(p => !knownElements.Contains(p.guid)))
                        {
                            if (p.constant)
                            {
                                newKnownElements.Add(p.guid);
                                values.Add(p.guid, "parameters[\"" + p.name.Replace(".", "\"][\"") + "\"]");
                                file.WriteLine("parameters[\"" + p.name.Replace(".", "\"][\"") + "\"] = " + p.value);
                                count++;
                            }
                            else if (knownElements.Contains(p.dependencies.First()))
                            {
                                newKnownElements.Add(p.guid);
                                values.Add(p.guid, values[p.dependencies.First()]);
                                file.WriteLine("parameters[\"" + p.name.Replace(".", "\"][\"") + "\"] = " + values[p.dependencies.First()]);
                                count++;
                            }
                        }
                        file.WriteLine("");

                        foreach (var f in functions.Where(f => !knownElements.Contains(f.guid)))
                        {
                            var unsatisfiedDeps = f.dependencies.Where(d => !knownElements.Contains(d));
                            if (!unsatisfiedDeps.Any())
                            {
                                if (f.type == Function.FunctionType.SIMPLE)
                                {
                                    var expressionString = Function.simpleFunctionTransform[f.simpleType] + "(" + String.Join(",",f.dependencies.Select(x => values[x]).ToList()) + ")";
                                    file.WriteLine("simpleResults.append(" + expressionString + ")");

                                    var valueString = "simpleResults[" + simpleResultsCount++ + "]";
                                    newKnownElements.Add(f.guid);
                                    values.Add(f.guid, valueString);
                                    count++;
                                }
                                else if (f.type == Function.FunctionType.COMPLEX)
                                {
                                    // Replace the META variables with their respective Python variables
                                    var templateStringIndex = 0;
                                    var output = new StringBuilder(f.expression);
                                    foreach (var d in f.dependencies)
                                        output.Replace("$" + templateStringIndex++, values[d]);
                                    var expressionString = "(" + output.ToString() + ")";
                                    file.WriteLine("complexResults.append(" + expressionString + ")");

                                    var valueString = "complexResults[" + complexResultsCount++ + "]";
                                    newKnownElements.Add(f.guid);
                                    values.Add(f.guid, valueString);
                                    count++;
                                }
                                else if (f.type == Function.FunctionType.PYTHON)
                                {
                                    var functionName = Path.GetFileNameWithoutExtension(f.python_filename);
                                    file.WriteLine("\nimport " + functionName);
                                    file.WriteLine("pythonResults.append("+ functionName + "." + functionName + "(");
                                    file.Write("    " + String.Join(",\n    ",f.dependencies.Select(x => values[x]).ToList()));
                                    file.WriteLine("))\n");
                                    int outputIndex = 0;
                                    foreach (var output in f.outputs)
                                    {
                                        var valueString = "pythonResults[" + pythonResultsCount + "][" + outputIndex++ + "]";
                                        newKnownElements.Add(output);
                                        values.Add(output, valueString);
                                        count++;
                                    }
                                    newKnownElements.Add(f.guid);
                                    pythonResultsCount++;
                                }
                            }
                        }
                        knownElements.AddRange(newKnownElements);
                        newKnownElements = new List<Guid>();
                    }
                    file.WriteLine("#------ Done! (No new values found.) -------");
                    file.WriteLine("");
                    file.WriteLine("print json.dumps(parameters, indent=2, sort_keys=True)");
                    file.WriteLine("");
                    file.WriteLine("with open('output.json', 'w') as f_out:");
                    file.WriteLine("    json.dump(parameters, f_out, indent=2, sort_keys=True)");
                }
            }

        }

        
        #region IMgaComponentEx Members

        MgaGateway MgaGateway { get; set; }
        
        public void InvokeEx(MgaProject project, MgaFCO currentobj, MgaFCOs selectedobjs, int param)
        {
            if (!enabled)
            {
                return;
            }

            try
            {
                var ProjectDirectory = Path.GetDirectoryName(currentobj.Project.ProjectConnStr.Substring("MGA=".Length));

                // set up the output directory
                MgaGateway.PerformInTransaction(delegate
                {
                    string outputDirName = project.Name;
                    if (currentobj != null)
                    {
                        outputDirName = currentobj.Name;
                    }

                    Main(project, currentobj, selectedobjs, Convert(param));                    
                });
                                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                MgaGateway = null;
                project = null;
                currentobj = null;
                selectedobjs = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private ComponentStartMode Convert(int param)
        {
            switch (param)
            {
                case (int)ComponentStartMode.GME_BGCONTEXT_START:
                    return ComponentStartMode.GME_BGCONTEXT_START;
                case (int)ComponentStartMode.GME_BROWSER_START:
                    return ComponentStartMode.GME_BROWSER_START;

                case (int)ComponentStartMode.GME_CONTEXT_START:
                    return ComponentStartMode.GME_CONTEXT_START;

                case (int)ComponentStartMode.GME_EMBEDDED_START:
                    return ComponentStartMode.GME_EMBEDDED_START;

                case (int)ComponentStartMode.GME_ICON_START:
                    return ComponentStartMode.GME_ICON_START;

                case (int)ComponentStartMode.GME_MAIN_START:
                    return ComponentStartMode.GME_MAIN_START;

                case (int)ComponentStartMode.GME_MENU_START:
                    return ComponentStartMode.GME_MENU_START;
                case (int)ComponentStartMode.GME_SILENT_MODE:
                    return ComponentStartMode.GME_SILENT_MODE;
            }

            return ComponentStartMode.GME_SILENT_MODE;
        }

        #region Component Information
        public string ComponentName
        {
            get { return GetType().Name; }
        }

        public string ComponentProgID
        {
            get
            {
                return ComponentConfig.progID;
            }
        }

        public componenttype_enum ComponentType
        {
            get { return ComponentConfig.componentType; }
        }
        public string Paradigm
        {
            get { return ComponentConfig.paradigmName; }
        }
        #endregion

        #region Enabling
        bool enabled = true;
        public void Enable(bool newval)
        {
            enabled = newval;
        }
        #endregion

        #region Interactive Mode
        protected bool interactiveMode = true;
        public bool InteractiveMode
        {
            get
            {
                return interactiveMode;
            }
            set
            {
                interactiveMode = value;
            }
        }
        #endregion

        #region Custom Parameters
        SortedDictionary<string, object> componentParameters = null;

        public object get_ComponentParameter(string Name)
        {
            if (Name == "type")
                return "csharp";

            if (Name == "path")
                return GetType().Assembly.Location;

            if (Name == "fullname")
                return GetType().FullName;

            object value;
            if (componentParameters != null && componentParameters.TryGetValue(Name, out value))
            {
                return value;
            }

            return null;
        }

        public void set_ComponentParameter(string Name, object pVal)
        {
            if (componentParameters == null)
            {
                componentParameters = new SortedDictionary<string, object>();
            }

            componentParameters[Name] = pVal;
        }
        #endregion

        #region Unused Methods
        // Old interface, it is never called for MgaComponentEx interfaces
        public void Invoke(MgaProject Project, MgaFCOs selectedobjs, int param)
        {
            throw new NotImplementedException();
        }

        // Not used by GME
        public void ObjectsInvokeEx(MgaProject Project, MgaObject currentobj, MgaObjects selectedobjs, int param)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        #region IMgaVersionInfo Members

        public GMEInterfaceVersion_enum version
        {
            get { return GMEInterfaceVersion_enum.GMEInterfaceVersion_Current; }
        }

        #endregion

        #region Registration Helpers

        [ComRegisterFunctionAttribute]
        public static void GMERegister(Type t)
        {
            Registrar.RegisterComponentsInGMERegistry();

        }

        [ComUnregisterFunctionAttribute]
        public static void GMEUnRegister(Type t)
        {
            Registrar.UnregisterComponentsInGMERegistry();
        }

        #endregion
        
        
    }
}

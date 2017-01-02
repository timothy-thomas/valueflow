using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using GME.CSharp;
using GME;
using GME.MGA;
using GME.MGA.Core;
using VF = ISIS.GME.Dsml.ValueFlow.Interfaces;
using VFClasses = ISIS.GME.Dsml.ValueFlow.Classes;
using System.Windows.Forms;

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
            public bool known;
            public string value;
            public List<System.Guid> dependencies;
        }
        
        class AssignmentLine : ValueFlowElement
        {
            public AssignmentLine(string nm, System.Guid id, string val)
            {
                name = nm;
                guid = id;
                known = true;
                value = val;
            }

            public AssignmentLine(string nm, System.Guid id, System.Guid dep)
            {
                name = nm;
                guid = id;
                known = false;
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
            public string body;

            public Function(string nm, System.Guid id, List<System.Guid> deps, FunctionType ty, string simTy)
            {
                known = false;
                name = nm;
                guid = id;
                dependencies = deps;
                type = ty;
                simpleType = simTy;
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

            public Function(string nm, System.Guid id, List<System.Guid> deps, FunctionType ty, Dictionary<System.Guid, string> cm, String bd)
            {
                known = false;
                name = nm;
                guid = id;
                dependencies = deps;
                type = ty;
                complexMapping = cm;
                body = bd;
            }

            //public Function(string nm, System.Guid id, List<System.Guid> deps, FunctionType ty, )
            //{
            //    known = false;
            //    name = nm;
            //    guid = id;
            //    dependencies = deps;
            //    type = ty;
            //}
        }

        void BuildLists(string parents, VF.Component component, List<string> components, List<AssignmentLine> assignmentLines, List<Function> functions)
        {
            components.Add(parents + component.Name);
            parents = parents + component.Name + ".";
            foreach (var p in component.Children.ParameterCollection) {
                var incoming = p.SrcConnections.ValueFlowCollection.Count();
                var outgoing = p.DstConnections.ValueFlowCollection.Count();
                var incoming2 = p.AllDstConnections.Any();
                var outgoing2 = p.AllSrcConnections.Any();
                if (!p.SrcConnections.ValueFlowCollection.Any()) // No incoming ValueFlow connections
                {
                    // Value is Constant
                    assignmentLines.Add(new AssignmentLine(parents + p.Name, p.Guid, p.Attributes.Value));
                }
                else
                {
                    var dep = p.SrcConnections.ValueFlowCollection.First().SrcEnd.Guid;
                    assignmentLines.Add(new AssignmentLine(parents + p.Name, p.Guid, dep));
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
                var body = f.Attributes.Expression.ToString();
                var deps = new List<System.Guid>();
                var table = new Dictionary<System.Guid, string>();
                foreach (var flow in f.SrcConnections.ValueFlowCollection)
                {
                    deps.Add(flow.SrcEnd.Guid);
                    table[flow.SrcEnd.Guid] = flow.Attributes.Name;
                }
                functions.Add(new Function(parents + f.Name, f.Guid, deps, Function.FunctionType.COMPLEX, table, body));
            }

            //foreach (var f in component.Children.PythonCollection)
            //{
            //    functions.Add(new Function(parents + f.Name, f));
            //}

            foreach (VF.Component c in component.Children.ComponentCollection) {
                BuildLists(parents, c, components, assignmentLines, functions);
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
                var assignmentLines = new List<AssignmentLine>();

                // functions
                var functions = new List<Function>();

                // Build the list of all constants and functions
                BuildLists("", dsCurrentObj, components, assignmentLines, functions);

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

                    foreach (var c in components)
                    {
                        file.WriteLine("parameters[\"" + c.Replace(".", "\"][\"") + "\"] = dict()");
                    }

                    var knownElements = new List<System.Guid>();
                    var values = new Dictionary<System.Guid, string>();

                    int lastCount = -1;
                    int count = 0;
                    while (count > lastCount)
                    {
                        lastCount = count;
                        foreach (var l in assignmentLines.Where(x => !knownElements.Contains(x.guid)))
                        {
                            if (l.known & !knownElements.Contains(l.guid))
                            {
                                knownElements.Add(l.guid);
                                values.Add(l.guid, "parameters[\"" + l.name.Replace(".", "\"][\"") + "\"]");
                                file.WriteLine("parameters[\"" + l.name.Replace(".", "\"][\"") + "\"] = " + l.value);
                                count++;
                            }
                            else if (!knownElements.Contains(l.guid))
                            {
                                if (knownElements.Contains(l.dependencies.First()))
                                {
                                    knownElements.Add(l.guid);
                                    values.Add(l.guid, values[l.dependencies.First()]);
                                    file.WriteLine("parameters[\"" + l.name.Replace(".", "\"][\"") + "\"] = " + values[l.dependencies.First()]);
                                    count++;
                                }
                            }
                        }

                        foreach (var f in functions.Where(x => !knownElements.Contains(x.guid)))
                        {
                            if (f.type == Function.FunctionType.SIMPLE)
                            {
                                var allDepsSatisfied = true;
                                foreach (var dep in f.dependencies)
                                {
                                    if (!knownElements.Contains(dep))
                                    {
                                        allDepsSatisfied = false;
                                        break;
                                    }
                                }
                                if (allDepsSatisfied)
                                {
                                    var valueString = Function.simpleFunctionTransform[f.simpleType] + "(" + String.Join(",",f.dependencies.Select(x => values[x]).ToList()) + ")";
                                    knownElements.Add(f.guid);
                                    values.Add(f.guid, valueString);
                                    count++;
                                }
                            }
                            else if (f.type == Function.FunctionType.COMPLEX)
                            {
                                var allDepsSatisfied = true;
                                foreach (var dep in f.dependencies)
                                {
                                    if (!knownElements.Contains(dep))
                                    {
                                        allDepsSatisfied = false;
                                        break;
                                    }
                                }
                                if (allDepsSatisfied)
                                {
                                    var valueMapping = new Dictionary<string, string>();
                                    foreach (var dep in f.dependencies)
                                    {
                                        valueMapping[f.complexMapping[dep]] = values[dep];
                                    }
                                    var output = new StringBuilder(f.body);
                                    foreach (var kvp in valueMapping)
                                        output.Replace(kvp.Key, kvp.Value);
                                    var valueString = output.ToString();
                                    // Only Allow unary operator to be assigned, i.e. not "5+7" or "5/7", but "(5+7)", "(5/7)", or "max(5,7)"
                                    if (!System.Text.RegularExpressions.Regex.IsMatch(valueString, @"^[a-zA-Z]*\(.*\)$"))
                                    {
                                        valueString = "(" + valueString + ")";
                                    }
                                    knownElements.Add(f.guid);
                                    values.Add(f.guid, valueString);
                                    count++;
                                }
                            }
                        }
                    }
                    file.WriteLine("");
                    file.WriteLine("print json.dumps(parameters, indent=2, sort_keys=True)");
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

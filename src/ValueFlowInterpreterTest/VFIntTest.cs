using GME.CSharp;
using GME.MGA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using ValueFlow = ISIS.GME.Dsml.ValueFlow.Interfaces;
using ValueFlowClasses = ISIS.GME.Dsml.ValueFlow.Classes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection;

namespace ValueFlowInterpreterTest
{
    public class Tests : IUseFixture<VFTestFixture>
    {
        [Fact]
        // OpenMETA's FormulaEvalutor uses the Source Parameter name if the ValueFlow Connection is unnamed.
        public void AliasExample()
        {
            var result = RunComponent("AliasExample");
            Assert.Equal(10, (int)result["AliasExample"]["P1"]);
            Assert.Equal(2, (int)result["AliasExample"]["P2"]);

            Assert.Equal(8, (int)result["AliasExample"]["Output"]);
        }

        [Fact]
        // OpenMETA's FormulaEvalutor uses the Source Parameter name if the ValueFlow Connection is unnamed.
        public void AliasExample2()
        {
            var result = RunComponent("AliasExample2");

            Assert.Equal(10, (int)result["AliasExample2"]["P1"]);
            Assert.Equal(2, (int)result["AliasExample2"]["P2"]);
            Assert.Equal(-12, (int)result["AliasExample2"]["P12"]);
            Assert.Equal(-21, (int)result["AliasExample2"]["P21"]);

            Assert.Equal(-33, (int)result["AliasExample2"]["Output"]);
        }

        [Fact]
        // This test makes sure using parameters multiple times doesn't break the Interpreter.
        public void AliasExample3()
        {
            var result = RunComponent("AliasExample3");

            Assert.Equal(2, (int)result["AliasExample3"]["P2"]);
            Assert.Equal(-3, (int)result["AliasExample3"]["P12"]);
            Assert.Equal(-5, (int)result["AliasExample3"]["P21"]);

            Assert.Equal(-392, (int)result["AliasExample3"]["Output"]);
        }

        [Fact]
        public void ComplexExampleTest()
        {
            var result = RunComponent("ComplexExample");

            Assert.Equal( 8, result["ComplexExample"]["Box1Height"]);
            Assert.Equal( 10, result["ComplexExample"]["Box1Length"]);
            Assert.Equal( 13, result["ComplexExample"]["Box1Width"]);
            Assert.Equal( 20, result["ComplexExample"]["Box2Height"]);
            Assert.Equal( 10, result["ComplexExample"]["Box2Length"]);
            Assert.Equal( 7, result["ComplexExample"]["Box2Width"]);
            Assert.Equal( 28, result["ComplexExample"]["ComplexContainer"]["Height"]);
            Assert.Equal( 13, result["ComplexExample"]["ComplexContainer"]["Width"]);
            Assert.Equal( 10, result["ComplexExample"]["ComplexContainer"]["Length"]);
            Assert.Equal( 3640, result["ComplexExample"]["ComplexContainer"]["Volume"]);
        }

        [Fact]
        // OpenMETA's FormulaEvalutor assumes that empty Parameter fields have value 0.
        public void EmptyInputFormula()
        {
            var result = RunComponent("EmptyInputFormula");
            Assert.Equal(5, (int)result["EmptyInputFormula"]["P_5"]);
            Assert.Equal(0, (int)result["EmptyInputFormula"]["P_Empty"]);

            Assert.Equal(5, (int)result["EmptyInputFormula"]["Output"]);
        }

        [Fact]
        // OpenMETA's FormulaEvalutor accepts single-divisor fractions as parameter values.
        public void FractionExample()
        {
            var result = RunComponent("FractionExample");
            Assert.Equal(0.25, result["FractionExample"]["P_Frac"]);
            Assert.Equal(1.25, result["FractionExample"]["P_Decimal"]);

            Assert.Equal(1.5, result["FractionExample"]["SimpleOutput"]);
            var complexOutput = (1 / 4.0) / 1.25;
            Assert.Equal(complexOutput, result["FractionExample"]["ComplexOutput"]);
        }

        [Fact]
        // This example demonstrates well the formatting of the output.py file.
        public void FullExampleTest()
        {
            var result = RunComponent("FullExample");

            Assert.Equal( 8, result["FullExample"]["Box1Height"]);
            Assert.Equal( 10, result["FullExample"]["Box1Length"]);
            Assert.Equal( 13, result["FullExample"]["Box1Width"]);
            Assert.Equal( 20, result["FullExample"]["Box2Height"]);
            Assert.Equal( 10, result["FullExample"]["Box2Length"]);
            Assert.Equal( 7, result["FullExample"]["Box2Width"]);
            Assert.Equal( 28, result["FullExample"]["ComplexContainer"]["Height"]);
            Assert.Equal( 13, result["FullExample"]["ComplexContainer"]["Width"]);
            Assert.Equal( 10, result["FullExample"]["ComplexContainer"]["Length"]);
            Assert.Equal( 3640, result["FullExample"]["ComplexContainer"]["Volume"]);
            Assert.Equal( 28, result["FullExample"]["PythonContainer"]["Height"]);
            Assert.Equal( 20, result["FullExample"]["PythonContainer"]["Width"]);
            Assert.Equal( 20, result["FullExample"]["PythonContainer"]["Length"]);
            Assert.Equal( 11200, result["FullExample"]["PythonContainer"]["Volume"]);
            Assert.Equal( 28, result["FullExample"]["SimpleContainer"]["Height"]);
            Assert.Equal( 13, result["FullExample"]["SimpleContainer"]["Width"]);
            Assert.Equal( 10, result["FullExample"]["SimpleContainer"]["Length"]);
            Assert.Equal( 3640, result["FullExample"]["SimpleContainer"]["Volume"]);
        }

        [Fact]
        public void ParamOnlyExample1Test()
        {
            var result = RunComponent("ParamOnlyExample1");

            Assert.Equal( 2, result["ParamOnlyExample1"]["Box1Height"]);
            Assert.Equal( 10, result["ParamOnlyExample1"]["Box1Length"]);
            Assert.Equal( 13, result["ParamOnlyExample1"]["Box1Width"]);
            Assert.Equal( 2, result["ParamOnlyExample1"]["SimpleContainer"]["Height"]);
            Assert.Equal( 13, result["ParamOnlyExample1"]["SimpleContainer"]["Width"]);
            Assert.Equal( 10, result["ParamOnlyExample1"]["SimpleContainer"]["Length"]);
        }

        [Fact]
        public void ParamOnlyExample2Test()
        {
            var result = RunComponent("ParamOnlyExample2");

            Assert.Equal( 2, result["ParamOnlyExample2"]["Box1Height"]);
            Assert.Equal( 10, result["ParamOnlyExample2"]["Box1Length"]);
            Assert.Equal( 13, result["ParamOnlyExample2"]["Box1Width"]);
            Assert.Equal( 2, result["ParamOnlyExample2"]["HeightSameLevel"]);
            Assert.Equal( 2, result["ParamOnlyExample2"]["SimpleContainer"]["Height"]);
            Assert.Equal( 13, result["ParamOnlyExample2"]["SimpleContainer"]["Width"]);
            Assert.Equal( 10, result["ParamOnlyExample2"]["SimpleContainer"]["Length"]);
            Assert.Equal( 25, result["ParamOnlyExample2"]["SimpleContainer"]["Volume"]);
            Assert.Equal( 25, result["ParamOnlyExample2"]["VolumeFromContainer"]);
        }

        [Fact]
        public void PythonExampleTest()
        {
            var result = RunComponent("PythonExample");

            Assert.Equal( 8, result["PythonExample"]["Box1Height"]);
            Assert.Equal( 10, result["PythonExample"]["Box1Length"]);
            Assert.Equal( 13, result["PythonExample"]["Box1Width"]);
            Assert.Equal( 20, result["PythonExample"]["Box2Height"]);
            Assert.Equal( 10, result["PythonExample"]["Box2Length"]);
            Assert.Equal( 7, result["PythonExample"]["Box2Width"]);
            Assert.Equal( 28, result["PythonExample"]["PythonContainer"]["Height"]);
            Assert.Equal( 20, result["PythonExample"]["PythonContainer"]["Width"]);
            Assert.Equal( 20, result["PythonExample"]["PythonContainer"]["Length"]);
            Assert.Equal( 11200, result["PythonExample"]["PythonContainer"]["Volume"]);
        }

        [Fact]
        public void SimpleExampleTest()
        {
            var result = RunComponent("SimpleExample");

            Assert.Equal( 2, result["SimpleExample"]["Box1Height"]);
            Assert.Equal( 10, result["SimpleExample"]["Box1Length"]);
            Assert.Equal( 13, result["SimpleExample"]["Box1Width"]);
            Assert.Equal( 20, result["SimpleExample"]["Box2Height"]);
            Assert.Equal( 10, result["SimpleExample"]["Box2Length"]);
            Assert.Equal( 7, result["SimpleExample"]["Box2Width"]);
            Assert.Equal( 22, result["SimpleExample"]["SimpleContainer"]["Height"]);
            Assert.Equal( 13, result["SimpleExample"]["SimpleContainer"]["Width"]);
            Assert.Equal( 10, result["SimpleExample"]["SimpleContainer"]["Length"]);
            Assert.Equal( 2860, result["SimpleExample"]["SimpleContainer"]["Volume"]);
        }

        private JObject RunComponent(string component_name)
        {
            if (File.Exists("output.py"))
            {
                File.Delete("output.py");
            }

            // Ensure output.py is deleted
            Assert.False(File.Exists("output.py"));

            PerformInTransaction(delegate
            {
                MgaFilter filter = proj.CreateFilter();
                filter.Kind = "Component";
                filter.Name = component_name;

                var fco_component = proj.AllFCOs(filter)
                    .Cast<MgaFCO>()
                    .First();

                Console.Out.Write(fco_component.ToString());

                ValueFlowInterpreter.ValueFlowInterpreter vfint = new ValueFlowInterpreter.ValueFlowInterpreter();
                vfint.Initialize(proj);
                vfint.Main(proj,
                           fco_component,
                           null,
                           ValueFlowInterpreter.ValueFlowInterpreter.ComponentStartMode.GME_CONTEXT_START);
            });

            // Ensure output.py was created
            Assert.True(File.Exists("output.py"));
            var str_result = RunPy();

            Console.Out.Write(str_result);

            var result = JObject.Parse(str_result) as JObject;
            return result;
        }

        private string RunPy()
        {
            var process = new System.Diagnostics.Process()
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo()
                {
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                    FileName = "python",
                    Arguments = "output.py",
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                }
            };

            process.Start();
            process.WaitForExit();

            Assert.True(0 == process.ExitCode, "Python script failed");

            return process.StandardOutput.ReadToEnd();
        }

        private void PerformInTransaction(MgaGateway.voidDelegate del)
        {
            var mgaGateway = new MgaGateway(proj);
            mgaGateway.PerformInTransaction(del, abort: false);
        }

        public void SetFixture(VFTestFixture data)
        {
            this.fixture = data;
        }

        private MgaProject proj
        {
            get
            {
                return this.fixture.proj;
            }
        }

        public Tests()
        {
        }
        VFTestFixture fixture;
    }

    public class VFTestFixture : IDisposable
    {
        public String xmePath = Path.Combine("..", "..", "..", "..",
                                             "my_value_flow_diagram.xme");

        public MgaProject proj { get; private set; }

        public VFTestFixture()
        {
            String mgaConnectionString;
            GME.MGA.MgaUtils.ImportXMEForTest(xmePath, out mgaConnectionString);
            var mgaPath = mgaConnectionString.Substring("MGA=".Length);

            Assert.True(File.Exists(Path.GetFullPath(mgaPath)),
                        String.Format("{0} not found. Model import may have failed.", mgaPath));

            // Copy the python scripts referenced in the Python Math Block
            System.IO.Directory.CreateDirectory("scripts");
            File.Copy(Path.Combine("..", "..", "..", "..", "scripts", "optimizeContainer.py"),
                      Path.Combine("scripts", "optimizeContainer.py"),
                      true); //Overwrite = true

            proj = new MgaProject();
            bool ro_mode;
            proj.Open("MGA=" + Path.GetFullPath(mgaPath), out ro_mode);
            proj.EnableAutoAddOns(true);
        }

        public void Dispose()
        {
            proj.Save();
            proj.Close();
        }
    }

    public class MainClass
    {
        [STAThread]
        public static int Main(string[] args)
        {
            int ret = Xunit.ConsoleClient.Program.Main(new string[] {
                Assembly.GetAssembly(typeof(Tests)).CodeBase.Substring("file:///".Length),
                //"/noshadow",
            });
            Console.In.ReadLine();
            return ret;
        }
    }

}

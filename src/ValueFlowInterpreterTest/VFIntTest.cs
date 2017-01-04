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
        public void ComplexExampleTest()
        {
            var result = RunComponent("ComplexExample");

            Assert.True((int)result["ComplexExample"]["Box1Height"] == 8);
            Assert.True((int)result["ComplexExample"]["Box1Length"] == 10);
            Assert.True((int)result["ComplexExample"]["Box1Width"] == 13);
            Assert.True((int)result["ComplexExample"]["Box2Height"] == 20);
            Assert.True((int)result["ComplexExample"]["Box2Length"] == 10);
            Assert.True((int)result["ComplexExample"]["Box2Width"] == 7);
            Assert.True((int)result["ComplexExample"]["ComplexContainer"]["Height"] == 28);
            Assert.True((int)result["ComplexExample"]["ComplexContainer"]["Width"] == 13);
            Assert.True((int)result["ComplexExample"]["ComplexContainer"]["Length"] == 10);
            Assert.True((int)result["ComplexExample"]["ComplexContainer"]["Volume"] == 3640);
        }

        [Fact]
        public void FullExampleTest()
        {
            var result = RunComponent("FullExample");

            Assert.True((int)result["FullExample"]["Box1Height"] == 8);
            Assert.True((int)result["FullExample"]["Box1Length"] == 10);
            Assert.True((int)result["FullExample"]["Box1Width"] == 13);
            Assert.True((int)result["FullExample"]["Box2Height"] == 20);
            Assert.True((int)result["FullExample"]["Box2Length"] == 10);
            Assert.True((int)result["FullExample"]["Box2Width"] == 7);
            Assert.True((int)result["FullExample"]["ComplexContainer"]["Height"] == 28);
            Assert.True((int)result["FullExample"]["ComplexContainer"]["Width"] == 13);
            Assert.True((int)result["FullExample"]["ComplexContainer"]["Length"] == 10);
            Assert.True((int)result["FullExample"]["ComplexContainer"]["Volume"] == 3640);
            Assert.True((int)result["FullExample"]["PythonContainer"]["Height"] == 28);
            Assert.True((int)result["FullExample"]["PythonContainer"]["Width"] == 20);
            Assert.True((int)result["FullExample"]["PythonContainer"]["Length"] == 20);
            Assert.True((int)result["FullExample"]["PythonContainer"]["Volume"] == 11200);
            Assert.True((int)result["FullExample"]["SimpleContainer"]["Height"] == 28);
            Assert.True((int)result["FullExample"]["SimpleContainer"]["Width"] == 13);
            Assert.True((int)result["FullExample"]["SimpleContainer"]["Length"] == 10);
            Assert.True((int)result["FullExample"]["SimpleContainer"]["Volume"] == 3640);
        }

        [Fact]
        public void ParamOnlyExample1Test()
        {
            var result = RunComponent("ParamOnlyExample1");

            Assert.True((int)result["ParamOnlyExample1"]["Box1Height"] == 2);
            Assert.True((int)result["ParamOnlyExample1"]["Box1Length"] == 10);
            Assert.True((int)result["ParamOnlyExample1"]["Box1Width"] == 13);
            Assert.True((int)result["ParamOnlyExample1"]["SimpleContainer"]["Height"] == 2);
            Assert.True((int)result["ParamOnlyExample1"]["SimpleContainer"]["Width"] == 13);
            Assert.True((int)result["ParamOnlyExample1"]["SimpleContainer"]["Length"] == 10);
        }

        [Fact]
        public void ParamOnlyExample2Test()
        {
            var result = RunComponent("ParamOnlyExample2");

            Assert.True((int)result["ParamOnlyExample2"]["Box1Height"] == 2);
            Assert.True((int)result["ParamOnlyExample2"]["Box1Length"] == 10);
            Assert.True((int)result["ParamOnlyExample2"]["Box1Width"] == 13);
            Assert.True((int)result["ParamOnlyExample2"]["HeightSameLevel"] == 2);
            Assert.True((int)result["ParamOnlyExample2"]["SimpleContainer"]["Height"] == 2);
            Assert.True((int)result["ParamOnlyExample2"]["SimpleContainer"]["Width"] == 13);
            Assert.True((int)result["ParamOnlyExample2"]["SimpleContainer"]["Length"] == 10);
            Assert.True((int)result["ParamOnlyExample2"]["SimpleContainer"]["Volume"] == 25);
            Assert.True((int)result["ParamOnlyExample2"]["VolumeFromContainer"] == 25);
        }

        [Fact]
        public void PythonExampleTest()
        {
            var result = RunComponent("PythonExample");

            Assert.True((int)result["PythonExample"]["Box1Height"] == 8);
            Assert.True((int)result["PythonExample"]["Box1Length"] == 10);
            Assert.True((int)result["PythonExample"]["Box1Width"] == 13);
            Assert.True((int)result["PythonExample"]["Box2Height"] == 20);
            Assert.True((int)result["PythonExample"]["Box2Length"] == 10);
            Assert.True((int)result["PythonExample"]["Box2Width"] == 7);
            Assert.True((int)result["PythonExample"]["PythonContainer"]["Height"] == 28);
            Assert.True((int)result["PythonExample"]["PythonContainer"]["Width"] == 20);
            Assert.True((int)result["PythonExample"]["PythonContainer"]["Length"] == 20);
            Assert.True((int)result["PythonExample"]["PythonContainer"]["Volume"] == 11200);
        }

        [Fact]
        public void SimpleExampleTest()
        {
            var result = RunComponent("SimpleExample");

            Assert.True((int)result["SimpleExample"]["Box1Height"] == 2);
            Assert.True((int)result["SimpleExample"]["Box1Length"] == 10);
            Assert.True((int)result["SimpleExample"]["Box1Width"] == 13);
            Assert.True((int)result["SimpleExample"]["Box2Height"] == 20);
            Assert.True((int)result["SimpleExample"]["Box2Length"] == 10);
            Assert.True((int)result["SimpleExample"]["Box2Width"] == 7);
            Assert.True((int)result["SimpleExample"]["SimpleContainer"]["Height"] == 22);
            Assert.True((int)result["SimpleExample"]["SimpleContainer"]["Width"] == 13);
            Assert.True((int)result["SimpleExample"]["SimpleContainer"]["Length"] == 10);
            Assert.True((int)result["SimpleExample"]["SimpleContainer"]["Volume"] == 2860);
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

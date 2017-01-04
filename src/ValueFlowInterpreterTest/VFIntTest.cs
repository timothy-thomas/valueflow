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

namespace ValueFlowInterpreterTest
{
    public class Tests : IClassFixture<VFTestFixture>
    {
        [Fact]
        public void TestTest()
        {
            String component_name = "ComplexExample";

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

                var rtn = proj.AllFCOs(filter)
                    .Cast<MgaFCO>()
                    .Select(x => ValueFlowClasses.Component.Cast(x))
                    .Cast<ValueFlow.Component>();

                Console.Out.Write(rtn.ToString());

                ValueFlowInterpreter.ValueFlowInterpreter vfint = new ValueFlowInterpreter.ValueFlowInterpreter();
                vfint.Initialize(proj);
                vfint.Main(proj,
                           rtn.First().Impl as MgaFCO,
                           null,
                           ValueFlowInterpreter.ValueFlowInterpreter.ComponentStartMode.GME_CONTEXT_START);
            });

            // Ensure output.py was created
            Assert.True(File.Exists("output.py"));
            var str_result = RunPy();
            Console.Out.Write(str_result);

            var result = JsonConvert.DeserializeObject(str_result);
            Console.Out.Write(result.ToString());

            // Make some assertions on the calculated result
        }

        private string RunPy()
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo()
            {
                WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden,
                FileName = "python",
                Arguments = "output.py",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            System.Diagnostics.Process process = new System.Diagnostics.Process()
            {
                StartInfo = startInfo
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

        private MgaProject proj
        {
            get
            {
                return this.fixture.proj;
            }
        }

        public Tests(VFTestFixture fixture)
        {
            this.fixture = fixture;
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
}

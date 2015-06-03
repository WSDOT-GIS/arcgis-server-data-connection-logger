// Copy this file and rename it UnitTest1.cs.

using ArcGisConnection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var root = new DirectoryInfo(@"C:\arcgisserver\directories\arcgissystem\arcgisinput"); // Replace with your directory.
            var connections = DatasetInfoCollector.GetDatasetInfos(root).SelectMany(e => e);

            Assert.IsNotNull(connections);
            Assert.IsTrue(connections.Count() > 1);

            var connGroups = connections.Where(p => p.ConnectionString != null).GroupBy(k => k.ConnectionString);

            foreach (var g in connGroups)
            {
                TestContext.WriteLine("{0}", g.Key);
                foreach (var c in g)
                {
                    TestContext.WriteLine("\t{0}", c.DataSet);
                }
            }
        }
    }
}

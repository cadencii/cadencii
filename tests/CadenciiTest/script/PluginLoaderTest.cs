using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using cadencii;
using Microsoft.CSharp;
using System.IO;

namespace cadencii
{
    [TestFixture]
    class PluginLoaderTest
    {
        [Test]
        public void loadTest()
        {
            PluginLoader.cleanupUnusedAssemblyCache();
            var files =
                from file in PortUtil.listFiles("./fixture/script", "")
                where
                    file.EndsWith(".cs") | file.EndsWith(".txt")
                select file;
            foreach (var file in files) {
                var loader = new PluginLoader();
                ScriptInvoker invoker = null;
                Assert.DoesNotThrow(() => { invoker = loader.loadScript(file); });
                Assert.IsNotNull(invoker);
                Console.Error.WriteLine(file + "\n" + invoker.ErrorMessage);
                Assert.IsNotNull(invoker.scriptDelegate);
            }
        }
    }
}

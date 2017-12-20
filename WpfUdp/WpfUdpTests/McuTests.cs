using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfUdp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfUdp.Tests
{
    [TestClass()]
    public class McuTests
    {
        [TestMethod()]
        public void UnitTestTest()
        {
            Mcu myMcu = Mcu.Instance;
            Assert.AreEqual(myMcu.UnitTest(), "UnitTest");
        }
    }
}
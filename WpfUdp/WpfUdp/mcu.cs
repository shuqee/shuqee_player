using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Threading;
using System.Diagnostics;

namespace WpfUdp
{
    public sealed class Mcu
    {
        private static readonly Mcu _Instance = new Mcu();

        static Mcu()
        {
            //构造函数
        }

        private Mcu()
        {
            Thread ThreadDebugInfo = new Thread(new ThreadStart(DebugInfoProc));
            ThreadDebugInfo.IsBackground = true; //设置为后台线程
            ThreadDebugInfo.Start();
            //构造函数
        }

        public static Mcu Instance
        {
            get
            {
                return _Instance;
            }
        }

        private void DebugInfoProc()
        {
            while (true)
            {
                Debug.WriteLine("worker thread: working...");
                Thread.Sleep(1000);
            }
        }

        public string UnitTest()
        {
            return "UnitTest";
        }
    }
}

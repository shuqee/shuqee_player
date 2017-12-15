using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfUdp
{
    public class Module
    {
        public int returnData;                       //定义全局变量returnData
                                                     //public MainWindow mainwindow = new MainWindow();
        public static bool changeWindow;             //定义静态变量
        public static byte[] chipID = new byte[12];  //用于存储shuqee.bin文件的数据
        public static byte YY;               
        public static byte MM;
        public static byte DD;
        public static byte deadlineYY;             //注册后的期限年
        public static byte deadlineMM;             //注册后的期限月
        public static byte deadlineDD;             //注册后的期限日
        public static byte deadlineOrPermanent;    //注册码是否为永久码 

        /// <summary>
        /// 发送动作数据与特效数据函数
        /// </summary>
        /// <param name="data1">1号缸数据</param>
        /// <param name="data2">2号缸数据</param>
        /// <param name="data3">3号缸数据</param>
        /// <param name="data4">环境特效数据</param>
        /// <param name="data5">座椅特效数据</param>
        public void sendData(byte data1, byte data2, byte data3, byte data4, byte data5)
        {

            byte[] data_buf = new byte[16];
            int data_len = 0;
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;

            IPAddress.TryParse("192.168.1.109", out remoteIP);

            UInt16 port = 1032;

            IPEndPoint iep = new IPEndPoint(remoteIP, port);
            try
            {

                data_buf[0] = 0xff;
                data_buf[1] = 0x4a;
                data_buf[2] = data1;
                data_buf[3] = data2;
                data_buf[4] = data3;
                data_buf[5] = data4;
                data_buf[6] = data5;
                data_buf[7] = 0x01;
                data_buf[8] = 0xee;
                data_len = 9;
                myUdpClient.Send(data_buf, data_len, iep);
                myUdpClient.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "发送失败");
            }
        }
        public void GetChipId()
        {
            System.DateTime currentTime = new System.DateTime();
            currentTime = System.DateTime.Now;    //获取当前时间年月日时分秒
            string strYear = currentTime.ToString("yy");             //获取当前年的后两位
            YY = Convert.ToByte(strYear);    //将字符串strYear转换成byte型             
            MM = (byte)currentTime.Month;    //将int型当前月转换成byte型
            DD = (byte)currentTime.Day;     //将int型当前日转换成byte型

            byte[] data_buf = new byte[16];
            int data_len = 0;
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;

            IPAddress.TryParse("192.168.1.109", out remoteIP);

            UInt16 port = 1032;

            IPEndPoint iep = new IPEndPoint(remoteIP, port);
            deadlineYY += YY;
            deadlineMM += MM;
            deadlineDD += DD;

            if (deadlineDD > 30)
            {
                deadlineMM += 1;
                deadlineDD -= 30;
            }
            if (deadlineMM > 12)
            {
                deadlineMM -= 12;
                deadlineYY += 1;
            }

            try
            {

                data_buf[0] = 0xFF;
                data_buf[1] = 0x58;
                data_buf[2] = 0x12;
                data_buf[3] = 0x34;
                data_buf[4] = deadlineYY;
                data_buf[5] = deadlineMM;
                data_buf[6] = deadlineDD;
                data_buf[7] = 0x00;
                data_buf[8] = 0x00;
                data_buf[9] = deadlineOrPermanent;
                data_buf[10] = YY;
                data_buf[11] = MM;
                data_buf[12] = DD;
                data_buf[13] = 0xEE;

                data_len = 14;
                myUdpClient.Send(data_buf, data_len, iep);
                myUdpClient.Close();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "发送失败");
            }
        }
  
        /// <summary>
        /// 发送校验代码函数，用于发送指令给串口板，校验信息
        /// </summary>
        public void sendCheckData()
        {
            byte[] data_buf = new byte[16];
            int data_len = 0;
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;
            IPAddress.TryParse("192.168.1.109", out remoteIP);
            UInt16 port = 1032;
            IPEndPoint iep = new IPEndPoint(remoteIP, port);
            try
            {
                //读取校验文件“shuqee.bin”
                byte[] checkOutFile = File.ReadAllBytes(@"C: \Users\shuqee\Desktop\shuqee.bin");
                if (checkOutFile[7] != 0x88)
                {
                    checkOutFile[0] ^= 0x33;
                    checkOutFile[1] ^= 0x44;
                    checkOutFile[2] ^= 0x55;
                    checkOutFile[3] ^= 0x66;
                    checkOutFile[4] ^= 0x77;
                    checkOutFile[5] ^= 0x88;
                    checkOutFile[6] ^= 0x99;
                    checkOutFile[7] = 0x88;
                    File.WriteAllBytes(@"C: \Users\shuqee\Desktop\shuqee.bin", checkOutFile);
                }
                checkOutFile[0] ^= 0x33;
                checkOutFile[1] ^= 0x44;
                checkOutFile[2] ^= 0x55;
                checkOutFile[3] ^= 0x66;
                checkOutFile[4] ^= 0x77;
                checkOutFile[5] ^= 0x88;
                checkOutFile[6] ^= 0x99;


                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;    //获取当前时间年月日时分秒
                //int years = currentTime.Year;         //获取当前年
                //int months = currentTime.Month;       //获取当前月
                //int days = currentTime.Day;           //获取当前日

                string strYear = currentTime.ToString("yy");             //获取当前年的后两位

                YY = Convert.ToByte(strYear);           //将字符串strYear转换成byte型             
                MM = (byte)currentTime.Month;           //将int型当前月转换成byte型
                DD = (byte)currentTime.Day;             //将int型当前日转换成byte型
                byte HH = (byte)currentTime.Hour;       //将int型当前时转换成byte型 
                byte mm = (byte)currentTime.Minute;     //将int型当前分转换成byte型
               
                data_buf[0] = 0xff;          
                data_buf[1] = 0x47;
                data_buf[2] = YY;
                data_buf[3] = MM;
                data_buf[4] = DD;
                data_buf[5] = HH;
                data_buf[6] = mm;
                data_buf[7] = (byte)(checkOutFile[0]^0x34);
                data_buf[8] = (byte)(checkOutFile[1]^0x75);
                data_buf[9] = (byte)(checkOutFile[2]^0x6a);
                data_buf[10] = (byte)(checkOutFile[3]^0x7b);
                data_buf[11] = (byte)(checkOutFile[4]^0x4a);
                data_buf[12] = (byte)(checkOutFile[5]^0x7c);
                data_buf[13] = (byte)(checkOutFile[6]^0x8f);           
                data_buf[14] = 0xee;

                data_len = 15;
                myUdpClient.Send(data_buf, data_len, iep);
                myUdpClient.Close();
                //textBoxSend.Focus();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "发送失败");
            }
        }


    }
}

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
        public int returnData;             //定义全局变量returnData
                                           //public MainWindow mainwindow = new MainWindow();
        public static bool changeWindow;   //定义静态变量
        public static byte[] chipID = new byte[7];
        public static byte YY;
        public static byte MM;
        public static byte DD;
        public static byte YYc;

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
            try
            {

                data_buf[0] = 0xFF;
                data_buf[1] = 0x58;
                data_buf[2] = 0x12;
                data_buf[3] = 0x34;
                data_buf[4] = data3;
                data_buf[5] = data4;
                data_buf[6] = data5;
                data_buf[7] = 0x01;
                data_buf[8] = 0xee;
                data_buf[9] = 0x01;
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

        //        Public Sub serialintt(ms As MSComm)
        //Dim mm1 As Byte
        //Dim mm2 As Byte
        //Dim mm3 As Byte
        //Dim mm4 As Byte
        //Dim mm5 As Byte
        //Dim mm6 As Byte
        //Dim mm7 As Byte
        //Dim mm8 As Byte
        //Dim DD1, DD2, yy2, dd3 As Byte

        //Dim soutt(13) As Byte

        //    ms.SThreshold = 14
        //    yy2 = YYc + YY
        //    mm2 = MMc + MM
        //    dd3 = DDc + DD

        //    If dd3 > 30 Then
        //    mm2 = mm2 + 1
        //    dd3 = dd3 - 30
        //    End If


        //    If mm2 > 12 Then
        //    mm2 = mm2 - 12
        //    yy2 = yy2 + 1
        //    End If


        //    soutt(0) = &HFF
        //    soutt(1) = &H58
        //    soutt(2) = &H12
        //    soutt(3) = &H34
        //    soutt(4) = yy2
        //    soutt(5) = mm2
        //    soutt(6) = dd3
        //    soutt(7) = DD1
        //    soutt(8) = DD2
        //    soutt(9) = dT
        //    soutt(10) = YY
        //    soutt(11) = MM
        //    soutt(12) = DD
        //    soutt(13) = &HEE
        //   ms.Output = soutt

        //End Sub


        //Public Sub serialint(ms As MSComm)
        //Dim mm1 As Byte
        //Dim mm2 As Byte
        //Dim mm3 As Byte
        //Dim mm4 As Byte
        //Dim mm5 As Byte
        //Dim mm6 As Byte
        //Dim mm7 As Byte
        //Dim mm8 As Byte

        //Dim sout1(14) As Byte

        //Open "C:\WINDOWS\system32\shuqee.bin" For Binary As #1
        //        Get #1, , mm1
        //        Get #1, , mm2
        //        Get #1, , mm3
        //        Get #1, , mm4
        //        Get #1, , mm5
        //        Get #1, , mm6
        //        Get #1, , mm7
        //        Get #1, , mm8
        //Close #1
        //    If mm8<> &H88 Then
        //        mm1 = mm1 Xor &H33
        //        mm2 = mm2 Xor &H44
        //        mm3 = mm3 Xor &H55
        //        mm4 = mm4 Xor &H66
        //        mm5 = mm5 Xor &H77
        //        mm6 = mm6 Xor &H88
        //        mm7 = mm7 Xor &H99
        //        mm8 = &H88
        //        Open "C:\WINDOWS\system32\shuqee.bin" For Binary As #1
        //        Put #1, , mm1
        //        Put #1, , mm2
        //        Put #1, , mm3
        //        Put #1, , mm4
        //        Put #1, , mm5
        //        Put #1, , mm6
        //        Put #1, , mm7
        //        Put #1, , mm8           '重新写数据给shuqee
        //        Close #1
        //End If
        //        mm1 = mm1 Xor &H33
        //        mm2 = mm2 Xor &H44
        //        mm3 = mm3 Xor &H55
        //        mm4 = mm4 Xor &H66
        //        mm5 = mm5 Xor &H77
        //        mm6 = mm6 Xor &H88
        //        mm7 = mm7 Xor &H99
        //     ms.SThreshold = 15
        //    sout1(0) = &HFF
        //    sout1(1) = &H47
        //    sout1(2) = YY
        //    sout1(3) = MM
        //    sout1(4) = DD
        //    sout1(5) = h_h
        //    sout1(6) = m_m
        //    sout1(7) = mm1 Xor &H34
        //    sout1(8) = mm2 Xor &H75
        //    sout1(9) = mm3 Xor &H6A
        //    sout1(10) = mm4 Xor &H7B
        //    sout1(11) = mm5 Xor &H4A
        //    sout1(12) = mm6 Xor &H7C
        //    sout1(13) = mm7 Xor &H8F
        //    sout1(14) = &HEE
        //   ms.Output = sout1

        //End Sub

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
                System.DateTime currentTime = new System.DateTime();
                currentTime = System.DateTime.Now;    //获取当前时间年月日时分秒
                //int years = currentTime.Year;         //获取当前年
                //int months = currentTime.Month;       //获取当前月
                //int days = currentTime.Day;           //获取当前日

                string strYear = currentTime.ToString("yy");             //获取当前年的后两位

                 YY = Convert.ToByte(strYear);    //将字符串strYear转换成byte型             
                 MM = (byte)currentTime.Month;    //将int型当前月转换成byte型
                 DD = (byte)currentTime.Day;     //将int型当前日转换成byte型


                //将int型转换成byte数组
                //byte[] YY = System.BitConverter.GetBytes(currentTime.Year );
                //byte[] MM = System.BitConverter.GetBytes(currentTime.Month );
                //byte[] DD = System.BitConverter.GetBytes(currentTime.Day);

                data_buf[0] = 0xff;
                data_buf[1] = 0x47;
                data_buf[2] = YY;
                data_buf[3] = MM;
                data_buf[4] = DD;
                data_buf[5] = checkOutFile[0];
                data_buf[6] = checkOutFile[1];
                data_buf[7] = checkOutFile[2];
                data_buf[8] = checkOutFile[3];
                data_buf[9] = checkOutFile[4];
                data_buf[10] = checkOutFile[5];
                data_buf[11] = checkOutFile[6];
                data_buf[12] = 0xee;
                data_buf[13] = 0x34;
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

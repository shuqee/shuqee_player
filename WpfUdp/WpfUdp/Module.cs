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
       public int returnData;       //定义全局变量returnData



       /// <summary>
       /// 发送动作数据与特效数据函数
       /// </summary>
       /// <param name="data1">1号缸数据</param>
       /// <param name="data2">2号缸数据</param>
       /// <param name="data3">3号缸数据</param>
       /// <param name="data4">环境特效数据</param>
       /// <param name="data5">座椅特效数据</param>
       public  void sendData(byte data1, byte data2, byte data3, byte data4, byte data5)
        {

            byte[] data_buf = new byte[16];
            int data_len = 0;
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;

            IPAddress.TryParse("192.168.1.109", out remoteIP);
            
            UInt16 port=1032;
    
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

        /// <summary>
        /// 发送校验代码函数，用于发送指令给串口板，校验信息
        /// </summary>
        public void sendCheckData()
        {
            byte[] data_buf = new byte[16];
            int data_len = 0;
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;
            IPAddress.TryParse ("192.168.1.109",out remoteIP );
            UInt16 port=1032;
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
                
                byte YY = Convert.ToByte(strYear);    //将字符串strYear转换成byte型             
                byte MM = (byte)currentTime.Month;    //将int型当前月转换成byte型
                byte DD = (byte)currentTime.Day;     //将int型当前日转换成byte型


                ///将int型转换成byte数组
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

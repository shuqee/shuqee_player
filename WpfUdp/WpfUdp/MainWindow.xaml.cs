using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;

namespace WpfUdp
{
    public partial class MainWindow : Window
    {
        public IPAddress localIPAddr;
        public UInt16 localPortNum;
        public UdpClient udpClient;
        public bool receiveThreadAlive;      //控制接收线程的退出
                                             //定义定时器DispatcherTimer
        System.Windows.Threading.DispatcherTimer readDataTimer = new System.Windows.Threading.DispatcherTimer();
        Module myClass = new Module();       //新建一个Module对象，用于在当前类调用Module类中的方法       

        public static byte[] actionFile;                //声明一个名为actionFile的数组，用于存储动作文件数据
        public static byte[] effectFile;                //声明一个名为effectFile的数组，用于存储特效文件数据  

        public MainWindow()
        {
            InitializeComponent();
            receiveThreadAlive = false;
            readFile();                 //执行读取动作文件函数
            Link();                     //执行连接函数，开启线程

            Module.sendCheckData();
            Module.SerialInit();

            //DispatcherTimer定时器初始化    
            readDataTimer.Tick += new EventHandler(timeCycle);
            readDataTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);

        }


        /// <summary>
        /// 将接收的数据显示在接收区
        /// </summary>
        /// <param name="str"></param>
        public void SetTextBox(string str)
        {
            //textBoxReceive.Text += str + "\r\n";
            ////textBoxReceive.Text += str.ToString("x2");
            ////myClass.sendCheckData();

            //textBoxReceive.ScrollToEnd();
            try
            {
                byte[] timeCode = new byte[12];
                timeCode = System.Text.Encoding.Default.GetBytes(str);
                //textBox.Text = timeCode.Length.ToString();
              

                if (timeCode[0]==0xFF && timeCode[1]==0xA1 && timeCode[2]==0x88)
                {
                    //成功打开标志
                }

                if (timeCode[0] == 0xFF && timeCode[1] == 0xA1 && (timeCode[2] != 0x88 && timeCode[2]!=0x78))
                {
                    byte a;
                    a = timeCode[2];
                }

                if (timeCode[0] == 0xFF && timeCode[1] == 0xA1 && timeCode[2] == 0x78)
                {
                    //成功打开标志
                    string reyear = "20" + timeCode[3];
                    string remonth = timeCode[4].ToString();
                    string reday = timeCode[5].ToString();
                    string redate = reyear + "-" + remonth + "-" + reday;
                    DateTime dateNow = Convert.ToDateTime ( DateTime.Now.ToShortDateString());
                    DateTime getDate = Convert.ToDateTime(redate);
                    TimeSpan ts = getDate - dateNow;
                    int getday = ts.Days;

                    switch (getday)
                    {
                        case 9:
                            MessageBox.Show("提示：使用期限还有10天");
                            break;
                        case 5:
                            MessageBox.Show("提示：使用期限还有6天");
                            break;
                        case 2:
                            MessageBox.Show("提示：使用期限还有3天");
                            break;
                        case 1:
                            MessageBox.Show("提示：使用期限还有2天");
                            break;
                        case 0:
                            MessageBox.Show("提示：使用期限还有1天");
                            break;
                        default:
                            Module.sendCheckData();
                            break;
                    }
                       
                }
               
                if (timeCode.Length == 12)
                {
                    int[] trueTimeCode = new int[14];
                    int Pswd = 0;

                    Pswd = (timeCode[11] - 65) * 1000;
                    Pswd = Pswd + (timeCode[6] - 65) * 100;
                    Pswd = Pswd + (timeCode[0] - 65) * 10;
                    Pswd = Pswd + (timeCode[4] - 65);

                    Pswd = Pswd * 43 + 10345;
                    Pswd = Pswd % 100000;

                    trueTimeCode[1] = Pswd / 10000;
                    trueTimeCode[2] = (Pswd / 1000) % 10;
                    trueTimeCode[3] = (Pswd / 100) % 10;
                    trueTimeCode[4] = (Pswd % 100) / 10;
                    trueTimeCode[5] = Pswd % 10;

                    ///将接收的字符串转换成最终的时间
                    trueTimeCode[6] = trueTimeCode[3] ^ (timeCode[10] - 65);     //小时十位
                    trueTimeCode[7] = trueTimeCode[4] ^ (timeCode[1] - 65);      //小时个位

                    trueTimeCode[8] = trueTimeCode[1] ^ (timeCode[3] - 65);      //分十位
                    trueTimeCode[9] = trueTimeCode[4] ^ (timeCode[8] - 65);      //分个位

                    trueTimeCode[10] = trueTimeCode[1] ^ (timeCode[5] - 65);    //秒十位
                    trueTimeCode[11] = trueTimeCode[3] ^ (timeCode[2] - 65);    //秒个位

                    trueTimeCode[12] = trueTimeCode[2] ^ (timeCode[9] - 65);    //帧十位
                    trueTimeCode[13] = trueTimeCode[5] ^ (timeCode[7] - 65);    //帧个位 

                    //textBox.Text = trueTimeCode[6].ToString() + trueTimeCode[7].ToString() + ":" + trueTimeCode[8].ToString() + trueTimeCode[9].ToString() + ":" + trueTimeCode[10].ToString() + trueTimeCode[11].ToString() + ":" + trueTimeCode[12].ToString() + trueTimeCode[13].ToString();

                    double hours = (trueTimeCode[6] * 10 + trueTimeCode[7]) * 60 * 60;
                    double minutes = (trueTimeCode[8] * 10 + trueTimeCode[9]) * 60;
                    double seconds = trueTimeCode[10] * 10 + trueTimeCode[11];
                    double frame = (trueTimeCode[12] * 10 + trueTimeCode[13]) / 24.000;
                    double doubleTimeCode = hours + minutes + seconds + frame;
                    //textBox2.Text = frame.ToString();
                    myClass.FlimValue(doubleTimeCode);
                }

            }
            catch
            {
                //    MessageBox.Show("数据不正确");
            }
        }

        delegate void TextBoxCallback(string str);

        /// <summary>
        /// 接收数据
        /// </summary>
        private void ReceiveThreadProc()
        {
            try
            {
                IPEndPoint ipEP = new IPEndPoint(localIPAddr, localPortNum);
                //在本机指定的端口接收
                udpClient = new UdpClient(ipEP);
            }
            catch (SocketException s)//IP地址不是本机IP时，会发生异常
            {
                MessageBox.Show("请检查IP地址和本机IP是否一致!", "提醒");
                return;
            }
            IPEndPoint remote = null;
            //接收从远程主机发送过来的信息；
            receiveThreadAlive = true;
            while (true)
            {
                try
                {
                    //关闭udpClient时此句会产生异常
                    byte[] bytes = udpClient.Receive(ref remote);
                    //string str = "";
                    //string[] strArray = new string[bytes.Length];     //定义一个字符串数组，用于存储接收到的byte数组
                    //try
                    //{
                    //    //以十进制形式显示数据

                    //    for (int i = 0; i < bytes.Length; i++)
                    //    {
                    //        strArray[i] = bytes[i].ToString();
                    //        str += strArray[i];
                    //    }

                    //    //以16进制形式显示数据

                    //    //for (int i = 0; i < bytes.Length; i++)
                    //    //{
                    //    //    strArray[i] = bytes[i].ToString("x");
                    //    //    str += strArray[i];
                    //    //    
                    //    //}

                    //}
                    //catch
                    //{

                    //}

                    //将bytes数组转换成字符串
                    string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    // str = remote.ToString() + ": " + str + "\r\n";                 //将ip地址与端口号和接收到数据赋值给str
                    TextBoxCallback tx = SetTextBox;
                    this.Dispatcher.Invoke(tx, str);

                }
                catch //捕捉关闭时产生的异常，结束接收线程
                {
                    //退出循环，结束线程
                    receiveThreadAlive = false;
                    break;
                }
            }
        }

        



        /// <summary>
        /// 根据定时器来执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeCycle(object sender, EventArgs e)
        {
            //int i = 0;
            //if (i < actionFile.Length / 3)
            //{
            //    sendData(actionFile[3 * i], actionFile[3 * i + 1], actionFile[3 * i + 2], effectFile[2 * i], effectFile[2 * i + 1]);
            //    i++;
            //}
        }



        /// <summary>
        /// 点击连接按钮建立连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            //    IPAddress localIP;
            //    if (IPAddress.TryParse(cmbIPAddress.Text, out localIP) == false)
            //    {
            //        MessageBox.Show("本地IP格式不正确");
            //        return;
            //    }
            //    UInt16 port;
            //    if (UInt16.TryParse(textBoxLocalPortNum.Text, out port) == false)
            //    {
            //        MessageBox.Show("不是一个正确的端口号");
            //        return;
            //    }
            //    if (receiveThreadAlive == true)
            //    {
            //        MessageBox.Show("连接已建立，请勿重复连接");
            //        return;
            //    }
            //    localIPAddr = localIP;
            //    localPortNum = port;
            //    Thread thread1 = new Thread(new ThreadStart(ReceiveThreadProc));
            //    thread1.IsBackground = true; //设置为后台线程
            //    thread1.Start();
            //    Thread.Sleep(5);
            //    if (receiveThreadAlive == true)
            //    {
            //        SetTextBox("已连接\r\n");
            //    }
            //    else
            //    {
            //        SetTextBox("连接失败\r\n");
            //    }

        }


        /// <summary>
        /// 建立连接，用于开启线程
        /// </summary>
        private void Link()
        { 
            IPAddress localIP;
            IPAddress.TryParse("192.168.1.109", out localIP);
            UInt16 port = 1032;             
            localIPAddr = localIP;
            localPortNum = port;
            //if (receiveThreadAlive == false)
            //{
            //    receiveThreadAlive = true;
            //}
            Thread thread1 = new Thread(new ThreadStart(ReceiveThreadProc));
            thread1.IsBackground = true; //设置为后台线程
            thread1.Start();
            Thread.Sleep(1);

        }



        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded_1(object sender, RoutedEventArgs e) //得到本机的IP地址
        {
            string hostName = Dns.GetHostName();//本机名     
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6     
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString()) //如果是IPv4地址
                {
                    //cmbIPAddress.Items.Add(ip.ToString());                    
                    localIPAddr = ip;
                }
            }

            //cmbIPAddress.SelectedIndex = 0;

            //cmbIPAddress.Items.Add("127.0.0.1");
        }





        /// <summary>
        /// 读取动作文件和特效文件
        /// </summary>
        private void readFile()
        {
            try
            {
                //actionFile = File.ReadAllBytes(@"C: \Users\shuqee\Desktop\A-D");
                //string filePath = Directory.GetCurrentDirectory() + @"\A-D";
                actionFile = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\A-D");
                //MessageBox.Show(filePath);
                try
                {
                    // effectFile = File.ReadAllBytes(@"C: \Users\shuqee\Desktop\A-T");
                    effectFile = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\A-T");
                }
                catch
                {

                    MessageBox.Show("特效文件不存在，请把A-T文件复制到当前目录");
                }
            }
            catch
            {
                MessageBox.Show("动作文件不存在，请把A-D文件复制到当前目录");
            }

        }

        /// <summary>
        /// check选框打勾定时器启动，自动发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void checkBox_Click(object sender, RoutedEventArgs e)
        //{
        //    if (checkBox.IsChecked == true)
        //    {
        //        readDataTimer.Start();
        //    }
        //    else
        //    {
        //        readDataTimer.Stop();
        //    }
        //}


        /// <summary>
        /// 点击弹出注册框
        /// </summary>
        private void btnRenewal_Click(object sender, RoutedEventArgs e)
        {

            //this.Close();                                //关闭当前窗口
            //this.Hide();                                 //隐藏当前窗口
            if (receiveThreadAlive == true)
            {
                receiveThreadAlive = false;
                if (udpClient != null)
                {
                    udpClient.Close();                     //断开udp连接  
                }
                //SetTextBox();
            }
            this.Hide();

            RegisterWindow win = new RegisterWindow();
            //win.Show();                                  //显示RegisterWindow窗口
            win.ShowDialog();

        }

      

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    if (button.Content.ToString() == "上升")
        //    {
        //        myClass.SendBytesData(myClass.com1, 255, 255, 255, 0, 0);
        //        button.Content = "下降";
        //    }
        //    else
        //    {
        //        myClass.SendBytesData(myClass.com1, 0, 0, 0, 0, 0);
        //        button.Content = "上升";
        //    }
        //}
    }
}

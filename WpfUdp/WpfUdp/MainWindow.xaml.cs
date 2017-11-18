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





namespace WpfUdp
{
    public partial class MainWindow : Window
    {
        private IPAddress localIPAddr;
        private UInt16 localPortNum;
        private UdpClient udpClient;
        bool receiveThreadAlive; //控制接收线程的退出
        public MainWindow()
        {
            InitializeComponent();
            receiveThreadAlive = false;
        }
        public void SetTextBox(string str)
        {
            textBoxReceive.Text += str;
            textBoxReceive.ScrollToEnd();
        }
        delegate void TextBoxCallback(string str);
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
                MessageBox.Show("请检查IP地址和本机IP是否一致!","提醒");
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
                    string str = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                    str = remote.ToString() + ": " + str + "\r\n";
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
        private void sendData()
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(textBoxSend.Text);
            if (bytes.Length == 0)
            {
                MessageBox.Show("请勿发送空字符串");
                return;
            }
            UdpClient myUdpClient = new UdpClient();
            IPAddress remoteIP;
            if (IPAddress.TryParse(textBoxIPAddress.Text, out remoteIP) == false)
            {
                MessageBox.Show("远程IP格式不正确");
                return;
            }
            UInt16 port;
            if (UInt16.TryParse(textBoxPortNum.Text, out port) == false)
            {
                MessageBox.Show("不是一个正确的端口号");
                return;
            }
            IPEndPoint iep = new IPEndPoint(remoteIP, port);
            try
            {
                myUdpClient.Send(bytes, bytes.Length, iep);
                //myUdpClient.SendAsync(11,23,iep);
                myUdpClient.Close();
                textBoxSend.Focus();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "发送失败");
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            sendData();
        }

        private void btnClearReceive_Click(object sender, RoutedEventArgs e)
        {
            textBoxReceive.Clear();
        }

        private void btnLink_Click(object sender, RoutedEventArgs e)
        {
            IPAddress localIP;
            if (IPAddress.TryParse(cmbIPAddress.Text, out localIP) == false)
            {
                MessageBox.Show("本地IP格式不正确");
                return;
            }
            UInt16 port;
            if (UInt16.TryParse(textBoxLocalPortNum.Text, out port) == false)
            {
                MessageBox.Show("不是一个正确的端口号");
                return;
            }
            if (receiveThreadAlive == true)
            {
                MessageBox.Show("连接已建立，请勿重复连接");
                return;
            }
            localIPAddr = localIP;
            localPortNum = port;
            Thread thread1 = new Thread(new ThreadStart(ReceiveThreadProc));
            thread1.IsBackground = true; //设置为后台线程
            thread1.Start();
            Thread.Sleep(5);
            if (receiveThreadAlive == true)
            {
                SetTextBox("已连接\r\n");
            }
            else
            {
                SetTextBox("连接失败\r\n");
            }
            
        }

        private void btnUnLink_Click(object sender, RoutedEventArgs e)
        {
            if (receiveThreadAlive == true)
            {
                receiveThreadAlive = false;
                if (udpClient != null)
                {
                    udpClient.Close();
                }
                SetTextBox("已断开\r\n");
                return;
            }
            else
            {
                MessageBox.Show("连接已断开");
                return;
            }
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e) //得到本机的IP地址
        {
            string hostName = Dns.GetHostName();//本机名     
            System.Net.IPAddress[] addressList = Dns.GetHostAddresses(hostName);//会返回所有地址，包括IPv4和IPv6     
            foreach (IPAddress ip in addressList)
            {
                if (ip.AddressFamily.ToString() == ProtocolFamily.InterNetwork.ToString()) //如果是IPv4地址
                {
                    cmbIPAddress.Items.Add(ip.ToString());
                }
            }
            cmbIPAddress.SelectedIndex = 0;
            cmbIPAddress.Items.Add("127.0.0.1");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            this.Hide();
            windowDebug win = new windowDebug();
            win.Show();  
        }
    }
}

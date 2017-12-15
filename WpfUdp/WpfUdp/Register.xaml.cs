using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace WpfUdp
{
    /// <summary>
    /// windowDebug.xaml 的交互逻辑
    /// </summary>
    public partial class RegisterWindow : Window
    {


        string userCode;                  //用户码
        string RegisterCode;              //注册码
        Module myClass = new Module();

        public RegisterWindow()
        {
            InitializeComponent();

        }

        //private void button_Click(object sender, RoutedEventArgs e)
        //{
        //    //this.Close();
        //    this.Hide();
        //    MainWindow win = new MainWindow();
        //    win.Show();  
        //}




        private void GetUserCode_Click(object sender, RoutedEventArgs e)
        {
            //Module  myClass = new Module ();
            //myClass.sendData(1,2,3,4,5);                   //调用类名为Module中的函数sendData

            Random rd = new Random();
            userCode = rd.Next().ToString();
            textUserCode.Text = userCode;                   //将用户码显示在文本框
        }

        /// <summary>
        /// md5 32位加密
        /// </summary>
        /// <param name="传入的参数值password"></param>
        /// <returns></returns>
        public static string MD5Encrypt32(string password)
        {
            string cl = password;
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
                                    // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            try
            {
                byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
                // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
                for (int i = 0; i < s.Length; i++)
                {
                    // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                    pwd = pwd + (s[i].ToString("X").Length <2?"0"+s[i].ToString("X"):s[i].ToString("X"));
                    //pwd = pwd + s[i].ToString("x");
                }

            }
            catch
            {
                MessageBox.Show("请点击获取用户码");
            }
            return pwd;
        }


        private void textUserCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show("123");

            RegisterCode = MD5Encrypt32(MD5Encrypt32(MD5Encrypt32(userCode)));


        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            try
            { 

            string changeRegisterCode = textRegisterCode.Text;        //注册码文本框的字符串包含期限信息

            string ss1 = changeRegisterCode.Substring(0, 4);
            string s11 = changeRegisterCode.Substring(4, 1);

            string ss2 = changeRegisterCode.Substring(5, 6);
            string s12 = changeRegisterCode.Substring(11, 1);

            string ss3 = changeRegisterCode.Substring(12, 8);
            string s13 = changeRegisterCode.Substring(20, 1);

            string ss4 = changeRegisterCode.Substring(21, 10);
            string s14 = changeRegisterCode.Substring(31, 1);


            string s15 = changeRegisterCode.Substring(32, 2);
            string ss5 = changeRegisterCode.Substring(34, 4);

            //得到最终的注册码
            string ss = ss1 + ss2 + ss3 + ss4 + ss5;
            //注册码的期限信息
            int s1 = Int32.Parse(s11) + Int32.Parse(s12) + Int32.Parse(s13) + Int32.Parse(s14);

            if (s1 != 36) //转换成实际的期限
            {
                Module.deadlineOrPermanent = 0x4;
                int totalDays = 1000 * (Int32.Parse(s11)) + 100 * (Int32.Parse(s12)) + 10 * (Int32.Parse(s13)) + Int32.Parse(s14);
                Module.deadlineYY = (byte)(totalDays / 365);
                int remainDays = totalDays % 365;
                Module.deadlineMM = (byte)(remainDays / 30);
                Module.deadlineDD = (byte)(remainDays % 30);
            }
            else     //永久码
            {
                Module.deadlineOrPermanent = 0x9;
            }


            if (ss == RegisterCode && s1 == Int32.Parse(s15))
            {
                MessageBox.Show("注册成功");
                myClass.GetChipId();
                this.Hide();
                MainWindow mainwindow = new MainWindow();
                mainwindow.Show();
                Module.chipID[7] = 0x88;
                Module.chipID[8] = 0x1;
                Module.chipID[9] = Module.YY;
                Module.chipID[10] = Module.MM;
                Module.chipID[11] = Module.DD;

                Module.YY ^= 0x5a;
                Module.MM ^= 0x7c;
                Module.DD ^= 0xB8;

                Module.chipID[0] ^= 0x33;
                Module.chipID[1] ^= 0x44;
                Module.chipID[2] ^= 0x55;
                Module.chipID[3] ^= 0x66;
                Module.chipID[4] ^= 0x77;
                Module.chipID[5] ^= 0x88;
                Module.chipID[6] ^= 0x99;

                Module.chipID[0] ^= 0x34;
                Module.chipID[1] ^= 0x75;
                Module.chipID[2] ^= 0x6a;
                Module.chipID[3] ^= 0x7b;
                Module.chipID[4] ^= 0x4a;
                Module.chipID[5] ^= 0x7c;
                Module.chipID[6] ^= 0x8f;

                File.WriteAllBytes(@"C: \Users\shuqee\Desktop\shuqee.bin", Module.chipID);

            }
            else
            {
                MessageBox.Show("注册失败");
            }
            }
            catch
            {

                MessageBox.Show("输入的注册码长度不正确，请检查");
            }
          
            //textRegisterCode.Text = RegisterCode;
            //myClass.sendCheckData();
        }



        private void Window_Closed(object sender, EventArgs e)
        {

            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();

        }
    }
}

using System;
using System.Collections.Generic;
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
        //Module myClass = new Module();
        
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
                    pwd = pwd + s[i].ToString("X");
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
//            Dim mm8 As Byte
//Dim mm9 As Byte
//Dim S As String
//Dim ss As String
//Dim ss1 As String
//Dim ss2 As String
//Dim ss3 As String
//Dim ss4 As String
//Dim ss5 As String
//Dim s1 As String
//Dim s11 As String
//Dim s12 As String
//Dim s13 As String
//Dim s14 As String
//Dim s15 As String
//S = Text2.Text
//ss1 = Mid(S, 1, 4)
//s11 = Mid(S, 5, 1)

//ss2 = Mid(S, 6, 6)
//s12 = Mid(S, 12, 1)

//ss3 = Mid(S, 13, 8)
//s13 = Mid(S, 21, 1)

//ss4 = Mid(S, 22, 10)
//s14 = Mid(S, 32, 1)

//s15 = Mid(S, 33, 2)
//ss5 = Mid(S, 35, 4)

//ss = ss1 & ss2 & ss3 & ss4 & ss5
//s1 = Val(s11) + Val(s12) + Val(s13) + Val(s14)
//If s1 <> 36 Then
//dT = &H4
//qixian = 1000 * Val(s11) + 100 * Val(s12) + 10 * Val(s13) + Val(s14)
//'YYc = (Int(qixian / 365))
//'qian1 = qixian Mod 365
//'MMc = (Int(qixian1 / 30))
//'DDc = (qixian1 Mod 30)
// 'Text3.Text = YYc
// 'Text4.Text = MMc
// 'Text5.Text = DDc


//Text3.Text = Int(qixian / 365)
//qixian1 = qixian Mod 365
//Text4.Text = Int(qixian1 / 30)
//Text5.Text = qixian1 Mod 30

//YYc = Text3.Text
//MMc = Text4.Text
//DDc = Text5.Text

//Else
//dT = &H9
//End If

//If SHUQEE = ss And s1 = Val(s15) Then
//MsgBox "注册成功"
//Call serialintt(Form1.MSComm1)
//Form11.Hide
//Form1.Show
//UDPStart = True
//mm8 = &H88
//mm9 = &H1

//        YY = YY Xor & H5A          '加密
//        MM = MM Xor & H7C
//        DD = DD Xor & HB8


//        id1 = id1 Xor & H33
//        id2 = id2 Xor & H44
//        id3 = id3 Xor & H55
//        id4 = id4 Xor & H66
//        id5 = id5 Xor & H77
//        id6 = id6 Xor & H88
//        id7 = id7 Xor & H99


//        id1 = id1 Xor & H34
//        id2 = id2 Xor & H75
//        id3 = id3 Xor & H6A
//        id4 = id4 Xor & H7B
//        id5 = id5 Xor & H4A
//        id6 = id6 Xor & H7C
//        id7 = id7 Xor & H8F
//   Open "C:\WINDOWS\system32\shuqee.bin" For Binary As #1
//        Put #1, , id1
//        Put #1, , id2
//        Put #1, , id3
//        Put #1, , id4
//        Put #1, , id5
//        Put #1, , id6
//        Put #1, , id7
//        Put #1, , mm8
//        Put #1, , mm9
//        Put #1, , YY
//        Put #1, , MM
//        Put #1, , DD
//                      '重新写数据给shuqee
//        Close #1
//Else
//MsgBox "注册失败"
//End If
//End Sub

            if (textRegisterCode.Text== RegisterCode)
            {
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show("注册失败");
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

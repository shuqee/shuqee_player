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
using System.Windows.Shapes;


namespace WpfUdp
{
    /// <summary>
    /// windowDebug.xaml 的交互逻辑
    /// </summary>
    public partial class windowDebug : Window
    {
        Module myClass = new Module();
        public windowDebug()
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
            myClass.sendData(1,2,3,4,5);                   //调用类名为Module中的函数sendData

            Random rd = new Random();
            int ii = rd.Next();
            textUserCode.Text = myClass.returnData.ToString()+ii.ToString() ; 
        }

      


        private void textUserCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            //MessageBox.Show("123");
            textRegisterCode.Text  = "123";
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            myClass.sendCheckData();
        }
    }
}

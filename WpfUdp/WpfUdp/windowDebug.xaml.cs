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
        public windowDebug()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //this.Close();
            this.Hide();
            MainWindow win = new MainWindow();
            win.Show();  
        }

        


        private void GetUserCode_Click(object sender, RoutedEventArgs e)
        {
            Class1 myClass = new Class1();
            //string s = myClass.sendData().tostring();
            myClass.sendData();

            textUserCode.Text = myClass.returnData.ToString(); 
        }

      
        private void Register_Click(object sender, RoutedEventArgs e)
        {

        }

     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net;
using System.IO;

namespace DoubanPrj
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        BookXmlData bkData = null;

        public MainWindow()
        {
            InitializeComponent();
            //BookXmlData bkData = null;

            //try
            //{
            ////    bkData = new BookXmlData("..\\..\\srch.xml");

            //    Binding bind = new Binding();
            //    bind.Source = bkData;
            //    bind.Path = new PropertyPath("Entry");


            //    listBox.SetBinding(ListBox.ItemsSourceProperty, bind);
            //}
            //catch (Exception e)
            //{
            //    Debug.Assert(false);
            //}


            //int i = 545;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(srchInput.Text.Trim() != "")
            {
                WebClient wc = new WebClient();
                wc.DownloadDataCompleted += new DownloadDataCompletedEventHandler(wc_DownloadDataCompleted);

                string arg = srchInput.Text;
                arg = arg.Replace("+", "%2b");
                string url = "http://api.douban.com/book/subjects?max-results=15&q=" + arg;
                wc.DownloadDataAsync(new Uri(url));

            }
        }

        void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
        //    throw new NotImplementedException();
            //string xmlData = Encoding.ASCII.GetString(e.Result);

            MemoryStream ms = new   MemoryStream(e.Result);
            try
            {
                bkData = new BookXmlData(ms);

                Binding bind = new Binding();
                bind.Source = bkData;
                bind.Path = new PropertyPath("Entry");


                listBox.SetBinding(ListBox.ItemsSourceProperty, bind);
            }
            catch (Exception err)
            {
                Debug.Assert(false);
            }

            int i = 4343;
        }
    }
}

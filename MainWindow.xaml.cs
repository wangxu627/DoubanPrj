using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DoubanProj;
using System.Diagnostics;

namespace WPFIntf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BookXmlData bkData = null;
            
            try 
            {
                bkData = new BookXmlData("..\\..\\srch.xml");
                
                Binding bind = new Binding();
                bind.Source = bkData;
                bind.Path = new PropertyPath("Entry");
                
                //listBox.ItemsSource = bind;
                listBox.SetBinding(ListBox.ItemsSourceProperty, bind);
                
                
            }
            catch(Exception e)
            {
                Debug.Assert(false);
            }

            
            int i = 545;
        }
    }
}

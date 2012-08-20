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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DoubanProj;

namespace WPFIntf
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            ColorPresenter colorPresenter = new ColorPresenter();

            Binding bind = new Binding();
            bind.Source = colorPresenter;
            bind.Path = new PropertyPath("Colors");

            listBox.SetBinding(ListBox.ItemsSourceProperty, bind);

            //BookXmlData bkData = new BookXmlData("..\\..\\srch.xml");
            
            //int i =5454;
        }
    }
}

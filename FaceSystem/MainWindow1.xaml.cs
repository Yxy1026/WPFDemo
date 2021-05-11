using FaceSystem.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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



namespace FaceSystem
{
    /// <summary>
    /// MainWindow1.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow1 : Window
    {
        
        public MainWindow1()
        {
            InitializeComponent();
            //WindowState = WindowState.Maximized;
        }

        public static int flag = 0;

        private void btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Register_Click(object sender, RoutedEventArgs e)
        {
         
            flag = 1;
            if (grid_IndexWarp.Visibility == System.Windows.Visibility.Visible)
            {
                //grid_IndexWarp.Visibility = System.Windows.Visibility.Collapsed;
                this.PageContext.Visibility = System.Windows.Visibility.Visible;
            }
            var btn = e.Source as Button;
            if (btn != null && btn.Tag != null)
            {
                Btn_Main.Visibility = System.Windows.Visibility.Visible;
                //Time.Visibility = System.Windows.Visibility.Collapsed;
                //week.Visibility = System.Windows.Visibility.Collapsed;
                this.PageContext.Navigate(new Uri(btn.Tag.ToString(), UriKind.Relative));

                Console.WriteLine("跳转到提示页面");
            }

        }

        private void Btn_Vrs_Click(object sender, RoutedEventArgs e)
        {
            grid_IndexWarp.Visibility = System.Windows.Visibility.Visible;

            this.PageContext.Visibility = System.Windows.Visibility.Collapsed;

            this.PageContext.Unloaded += PageContext_Unloaded;

            Btn_Main.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void PageContext_Unloaded(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Btn_Inquire_Click(object sender, RoutedEventArgs e)
        {
            flag = 2;
            if (grid_IndexWarp.Visibility == System.Windows.Visibility.Visible)
            {
                //grid_IndexWarp.Visibility = System.Windows.Visibility.Collapsed;
                this.PageContext.Visibility = System.Windows.Visibility.Visible;
            }
            var btn = e.Source as Button;
            if (btn != null && btn.Tag != null)
            {
                
                Btn_Main.Visibility = System.Windows.Visibility.Visible;
                //Time.Visibility = System.Windows.Visibility.Collapsed;
                //week.Visibility = System.Windows.Visibility.Collapsed;

                this.PageContext.Navigate(new Uri(btn.Tag.ToString(), UriKind.Relative));

            }

        }

        public Frame GetFrame()
        {
            return PageContext;
        }
    }
}

using KSHWP;
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


namespace KSHGUI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ContentArea.Content = new PlanKont();

        }

        private void KG_Checked(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ContentArea.Content = new KsiegaGlowna();
            Naglowek.Text = "KSIĘGA GŁÓWNA";
            this.Cursor = Cursors.Arrow;
        }

        private void UsunDekretacje_Checked(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ContentArea.Content = new UsuńDekretacje();
            Naglowek.Text = "USUŃ DEKRETACJE";
            this.Cursor = Cursors.Arrow;
        }

        private void Dekretacja_Checked(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ContentArea.Content = new Dekretacja();
            Naglowek.Text = "DEKRETACJA";
            this.Cursor = Cursors.Arrow;
        }

        private void RZiS_Checked(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            ContentArea.Content = new RZiS();
            Naglowek.Text = "RZiS";
            this.Cursor = Cursors.Arrow;
        }

       

        private void ZOiS_Checked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new ZOiS();
            Naglowek.Text = "ZOiS";
        }

        private void Bilans_Checked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new Bilans();
            Naglowek.Text = "BILANS";
        }

       

        private void PlanKont_Checked(object sender, RoutedEventArgs e)
        {
            ContentArea.Content = new PlanKont();
            Naglowek.Text = "PLAN KONT";
        }

        
    }
    }


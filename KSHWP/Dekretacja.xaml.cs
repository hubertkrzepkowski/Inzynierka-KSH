using BespokeFusion;
using KSHGUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace KSHWP
{
    /// <summary>
    /// Logika interakcji dla klasy DG.xaml
    /// </summary>
    public partial class Dekretacja : UserControl, INotifyPropertyChanged
    {

        String connectionString = new Connection().ConnectionString.Trim();
        public string Text { get; set; }
        private string hint1;
        public string Hint1
        {
            get { return hint1; }
            set
            {
                hint1 = value;
                OnPropertyChanged("Hint1");
            }
        }



        public Dekretacja()
        {
            InitializeComponent();
            Hint1 = "Koszt Netto";
            opis.Text = "DekretacjaKosztow410MatEn";
        }





        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = (sender as ListBox).SelectedItem as TextBlock;
            if (item != null)
            {
                KWS.Visibility = Visibility.Hidden;
                H1.Text = item.Text;
                opis.Text = item.Text;
                switch (H1.Text)
                {
                    case "DekretacjaKosztow460Wynagr":
                        Hint1 = "Wynagrodzenie Brutto";
                        break;

                    case "DekretacjaKosztow410MatEn":
                        Hint1 = "Koszt Netto";
                        break;
                    case "DekretacjaKosztow420UslObce":
                        Hint1 = "Koszt Netto";
                        break;
                    case "DekretacjaSprzedazy":
                        Hint1 = "Sprzedaż Netto";
                        KWS.Visibility = Visibility.Visible;
                        break;
                    case "DekretacjaZakupu":
                        Hint1 = "Zakup Netto";
                        break;
                    default:
                        Hint1 = "Kwota Zapłaty";
                        break;
                }




            }

        }

        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event 
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public static bool IsDoubleRealNumber(string valueToTest)
        {
            if (double.TryParse(valueToTest, out double d) && !Double.IsNaN(d) && !Double.IsInfinity(d))
            {
                return true;
            }

            return false;
        }

        private bool Validacjia()
        {
            Regex regex = new Regex("^[1-9][0-9]*$");
            Match matchKwota = regex.Match(Kwota.Text);
            Match matchKWS = regex.Match(KWS.Text);

            Color kolor = (Color)ColorConverter.ConvertFromString("#89FFFFFF");
            Brush brush = new SolidColorBrush(kolor);
            Kwota.BorderBrush = brush;
            data.BorderBrush = brush;
            kontarhent.BorderBrush = brush;
            ksiegujacy.BorderBrush = brush;
            nrdok.BorderBrush = brush;
            opis.BorderBrush = brush;
            KWS.BorderBrush = brush;
            bool wynik = true;
            if (!matchKwota.Success) { Kwota.BorderBrush = Brushes.Red; wynik = false; }
            if (data.Text == "") { data.BorderBrush = Brushes.Red; wynik = false;}
            if (kontarhent.SelectedIndex == -1) { kontarhent.BorderBrush = Brushes.Red; wynik = false; }
            if (ksiegujacy.SelectedIndex == -1) { ksiegujacy.BorderBrush = Brushes.Red; wynik = false; }
            if (nrdok.Text == "") { nrdok.BorderBrush = Brushes.Red; wynik = false; }
            if (opis.Text == "") { opis.BorderBrush = Brushes.Red; wynik = false; }
            if(KWS.Visibility == Visibility.Visible)
            {
                if (!matchKWS.Success) { KWS.BorderBrush = Brushes.Red; wynik = false; }
            }

            return wynik;
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {

            if (Validacjia())
            {
                SnackbarOne.IsActive = false;
                var param = Hint1.Replace(" ", "");
                if (Hint1 == "Wynagrodzenie Brutto") param = "WynagrBrutto";
                if (Hint1 == "Sprzedaż Netto") param = "SprzedazNetto";
                if (Hint1 == "Kwota Zapłaty") param = "KwotaZaplaty";
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("sp" + H1.Text, con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@" + param, SqlDbType.Money).Value = Kwota.Text;
                            cmd.Parameters.Add("@OpisOperacji", SqlDbType.VarChar).Value = opis.Text;
                            cmd.Parameters.Add("@DataOperacji", SqlDbType.Date).Value = data.Text;
                            cmd.Parameters.Add("@kontrahent", SqlDbType.VarChar).Value = kontarhent.Text;
                            cmd.Parameters.Add("@ksiegujacy", SqlDbType.VarChar).Value = ksiegujacy.Text;
                            cmd.Parameters.Add("@NrDokumentu", SqlDbType.VarChar).Value = nrdok.Text;
                            if(KWS.Visibility == Visibility.Visible)
                            {
                            cmd.Parameters.Add("@KWS", SqlDbType.Money).Value = KWS.Text;
                            }

                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                    SnackbarOne.IsActive = true;
                }
                catch (Exception error)
                {
                    Error komunikat = new Error(error);
                }
            }
        }

       

        private void OK_ActionClick(object sender, RoutedEventArgs e)
        {
            SnackbarOne.IsActive = false;
        }
    }

}

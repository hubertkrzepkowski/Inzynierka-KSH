using BespokeFusion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

namespace KSHWP
{
    /// <summary>
    /// Logika interakcji dla klasy RZiS.xaml
    /// </summary>
    public partial class RZiS : UserControl
    {
        String connectionString = new Connection().ConnectionString.Trim();

        public RZiS()
        {
            InitializeComponent();
            PobierzRZiS();
        }

        private void PobierzRZiS()
        {

            DataTable wynik = new DataTable();

            {

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("spRZiS", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            adp.Fill(wynik);
                            con.Close();
                            RZiSDataGrid.ItemsSource = wynik.AsDataView();

                            RZiSDataGrid.AutoGeneratingColumn += (s, e) =>
                            {

                                if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                                if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                                if (e.PropertyName == "OpisPozycjiRziS") { e.Column.Header = "Opis"; }
                                if (e.PropertyName == "WartoscPozycjiRZiS") { e.Column.Header = "Wartość"; }
                          
                            };

                        }
                    }
                }
                catch (Exception error)
                {
                    Error komunikat = new Error(error);
                }
            }

        }

       
    }
}

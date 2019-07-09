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
    /// Logika interakcji dla klasy ZOiS.xaml
    /// </summary>
    public partial class ZOiS : UserControl
    {
        String connectionString = new Connection().ConnectionString.Trim();

        public ZOiS()
        {
            InitializeComponent();
            PobierzZOiS();
        }

        private void PobierzZOiS()
        {

            DataSet wynik = new DataSet();

            {

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("spZestawienieObrotowISald", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            adp.Fill(wynik);
                            con.Close();
                            ZOiSDataGrid.ItemsSource = wynik.Tables[0].AsDataView();
                            PodsumowanieDataGrid.ItemsSource = wynik.Tables[1].AsDataView();

                            ZOiSDataGrid.AutoGeneratingColumn += (s, e) =>
                            {

                                if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                                if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                                if (e.PropertyName == "konto") { e.Column.Header = "Konto"; }
                                if (e.PropertyName == "nazwaKonta") { e.Cancel = true; }
                                if (e.PropertyName == "BO_WN") { e.Column.Header = "BO WN"; }
                                if (e.PropertyName == "BO_MA") { e.Column.Header = "BO MA"; }
                                if (e.PropertyName == "Saldo_WN") { e.Column.Header = "Saldo WN"; }
                                if (e.PropertyName == "Saldo_MA") { e.Column.Header = "Saldo MA"; }
                                if (e.PropertyName == "BZ_WN") { e.Column.Header = "BZ WN"; }
                                if (e.PropertyName == "BZ_MA") { e.Column.Header = "BZ MA"; }

                            };

                            PodsumowanieDataGrid.AutoGeneratingColumn += (s, e) =>
                            {

                                if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                                if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                                if (e.PropertyName == "BOwn") { e.Column.Header = "BO WN"; }
                                if (e.PropertyName == "BOma") { e.Column.Header = "BO MA"; }
                                if (e.PropertyName == "ObrotyWN") { e.Column.Header = "Obroty WN"; }
                                if (e.PropertyName == "ObrotyMA") { e.Column.Header = "Obroty MA"; }
                                if (e.PropertyName == "BZwn") { e.Column.Header = "BZ WN"; }
                                if (e.PropertyName == "BZma") { e.Column.Header = "BZ MA"; }

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

using BespokeFusion;
using KSHGUI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
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
    /// Logika interakcji dla klasy DekretacjaView.xaml
    /// </summary>
    public partial class KsiegaGlowna : UserControl
    {
        String connectionString = new Connection().ConnectionString.Trim();


        public KsiegaGlowna()
        {
            InitializeComponent();
            PobierzKg();

        }

        private void PobierzKg()
        {

            DataTable KG = new DataTable();

            {

                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("spDziennikGlowna", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter adp = new SqlDataAdapter(cmd);
                            con.Open();
                            cmd.ExecuteNonQuery();
                            adp.Fill(KG);
                            con.Close();
                            KsiegaGlownaDataGrid.ItemsSource = KG.AsDataView();

                            KsiegaGlownaDataGrid.AutoGeneratingColumn += (s, e) =>
                            {

                                if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                                if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}"+" ZŁ"; }
                                if (e.PropertyName == "NrDokumentu") { e.Column.Header = "Nr Dokumentu"; }
                                if (e.PropertyName == "kontrahent") { e.Column.Header = "Kontrahent"; }
                                if (e.PropertyName == "opisOperacji") { e.Column.Header = "Opis Operacji"; }
                                if (e.PropertyName == "DataOperacji") { e.Column.Header = "Data Operacji"; }
                                if (e.PropertyName == "DataKsiegowania") { e.Column.Header = "Data Księgowania"; }
                                if (e.PropertyName == "konto") { e.Column.Header = "Konto"; }
                                if (e.PropertyName == "idAutodekretacji") { e.Column.Header = "ID Autodekretacji"; }
                                if (e.PropertyName == "Obroty_WN") { e.Column.Header = "Obroty WN"; }
                                if (e.PropertyName == "Obroty_MA") { e.Column.Header = "Obroty MA"; }

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


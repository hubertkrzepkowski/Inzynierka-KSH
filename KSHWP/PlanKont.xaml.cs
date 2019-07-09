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
    /// Logika interakcji dla klasy PlanKont.xaml
    /// </summary>
    public partial class PlanKont : UserControl
    {
        String connectionString = new Connection().ConnectionString.Trim();

        public PlanKont()
        {
            InitializeComponent();
            pobierzPlanKont();

        }

        private void pobierzPlanKont()
        {
            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    string queryStatement = "SELECT * FROM tbPlanKont";
                    using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                    {
                        DataTable wynik = new DataTable();
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(wynik);
                        _con.Close();
                        PlanKontDataGrid.ItemsSource = wynik.AsDataView();

                    }
                }
            }
            catch (Exception error)
            {
                Error komunikat = new Error(error);
            }
            PlanKontDataGrid.AutoGeneratingColumn += (s, e) =>
                    {
                        if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                        if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                        if (e.PropertyName == "IdSyntetyka") { e.Column.Header = "ID Syntetyki"; }
                        if (e.PropertyName == "OpisKonta") { e.Column.Header = "Opis Konta"; }
                        if (e.PropertyName == "TypKonta") { e.Column.Header = "Typ Konta"; }
                        if (e.PropertyName == "AktywnePasywneMieszane") { e.Column.Header = "Aktywne/Pasywne/Mieszane"; }
                        if (e.PropertyName == "NazwaTabeliKonta") { e.Column.Header = "Nazwa Tabeli Konta"; }
                        if (e.PropertyName == "BOWn") { e.Column.Header = "BO WN"; }
                        if (e.PropertyName == "BOMa") { e.Column.Header = "BO MA"; }
                        if (e.PropertyName == "BZWn") { e.Column.Header = "BZ WN"; }
                        if (e.PropertyName == "BZMa") { e.Column.Header = "BZ MA"; }


                    };
        }
    }
}
    


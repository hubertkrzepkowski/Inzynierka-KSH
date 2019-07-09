using BespokeFusion;
using MaterialDesignThemes.Wpf;
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
    /// Logika interakcji dla klasy UsuńDekretacje.xaml
    /// </summary>
    public partial class UsuńDekretacje : UserControl
    {
        String connectionString = new Connection().ConnectionString.Trim();
        DataView Dt1 { get; set; }

        public UsuńDekretacje()
        {
            InitializeComponent();

            pobierzIDdekretacji();

        }

        private void pobierzIDdekretacji()
        {

            try
            {
                using (SqlConnection _con = new SqlConnection(connectionString))
                {
                    string queryStatement = "SELECT * FROM tblIdentyfikatoryAutodekretacji";
                    using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                    {
                        DataTable Iddekretacji = new DataTable("IdDekretacji");
                        SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                        _con.Open();
                        _dap.Fill(Iddekretacji);
                        _con.Close();
                        IdDekretacjiDG.ItemsSource = Iddekretacji.AsDataView();
                    }
                }
            }
            catch (Exception error)
            {
                Error komunikat = new Error(error);
            }


            IdDekretacjiDG.AutoGeneratingColumn += (s, e) =>
            {
                if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                if (e.PropertyName == "NrDokumentu") { e.Column.Header = "Nr Dokumentu"; }
                if (e.PropertyName == "kontrahent") { e.Column.Header = "Kontrahent"; }
                if (e.PropertyName == "ksiegujacy") { e.Column.Header = "Księgujący"; }
                if (e.PropertyName == "dataAutodekretacji") { e.Column.Header = "Data Dekretacji"; }
                if (e.PropertyName == "opisAutodekrtacji") { e.Column.Header = "Opis"; }
                if (e.PropertyName == "IdAutodekretacji") { e.Column.Header = "ID Autodekretacji"; }
                if (e.PropertyName == "doUsuniecia")
                {
                    e.Column.Header = "Usuń";
                    DataGridTemplateColumn buttonColumn = new DataGridTemplateColumn();
                    DataTemplate buttonTemplate = new DataTemplate();
                    FrameworkElementFactory buttonFactory = new FrameworkElementFactory(typeof(Button));
                    buttonTemplate.VisualTree = buttonFactory;
                    buttonFactory.AddHandler(Button.ClickEvent, new RoutedEventHandler(Usun_Click));
                    buttonFactory.SetValue(ContentProperty, "Usuń");
                    buttonColumn.CellTemplate = buttonTemplate;
                    e.Column = buttonColumn;
                }
            };
        }



        private void Usun_Click(object sender, RoutedEventArgs e)
        {
            DataRowView row = IdDekretacjiDG.SelectedItem as DataRowView;
            var id = row.Row.ItemArray[0].ToString();
            var msg = new CustomMaterialMessageBox
            {
                TxtMessage = { Text = "CZY NA PEWNO CHCESZ USUNĄĆ REKORD O ID " + id + " Z BAZY DANYCH ? ", Foreground = Brushes.White },
                TxtTitle = { Text = "USUWANIE", Foreground = Brushes.White },
                BtnOk = { Content = "TAK", Background = Brushes.Orange, BorderBrush = Brushes.DimGray },
                BtnCancel = { Content = "NIE", Background = Brushes.Orange, BorderBrush = Brushes.DimGray },
                MainContentControl = { Background = Brushes.Gray },
                TitleBackgroundPanel = { Background = Brushes.Orange },
                BorderBrush = Brushes.Orange
            };

            msg.Show();
            var results = msg.Result;

            if (results.ToString() == "OK")
            {
                try
                {
                    using (var sc = new SqlConnection(connectionString))
                    using (var cmd = sc.CreateCommand())
                    {
                        sc.Open();
                        cmd.CommandText = "DELETE FROM tblIdentyfikatoryAutodekretacji WHERE IdAutodekretacji = @id";
                        cmd.Parameters.AddWithValue("@id", row.Row.ItemArray[0]);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception error)
                {
                    Error komunikat = new Error(error);
                }

                pobierzIDdekretacji();



            }


        }





        private void Szukaj_Click(object sender, RoutedEventArgs e)
        {

            DataTable wyszukane = new DataTable();
            this.Cursor = Cursors.Wait;
            try
            {
                this.Cursor = Cursors.Wait;
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spSzukajDekretacji", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (Id.Text != "") { cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = Id.Text; } else { cmd.Parameters.Add("@ID", SqlDbType.VarChar).Value = DBNull.Value; }
                        if (Opis.Text != "") { cmd.Parameters.Add("@Opis", SqlDbType.VarChar).Value = Opis.Text; } else { cmd.Parameters.Add("@Opis", SqlDbType.VarChar).Value = DBNull.Value; }
                        if (DataOd.Text != "") { cmd.Parameters.Add("@DataOd", SqlDbType.Date).Value = DataOd.Text; } else { cmd.Parameters.Add("@DataOd", SqlDbType.Date).Value = DBNull.Value; }
                        if (DataDo.Text != "") { cmd.Parameters.Add("@DataDo", SqlDbType.Date).Value = DataDo.Text; } else { cmd.Parameters.Add("@DataDo", SqlDbType.Date).Value = DBNull.Value; }
                        if (DataOd.Text != "" && DataDo.Text == "") { cmd.Parameters.RemoveAt("@DataDo"); cmd.Parameters.Add("@DataDo", SqlDbType.Date).Value = DataOd.Text; }
                        if (Ksiegujacy.Text != "") { cmd.Parameters.Add("@Ksiegujacy", SqlDbType.VarChar).Value = Ksiegujacy.Text; } else { cmd.Parameters.Add("@Ksiegujacy", SqlDbType.VarChar).Value = DBNull.Value; }
                        if (Kontrahent.Text != "") { cmd.Parameters.Add("@Kontrahent", SqlDbType.VarChar).Value = Kontrahent.Text; } else { cmd.Parameters.Add("@Kontrahent", SqlDbType.VarChar).Value = DBNull.Value; }
                        if (NrDokumentu.Text != "") { cmd.Parameters.Add("@NrDokumentu", SqlDbType.VarChar).Value = NrDokumentu.Text; } else { cmd.Parameters.Add("@NrDokumentu", SqlDbType.VarChar).Value = DBNull.Value; }
                        if (Kwota.Text != "") { cmd.Parameters.Add("@Kwota", SqlDbType.Money).Value = Kwota.Text; } else { cmd.Parameters.Add("@Kwota", SqlDbType.Money).Value = DBNull.Value; }

                        SqlDataAdapter adp = new SqlDataAdapter(cmd);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        adp.Fill(wyszukane);
                        con.Close();
                        var grid = sender as DataGrid;
                        IdDekretacjiDG.ItemsSource = wyszukane.AsDataView();
                        this.Cursor = Cursors.Arrow;


                    }
                }
            }
            catch (Exception error)
            {
                Error komunikat = new Error(error);

            }
        }
        private void zwinSzczegoly_Click(object sender, RoutedEventArgs e)
        {
            IdDekretacjiDG.SelectedItem = null;
        }



        private void DataGrid_Loaded(object sender, RoutedEventArgs ev)
        {

            DataRowView wiersz = IdDekretacjiDG.SelectedItem as DataRowView;
            if (wiersz != null)
            {
                var id = wiersz.Row.ItemArray[0].ToString();
                var opis = wiersz.Row.ItemArray[1].ToString();
                DataTable szczegoly = new DataTable();

                {
                    this.Cursor = Cursors.Wait;
                    try
                    {
                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("spSzukajSzczegoly", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@Opis", SqlDbType.VarChar).Value = opis;
                                cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                adp.Fill(szczegoly);
                                con.Close();
                                var grid = sender as DataGrid;
                                grid.ItemsSource = szczegoly.AsDataView();
                                this.Cursor = Cursors.Arrow;


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

        private void Szczegoly_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs ev)
        {
            var grid = sender as DataGrid;
           

            grid.AutoGeneratingColumn += (s, e) =>
                                {
                                    if (e.PropertyType == typeof(System.DateTime)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "dd/MM/yyyy"; }
                                    if (e.PropertyType == typeof(System.Decimal)) { (e.Column as DataGridTextColumn).Binding.StringFormat = "{0:N}" + " ZŁ"; }
                                    if (e.PropertyName == "IdWpisu") { e.Column.Header = "ID Wpisu"; }
                                    if (e.PropertyName == "NrDokumentu") { e.Column.Header = "Nr Dokumentu"; }
                                    if (e.PropertyName == "kontrahent") { e.Column.Header = "Kontrahent"; }
                                    if (e.PropertyName == "DataOperacji") { e.Column.Header = "Data Operacji"; }
                                    if (e.PropertyName == "DataKsiegowania") { e.Column.Header = "Data Księgowania"; }
                                    if (e.PropertyName == "analityka") { e.Column.Header = "Analityka"; }
                                    if (e.PropertyName == "OpisOperacji") { e.Column.Header = "Opis Operacji"; }
                                    if (e.PropertyName == "fk_IdAutodekretacji") { e.Column.Header = "ID Autodekretacji"; }
                                    if (e.PropertyName == "BO_WN") { e.Column.Header = "BO WN"; }
                                    if (e.PropertyName == "BO_MA") { e.Column.Header = "BO MA"; }

                                };

        }
    }
}
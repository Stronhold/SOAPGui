using AseguradoraUI.ServicePolicy;
using AseguradoraUI.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace AseguradoraUI
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AddOptionsToCombobox();
        }

        private void AddOptionsToCombobox()
        {
            XmlSerializer ser = new XmlSerializer(typeof(string[]));
            StreamReader reader = new StreamReader(Constants.PATH_ASSETS);
            string [] types = (string [])ser.Deserialize(reader);
            reader.Close();
            foreach(var t in types)
            {
                comboBoxType.Items.Add(t);
            }
            if(comboBoxType.Items.Count > 0)
            {
                comboBoxType.SelectedIndex = 0;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            refreshTable();
        }

        private async void refreshTable()
        {
            var ServicePolicy = new ServicePolicy.Service1Client();
            var pol = await ServicePolicy.GetAllPoliciesAsync();
            RemoveRows();
            AddRows(pol);
        }

        private void AddRows(Policies[] pol)
        {
            var rg = new TableRowGroup();//TablePoliza.RowGroups[0];
            int i = 0;

            foreach (var policy in pol)
            {
                TableRow tR = new TableRow();
                rg.Rows.Add(tR);
                TableCell id = new TableCell(new Paragraph(new Run(policy.ID.ToString())));
                TableCell name = new TableCell(new Paragraph(new Run(policy.name)));
                TableCell description = new TableCell(new Paragraph(new Run(policy.description)));
                TableCell type = new TableCell(new Paragraph(new Run(policy.type)));

                if (i % 2 == 0)
                    tR.Background = Brushes.LightGray;
                else
                    tR.Background = Brushes.White;

                id.TextAlignment = TextAlignment.Center;
                name.TextAlignment = TextAlignment.Center;
                description.TextAlignment = TextAlignment.Center;
                type.TextAlignment = TextAlignment.Center;

                i++;

                tR.Cells.Add(id);
                tR.Cells.Add(name);
                tR.Cells.Add(description);
                tR.Cells.Add(type);
            }
            TablePoliza.RowGroups.Add(rg);
            btnEditar.IsEnabled = true;
        }

        private void RemoveRows()
        {
            for (int i = 1; i < TablePoliza.RowGroups.Count; i++)
            {
                TablePoliza.RowGroups.Remove(TablePoliza.RowGroups[i]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            editRows ed = new editRows();
            ed.Show();
        }

        private async void enviar_Click(object sender, RoutedEventArgs e)
        {
            var ServicePolicy = new Service1Client();
            var p = new Policies();
            int ident;
            bool result = Int32.TryParse(id.Text, out ident);
            if (!result)
            {
                MessageBox.Show("Error, the ID must be a number", "Error with parameter: ID", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string n = name.Text;
            string desc = description.Text;
            string type = comboBoxType.Text;
            bool added = await ServicePolicy.AddPolicyAsync(ident, n, desc, type);
            if (!added)
            {
                MessageBox.Show("Error, your policy exists in the database", "Error adding to DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Added the new policy to our database", "Action completed", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Table_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            TableRow row = sender as TableRow;
            TableCell tcID = row.Cells[0];
            TableCell tcName = row.Cells[1];
            TableCell tcDesc = row.Cells[2];
            Console.WriteLine();
        }
    }
}

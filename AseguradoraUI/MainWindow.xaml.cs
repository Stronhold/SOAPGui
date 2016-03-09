using AseguradoraUI.ServicePolicy;
using AseguradoraUI.Utilities;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
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
        }

        private void FlowDocumentScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Ha hecho click");
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var pol = await GetPolicies();
            RemoveRows();
            AddRows(pol);
        }

        private async Task<Policy []> GetPolicies()
        {
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            var pol = ServicePolicy.GetAllPolicies();
            return pol;
        }

        private void AddRows(Policy[] pol)
        {
            var rg = TablePoliza.RowGroups[0];
            foreach (var policy in pol)
            {
                TableRow tR = new TableRow();
                rg.Rows.Add(tR);
                TableCell id = new TableCell(new Paragraph(new Run(policy.ID.ToString())));
                TableCell name = new TableCell(new Paragraph(new Run(policy.Name)));
                TableCell description = new TableCell(new Paragraph(new Run(policy.Description)));
                TableCell type = new TableCell(new Paragraph(new Run(policy.Type)));
                id.TextAlignment = TextAlignment.Center;
                name.TextAlignment = TextAlignment.Center;
                description.TextAlignment = TextAlignment.Center;
                type.TextAlignment = TextAlignment.Center;
                tR.Cells.Add(id);
                tR.Cells.Add(name);
                tR.Cells.Add(description);
                tR.Cells.Add(type);
            }
        }

        private void RemoveRows()
        {
            for (int i = 1; i < TablePoliza.RowGroups.Count; i++)
            {
                TablePoliza.RowGroups[0].Rows.Remove(TablePoliza.RowGroups[0].Rows[i]);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void enviar_Click(object sender, RoutedEventArgs e)
        {
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            Policy p = new Policy();
            int ident = Convert.ToInt32(id.Text);
            string n = name.Text;
            string desc = description.Text;
            bool added = ServicePolicy.AddPolicy(ident, n, desc, "");
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
            TableCell tcDesc =    row.Cells[2];
            Console.WriteLine();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            Console.WriteLine("sadfdsf");
        }
    }
}

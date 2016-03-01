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
using AseguradoraUI.ServicePolicy;

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
        }

        private void FlowDocumentScrollViewer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Ha hecho click");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            Policy[] pol = ServicePolicy.GetAllPolicies();
            RemoveRows();
            AddRows(pol);
        }

        private void AddRows(Policy[] pol)
        {
            foreach (var policy in pol)
            {
                var rg = new TableRowGroup();
                TableRow tR = new TableRow();
                rg.Rows.Add(tR);
                TableCell id = new TableCell(new Paragraph(new Run(policy.ID.ToString())));
                TableCell name = new TableCell(new Paragraph(new Run(policy.Name)));
                TableCell description = new TableCell(new Paragraph(new Run(policy.Description)));
                id.TextAlignment = TextAlignment.Center;
                name.TextAlignment = TextAlignment.Center;
                description.TextAlignment = TextAlignment.Center;
                tR.Cells.Add(id);
                tR.Cells.Add(name);
                tR.Cells.Add(description);
                TablePoliza.RowGroups.Add(rg);
            }
        }

        private void RemoveRows()
        {
            for(int i = 1; i < TablePoliza.RowGroups.Count; i++)
            {
                TablePoliza.RowGroups.RemoveAt(i);
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
            bool added = ServicePolicy.AddPolicy(ident, n, desc);
            if (!added)
            {
                MessageBox.Show("Error, your policy exists in the database", "Error adding to DB", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Added the new policy to our database", "Action completed", MessageBoxButton.OK, MessageBoxImage.Information);

            }

        }
    }
}

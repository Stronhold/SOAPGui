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
using System.Windows.Shapes;
using AseguradoraUI.ServicePolicy;
using System.Xml.Serialization;
using System.IO;
using AseguradoraUI.Utilities;

namespace AseguradoraUI
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class editRows : Window
    {
        public editRows()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            int[] listId = ServicePolicy.GetAllID();
            foreach (int i in listId)
            {
                cboxID.Items.Add(listId[i]);
            }

            XmlSerializer ser = new XmlSerializer(typeof(string[]));
            StreamReader reader = new StreamReader(Constants.PATH_ASSETS);
            string[] types = (string[])ser.Deserialize(reader);
            reader.Close();
            foreach (var t in types)
            {
                cboxType.Items.Add(t);
            }
            if (cboxType.Items.Count > 0)
            {
                cboxType.SelectedIndex = 0;
            }
        }

        private void cboxIDChange(object sender, SelectionChangedEventArgs e)
        {
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            Policy pol = ServicePolicy.GetData(int.Parse(cboxID.SelectedItem.ToString()));
            txtName.Text = pol.Name;
            txtDesc.Text = pol.Description;
            cboxType.SelectedItem = pol.Type;
            btnAceptar.IsEnabled = true;
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            int id = int.Parse(cboxID.Text);
            String name = txtName.Text;
            String desc = txtDesc.Text;
            String type = cboxType.Text;
            ServicePolicyClient ServicePolicy = new ServicePolicyClient();
            bool pol = ServicePolicy.UpdatePolicy(id, name, desc, type);
            this.Close();
        }
    }
}
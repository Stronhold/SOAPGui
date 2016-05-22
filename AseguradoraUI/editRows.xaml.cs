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
            var ServicePolicy = new Service1Client();
            int[] listId = ServicePolicy.GetAllID();
            foreach (var i in listId)
            {
                cboxID.Items.Add(i);
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
            var ServicePolicy = new Service1Client();
            var pol = ServicePolicy.GetData(int.Parse(cboxID.SelectedItem.ToString()));
            txtName.Text = pol.name;
            txtDesc.Text = pol.description;
            cboxType.SelectedItem = pol.type;
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
            var ServicePolicy = new Service1Client();
            bool pol = ServicePolicy.UpdatePolicy(id, name, desc, type);
            this.Close();
        }
    }
}
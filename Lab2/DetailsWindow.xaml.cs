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

namespace Lab2
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(Threat threat)
        {
            InitializeComponent();
            threatTextBox.Text = $"ID: УБИ.{threat.Id}\n\nName: {threat.Name}\n\nDescription: {threat.Description}\n\nThreat source: {threat.SourceOfAttack}\n\nObject of attack: {threat.ObjectOfAttack}\n\nViolates confidentiality: {(threat.ViolatesConfidentiality ? "Yes": "No")}\n\nViolates integrity: {(threat.ViolatesIntegrity ? "Yes": "No")}\n\nViolates accessibility: {(threat.ViolatesAccessibility ? "Yes": "No")}";
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

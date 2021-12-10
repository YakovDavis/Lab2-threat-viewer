using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.IO;

namespace Lab2
{
    public partial class FileNotFoundWindow : Window
    {
        public FileNotFoundWindow()
        {
            InitializeComponent();
            Focus();
        }

        private void YesButtonClick(object sender, RoutedEventArgs e)
        {
            /*using (var client = new WebClient())
            {
                client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
            }
            MainWindow.LoadFromFile();*/
            UpdateResultsWindow updateResultsWindow = new UpdateResultsWindow();
            updateResultsWindow.Show();
            Close();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

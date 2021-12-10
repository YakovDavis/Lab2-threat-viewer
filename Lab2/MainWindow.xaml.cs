using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Lab2
{
    public partial class MainWindow : Window
    {
        private static DataGrid dataGrid1;
        private static List<Threat> threats;
        private static List<ShortThreat> shortTreats = new List<ShortThreat>();
        private static List<ShortThreat> shortTreatsPage = new List<ShortThreat>();
        private static int pageNumber = 0;


        public MainWindow()
        {
            InitializeComponent();
            dataGrid1 = dataGrid;
            threats = new List<Threat>();
            if (File.Exists("thrlist.xlsx"))
                LoadFromFile();
            else
            {
                FileNotFoundWindow window = new FileNotFoundWindow();
                window.Show();
            }
            RetrievePage(pageNumber);
        }

        public static void LoadFromFile()
        {
            threats.Clear();
            shortTreats.Clear();
            shortTreatsPage.Clear();
            pageNumber = 0;
            using (SpreadsheetDocument document = SpreadsheetDocument.Open("thrlist.xlsx", true))
            {
                WorkbookPart workbookPart = document.WorkbookPart;
                WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                foreach (Row r in sheetData.Elements<Row>())
                {
                    if (r.RowIndex < 3)
                        continue;
                    List<string> s = new List<string>();
                    foreach (Cell cell in r)
                    {
                        string cellValue = "";
                        if (cell.DataType != null)
                        {
                            if (cell.DataType == CellValues.SharedString)
                            {
                                int id = -1;

                                if (Int32.TryParse(cell.InnerText, out id))
                                {
                                    SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                                    if (item.Text != null)
                                    {
                                        cellValue = item.Text.Text;
                                    }
                                    else if (item.InnerText != null)
                                    {
                                        cellValue = item.InnerText;
                                    }
                                    else if (item.InnerXml != null)
                                    {
                                        cellValue = item.InnerXml;
                                    }
                                }
                            }
                        }
                        if (cellValue == "")
                            cellValue = cell.CellValue.InnerText;
                        s.Add(cellValue);
                    }
                    threats.Add(new Threat(Int32.Parse(s[0]), s[1], s[2], s[3], s[4], (s[5] == "1"), (s[6] == "1"), (s[7] == "1")));
                }
            }
            foreach (Threat thr in threats)
            {
                shortTreats.Add(new ShortThreat($"УБИ.{thr.Id.ToString("D3")}", thr.Name));
            }
            RetrievePage(pageNumber);
        }

        public static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }

        private void RowMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ShortThreat st = (ShortThreat)dataGrid.SelectedItem;
            if (st == null)
                return;
            DetailsWindow detailsWindow = new DetailsWindow(threats.Find(x => x.Name == st.Name));
            detailsWindow.Show();
        }

        private void DetailsButtonClick(object sender, RoutedEventArgs e)
        {
            ShortThreat st = (ShortThreat)dataGrid.SelectedItem;
            if (st == null)
                return;
            DetailsWindow detailsWindow = new DetailsWindow(threats.Find(x => x.Name == st.Name));
            detailsWindow.Show();
        }

        private static void RetrievePage(int page)
        {
            shortTreatsPage.Clear();
            for (int i = page * 15; i < (shortTreats.Count > (page + 1) * 15 ? (page + 1) * 15 : shortTreats.Count); i++)
                shortTreatsPage.Add(shortTreats[i]);
            dataGrid1.ItemsSource = null;
            dataGrid1.ItemsSource = shortTreatsPage;
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            nextButton.IsEnabled = true;
            pageNumber--;
            RetrievePage(pageNumber);
            if (pageNumber == 0)
                previousButton.IsEnabled = false;
        }

        private void NextButtonClick(object sender, RoutedEventArgs e)
        {
            previousButton.IsEnabled = true;
            pageNumber++;
            RetrievePage(pageNumber);
            if ((pageNumber + 1) * 15 >= threats.Count)
                nextButton.IsEnabled = false;
        }

        private void UpdateButtonClick(object sender, RoutedEventArgs e)
        {
            UpdateResultsWindow updateResultsWindow = new UpdateResultsWindow();
            updateResultsWindow.Show();
        }

        private void ExitButtonClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            if (File.Exists("thrlist.xlsx"))
                File.Delete("thrlist.xlsx");
            dataGrid1.ItemsSource = null;
        }
    }
}

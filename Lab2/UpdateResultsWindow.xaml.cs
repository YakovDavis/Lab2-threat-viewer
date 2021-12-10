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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Lab2
{
    public partial class UpdateResultsWindow : Window
    {
        public UpdateResultsWindow()
        {
            InitializeComponent();
            bool sFlag = true;
            bool oldExists = false;
            if (File.Exists("thrlist.xlsx"))
            {
                File.Move("thrlist.xlsx", "thrlist_old.xlsx");
                oldExists = true;
            }
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile("https://bdu.fstec.ru/files/documents/thrlist.xlsx", "thrlist.xlsx");
                }
                catch (Exception e)
                {
                    textBlock.Text = "Error: " + e.ToString();
                    sFlag = false;
                }
            }
            if (sFlag && oldExists)
            {
                textBlock.Text = "Successfully updated the database!\n" + CompareFiles();
                File.Delete("thrlist_old.xlsx");
            }
            else if (sFlag && !oldExists)
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open("thrlist.xlsx", true))
                {
                    WorkbookPart workbookPart = document.WorkbookPart;
                    WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                    SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
                    List<Row> rows = new List<Row>(sheetData.Elements<Row>());
                    textBlock.Text = "Successfully downloaded the database!\n" + rows.Count + " entries added!";
                }
            }
            MainWindow.LoadFromFile();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string CompareFiles()
        {
            string res = "";
            using (SpreadsheetDocument document1 = SpreadsheetDocument.Open("thrlist.xlsx", true))
            {
                using (SpreadsheetDocument document2 = SpreadsheetDocument.Open("thrlist_old.xlsx", true))
                {
                    WorkbookPart workbookPart1 = document1.WorkbookPart;
                    WorksheetPart worksheetPart1 = workbookPart1.WorksheetParts.First();
                    SheetData sheetData1 = worksheetPart1.Worksheet.Elements<SheetData>().First();
                    WorkbookPart workbookPart2 = document2.WorkbookPart;
                    WorksheetPart worksheetPart2 = workbookPart2.WorksheetParts.First();
                    SheetData sheetData2 = worksheetPart2.Worksheet.Elements<SheetData>().First();
                    List<Row> rows1 = new List<Row>(sheetData1.Elements<Row>());
                    List<Row> rows2 = new List<Row>(sheetData2.Elements<Row>());
                    res += rows1.Count + " entries updated\n";
                    for (int i = 3; i < rows1.Count; i++)
                    {
                        if (i < rows2.Count)
                        {
                            res += CompareRows(rows1[i], workbookPart1, rows2[i], workbookPart2);
                        }
                        else
                        {
                            foreach (Cell cell in rows1[i])
                            {
                                string cellValue = "";
                                if (cell.DataType != null)
                                {
                                    if (cell.DataType == CellValues.SharedString)
                                    {
                                        int id = -1;

                                        if (Int32.TryParse(cell.InnerText, out id))
                                        {
                                            SharedStringItem item = MainWindow.GetSharedStringItemById(workbookPart1, id);

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
                                res += "<no info> - " + cellValue + "\n";
                            }
                        }
                    }
                }
            }
            return res;
        }

        private string CompareRows(Row r1, WorkbookPart workbookPart1, Row r2, WorkbookPart workbookPart2)
        {
            string res = "";
            List<string> s1 = new List<string>();
            foreach (Cell cell in r1)
            {
                string cellValue = "";
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        int id = -1;

                        if (Int32.TryParse(cell.InnerText, out id))
                        {
                            SharedStringItem item = MainWindow.GetSharedStringItemById(workbookPart1, id);

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
                s1.Add(cellValue);
            }
            List<string> s2 = new List<string>();
            foreach (Cell cell in r2)
            {
                string cellValue = "";
                if (cell.DataType != null)
                {
                    if (cell.DataType == CellValues.SharedString)
                    {
                        int id = -1;

                        if (Int32.TryParse(cell.InnerText, out id))
                        {
                            SharedStringItem item = MainWindow.GetSharedStringItemById(workbookPart2, id);

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
                s2.Add(cellValue);
            }
            for (int i = 0; i < s1.Count; i++)
            {
                if (s1[i] != s2[i])
                    res += s2[i] + " - " + s1[i] + "\n";
            }
            return res;
        }
    }
}

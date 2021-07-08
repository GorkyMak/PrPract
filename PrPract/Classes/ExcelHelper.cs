using System;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace PrPract.Classes
{
    class ExcelHelper
    {
        Excel.Application application;
        Excel.Workbook workbook;
        Excel.Worksheet sheets;
        Task CreateNewApp;
        string[] columns = new string[]
        {
            "Код акта",
            "Код показаний прибора учета",
            "Дата сдачи акта",
            "Предыдущие показания",
            "Текущие показания",
            "Дата снятия показаний",
            "Расход"
        };

        public ExcelHelper()
        {
            if (application == null)
                CreateNewApp = Task.Run(() => application = new Excel.Application());
        }

        public async void Write(DataTable dataTable, string path)
        {
            try
            {
                await Task.Run(() =>
                {
                    CreateNewApp.Wait();
                    workbook = application.Workbooks.Add();
                    sheets = workbook.ActiveSheet;

                    Parallel.Invoke(() =>
                    {
                        Parallel.For(0, dataTable.Columns.Count, (int column) =>
                        {
                            sheets.Cells[1, column + 1] = columns[column];
                            (sheets.Columns[column + 1] as Excel.Range).AutoFit();
                        });
                    },
                    () =>
                    {
                        Parallel.For(0, dataTable.Rows.Count, (int row) =>
                        {
                            var rowArray = dataTable.Rows[row].ItemArray;
                            Parallel.For(0, dataTable.Columns.Count, (int column) =>
                            {
                                sheets.Cells[row + 2, column + 1] = rowArray[column].ToString();
                            });
                        });
                    });

                    Excel.Range Range = sheets.UsedRange;
                    Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                    Range.Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;

                    Excel.ChartObjects chartObjs = (Excel.ChartObjects)sheets.ChartObjects();
                    Excel.ChartObject chartObj = chartObjs.Add(800, 10, 300, 200);
                    Excel.Chart xlChart = chartObj.Chart;

                    Excel.SeriesCollection seriesCollection = (Excel.SeriesCollection)xlChart.SeriesCollection(Type.Missing);
                    Excel.Series series = seriesCollection.NewSeries();
                    series.XValues = sheets.get_Range("C2", $"C{dataTable.Rows.Count + 1}");
                    series.Values = sheets.get_Range("G2", $"G{dataTable.Rows.Count + 1}");
                    series.Name = "Расход";

                    xlChart.ChartType = Excel.XlChartType.xl3DColumn;
                    xlChart.HasLegend = true;

                    workbook.SaveAs(path);

                    MessageBox.Show($"Файл успешно сохранён!", "Результат");
                });
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
            finally
            {
                Close();
            }
        }

        public void Close()
        {
            try
            {
                if (workbook != null)
                {
                    workbook.Close(false);
                    workbook = null;
                }
                if (application != null)
                {
                    application.Quit();
                }
            }
            catch (COMException ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
using System;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using System.Windows;
using System.Threading.Tasks;
using System.IO;
using PrPract.Database;
using System.Runtime.InteropServices;

namespace PrPract.Classes
{
    public class WordHelper
    {
        Word.Application application;
        Document doc;
        Bookmarks bookmarks;
        System.Threading.Tasks.Task CreateNewApp;
        string[] columns = new string[]
        {
            "№",
            "№ лицевого счета (№ договора энергоснабжения)",
            "Марка, тип, номер прибора учета",
            "Показания прибора учета предыдущие",
            "Показания прибора учета текущие",
            "Расчетный коэффициент",
            "Расход электрической энергии"
        };

        public WordHelper()
        {
            if (application == null)
                CreateNewApp = System.Threading.Tasks.Task.Run(() =>
                    application = new Word.Application());
        }

        public async void Write(System.Data.DataTable dataTable, string path,
            Акт_снятия_показаний_приборов_учета акт)
        {
            try
            {
                await System.Threading.Tasks.Task.Run(() =>
                {
                    CreateNewApp.Wait();

                    string source = $@"{Directory.GetCurrentDirectory()}/Шаблон отчёта.docx";
                    doc = application.Documents.Open(source);
                    doc.Activate();
                    bookmarks = doc.Bookmarks;

                    var month = DateTime.Today.ToString("MMMM");

                    bookmarks["День"].Range.Text = DateTime.Today.Day.ToString();
                    bookmarks["Месяц"].Range.Text = month[month.Length - 1] == 'ь' ?
                        month.Replace('ь', 'я') : month + "а";
                    bookmarks["Год"].Range.Text = DateTime.Today.Year.ToString();
                    bookmarks["ФИО"].Range.Text = акт.Показания_прибора_учета.Договор_энергоснабжения
                        .Абонент.Личные_данные.ToString();
                    bookmarks["Адрес"].Range.Text = акт.Показания_прибора_учета.Договор_энергоснабжения
                        .Абонент.Контактные_данные.Адрес.ToString();
                    Range range = bookmarks["Таблица"].Range;

                    var Table = doc.Tables.Add(range, dataTable.Rows.Count + 1, dataTable.Columns.Count, dataTable.Columns.Count);

                    Parallel.Invoke(() =>
                    {
                        Parallel.For(0, Table.Columns.Count, (int column) =>
                        {
                            Table.Cell(1, column + 1).Range.Text = columns[column];
                        });
                    },
                    () =>
                    {
                        Parallel.For(0, dataTable.Rows.Count, (int row) =>
                        {
                            var rowArray = dataTable.Rows[row].ItemArray;
                            Parallel.For(0, dataTable.Columns.Count, (int column) =>
                            {
                                Table.Cell(row + 2, column + 1).Range.Text = rowArray[column].ToString();
                            });
                        });
                    });

                    string format = path.Substring(path.Length - 4);

                    if (format == ".pdf")
                        doc.SaveAs2(path, WdSaveFormat.wdFormatPDF);
                    else
                        doc.SaveAs2(path);

                    MessageBox.Show("Отчёт успешно создан!", "Результат");
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
                if (doc != null)
                {
                    doc.Close(false);
                    doc = null;
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

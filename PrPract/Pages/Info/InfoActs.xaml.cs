using Microsoft.Win32;
using PrPract.Classes;
using PrPract.Database;
using PrPract.Pages.Edit;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PrPract.Pages.Info
{
    public partial class InfoActs : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        List<Акт_снятия_показаний_приборов_учета> акты, searchResult;
        const int countColumnsWord = 7;
        public InfoActs()
        {
            InitializeComponent();
            RefreshDG();
            DPMaxDate.SelectedDate = dbcontext.Акт_снятия_показаний_приборов_учета.Count() == 0 ?
                System.DateTime.Today :
                dbcontext.Акт_снятия_показаний_приборов_учета.Max(i => i.Дата_сдачи_акта);
        }

        private async void RefreshDG()
        {
            GetActs();
            SetActs();
        }

        private void GetActs()
        {
            акты = dbcontext.Акт_снятия_показаний_приборов_учета
                .AsNoTracking()
                .Include(i => i.Показания_прибора_учета)
                .ToList();
            searchResult = акты;
        }

        private void SetActs()
        {
            DGActs.ItemsSource = акты;
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditActs());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditActs(DGActs.SelectedItem as Акт_снятия_показаний_приборов_учета));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var record = DGActs.SelectedItem as Акт_снятия_показаний_приборов_учета;

            dbcontext.Entry(record).State = EntityState.Deleted;

            dbcontext.SaveChanges();

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            searchResult = акты.Where(i =>
                i.Код_акта_снятия_показаний_приборов_учета.ToString().Contains(TBSearchLine.Text) &&
                i.Дата_сдачи_акта >= DPMinDate.SelectedDate &&
                i.Дата_сдачи_акта <= DPMaxDate.SelectedDate).ToList();

            DGActs.ItemsSource = null;
            DGActs.ItemsSource = searchResult;
        }

        private DataTable GetDataTableWord(Акт_снятия_показаний_приборов_учета act)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            while (dataTable.Columns.Count < countColumnsWord)
                dataTable.Columns.Add();

            dataRow = dataTable.NewRow();

            var stat = act.Показания_прибора_учета;
            var statMeter = stat.Договор_энергоснабжения.Прибор_учёта;
            dataRow.ItemArray = new string[]
            {
                    act.Код_акта_снятия_показаний_приборов_учета.ToString(),
                    stat.Номер_договора.ToString(),
                    string.Join(", ", statMeter.Марка, statMeter.Тип, statMeter.Номер_прибора_учета),
                    stat.Предыдущие_показания.ToString(),
                    stat.Текущие_показания.ToString(),
                    statMeter.Расчетный_коэффициент.ToString(),
                    stat.Расход.ToString()
            };

            dataTable.Rows.Add(dataRow);

            return dataTable;
        }

        private DataTable GetDataTableExcel()
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            while (dataTable.Columns.Count < DGActs.Columns.Count)
                dataTable.Columns.Add();

            for (int i = 0; i < searchResult.Count; i++)
            {
                dataRow = dataTable.NewRow();

                var act = searchResult[i];
                var stat = searchResult[i].Показания_прибора_учета;

                dataRow.ItemArray = new string[]
                {
                    act.Код_акта_снятия_показаний_приборов_учета.ToString(),
                    act.Код_показаний_прибора_учета.ToString(),
                    act.Дата_сдачи_акта.ToShortDateString(),
                    stat.Предыдущие_показания.ToString(),
                    stat.Текущие_показания.ToString(),
                    stat.Дата_снятия_показаний.ToShortDateString(),
                    stat.Расход.ToString()
                };

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private void BtnOtchetWord_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var act = DGActs.SelectedItem as Акт_снятия_показаний_приборов_учета;

            var dataTable = GetDataTableWord(act);

            WordHelper wordHelper = new WordHelper();

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.Filter = "Документ Word (*.docx)|*.docx|" +
                "PDF (*.pdf)|*.pdf";

            if (savedialog.ShowDialog() == false)
            {
                wordHelper.Close();
                return;
            }

            wordHelper.Write(dataTable, savedialog.FileName, act);
        }

        private void BtnOtchetExcel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var dataTable = GetDataTableExcel();

            var excelHelper = new ExcelHelper();

            SaveFileDialog savedialog = new SaveFileDialog();
            savedialog.Title = "Сохранить файл как...";
            savedialog.Filter = "Книга Excel (*.xlsx)|*.xlsx";

            if (savedialog.ShowDialog() == false)
            {
                excelHelper.Close();
                return;
            }

            excelHelper.Write(dataTable, savedialog.FileName);
        }

        private void DGActs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnOtchetWord.IsEnabled = DGActs.SelectedItem == null ? false : true;
        }
    }
}
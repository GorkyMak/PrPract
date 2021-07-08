using PrPract.Database;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Windows.Controls;
using PrPract.Pages.Edit;
using System.Windows;
using System.Data;

namespace PrPract.Pages.Info
{
    public partial class InfoAbonent : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        List<Договор_энергоснабжения> абоненты;
        public InfoAbonent()
        {
            InitializeComponent();
            RefreshDG();
            DPMaxDate.SelectedDate = dbcontext.Договор_энергоснабжения.Count() == 0 ?
                            System.DateTime.Today :
                            dbcontext.Договор_энергоснабжения.Max(i => i.Дата_заключения_договора);
        }

        private async void RefreshDG()
        {
            GetAbonents();
            SetAbonents();
        }

        private void GetAbonents()
        {
            абоненты = dbcontext.Договор_энергоснабжения
                .AsNoTracking()
                .Include(i => i.Абонент.Данные_о_жилой_площади)
                .Include(i => i.Абонент.Контактные_данные.Адрес)
                .Include(i => i.Абонент.Личные_данные.Паспорт)
                .Include(i => i.Представитель_компании.Личные_данные.Паспорт)
                .Include(i => i.Представитель_компании.Доверенность)
                .Include(i => i.Прибор_учёта)
                .Include(i => i.Тариф)
                .ToList();
        }

        private void SetAbonents()
        {
            DGAbonent.ItemsSource = абоненты;
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAbonent());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditAbonent(DGAbonent.SelectedItem as Договор_энергоснабжения));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var record = DGAbonent.SelectedItem as Договор_энергоснабжения;

            if (dbcontext.Показания_прибора_учета.Any(i => i.Номер_договора == record.Номер_договора))
            {
                MessageBox.Show("Нельзя удалить этого абонента, так как у него есть показания со счётчиков", "Ошибка");
                return;
            }

            dbcontext.Entry(record).State = EntityState.Deleted;

            dbcontext.SaveChanges();

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DGAbonent.ItemsSource = null;
            DGAbonent.ItemsSource = абоненты.Where(i =>
                i.Номер_договора.ToString().Contains(TBSearchLine.Text) &&
                i.Дата_заключения_договора >= DPMinDate.SelectedDate &&
                i.Дата_заключения_договора <= DPMaxDate.SelectedDate).ToList();
        }

        private DataTable GetDataTableWord(List<Показания_прибора_учета> показания)
        {
            DataTable dataTable = new DataTable();
            DataRow dataRow;

            dataTable.Columns.AddRange(new DataColumn[7]);

            for (int i = 0; i < показания.Count; i++)
            {
                dataRow = dataTable.NewRow();
                dataRow.ItemArray = new string[]
                {
                };

                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }
    }
}
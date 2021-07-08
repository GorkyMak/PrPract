using PrPract.Database;
using PrPract.Pages.Edit;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Pages.Info
{
    public partial class InfoTariff : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        List<Тариф> тарифы;
        public InfoTariff()
        {
            InitializeComponent();
            RefreshDG();
        }

        private void RefreshDG()
        {
            GetTariff();
            SetTariff();
        }

        private void GetTariff()
        {
            тарифы = dbcontext.Тариф
                .AsNoTracking()
                .Include(i => i.Ставка.Select(j => j.Зона_суток))
                .ToList();
        }

        private void SetTariff()
        {
            DGTariff.ItemsSource = тарифы;
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditTariff());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditTariff(DGTariff.SelectedItem as Тариф));
        }

        private void MIDelete_Click(object sender, RoutedEventArgs e)
        {
            var record = DGTariff.SelectedItem as Тариф;

            if (dbcontext.Договор_энергоснабжения.Any(i => i.Код_тарифа == record.Код_тарифа))
            {
                MessageBox.Show("Нельзя удалить этот тариф, так как он прикреплён у определённым абонентам", "Ошибка");
                return;
            }

            while (record.Ставка.Count() > 0)
            {
                dbcontext.Entry(record.Ставка.ElementAt(0)).State =
                    EntityState.Deleted;
            }

            dbcontext.Entry(record).State =
                EntityState.Deleted;

            dbcontext.SaveChanges();

            RefreshDG();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchResult = new List<Тариф>();
            foreach (var item in SpSearch.Children)
                if (item is CheckBox checkBox && (bool)checkBox.IsChecked)
                    searchResult = searchResult.Union(тарифы
                                               .Where(i => i.Тип_учёта.Contains(checkBox.Content.ToString())))
                                               .ToList();
            if (searchResult.Count == 0)
                searchResult = тарифы;

            searchResult = searchResult.Where(i => i.Код_тарифа.ToString().Contains(TBSearchLine.Text)).ToList();

            DGTariff.ItemsSource = null;
            DGTariff.ItemsSource = searchResult;
        }
    }
}
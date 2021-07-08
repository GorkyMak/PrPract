using PrPract.Classes;
using PrPract.Database;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Pages.Edit
{
    public partial class EditTariff : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        Тариф тариф;
        Ставка ставка;
        bool hasAdd, hasAddRate;
        public EditTariff()
        {
            InitializeComponent();
            тариф = new Тариф()
            {
                Код_тарифа = dbcontext.Тариф.Count() == 0 ? 0 : dbcontext.Тариф.Max(i => i.Код_тарифа) + 1,
                Ставка = new List<Ставка>()
            };

            DataContext = тариф;
            hasAdd = true;

            dbcontext.Entry(тариф).State = hasAdd ?
                EntityState.Added :
                EntityState.Modified;
        }

        public EditTariff(Тариф тариф)
        {
            InitializeComponent();

            this.тариф = тариф;
            DataContext = this.тариф;
            hasAdd = false;
        }

        private void BtnAddEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFields(SpTariff.Children))
                return;

            try
            {
                for (int i = 0; i < тариф.Ставка.Count(); i++)
                {
                    var item = тариф.Ставка.ElementAt(i);

                    dbcontext.Entry(item.Зона_суток).State = EntityState.Unchanged;

                    if (dbcontext.Ставка.Any(j => j.Код_ставки == item.Код_ставки))
                        dbcontext.Entry(item).State = EntityState.Modified;
                    else
                        dbcontext.Entry(item).State = EntityState.Added;
                }

                dbcontext.SaveChanges();

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }

        private void BtnAddEdit_ClickSecond(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!DataChecker.CheckFieldsExcept(SpEdit.Children))
                return;

            var zoneDay = dbcontext.Зона_суток.FirstOrDefault(i => i.Код_зоны_суток.ToString() == CmbZoneDay.Text);

            ставка.Зона_суток = zoneDay;

            if (hasAddRate)
                тариф.Ставка.Add(ставка);

            SpEdit.DataContext = null;
            CmbZoneDay.ItemsSource = null;
            hasAddRate = false;
            SetVisibilityEditDel();
            RefreshDG();
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            BtnAddEditMedDel.Content = "Добавить";
            ставка = new Ставка()
            {
                Код_ставки = dbcontext.Ставка.Count() == 0 ? 0 : dbcontext.Ставка.Max(i => i.Код_ставки) + 1,
                Код_тарифа = тариф.Код_тарифа
            };
            hasAddRate = true;
            GetZoneDays();
            SetVisibilityEditMedDel();
        }
        private void SetVisibilityEditMedDel()
        {
            SpEdit.DataContext = ставка;
            SpEdit.Visibility = System.Windows.Visibility.Visible;
            DGTariff.Visibility = SpTariff.Visibility = System.Windows.Visibility.Collapsed;
            BtnAddEditDel.Visibility = System.Windows.Visibility.Collapsed;
            BtnAddEditMedDel.Visibility = System.Windows.Visibility.Visible;
        }

        private void SetVisibilityEditDel()
        {
            SpEdit.Visibility = System.Windows.Visibility.Collapsed;
            DGTariff.Visibility = SpTariff.Visibility = System.Windows.Visibility.Visible;
            BtnAddEditDel.Visibility = System.Windows.Visibility.Visible;
            BtnAddEditMedDel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void GetZoneDays()
        {
            var listRates = тариф.Ставка.Select(j => j.Код_зоны_суток);

            var zoneDays =
                dbcontext.Зона_суток
                .Where(i => !listRates.Contains(i.Код_зоны_суток))
                .Select(i => i.Код_зоны_суток)
                .ToList();

            if (!hasAddRate)
                zoneDays.Add((int)ставка.Код_зоны_суток);

            CmbZoneDay.ItemsSource = zoneDays;
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGTariff.SelectedItem == null)
                return;

            ставка = DGTariff.SelectedItem as Ставка;
            GetZoneDays();
            SetVisibilityEditMedDel();
            BtnAddEditMedDel.Content = "Изменить";
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGTariff.SelectedItem == null)
                return;

            var record = DGTariff.SelectedItem as Ставка;

            if (dbcontext.Ставка.Any(i => i.Код_ставки == record.Код_ставки))
                dbcontext.Entry(record).State = EntityState.Deleted;

            тариф.Ставка.Remove(record);
            RefreshDG();
        }

        private void RefreshDG()
        {
            DGTariff.ItemsSource = null;
            DGTariff.ItemsSource = тариф.Ставка;
        }
    }
}
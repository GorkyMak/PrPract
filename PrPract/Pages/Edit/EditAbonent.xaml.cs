using PrPract.Database;
using System.Linq;
using System.Data.Entity;
using System.Windows.Controls;
using PrPract.Classes;
using System.Collections.Generic;
using System.Windows;
using System;
using System.Text.RegularExpressions;

namespace PrPract.Pages.Edit
{
    public partial class EditAbonent : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        Договор_энергоснабжения договор;
        List<Представитель_компании> представители;
        List<Тариф> тарифы;
        bool hasAdd = false;
        public EditAbonent()
        {
            InitializeComponent();
            договор = new Договор_энергоснабжения()
            {
                Дата_заключения_договора = System.DateTime.Today,
                Абонент = new Абонент()
                {
                    Данные_о_жилой_площади = new Данные_о_жилой_площади(),
                    Личные_данные = new Личные_данные()
                    {
                        Дата_рождения = System.DateTime.Today,
                        Паспорт = new Паспорт()
                    },
                    Контактные_данные = new Контактные_данные()
                    {
                        Адрес = new Адрес()
                    }
                },
                Прибор_учёта = new Прибор_учёта()
                {
                    Дата_установки_введения_в_эксплуатацию__ = System.DateTime.Today
                }
            };

            GetCmbItems();
            DataContext = договор;
            hasAdd = true;
        }

        private void GetCmbItems()
        {
            тарифы = dbcontext.Тариф
                .AsNoTracking()
                .ToList();
            CmbTariff.ItemsSource = тарифы;

            представители = dbcontext.Представитель_компании
                .AsNoTracking()
                .ToList();
            CmbDelegate.ItemsSource = представители
                .Select(i => i.Личные_данные)
                .ToList();
        }

        public EditAbonent(Договор_энергоснабжения договор_)
        {
            InitializeComponent();
            договор = договор_;

            GetCmbItems();
            DataContext = договор;
        }

        private void BtnAddEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                Regex CorrectDateTime = new Regex(@"^[+]\d[(]\d{3}[)]\d{3}-\d{2}-\d{2}$");

                if ((TBPhone.Text != "" && !CorrectDateTime.IsMatch(TBPhone.Text)) || !CorrectDateTime.IsMatch(TBMobPhone.Text))
                {
                    MessageBox.Show("Неверный формат телефона", "Ошибка");
                    return;
                }

                if (!DataChecker.CheckFieldsExcept(Sp.Children, "TBPhone"))
                    return;

                if(TBNumAct.Text.Length != 8)
                {
                    MessageBox.Show("Длина номера договора должна быть равна 8", "Ошибка");
                    return;
                }

                if(TBNumStatMeter.Text.Length != 6)
                {
                    MessageBox.Show("Длина номера прибора учета должна быть равна 8", "Ошибка");
                    return;
                }

                if(TBSerPass.Text.Length != 4 || TBNumPass.Text.Length != 6)
                {
                    MessageBox.Show("Длина серии паспорта должна быть равна 4, а номера - 6", "Ошибка");
                    return;
                }

                foreach(var item in TBSerPass.Text)
                {
                    if(!char.IsDigit(item))
                    {
                        MessageBox.Show("В серии паспорта должны быть только числа", "Ошибка");
                        return;
                    }
                }

                foreach(var item in TBNumPass.Text)
                {
                    if(!char.IsDigit(item))
                    {
                        MessageBox.Show("В номере паспорта должны быть только числа", "Ошибка");
                        return;
                    }
                }

                договор.Прибор_учёта.Номер_прибора_учета = договор.Номер_прибора_учета;

                договор.Тариф = тарифы[CmbTariff.SelectedIndex];
                договор.Код_тарифа = договор.Тариф.Код_тарифа;

                договор.Код_представителя_компании = представители[CmbDelegate.SelectedIndex].Код_представителя_компании;
                договор.Представитель_компании= представители[CmbDelegate.SelectedIndex];

                договор.Абонент.Личные_данные.Серия_паспорта = договор.Абонент.Личные_данные.Паспорт.Серия_паспорта;
                договор.Абонент.Личные_данные.Номер_паспорта = договор.Абонент.Личные_данные.Паспорт.Номер_паспорта;

                if (TBPhone.Text == "")
                    договор.Абонент.Контактные_данные.Телефон = null;

                

                if (hasAdd)
                {
                    dbcontext.Entry(договор).State =
                    dbcontext.Entry(договор.Прибор_учёта).State =
                    dbcontext.Entry(договор.Абонент).State =
                    dbcontext.Entry(договор.Абонент.Данные_о_жилой_площади).State =
                    dbcontext.Entry(договор.Абонент.Контактные_данные).State =
                    dbcontext.Entry(договор.Абонент.Контактные_данные.Адрес).State =
                    dbcontext.Entry(договор.Абонент.Личные_данные).State =
                    dbcontext.Entry(договор.Абонент.Личные_данные.Паспорт).State =
                        EntityState.Added;
                }
                else
                {
                    dbcontext.Entry(договор.Тариф).State = EntityState.Unchanged;

                    dbcontext.Entry(договор).State = EntityState.Modified;
                    dbcontext.Entry(договор.Прибор_учёта).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент.Данные_о_жилой_площади).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент.Контактные_данные).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент.Контактные_данные.Адрес).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент.Личные_данные).State = EntityState.Modified;
                    dbcontext.Entry(договор.Абонент.Личные_данные.Паспорт).State =
                        EntityState.Modified;
                }

                dbcontext.SaveChanges();

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка");
            }
        }
    }
}
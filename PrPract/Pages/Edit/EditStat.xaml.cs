using PrPract.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Pages.Edit
{
    public partial class EditStat : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        Показания_прибора_учета показанияПрибора;
        bool hasAdd;
        public EditStat()
        {
            InitializeComponent();

            GetNumberActs();
            показанияПрибора = new Показания_прибора_учета()
            {
                Дата_снятия_показаний = System.DateTime.Today
            };
            DataContext = показанияПрибора;
            hasAdd = true;
        }

        private void GetNumberActs()
        {
            CmbAct.ItemsSource = dbcontext.Договор_энергоснабжения
                            .Select(i => i.Номер_договора)
                            .ToList();
        }

        public EditStat(Показания_прибора_учета показанияПрибора)
        {
            InitializeComponent();

            GetNumberActs();
            this.показанияПрибора = показанияПрибора;
            DataContext = this.показанияПрибора;
            hasAdd = false;
        }

        private void BtnAddEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                dbcontext.Entry(показанияПрибора).State = hasAdd ?
                    System.Data.Entity.EntityState.Added :
                    System.Data.Entity.EntityState.Modified;

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
using PrPract.Classes;
using PrPract.Database;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Pages.Edit
{
    public partial class EditActs : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        Акт_снятия_показаний_приборов_учета акт;
        bool hasAdd;
        public EditActs()
        {
            InitializeComponent();

            GetNumberActs();
            акт = new Акт_снятия_показаний_приборов_учета()
            {
                Дата_сдачи_акта = System.DateTime.Today
            };
            DataContext = акт;
            hasAdd = true;
        }

        private void GetNumberActs()
        {
            CmbAct.ItemsSource = dbcontext.Показания_прибора_учета
                .AsNoTracking()
                .Select(i => i.Код_показаний_прибора_учета)
                .ToList();
        }

        public EditActs(Акт_снятия_показаний_приборов_учета акт)
        {
            InitializeComponent();

            GetNumberActs();
            this.акт = акт;
            DataContext = this.акт;
            hasAdd = false;
        }

        private void BtnAddEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                if (!DataChecker.CheckFields(Sp.Children))
                    return;

                var stat = dbcontext.Показания_прибора_учета.Find(акт.Код_показаний_прибора_учета);

                if (stat != null)
                {
                    акт.Показания_прибора_учета = stat;
                    акт.Показания_прибора_учета1 = stat;
                }

                dbcontext.Entry(акт).State = hasAdd ?
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
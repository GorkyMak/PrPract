using PrPract.Database;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Windows.Controls;
using PrPract.Pages.Edit;
using System.Threading.Tasks;
using System.Windows;

namespace PrPract.Pages.Info
{
    public partial class InfoStat : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        List<Показания_прибора_учета> показания_Прибора_Учетаs;
        public InfoStat()
        {
            InitializeComponent();
            RefreshDG();
            DPMaxDate.SelectedDate = dbcontext.Показания_прибора_учета.Count() == 0 ?
                                System.DateTime.Today :
                                dbcontext.Показания_прибора_учета.Max(i => i.Дата_снятия_показаний);
        }

        private async void RefreshDG()
        {
            GetAbonents();
            SetAbonents();
        }

        private void GetAbonents()
        {
            показания_Прибора_Учетаs = dbcontext.Показания_прибора_учета.AsNoTracking().ToList();
        }

        private void SetAbonents()
        {
            DGStat.ItemsSource = показания_Прибора_Учетаs;
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditStat());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditStat(DGStat.SelectedItem as Показания_прибора_учета));
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            DGStat.ItemsSource = null;
            DGStat.ItemsSource = показания_Прибора_Учетаs.Where(i =>
                i.Код_показаний_прибора_учета.ToString().Contains(TBSearchLine.Text) &&
                i.Дата_снятия_показаний >= DPMinDate.SelectedDate &&
                i.Дата_снятия_показаний <= DPMaxDate.SelectedDate).ToList();
        }
    }
}
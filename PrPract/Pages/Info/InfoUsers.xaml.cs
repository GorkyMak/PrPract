using PrPract.Database;
using PrPract.Pages.Edit;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace PrPract.Pages.Info
{
    public partial class InfoUsers : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        List<Пользователь> users;
        public InfoUsers()
        {
            InitializeComponent();
            RefreshDG();
        }

        private async void RefreshDG()
        {
            GetUsers();
            SetUsers();
        }

        private void GetUsers()
        {
            users = dbcontext.Пользователь.ToList();
        }

        private void SetUsers()
        {
            DGInfoUsers.ItemsSource = users;
        }

        private void MIAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditUser());
        }

        private void MIChange_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(DGInfoUsers.SelectedItem != null)
                NavigationService.Navigate(new EditUser(DGInfoUsers.SelectedItem as Пользователь));
        }

        private void MIDelete_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DGInfoUsers.SelectedItem == null)
                return;

            var record = DGInfoUsers.SelectedItem as Пользователь;

            dbcontext.Entry(record).State = System.Data.Entity.EntityState.Deleted;

            dbcontext.SaveChanges();

            users.Remove(record);
        }

        private void BtnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var searchResult = new List<Пользователь>();
            string searchText = "";
            using (var dbcontext = new АСКУЭEntities())
            {
                foreach (var item in SpSearch.Children)
                {
                    if (item is CheckBox checkBox && (bool)checkBox.IsChecked)
                        searchResult = searchResult.Union(dbcontext.Пользователь
                                                   .Where(i => i.Роль == checkBox.Content.ToString()))
                                                   .ToList();
                    else if (item is TextBox textBox && textBox.Text != "")
                        searchText = textBox.Text.ToLower();
                }
                if (searchResult.Count == 0)
                    searchResult = users;

                searchResult = searchResult.Where(i => i.Код_пользователя.ToString().Contains(searchText) ||
                    i.Логин.ToLower().Contains(searchText) ||
                    i.Пароль.ToLower().Contains(searchText)).ToList();
            }

            DGInfoUsers.ItemsSource = searchResult;
        }
    }
}
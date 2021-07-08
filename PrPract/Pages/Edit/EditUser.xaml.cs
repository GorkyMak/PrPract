using PrPract.Database;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Pages.Edit
{
    public partial class EditUser : Page
    {
        АСКУЭEntities dbcontext = new АСКУЭEntities();
        Пользователь user;
        public EditUser()
        {
            InitializeComponent();
            BtnAddEdit.Content = "Добавить";
            DataContext = new Пользователь();
        }

        public EditUser(Пользователь user)
        {
            InitializeComponent();
            this.user = user;
            DataContext = this.user;
        }

        private void BtnAddEdit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                dbcontext.Entry((Пользователь)DataContext).State = user == null ?
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
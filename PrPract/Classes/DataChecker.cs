using PrPract.Database;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace PrPract.Classes
{
    static public class DataChecker
    {
        public static bool CheckFieldsAuth(ref string Login, ref string Password)
        {
            if (string.IsNullOrEmpty(Login))
            {
                MessageBox.Show(string.IsNullOrEmpty(Password) ?
                    "Поля не заполнены" :
                    "Логин не заполнен",
                    "Ошибка");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Пароль не заполнен", "Ошибка");
                return false;
            }

            return true;
        }

        public static bool CheckUser(Пользователь user, ref string Password)
        {
            if (user == null)
            {
                MessageBox.Show("Пользователя с таким логином не существует", "Ошибка");
                return false;
            }

            if (user.Пароль != Password)
            {
                MessageBox.Show("Неверный пароль", "Ошибка");
                return false;
            }

            return true;
        }

        public static bool CheckFields(UIElementCollection collection)
        {
            foreach (var item in collection)
                if (!CheckField(item))
                    return false;

            return true;
        }

        public static bool CheckFieldsExcept(UIElementCollection collection, params string[] exceptNames)
        {
            foreach (var item in collection)
            {
                if (Contains(GetNameObject(item), ref exceptNames))
                    continue;

                if (!CheckField(item))
                        return false;
            }

            return true;
        }

        private static string GetNameObject(object item) =>
            (item as FrameworkElement).Name;

        private static bool CheckField(object item)
        {
            if
            (
                item is TextBox textBox && (textBox.Text == "" || textBox.Text == "0") ||
                item is ComboBox comboBox && comboBox.Text == ""
            )
            {
                MessageBox.Show("Заполните поля", "Ошибка");
                return false;
            }

            return true;
        }

        private static bool Contains(string name, ref string[] exceptNames)
        {
            foreach (var i in exceptNames)
                if (name == i)
                    return true;

            return false;
        }

        public static bool CheckDataGrid(DataGrid dataGrid)
        {
            if (dataGrid.Items.Count != 0)
                return true;

            MessageBox.Show("Заполните список препаратов", "Ошибка");
            return false;
        }

        public static bool CheckFieldSearch(UIElementCollection collection)
        {
            foreach (var item in collection)
            {
                if (item is ComboBox comboBox && comboBox.Text != "" ||
                    item is TextBox textBox && textBox.Text != "" ||
                    item is CheckBox checkBox && (bool)checkBox.IsChecked)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CheckCorrectTime(params string[] times)
        {
            Regex CorrectDateTime = new Regex(@"^(\d|[0-1]?\d|2[0-3]):([0-5]\d)$");

            foreach (var item in times)
                if (!CorrectDateTime.IsMatch(item))
                    return false;

            return true;
        }
    }
}
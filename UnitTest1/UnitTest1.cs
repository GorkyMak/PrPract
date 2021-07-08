using Microsoft.VisualStudio.TestTools.UnitTesting;
using PrPract.Classes;
using PrPract.Database;
using System;
using System.Linq;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string login = "Lairil", password = "jX20W3";
            Assert.AreEqual(true, Authorization.LogIn(login, password));
        }

        [TestMethod]
        public void TestMethod2()
        {
            string searchText = "48943584";
            Assert.AreEqual(true, SearchAbonent.Search(searchText));
        }

        [TestMethod]
        public void TestMethod3()
        {
            DateTime minDate = new DateTime(2000, 1, 1), maxDate = DateTime.Today;
            Assert.AreEqual(true, FilterStat.Filter(minDate, maxDate));
        }
    }

    public static class Authorization
    {
        public static bool LogIn(string Login, string Password)
        {
            if (DataChecker.CheckFieldsAuth(ref Login, ref Password) == false)
            {
                return false;
            }

            Пользователь User = FindUser(Login);

            if (DataChecker.CheckUser(User, ref Password) == false)
            {
                return false;
            }

            return true;
        }

        public static Пользователь FindUser(string Login)
        {
            using (var dbcontext = new АСКУЭEntities())
                return dbcontext.Пользователь.FirstOrDefault(i => i.Логин == Login);
        }
    }

    public static class SearchAbonent
    {
        public static bool Search(string TBSearchLine)
        {
            using (var dbcontext = new АСКУЭEntities())
            {
                return dbcontext.Договор_энергоснабжения.Where(i =>
                i.Номер_договора.ToString().Contains(TBSearchLine)).ToList() != null;
            }
        }
    }

    public static class FilterStat
    {
        public static bool Filter(DateTime DPMinDate, DateTime DPMaxDate)
        {
            using (var dbcontext = new АСКУЭEntities())
            {
                return dbcontext.Показания_прибора_учета.Where(i =>
                i.Дата_снятия_показаний >= DPMinDate &&
                i.Дата_снятия_показаний <= DPMaxDate).ToList() != null;
            }
        }
    }
}
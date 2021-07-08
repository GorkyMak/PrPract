using System.Text;

namespace PrPract.Database
{
    public partial class Зона_суток
    {
        public override string ToString()
        {
            StringBuilder times = new StringBuilder();

            foreach (var item in Временной_период)
            {
                if (times.Length != 0)
                    times.Append(",");

                times.Append($"{item.Начало_временного_периода} - {item.Конец_временного_периода}");
            }

            return times.ToString();
        }
    }
}

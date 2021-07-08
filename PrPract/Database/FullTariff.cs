namespace PrPract.Database
{
    public partial class Тариф
    {
        public override string ToString()
        {
            return $"{Код_тарифа} {Тип_учёта} {Группа_населения}";
        }
    }
}

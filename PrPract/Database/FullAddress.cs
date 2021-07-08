namespace PrPract.Database
{
    public partial class Адрес
    {
        public override string ToString()
        {
            return string.Concat($"ул. {Улица}, д. {Дом_влад_}", Квартира != null ? $", кв. {Квартира}" : "");
        }
    }
}

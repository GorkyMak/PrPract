//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PrPract.Database
{
    using System;
    
    public partial class ПоискПоказанийАбонента_Result
    {
        public int Код_показаний_прибора_учета { get; set; }
        public int Номер_договора { get; set; }
        public int Предыдущие_показания { get; set; }
        public int Текущие_показания { get; set; }
        public System.DateTime Дата_снятия_показаний { get; set; }
        public bool Конечные_показания { get; set; }
        public Nullable<int> Расход { get; set; }
    }
}

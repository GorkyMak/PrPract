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
    using System.Collections.Generic;
    
    public partial class ПредставлениеЗоны_суток
    {
        public int Код_зоны_суток { get; set; }
        public string Название_зоны_суток { get; set; }
        public System.TimeSpan Начало_временного_периода { get; set; }
        public System.TimeSpan Конец_временного_периода { get; set; }
    }
}

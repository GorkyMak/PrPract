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
    
    public partial class Адрес
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Адрес()
        {
            this.Контактные_данные = new HashSet<Контактные_данные>();
            this.Контактные_данные1 = new HashSet<Контактные_данные>();
            this.Паспорт = new HashSet<Паспорт>();
            this.Паспорт1 = new HashSet<Паспорт>();
        }
    
        public int Код_адреса { get; set; }
        public string Улица { get; set; }
        public string Дом_влад_ { get; set; }
        public Nullable<int> Квартира { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Контактные_данные> Контактные_данные { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Контактные_данные> Контактные_данные1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Паспорт> Паспорт { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Паспорт> Паспорт1 { get; set; }
    }
}

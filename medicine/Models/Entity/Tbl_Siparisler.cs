//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace medicine.Models.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tbl_Siparisler
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tbl_Siparisler()
        {
            this.Tbl_SiparisDetay = new HashSet<Tbl_SiparisDetay>();
        }
    
        public int ID { get; set; }
        public string SIPARISADI { get; set; }
        public Nullable<System.DateTime> TARIH { get; set; }
        public Nullable<bool> DURUM { get; set; }
        public string KDV { get; set; }
        public string KAR { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tbl_SiparisDetay> Tbl_SiparisDetay { get; set; }
    }
}

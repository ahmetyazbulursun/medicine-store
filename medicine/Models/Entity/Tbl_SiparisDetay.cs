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
    
    public partial class Tbl_SiparisDetay
    {
        public int ID { get; set; }
        public Nullable<int> SIPARISID { get; set; }
        public Nullable<int> URUNID { get; set; }
        public Nullable<int> ADET { get; set; }
        public Nullable<decimal> FIYAT { get; set; }
        public string AY { get; set; }
        public string YIL { get; set; }
        public Nullable<decimal> BIRIMFIYAT { get; set; }
    
        public virtual Tbl_Urunler Tbl_Urunler { get; set; }
        public virtual Tbl_Siparisler Tbl_Siparisler { get; set; }
    }
}

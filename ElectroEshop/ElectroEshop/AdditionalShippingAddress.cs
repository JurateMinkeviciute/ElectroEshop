//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ElectroEshop
{
    using System;
    using System.Collections.Generic;
    
    public partial class AdditionalShippingAddress
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AdditionalShippingAddress()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.Orders = new HashSet<Order>();
        }
    
        public int shipping_id { get; set; }
        public string shipping_firstName { get; set; }
        public string shipping_lastName { get; set; }
        public string shipping_email { get; set; }
        public string shipping_address { get; set; }
        public string shipping_city { get; set; }
        public string shipping_country { get; set; }
        public string shipping_zipCode { get; set; }
        public string shipping_telephone { get; set; }
        public Nullable<System.DateTime> shipping_createDate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }
    }
}

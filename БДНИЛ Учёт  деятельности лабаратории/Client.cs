//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace БДНИЛ_Учёт__деятельности_лабаратории
{
    using System;
    using System.Collections.Generic;
    
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            this.Plants = new HashSet<Plants>();
        }
    
        public int IdClient { get; set; }
        public Nullable<bool> TypeOfInstitutionalUnits { get; set; }
        public Nullable<int> OrderVolume { get; set; }
        public Nullable<int> PlantId { get; set; }
        public Nullable<bool> TheProspectOfFurtherCooperation { get; set; }
        public Nullable<int> AccountClientId { get; set; }
        public int AdressId2 { get; set; }
    
        public virtual Adress Adress { get; set; }
        public virtual BankClient BankClient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Plants> Plants { get; set; }
    }
}

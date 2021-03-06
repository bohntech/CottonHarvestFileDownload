//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CottonHarvestDataTransferApp.Storage
{
    using System;
    using System.Collections.Generic;
    
    public partial class Organization
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Organization()
        {
            this.RecentFiles = new HashSet<RecentFile>();
        }
    
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string Email { get; set; }
        public System.DateTime Created { get; set; }
        public Nullable<System.DateTime> Updated { get; set; }
        public int DataSourceId { get; set; }
        public string DataSourcePartnerId { get; set; }
        public Nullable<System.DateTime> LastDownload { get; set; }
    
        public virtual DataSource DataSource { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecentFile> RecentFiles { get; set; }
    }
}

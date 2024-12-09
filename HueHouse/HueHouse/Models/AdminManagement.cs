namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AdminManagement")]
    public partial class AdminManagement
    {
        public int AdminManagementID { get; set; }

        [Required]
        [StringLength(255)]
        public string DataType { get; set; }

        public string Action { get; set; }

        public int AdminID { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Admin Admin { get; set; }
    }
}

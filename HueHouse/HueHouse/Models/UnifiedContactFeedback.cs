namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UnifiedContactFeedback")]
    public partial class UnifiedContactFeedback
    {
        [Key]
        public int ContactFeedbackID { get; set; }

        [Required]
        [StringLength(20)]
        public string ContactType { get; set; }

        public int UserID { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public virtual Users Users { get; set; }
    }
}

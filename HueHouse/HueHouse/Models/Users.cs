namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Users
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Users()
        {
            Cart = new HashSet<Cart>();
            OrderHistory = new HashSet<OrderHistory>();
            Orders = new HashSet<Orders>();
            UnifiedContactFeedback = new HashSet<UnifiedContactFeedback>();
        }

        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống.")]
        [MaxLength(255, ErrorMessage = "Họ và tên không được vượt quá 255 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Giới tính không được để trống.")]
        [MaxLength(5, ErrorMessage = "Giới tính không được vượt quá 5 ký tự.")]
        public string Sex { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm 10 chữ số.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Email không được để trống.")]
        [EmailAddress(ErrorMessage = "Định dạng email không hợp lệ.")]
        [StringLength(300, ErrorMessage = "Email không được vượt quá 300 ký tự.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        [MaxLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MaxLength(500, ErrorMessage = "Họ và tên không được vượt quá 500 ký tự.")]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Cart { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderHistory> OrderHistory { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UnifiedContactFeedback> UnifiedContactFeedback { get; set; }
    }
}

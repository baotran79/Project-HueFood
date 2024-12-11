namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class OrderViewModel
    {
        public Orders Orders { get; set; }
        public List<OrderDetails> OrderDetails { get; set; }
        public List<Cart> CartItems { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhone { get; set; }
        public string UserAddress { get; set; }
        public int TotalAmount { get; set; }
    }
}

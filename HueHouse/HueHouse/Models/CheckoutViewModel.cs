namespace HueHouse.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class CheckoutViewModel
    {
        public List<Cart> CartItems { get; set; }
        public Users Users { get; set; }
    }


}

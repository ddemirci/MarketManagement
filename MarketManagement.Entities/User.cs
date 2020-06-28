using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MarketManagement.Entities
{
    public class User
    {
        [Key]
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}

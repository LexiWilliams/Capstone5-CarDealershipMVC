using System;
using System.Collections.Generic;

namespace Capstone5_CarDealershipMVC.Models
{
    public partial class FavoriteCars
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int CarId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        public virtual AspNetUsers User { get; set; }
    }
}

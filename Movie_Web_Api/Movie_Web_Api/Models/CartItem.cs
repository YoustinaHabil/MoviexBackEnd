using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movie_Web_Api.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public Movie Movie { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public CartItem()
        {
            CreatedAt = DateTime.Now;
        }
    }
}

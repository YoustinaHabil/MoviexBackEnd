using System;
using Movie_Web_Api.Models;

namespace Movie_Web_Api.Dto
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public Movie movie { get; set; }
    }
}

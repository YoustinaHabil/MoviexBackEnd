using System;
using System.ComponentModel.DataAnnotations;

namespace Movie_Web_Api.Dto
{
    public class CartItemRequestDTO
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Quantity { get; set; }
        public int MovieId { get; set; }
    }
}

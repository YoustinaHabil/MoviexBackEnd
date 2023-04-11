using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Web_Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using Movie_Web_Api.Dto;
using AutoMapper;
using Movie_Web_Api.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Newtonsoft.Json;

namespace Movie_Web_Api.Controllers
{
    [Route("api/CartItems")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly CartItemReposatory Cartrepo;
        private readonly ILogger<CartItemsController> _logger;
        IMapper mapper;

        public CartItemsController(AppDbContext context, ILogger<CartItemsController> logger,IMapper mapper)
        {
            _db = context;
            _logger = logger;
             this.mapper = mapper;
        }

        // GET: api/CartItems
        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<CartItem>>>> GetCarts(
           // [FromQuery(Name = "phoneNumber")] string phoneNumber = "",
           // [FromQuery(Name = "User")] int Userid = 0,
          //  [FromQuery(Name = "product")] long MovieId = 0
        )
        {
            var items = await _db.CartItems
                //.Where(e => Userid == 0 || (e.User.UserId == Userid))
                //.Where(e =>MovieId == 0 || (MovieId != 0 && e.MovieId == MovieId))
                .Include(e => e.User)
                .Include(e => e.Movie)
                .ToListAsync();

            return Ok(new ApiResponseDTO
            {
                Status = (int)HttpStatusCode.OK,
                Success = items.Count != 0,
                Message = items.Count == 0 ? "No cart item found." : "Found.",
                Data = items
            });
        }

        // GET: api/CartItems/user/3
        //[Authorize(Roles = "User")]
        [HttpGet("user/{id}")]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<CartItemDTO>>>> GetUserCart(string id)
        {
            var items = await _db.CartItems
                .Where(e => e.UserId == id)
                .Select(e => new CartItemDTO
                {
                    CartItemId = e.CartItemId,
                    Quantity = e.Quantity,
                    movie = e.Movie
                })
                .ToListAsync();

            return Ok(new ApiResponseDTO
            {
                Status = (int)HttpStatusCode.OK,
                Success = items.Count != 0,
                Message = items.Count == 0 ? "No cart item found." : "Found.",
                Data = items
            });
        }
        [HttpGet("Cart/{id}")]
        public async Task<ActionResult<ApiResponseDTO<IEnumerable<CartItemDTO>>>> GetCartByID(int id)
        {

            var cart = _db.CartItems.FirstOrDefault(e => e.CartItemId == id);
            var jsonData = new { Data = cart };
            var json = JsonConvert.SerializeObject(jsonData);
            if (cart == null)
            {
                return NotFound();
            }
            return Content(json, "application/json");
        }
        [HttpPost]
        public async Task<ActionResult> PostCartItem( CartItemRequestDTO cartItem)
        {
            try
            {
                await ValidateRequestBody(cartItem);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponseDTO
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Success = false,
                    Message = ex.Message,
                    Data = cartItem
                });
            }

            CartItem item = await GetDbItemWithUser(cartItem);

            if (item != null)
            {
                _db.CartItems.Update(item);
                item.Quantity += cartItem.Quantity;
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Cart item quantity increased for user {cartItem.UserId}");

                return CreatedAtAction(nameof(GetUserCart), new { id = item.UserId }, new
                {
                    status = HttpStatusCode.Created,
                    success = true,
                    message = "Product added to cart successfully",
                    data = item
                });
            }
            else
            {
               
                var cart = mapper.Map<CartItem>(cartItem);
                await _db.CartItems.AddAsync(cart);
                await _db.SaveChangesAsync();
                return Ok("Created");




                await _db.SaveChangesAsync();
                _logger.LogInformation($"New cart item created for user {cartItem.UserId}");

                return CreatedAtAction(nameof(GetUserCart), new { id = cartItem.UserId }, new
                {
                    status = HttpStatusCode.Created,
                    success = true,
                    message = "Product added to cart successfully",
                    data = cartItem
                });
            }
        }

        // DELETE: api/CartItems
        [HttpDelete]
        public async Task<ActionResult<ApiResponseDTO>> DeleteCartItem([FromBody] CartItemRequestDTO cartItem)
        {
            CartItem item = await GetDbItem(cartItem);

            if (item == null)
            {
                return NotFound(new ApiResponseDTO
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Success = false,
                    Message = "Cart item not found",
                    Data = cartItem
                });
            }

            item.Quantity -= cartItem.Quantity;

            if (item.Quantity >= 1)
            {
                _db.Entry(item).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Product {cartItem.UserId} removed in user {cartItem.UserId}'s cart");
            }
            else
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Product {cartItem.UserId} removed in user {cartItem.UserId}'s cart");
            }

            return Ok(new ApiResponseDTO
            {
                Status = (int) HttpStatusCode.OK,
                Success = true,
                Message = "Product deleted successfully from cart",
                Data = (Object)null
            });
        }

        private bool CartItemExists(long id)
        {
            return _db.CartItems.Any(e => e.CartItemId == id);
        }

        private async Task<CartItem> GetDbItem(CartItemRequestDTO cartItem)
        {
            return await _db.CartItems
               .Where(e => e.UserId == cartItem.UserId && e.MovieId == cartItem.MovieId)
               .FirstOrDefaultAsync();
            }

            private async Task<CartItem> GetDbItemWithUser(CartItemRequestDTO cartItem)
        {
            return await _db.CartItems
                .Where(e => e.UserId == cartItem.UserId && e.MovieId == cartItem.MovieId)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        private async Task ValidateRequestBody(CartItemRequestDTO cartItem)
        {
            if (cartItem.Quantity <= 0)
            {
                throw new ArgumentException("Invalid product quantity.");
            }

            Movie product = await _db.Movies.FindAsync(cartItem.MovieId);

            if (product == null)
            {
                throw new ArgumentException("Invalid product.");
            }

            ApplicationUser user = await _db.Users.FindAsync(cartItem.UserId);

            if (user == null)
            {
                throw new ArgumentException("Invalid user.");
            }
        }
    }
}

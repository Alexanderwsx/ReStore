using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        public StoreContext _context { get; }
        public BasketController(StoreContext context)
        {
            this._context = context;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) return NotFound();
            return basket.MapBasketToDto();
        }




        [HttpPost] //api/basket?productId=1&quantity=5
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int ProductId, int quantity)
        {
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) basket = CreateBasket();

            var product = await _context.Products.FindAsync(ProductId);
            if (product == null) return BadRequest(new ProblemDetails { Title = "Product not found" });

            //additemm
            basket.AddItem(product, quantity);

            //save changes
            var result = await _context.SaveChangesAsync() > 0;

            //good
            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

            //changes not saved
            return BadRequest(new ProblemDetails { Title = "Problem adding item to basket" });
        }



        [HttpDelete]
        public async Task<ActionResult> RemoveBasketItem(int productId, int quantity)
        {
            //get basket
            var basket = await RetrieveBasket(GetBuyerId());
            if (basket == null) return NotFound();

            //remove item
            basket.RemoveItem(productId, quantity);
            //save changes
            var result = await _context.SaveChangesAsync() > 0;
            if (result) return Ok();
            return BadRequest(new ProblemDetails { Title = "Problem removing item from basket" });
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            //si on a pas de buyerid ca veut dire qu'on a pas un cookie basket
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _context.Baskets
            .Include(i => i.Items)
            .ThenInclude(p => p.Product)
            .FirstOrDefaultAsync(x => x.BuyerId == buyerId);
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }
        private Basket CreateBasket()
        {
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions
                {
                    IsEssential = true,
                    Expires = DateTime.Now.AddDays(30),
                };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }

            var basket = new Basket
            {
                BuyerId = buyerId
            };
            _context.Baskets.Add(basket);
            return basket;

        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public OrderDetailsCart DetailCart { get; set; }

        public void OnGet()
        {
            DetailCart = new OrderDetailsCart()
            {
                OrderHeader = new OrderHeader()
            };

            DetailCart.OrderHeader.OrderTotal = 0;


            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            var cart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value);

            if (cart != null)
            {
                DetailCart.ListCart = cart.ToList();
            }

            foreach (var list in DetailCart.ListCart)
            {
                list.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == list.MenuItemId);
                DetailCart.OrderHeader.OrderTotal = DetailCart.OrderHeader.OrderTotal + (list.MenuItem.Price * list.Count);
                if (list.MenuItem.Description.Length > 100)
                {
                    list.MenuItem.Description = list.MenuItem.Description.Substring(0, 99) + "...";
                }
            }

            DetailCart.OrderHeader.PickUpTime = DateTime.Now;
        }

        public IActionResult OnPostPlus(int cartId)
        {
            var cart = _db.ShoppingCart.Where(c => c.Id == cartId).FirstOrDefault();
            cart.Count += 1;
            _db.SaveChanges();
            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPostMinus(int cartId)
        {
            var cart = _db.ShoppingCart.Where(c => c.Id == cartId).FirstOrDefault();

            if (cart.Count == 1)
            {
                _db.ShoppingCart.Remove(cart);
                _db.SaveChanges();
                var cnt = _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count;
                HttpContext.Session.SetInt32("CartCount", _db.ShoppingCart.Where(u => u.ApplicationUserId == cart.ApplicationUserId).ToList().Count);
            }
            else
            {
                cart.Count -= 1;
                _db.SaveChanges();
            }

            return RedirectToPage("/Cart/Index");
        }

        public IActionResult OnPost()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            DetailCart.ListCart = _db.ShoppingCart.Where(c => c.ApplicationUserId == claim.Value).ToList();

            OrderHeader orderHeader = DetailCart.OrderHeader;
            DetailCart.OrderHeader.OrderDate = DateTime.Now;
            DetailCart.OrderHeader.UserId = claim.Value;
            DetailCart.OrderHeader.Status = SD.StatusSubmitted;
            _db.OrderHeader.Add(orderHeader);
            _db.SaveChanges();

            foreach (var item in DetailCart.ListCart)
            {
                item.MenuItem = _db.MenuItem.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrderDetail orderDetails = new OrderDetail
                {
                    MenuItemId = item.MenuItemId,
                    OrderId = orderHeader.Id,
                    Name = item.MenuItem.Name,
                    Description = item.MenuItem.Description,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                _db.OrderDetail.Add(orderDetails);
            }
            _db.ShoppingCart.RemoveRange(DetailCart.ListCart);
            HttpContext.Session.SetInt32("CartCount", 0);
            _db.SaveChanges();
            return RedirectToPage("../Order/OrderConfirmation", new { id = orderHeader.Id } );
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TasteRestaurant.Data;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.Order
{
    public class OrderHistoryModel : PageModel
    {
        private ApplicationDbContext _db;

        public OrderHistoryModel(ApplicationDbContext db)
        {
            _db = db;
        }

        [BindProperty]
        public List<OrderDetailsViewModel> OrderDetailsViewModel { get; set; }

        //Se id = 0 exibir apenas 5 pedidos anteriores mais exibem todos os pedidos
        public void OnGet(int id = 0)
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsViewModel = new List<ViewModel.OrderDetailsViewModel>();

            List<OrderHeader> OrderHeaderList = _db.OrderHeader.Where(u => u.UserId == claim.Value).OrderByDescending(u => u.OrderDate).ToList();

            if (id == 0 && OrderHeaderList.Count > 4)
            {
                OrderHeaderList = OrderHeaderList.Take(5).ToList();
            }

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel Individual = new ViewModel.OrderDetailsViewModel();
                Individual.OrderHeader = item;
                Individual.OrderDetail = _db.OrderDetail.Where(o => o.Id == item.Id).ToList();

                OrderDetailsViewModel.Add(Individual);
            }
        }
    }
}
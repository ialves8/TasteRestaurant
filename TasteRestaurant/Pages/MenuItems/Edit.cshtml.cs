using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TasteRestaurant.Data;
using TasteRestaurant.Utility;
using TasteRestaurant.ViewModel;

namespace TasteRestaurant.Pages.MenuItems
{
    [Authorize(Roles = SD.AdminAndUser)]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _db;
        private readonly IHostingEnvironment _hostingEnvironment;

        public EditModel(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public MenuItemViewModel MenuItemViewModel { get; set; }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MenuItemViewModel = new MenuItemViewModel()
            {
                MenuItem = _db.MenuItem.Include(m => m.CategoryType).Include(m => m.FoodType).SingleOrDefault(m => m.Id == id),
                CategoryType = _db.CategoryType.ToList(),
                FoodType = _db.FoodType.ToList()
            };

            if (MenuItemViewModel.MenuItem == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string webRootPath = _hostingEnvironment.WebRootPath;
            var files = HttpContext.Request.Form.Files;
            var MenuItemFromDb = _db.MenuItem.Where(m => m.Id == MenuItemViewModel.MenuItem.Id).FirstOrDefault();

            if (files[0] != null && files[0].Length > 0)
            {
                var uploads = Path.Combine(webRootPath, "images");
                var extension = MenuItemFromDb.Image.Substring(MenuItemFromDb.Image.LastIndexOf("."), MenuItemFromDb.Image.Length - MenuItemFromDb.Image.LastIndexOf("."));

                if (System.IO.File.Exists(Path.Combine(uploads, MenuItemViewModel.MenuItem.Id + extension)))
                {
                    System.IO.File.Delete(Path.Combine(uploads, MenuItemViewModel.MenuItem.Id + extension));
                }

                extension = files[0].FileName.Substring(files[0].FileName.LastIndexOf("."), files[0].FileName.Length - files[0].FileName.LastIndexOf("."));

                using (var fileStream = new FileStream(Path.Combine(uploads, MenuItemViewModel.MenuItem.Id + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                MenuItemViewModel.MenuItem.Image = @"\images\" + MenuItemViewModel.MenuItem.Id + extension;
            }

            if (MenuItemViewModel.MenuItem.Image != null)
            {
                MenuItemFromDb.Image = MenuItemViewModel.MenuItem.Image;
            }
            MenuItemFromDb.Name = MenuItemViewModel.MenuItem.Name;
            MenuItemFromDb.Description = MenuItemViewModel.MenuItem.Description;
            MenuItemFromDb.Price = MenuItemViewModel.MenuItem.Price;
            MenuItemFromDb.Spicyness = MenuItemViewModel.MenuItem.Spicyness;
            MenuItemFromDb.FoodTypeId = MenuItemViewModel.MenuItem.FoodTypeId;
            MenuItemFromDb.CategoryId = MenuItemViewModel.MenuItem.CategoryId;

            await _db.SaveChangesAsync();

            return RedirectToPage("./Index");
        } 
    }
}
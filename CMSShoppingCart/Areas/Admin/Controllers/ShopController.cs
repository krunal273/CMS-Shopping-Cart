using CMSShoppingCart.Models.Data;
using CMSShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSShoppingCart.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            // Declare the list
            List<CategoryVM> categoryListVM;
             
            using (Db db = new Db())
            {
                // init the list
                categoryListVM = db.Categories
                                .ToArray()
                                .OrderBy(x => x.Sorting)
                                .Select(x => new CategoryVM(x))
                                .ToList();
            }
            // return the view with list

            return View(categoryListVM);
        }
    }
}
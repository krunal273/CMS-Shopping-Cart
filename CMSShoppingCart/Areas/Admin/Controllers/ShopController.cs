using CMSShoppingCart.Models.Data;
using CMSShoppingCart.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

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

        // POST : Admin/Shop/AddNewCategory
        [HttpPost]
        public string AddNewCategory(string catName)
        {
            // Declare the id
            string id;

            using (Db db = new Db())
            {

                // check that the category  name is unique
                if (db.Categories.Any(x => x.Name == catName))
                    return "titletaken";

                // Init DTO
                CategoryDTO dto = new CategoryDTO();

                // Add to DTO
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;

                // Save to DTO
                db.Categories.Add(dto);
                db.SaveChanges();

                // Get the id
                id = dto.Id.ToString();

            }

            // Return id
            return id;
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;

                // Declare categoryDTO
                CategoryDTO categoryDTO;

                // Set sorting for each page
                foreach (var categoryId in id)
                {
                    categoryDTO = db.Categories.Find(categoryId);
                    categoryDTO.Sorting = count;
                    db.SaveChanges();
                    count++;
                }

            }
        }

        // GET: Admin/Shop/DeleteCategory/id 
        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            using (Db db = new Db())
            {
                // Get the page 
                CategoryDTO categoryDTO = db.Categories.Find(id);

                // remove the page
                db.Categories.Remove(categoryDTO);

                // save
                db.SaveChanges();
            }

            // redirect
            return RedirectToAction("Categories");
        }

        // POST : Admin/Shop/AddNewCategory
        [HttpPost]
        public string RenameCategory(string newCatName, int id)
        {

            using (Db db = new Db())
            {

                // check that the category  name is unique
                if (db.Categories.Any(x => x.Name == newCatName))
                    return "titletaken";

                // Get DTO
                CategoryDTO dto = db.Categories.Find(id);

                // Edit DTO
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();

                // Save to DTO
                db.SaveChanges();

                // Get the id
                return dto.Id.ToString();

            }
        }

        // GET: Admin/Shop/AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            //Init Model
            ProductVM model = new ProductVM();

            // add select list of categories to model
            using (Db db = new Db())
            {
                model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            // return view with model
            return View(model);
        }

        // POST : Admin/Shop/AddProduct
        [HttpPost]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Make sure product name is unique

            // Declare product id

            // Init and save productDTO

            // Get inserted id

            // set Tempdata message

            #region upload Image


            #endregion


            // Redirect 

            return View();
        }
    }
}
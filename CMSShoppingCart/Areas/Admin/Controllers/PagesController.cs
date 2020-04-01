using CMSShoppingCart.Models.Data;
using CMSShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMSShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            // Declare list of PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                // Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }


            //Return view with list
            return View(pagesList);
        }

        // GET: Admin/Pages/AddPages
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        // POST: Admin/Pages/AddPages
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            // Check model state

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // Declare slug
                string slug;

                // Init PageDTO
                PageDTO dto = new PageDTO();

                // DTO title
                dto.Title = model.Title;

                // Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }


                // Make sure title and slug are unique 

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == slug))
                {
                    // To add a custom validation error 
                    ModelState.AddModelError("", "Title or Slug is alredy exist.");
                    return View(model); // If I don't Write this It will save data to database. In case of error return view to User.
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            // set Tempdata message
            TempData["SM"] = "You have added a new page.";

            // Ridect
            return RedirectToAction("AddPage");
        }

        // GET: Admin/Pages/EditPage/id 
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // Declare pageVM
            PageVM model;

            using (Db db = new Db())
            {
                // Get the page
                PageDTO pageDTO = db.Pages.Find(id);

                // confirm page exists
                if (pageDTO == null)
                {
                    // return a string
                    return Content("Page does not exist");
                }

                // Init pageVM
                model = new PageVM(pageDTO);

            }
            // Return view with model 
            return View(model);
        }

        // POST: Admin/Pages/EditPage/id 
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            // check model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                // get page id
                int id = model.Id;

                // declare the slug
                string slug = "home";

                // get the page
                PageDTO dto = db.Pages.Find(id);

                // DTO the title
                dto.Title = model.Title;

                // check the slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }


                // Make sure title and slug are unique

                //? db.Pages.Where(x => x.Id != id) select all row except the current row that you are trying to change.

                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) ||
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug alredy exist");
                    return View(model);
                }

                // DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                // save DTO
                db.SaveChanges();
            }

            // set TempData message
            TempData["SM"] = "You have edited the page";

            // Redirect 

            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails/id 
        [HttpGet]
        public ActionResult PageDetails(int id)
        {
            // Declare pageVM
            PageVM model;

            using (Db db = new Db())
            {
                // Get the page
                PageDTO pageDTO = db.Pages.Find(id);

                // confirm page exists
                if (pageDTO == null)
                {
                    // return a string
                    return Content("Page does not exist");
                }

                // Init pageVM
                model = new PageVM(pageDTO);

            }
            // Return view with model 
            return View(model);
        }

        // GET: Admin/Pages/DeletePage/id 
        [HttpGet]
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                // Get the page 
                PageDTO pageDTO = db.Pages.Find(id);

                // remove the page
                db.Pages.Remove(pageDTO);

                // save
                db.SaveChanges();
            }

            // redirect
            return RedirectToAction("Index");
        }

        // POST: Admin/Pages/ReorderPages
        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (Db db = new Db())
            {
                // set initial count
                int count = 1;

                // Declare PageDTO
                PageDTO pageDTO;

                // Set sorting for each page
                foreach (var pageId in id)
                {
                    pageDTO = db.Pages.Find(pageId);
                    pageDTO.Sorting = count;
                    db.SaveChanges();
                    count++;
                }

            }
        }

        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            // Declare model
            SidebarVM model;

            using (Db db = new Db())
            {
                // get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // Init Model
                model = new SidebarVM(dto);

            }

            // Return view with model 
            return View(model);
        }

        // POST: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {

            using (Db db = new Db())
            {
                // Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);

                // DTO the Body
                dto.Body = model.Body;

                // Save
                db.SaveChanges();
            }

            // Set TempData Message
            TempData["SM"] = "You have edited a sidebar";

            // Redirect
            return RedirectToAction("EditSidebar");
        }
    }
}
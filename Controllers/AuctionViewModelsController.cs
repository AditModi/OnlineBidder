using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using practice.Models;
using System.IO;
using System.Security.Permissions;
using System.Web.Security;

namespace practice.Controllers
{
    public class AuctionViewModelsController : Controller
    {
        private AuctionViewModelDBContext db = new AuctionViewModelDBContext();
        int a=0;
        // GET: AuctionViewModels
       // [Authorize]
        public ActionResult Index(string searchString)
        {
            var ProductName = from c in db.AuctionViewModels
                          select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                //courses = courses.Where(x => x.CourseName == searchString);
                ProductName = ProductName.Where(x => x.ProductName.Contains(searchString));
            }
            return View(ProductName);
        }

        // Advanced Searching
        public ActionResult Index2(string searchString, int Duration)
        {
            var ProductName = from c in db.AuctionViewModels
                              select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                //courses = courses.Where(x => x.CourseName == searchString);
                ProductName = ProductName.Where(x => x.ProductName.Contains(searchString));
            }
            
            if ((Duration)!=0)
            {
                ProductName = ProductName.Where(x => x.BidPrice == Duration);
            }
            return View(ProductName);
        }


        

       

        // GET: AuctionViewModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisterViewModel rb = new RegisterViewModel();
            ViewBag.username = rb.UserName;
            AuctionViewModel auctionViewModel = db.AuctionViewModels.Find(id);
            if (auctionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(auctionViewModel);
        }

        // GET: AuctionViewModels/Create
       
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuctionViewModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "ProductId,ProductName,ProductPrice,ImagePath,ImageFile,ProductInfo,BidPrice")] AuctionViewModel auctionViewModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(auctionViewModel.ImageFile.FileName);
            string extension = Path.GetExtension(auctionViewModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            auctionViewModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            auctionViewModel.ProductId =a + 1;
            auctionViewModel.ImageFile.SaveAs(fileName);
            if (ModelState.IsValid)
            {
                db.AuctionViewModels.Add(auctionViewModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(auctionViewModel);
        }

        // GET: AuctionViewModels/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionViewModel auctionViewModel = db.AuctionViewModels.Find(id);
            if (auctionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(auctionViewModel);
        }

        // POST: AuctionViewModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize (Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ProductId,ProductName,ProductPrice,ImagePath,ProductInfo,BidPrice")] AuctionViewModel auctionViewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auctionViewModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auctionViewModel);
        }

        // GET: AuctionViewModels/Delete/5
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionViewModel auctionViewModel = db.AuctionViewModels.Find(id);
            if (auctionViewModel == null)
            {
                return HttpNotFound();
            }
            return View(auctionViewModel);
        }

        // POST: AuctionViewModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            AuctionViewModel auctionViewModel = db.AuctionViewModels.Find(id);
            db.AuctionViewModels.Remove(auctionViewModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

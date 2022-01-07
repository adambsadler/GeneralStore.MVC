using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GeneralStore.MVC.Controllers
{
    public class TransactionController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();
        // GET: Transaction
        public ActionResult Index()
        {
            List<Transaction> transactionList = _db.Transactions.ToList();
            List<Transaction> orderedList = transactionList.OrderBy(trans => trans.DateOfTransaction).ToList();
            return View(orderedList);
        }

        // GET: Transaction
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(_db.Customers, "CustomerID", "FullName");
            ViewBag.ProductID = new SelectList(_db.Products, "ProductId", "Name");
            return View();
        }

        // POST: Create
        // Transaction/Create
        [HttpPost]
        public ActionResult Create(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Product product = _db.Products.Find(transaction.ProductID);
                if(transaction.ItemCount <= product.InventoryCount)
                {
                    _db.Transactions.Add(transaction);
                    product.InventoryCount -= transaction.ItemCount;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            // Still need to figure out how to tell the user that there isn't enough inventory
            return View(transaction);
        }

        // GET: Delete
        // Transaction/Delete/{id}
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Transaction transaction = _db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Delete
        // Transaction/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Transaction transaction = _db.Transactions.Find(id);
            Product product = _db.Products.Find(transaction.ProductID);
            _db.Transactions.Remove(transaction);
            product.InventoryCount += transaction.ItemCount;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
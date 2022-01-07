using GeneralStore.MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        [ValidateAntiForgeryToken]
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
                else
                {
                    ModelState.AddModelError("ItemCount", "There is not enough inventory to complete this transaction.");
                    
                }
            }
            ViewBag.Customers = new SelectList(_db.Customers.OrderBy(c => c.FirstName).ToList(), "CustomerID", "FullName");
            ViewBag.Products = new SelectList(_db.Products.OrderBy(p => p.Name).ToList(), "ProductId", "Name");
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

        // GET: Edit
        // Transaction/Edit/{id}
        public ActionResult Edit(int? id)
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
            ViewBag.Customers = new SelectList(_db.Customers.OrderBy(c => c.FirstName).ToList(), "CustomerID", "FullName");
            ViewBag.Products = new SelectList(_db.Products.OrderBy(p => p.Name).ToList(), "ProductId", "Name");
            return View(transaction);
        }

        // POST: Edit
        // Transaction/Edit/{id}
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var tempTransaction = _db.Transactions.Find(transaction.OrderID);
                _db.Entry(tempTransaction).State = System.Data.Entity.EntityState.Detached;
                int productCountBefore = tempTransaction.ItemCount;
                var product = _db.Products.Find(transaction.ProductID);

                if (transaction.ItemCount <= (productCountBefore + product.InventoryCount))
                {
                    _db.Entry(transaction).State = EntityState.Modified;
                    product.InventoryCount += productCountBefore;
                    product.InventoryCount -= transaction.ItemCount;
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            ViewBag.Customers = new SelectList(_db.Customers.OrderBy(c => c.FirstName).ToList(), "CustomerID", "FullName");
            ViewBag.Products = new SelectList(_db.Products.OrderBy(p => p.Name).ToList(), "ProductId", "Name");
            return View(transaction);
        }

        // GET: Details
        // Transaction/Details/{id}
        public ActionResult Details(int? id)
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
    }
}
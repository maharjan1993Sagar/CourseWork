using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using coursework02.Models;
using coursework02.DAL;
using coursework02.ViewModels;

namespace coursework02.Controllers
{
    public class LoansController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Loans
        public ActionResult Index()
        {
            var loans = db.Loans.Include(l => l.Album).Include(l => l.LoanTypes).Include(l => l.Members);
            return View(loans.ToList());
        }

        // GET: Loans/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        //6
        // GET: Loans/Create
        public ActionResult Create()
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "id", "Name");
            List<DropDownItem> loanTypes = (from l in db.LoanTypes
                                            select new DropDownItem
                                            {
                                                Name = l.Description + ",Days " + l.Days,
                                                Id = l.Id
                                            }).ToList();
            ViewBag.LoanTypeId = new SelectList(loanTypes, "Id", "Name");
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName");
            return View();
        }

        // POST: Loans/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Loan loan)
        {
            ViewBag.AlbumId = new SelectList(db.Albums, "id", "Name", loan.AlbumId);
            List<DropDownItem> loanTypes = (from l in db.LoanTypes
                                            select new DropDownItem
                                            {
                                                Name = l.Description + "," + l.Days,
                                                Id = l.Id
                                            }).ToList();
            ViewBag.LoanTypeId = new SelectList(loanTypes, "Id", "Name", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", loan.MemberId);
            if (ModelState.IsValid)
            {
                Albums album = db.Albums.Find(loan.AlbumId);
                Member member = db.Members.Include(m=>m.MemberCatagories).FirstOrDefault(m=>m.Id==loan.MemberId);
                int age =(int)(DateTime.Now - member.DateOfBirth).TotalDays/325;
                int maxDVD = member.MemberCatagories.TotalLoan;
                int loanDVD = db.Loans.Any(m => m.MemberId == loan.MemberId && m.ReturnedDate == null) ?
                            db.Loans.Where(m => m.MemberId == loan.MemberId && m.ReturnedDate == null).Count() :
                            0;



                if (loanDVD > maxDVD)
                {
                    ModelState.AddModelError("", "Max Limit Accessed.");
                    return View(loan);
                }

                if ((album.IsAgeBar && age > 18)|| !album.IsAgeBar)
                {
                    loan.TotalAmount = db.LoanTypes.Find(loan.LoanTypeId).Days * db.Albums.Find(loan.AlbumId).FinePerDay;
                    loan.ReturnedDate = null;
                    db.Loans.Add(loan);
                    db.SaveChanges();
                    TempData["Message"] = "Total Amount for Album is " + loan.TotalAmount;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Age Restricted");
                    return View(loan);
                }
               
            }

            
            return View(loan);
        }

        //7
        public ActionResult Return(int? id)
        
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Include(m=>m.Album).FirstOrDefault(m=>m.Id==id.Value);
            if (loan == null)
            {
                return HttpNotFound();
            }
            if (loan.ReturnedDate != null)
            {
                return RedirectToAction("Index");
            }
            string FineAmount = "";
            if (loan.DueDate < DateTime.Now)
            {
                loan.FineAmount = (int)(DateTime.Now - loan.DueDate).TotalDays  * loan.Album.FinePerDay;
                FineAmount = loan.FineAmount.ToString() + " has been Fined.";
            }
            else
            {
                FineAmount = "No Fine Amount.";
            }

            loan.ReturnedDate = DateTime.Now;
            db.SaveChanges();

            TempData["Message"] = "Returned Successfully. "+FineAmount;

            return RedirectToAction("Index");
        }
        
        

        // GET: Loans/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "id", "Name", loan.AlbumId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "Id", "Description", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", loan.MemberId);
            return View(loan);
        }


        


        // POST: Loans/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MemberId,AlbumId,LoanTypeId,IssuedDate,ReturnDate")] Loan loan)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loan).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumId = new SelectList(db.Albums, "id", "Name", loan.AlbumId);
            ViewBag.LoanTypeId = new SelectList(db.LoanTypes, "Id", "Description", loan.LoanTypeId);
            ViewBag.MemberId = new SelectList(db.Members, "Id", "FullName", loan.MemberId);
            return View(loan);
        }

        // GET: Loans/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Loan loan = db.Loans.Find(id);
            if (loan == null)
            {
                return HttpNotFound();
            }
            return View(loan);
        }

        // POST: Loans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Loan loan = db.Loans.Find(id);
            db.Loans.Remove(loan);
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

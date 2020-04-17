using coursework02.DAL;
using coursework02.Models;
using coursework02.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;

namespace coursework02.Controllers
{
    public class SearchController : Controller
    {
        private DataContext db = new DataContext();

        // 1
        public ActionResult GetAlbumByArtist(string name)
        {
            IQueryable<Albums> albums = db.Artists.SelectMany(m => m.Album);
            if (!string.IsNullOrEmpty(name))
            {
                albums = db.Artists.Where(m => m.Name.Contains(name.Trim())).SelectMany(m => m.Album);
            }
            return View(albums);

        }
        //2
        public ActionResult GetAlbumOnShelves(string name)
        {
            IQueryable<Albums> albums = db.Artists.SelectMany(m => m.Album);
            if (!string.IsNullOrEmpty(name))
            {
                albums = db.Artists.Where(m => m.Name.Contains(name.Trim())).SelectMany(m => m.Album);
            }
            return View(albums);

        }

        //3
        [HttpGet]
        public ActionResult GetLoansByMember()
        {
            IEnumerable<Member> members = db.Members.AsEnumerable();
            List<DropDownItem> memItems = (from m in members
                                           select new DropDownItem
                                           {
                                               Name = m.FullName + " , " + m.Contact,
                                               Id = m.Id
                                           }).ToList();
            ViewBag.MemberId = new SelectList(memItems, "Id", "Name");

            return View();

        }

        [HttpPost]
        public ActionResult GetLoansByMember(int? MemberId)
        {
            IEnumerable<Member> members = db.Members.AsEnumerable();
            List<DropDownItem> memItems = (from m in members
                                           select new DropDownItem
                                           {
                                               Name = m.FullName + " , " + m.Contact,
                                               Id = m.Id
                                           }).ToList();
            ViewBag.MemberId = new SelectList(memItems, "Id", "Name");

            if (MemberId != null)
            {
                IQueryable<Albums> albums = db.Loans.Where(m => m.MemberId == MemberId.Value).Select(m => m.Album);
                return View(albums);
            }

            return View();

        }

        //4
        public ActionResult GetAlbumsAll()
        {
            var albums = db.Albums.Include(m => m.Artists).Include(m => m.Producers).OrderBy(m => m.ReleaseDate).ToList();
            return View(albums);
        }

        //5
        public ActionResult GetByCopyNumber()
        {
            IEnumerable<Albums> albums = db.Loans.Include(m => m.Album).Select(m => m.Album);
            ViewBag.Id = new SelectList(albums, "Id", "CopyNumber");
            return View();
        }

        [HttpPost]
        public ActionResult GetByCopyNumber(int? Id)
        {
            ViewBag.Id = new SelectList(db.Albums.ToList(), "Id", "CopyNumber");

            if (Id != null)
            {
                Loan loan = db.Loans.OrderByDescending(m => m.IssuedDate).FirstOrDefault();
                return View(loan);
            }
            ViewBag.Error = "Error";
            return View();

        }

        //8
        public ActionResult GetMemberDetail()
        {
            IEnumerable<MemberLoanDetails> loans = (from l in db.Members.Include(m => m.MemberCatagories).ToList()
                                                    select new MemberLoanDetails
                                                    {
                                                        MemberName = l.FullName,
                                                        TotalLoan = db.Loans.Any(m => m.MemberId == l.Id && m.ReturnedDate == null) ?
                                                        db.Loans.Where(m => m.MemberId == l.Id && m.ReturnedDate == null).ToList().Count() : 0,
                                                        MaxLoan = l.MemberCatagories.TotalLoan
                                                    }).AsEnumerable();

            return View(loans);

        }


        //10
        public ActionResult GetAYearOldAlbum()
        {
            int[] albumIds = db.Loans.Where(m => m.ReturnedDate == null).Select(m => m.AlbumId).Distinct().ToArray();
            
            IEnumerable<Albums> albums
            = albumIds.Count() > 0 ? db.Albums.Where(m => !albumIds.Contains(m.id)).Where(m => m.ReleaseDate.AddDays(365.00) > DateTime.Now)
                        : db.Albums.Where(m => m.ReleaseDate.AddDays(365.00) > DateTime.Now);

            return View(albums);
        }


        //11
        public ActionResult GetLoanAlbum()
        {
            List<Loan> loans = db.Loans.Include(m => m.Album).Include(m => m.Members).Where(m => m.ReturnedDate == null).ToList();

            IEnumerable<LoanAlbumVM> loanAlbums = (from l in loans
                                                   select new LoanAlbumVM
                                                   {
                                                       Loan = l,
                                                       TotalLoan = db.Loans.Where(m => m.IssuedDate == l.IssuedDate).ToList().Count()
                                                   }).AsEnumerable();

            return View(loanAlbums);
        }

        //12
        public ActionResult GetMemberInActive()
        {
            IEnumerable<Loan> loans = db.Loans.Include(m => m.Members).
                                Where(m => m.IssuedDate.AddDays(30) >= DateTime.Now);

           
            return View(loans);
        }

        //13
        public ActionResult GetAlbumInActive()
        {
            int[] albumIds = db.Loans.Include(m => m.Album).Where(m => m.IssuedDate.AddDays(30)>=DateTime.Now).Select(m=>m.AlbumId).ToArray();

            IEnumerable<Albums> albums = db.Albums.Where(m => !albumIds.Contains(m.id));

            return View(albums);

        }

            



    }
}
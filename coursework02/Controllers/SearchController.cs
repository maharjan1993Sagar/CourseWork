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
    [Authorize]
    public class SearchController : Controller
    {
        private DataContext db = new DataContext();

        public ActionResult Index()
        {
            return View();
        }
        // 1
        [AllowAnonymous]
        public ActionResult GetAlbumByArtist(string name)
        {
            List<Albums> albums = new List<Albums>();// db.Artists.SelectMany(m => m.Album).Distinct();
            if (!string.IsNullOrEmpty(name))
            {
                albums = db.Artists.Where(m => m.Name.Contains(name.Trim())).SelectMany(m => m.Album).Include(m=>m.Artists).Include(m=>m.Producers).ToList();
            }
            return View(albums);

        }
        //2
        [AllowAnonymous]
        public ActionResult GetAlbumOnShelves(string name)
        {
            List<Albums> albums = new List<Albums>();// db.Artists.SelectMany(m => m.Album).Distinct();
            if (!string.IsNullOrEmpty(name))
            {
                albums = db.Artists.Where(m => m.Name.Contains(name.Trim())).SelectMany(m => m.Album).Include(m=>m.Artists).Include(m=>m.Producers).ToList();
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
                                               Name = m.FullName + " , " + m.MemberNo,
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
                DateTime currentDate = DateTime.Now.AddDays(-31);
                IEnumerable<Loan> loans = db.Loans.Where(m => m.MemberId == MemberId.Value&&m.IssuedDate>=currentDate)
                                            .Include(m => m.Album).Include(m=>m.Members).Include(m=>m.LoanTypes);
                return View(loans.AsEnumerable());
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
            List<DropDownItem> memItems = (from m in albums
                                           select new DropDownItem
                                           {
                                               Name = m.CopyNumber + " , " + m.Name,
                                               Id = m.id
                                           }).ToList();
            ViewBag.Id = new SelectList(memItems, "Id", "Name");
           // ViewBag.Id = new SelectList(albums, "Id", "CopyNumber");
            return View();
        }

        [HttpPost]
        public ActionResult GetByCopyNumber(int? Id)
        {
            IEnumerable<Albums> albums = db.Loans.Include(m => m.Album).Select(m => m.Album);
            List<DropDownItem> memItems = (from m in albums
                                           select new DropDownItem
                                           {
                                               Name = m.CopyNumber + " , " + m.Name,
                                               Id = m.id
                                           }).ToList();
            ViewBag.Id = new SelectList(memItems, "Id", "Name");

            if (Id != null)
            {
                Loan loan = db.Loans.Where(m=>m.AlbumId==Id.Value).OrderByDescending(m => m.IssuedDate).FirstOrDefault();
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
                                                                    db.Loans.Where(m => m.MemberId == l.Id && m.ReturnedDate == null).ToList().Count()
                                                                    : 0,
                                                        MaxLoan = l.MemberCatagories.TotalLoan
                                                    }).AsEnumerable();

            return View(loans);

        }


        //10
        public ActionResult GetAYearOldAlbum()
        {
            int[] albumIds = db.Loans.Where(m => m.ReturnedDate == null).Select(m => m.AlbumId).Distinct().ToArray();
            DateTime ReleaseDate = DateTime.Now.AddDays(-365);

            IEnumerable<Albums> albums
            = albumIds.Count() > 0 ? db.Albums.Include(m=>m.Artists).Include(m=>m.Producers).Where(m => !albumIds.Contains(m.id)).Where(m => m.ReleaseDate <ReleaseDate)
                        : db.Albums.Include(m => m.Artists).Include(m => m.Producers).Where(m => m.ReleaseDate < ReleaseDate);

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
            DateTime IssuedDate = DateTime.Now.AddDays(-30);

            int[] membersIds = db.Loans.Where(m => m.IssuedDate>IssuedDate).Select(m => m.MemberId).Distinct().ToArray();
            
            IEnumerable<Member> members = db.Loans.Include(m => m.Members).
                                Where(m => m.IssuedDate<IssuedDate&& !membersIds.Contains(m.MemberId)).
                                Select(m=>m.Members).Include(m=>m.MemberCatagories).Distinct().AsEnumerable();
            
            return View(members);
        }

        //13
        public ActionResult GetAlbumInActive()
        {
            DateTime IssuedDate = DateTime.Now.AddDays(-30);

            int[] albumIds = db.Loans.Include(m => m.Album).Where(m => m.IssuedDate>IssuedDate).Select(m=>m.AlbumId).ToArray();

            IEnumerable<Albums> albums = db.Albums.Where(m => !albumIds.Contains(m.id)).
                                        Include(m => m.Artists).Include(m => m.Producers);

            return View(albums);

        }

            



    }
}
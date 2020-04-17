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
using System.IO;

namespace coursework02.Controllers
{
    public class AlbumsController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Albums
        public ActionResult Index()
        {
            var albums = db.Albums.Include(m => m.Artists).Include(m => m.Artists).Include(m => m.Producers).OrderBy(m => m.ReleaseDate).ToList();
            //  return View(db.Albums.Include(m=>m.Artists).ToList());
            return View(albums);
        }

        // GET: Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(CreateAlbumVM album, HttpPostedFileBase CoverImage)
        {
            if (ModelState.IsValid)
            {
                List<int> artistIds = new List<int>();
                List<int> producerIds = new List<int>();

                foreach (var item in album.ArtistList)
                {
                    Artist artist = new Artist {
                        BirthDate = item.BirthDate,
                        Email=item.Email,
                        Gender=item.Gender,
                        Name=item.Name,
                        PhoneNumber=item.PhoneNumber
                    };
                    if (!db.Artists.Any(m => m.Email == item.Email && m.Name == item.Name))
                    {
                        db.Artists.Add(artist);
                        db.SaveChanges();
                        artistIds.Add(artist.Id);

                    }
                    else
                    {
                        artist = db.Artists.FirstOrDefault(m => m.Email == item.Email && m.Name == item.Name);
                        artistIds.Add(artist.Id);
                    }
                }


                 foreach (var item in album.ProducerList)
                {
                    Producer producer = new Producer {
                        DateOfBirth=item.DateOfBirth,
                        Name=item.Name,
                        Studio=item.Studio
                    };
                    if (!db.Producers.Any(m => m.DateOfBirth == item.DateOfBirth && m.Name == item.Name && m.Studio==item.Studio))
                    {
                        db.Producers.Add(producer);
                        db.SaveChanges();
                        producerIds.Add(producer.Id);

                    }
                    else
                    {
                        producer = db.Producers.FirstOrDefault(m => m.DateOfBirth == item.DateOfBirth && m.Name == item.Name && m.Studio == item.Studio);
                        producerIds.Add(producer.Id);
                    }
                }

                Albums albums = new Albums
                {
                    Producers = db.Producers.Where(m => producerIds.Contains(m.Id)).ToList(),
                    Artists = db.Artists.Where(m => artistIds.Contains(m.Id)).ToList(),
                    Length = album.Length,
                    Name = album.Name,
                    ReleaseDate = album.ReleaseDate,
                    StudioName = album.StudioName,
                    CoverImagePath = "~/Images/Albums/"
                };

                bool Status = db.Albums.Include(m=>m.Artists).Include(m=>m.Producers)
                            .Any(m => m.Name == albums.Name && m.Producers == albums.Producers && m.Artists == albums.Artists);

                if (!Status)
                {
                    if (!Directory.Exists(Server.MapPath("~/Images/Albums")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Images/Albums"));
                    }

                    if (CoverImage.ContentLength > 0)
                    {
                        CoverImage.SaveAs(Server.MapPath("~/Images/Albums/" + albums.id + ".jpg"));
                    }


                }


            }
            return View();
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            AlbumVM album = new AlbumVM();
            album.Producers = new SelectList(db.Producers.ToList(), "Id", "Name");
            album.ArtistList = new SelectList(db.Artists.ToList(), "Id", "Name");
            return View(album);
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AlbumVM albums, HttpPostedFileBase CoverImage)
        {
            if (ModelState.IsValid)
            {

                Albums album = new Albums
                {
                    Producers = db.Producers.Where(m => albums.ProducerIds.Contains(m.Id)).ToList(),
                    Artists = db.Artists.Where(m => albums.ArtistIds.Contains(m.Id)).ToList(),
                    Length = albums.Length,
                    Name = albums.Name,
                    ReleaseDate = albums.ReleaseDate,
                    StudioName = albums.StudioName,
                    CoverImagePath = "~/Images/Albums/"
                };
                db.Albums.Add(album);
                db.SaveChanges();

                album = db.Albums.OrderByDescending(m => m.id).FirstOrDefault();


                if (!Directory.Exists(Server.MapPath("~/Images/Albums")))
                {
                    Directory.CreateDirectory(Server.MapPath("~/Images/Albums"));
                }

                if (CoverImage.ContentLength > 0)
                {
                    CoverImage.SaveAs(Server.MapPath("~/Images/Albums/" + album.id + ".jpg"));
                }

                return RedirectToAction("Index");
            }
            albums.Producers = new SelectList(db.Producers.ToList(), "Id", "Name");
            albums.ArtistList = new SelectList(db.Artists.ToList(), "Id", "Name");
            return View(albums);
        }

        // GET: Albums/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Albums albums)
        {
            if (ModelState.IsValid)
            {
                db.Entry(albums).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(albums);
        }

        // GET: Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Albums albums = db.Albums.Find(id);
            if (albums == null)
            {
                return HttpNotFound();
            }
            return View(albums);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Albums albums = db.Albums.Find(id);
            db.Albums.Remove(albums);
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

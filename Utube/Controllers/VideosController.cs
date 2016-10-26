using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Utube.Models;

namespace Utube.Controllers
{
    public class VideosController : Controller
    {
        private VideoContext db = new VideoContext();

        // GET: Videos
        public ActionResult Index()
        {
            return View(db.Videos.ToList());
        }

        // GET: Videos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // GET: Videos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Videos/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Videotitle,Vlink,Shared,Datein,Like,Dislike,Profileid,Views,Info")] Video video)
        {
            if (ModelState.IsValid)
            {
                video.Dislike = 0;
                video.Like = 0;
                video.Shared = false;
                video.Views = 0;
                video.Datein = DateTime.Now;


                db.Videos.Add(video);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(video);
        }

        // GET: Videos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Videotitle,Vlink,Shared,Datein,Like,Dislike,Profileid,Views,Info")] Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(video);
        }

        // GET: Videos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        // POST: Videos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
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


        public ActionResult ShowVideo(int id)
        {
            Video v = db.Videos.Find(id);

            return View(v);
        }

        public ActionResult SeachVideo(  string search, Video vkey)
        {
            
            string req = "https://www.googleapis.com/youtube/v3/search?part=id&q=" + search + "&type=video&key="+ vkey.Vkey ;
            Uri uri = new Uri(req);


            WebProxy proxy = new WebProxy("10.3.0.9", 3128);
            proxy.Credentials = new NetworkCredential("07422", "rigame56");
            var request = (HttpWebRequest)HttpWebRequest.Create(req);
            //request.Proxy = proxy;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string json = "";
            using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                json = sr.ReadToEnd();

            }
            RootObject robj = JsonConvert.DeserializeObject<RootObject>(json);




            #region   для получения информации о видео
            /*string[] m = new string[5];
            int k = 0;
            foreach(var i in   robj.items)
            {
                m[k] = i.id.ToString();
                k++;
            }

            string getinfo = "https://www.googleapis.com/youtube/v3/videos?id=" + m[0] + "%2C+" + m[1] + "%2C+" + m[2] + "%2C+" + m[3] + "%2C+" + m[4] + "&key" + vkey.Vkey + "& part=snippet,statistics";
            var info = (HttpWebRequest)HttpWebRequest.Create(getinfo);
            HttpWebResponse respon = (HttpWebResponse)info.GetResponse();


            string jsonInfo = "";
            using (StreamReader sr = new StreamReader(respon.GetResponseStream(), Encoding.UTF8))
            {
                jsonInfo = sr.ReadToEnd();

            }
            RootObject1 rinfo = JsonConvert.DeserializeObject<RootObject1>(json);*/
            #endregion



            return View(robj.items);
        }
    }
}

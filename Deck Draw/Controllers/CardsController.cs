using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Deck_Draw.Models;
using Newtonsoft.Json.Linq;

namespace Deck_Draw.Controllers
{
    public class CardsController : Controller
    {
        private DeckTable db = new DeckTable();

        // GET: Cards
        //[HttpPost]
        public ActionResult Index()
        {
            Deck d = new Deck();
            d = (Deck)Session["Deck"];

            string prefix = "https://deckofcardsapi.com/api/deck/";
            string postfix = "/draw/?count=5";

            string final = prefix + d.DeckID + postfix;

            HttpWebRequest request = WebRequest.CreateHttp(final);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();

            JObject CardsJson = JObject.Parse(data);

            List<Card> hand = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                Card c = new Card();
                c.Name = (CardsJson["cards"][i]["value"].ToString() + " of " + CardsJson["cards"][i]["suit"].ToString());
                c.ImagePath = CardsJson["cards"][i]["image"].ToString();
                hand.Add(c);
            }

            Session["Hand"] = hand;
            ViewBag.Hand = hand;
            return View();
        }

        // GET: Cards/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // GET: Cards/Create
        //[HttpPost]
        public ActionResult Create()
        {
            string final = "https://deckofcardsapi.com/api/deck/new/";
            HttpWebRequest request = WebRequest.CreateHttp(final);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string data = rd.ReadToEnd();


            JObject CardsJson = JObject.Parse(data);

            Deck d = new Deck
            {
                DeckID = CardsJson["deck_id"].ToString()
            };
            Session["Deck"] = d;

            return View();
        }

        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "DeckID,Name,ImagePath")] Card card)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Decks.Add(card);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(card);
        //}

        // GET: Cards/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DeckID,Name,ImagePath")] Card card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(card);
        }

        // GET: Cards/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Card card = db.Cards.Find(id);
            if (card == null)
            {
                return HttpNotFound();
            }
            return View(card);
        }

        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Card card = db.Cards.Find(id);
            db.Decks.Remove(card);
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

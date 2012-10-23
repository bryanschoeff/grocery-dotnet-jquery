using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using GroceryList.Models;

namespace GroceryList.Controllers
{
    public class GroceryController : ApiController
    {
        private GroceryContext db = new GroceryContext();

        // GET api/Grocery
        public Object GetGroceries()
        {
            var soretd = db.Db.OrderBy(g => g.Section)
                              .ThenBy(g => g.Item);
            var result = soretd.GroupBy(s => s.Section)
                               .Select(s => new { Section = s.Key, Items = s });

            return result;
        }

        // GET api/Grocery/5
        public Grocery GetGrocery(int id)
        {
            Grocery grocery = db.Db.Find(id);
            if (grocery == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return grocery;
        }

        // PUT api/Grocery/5
        public HttpResponseMessage PutGrocery(int id, Grocery grocery)
        {
            if (ModelState.IsValid && id == grocery.Id)
            {
                db.Entry(grocery).State = EntityState.Modified;

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Grocery
        public HttpResponseMessage PostGrocery(Grocery grocery)
        {
            if (ModelState.IsValid)
            {
                db.Db.Add(grocery);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, grocery);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = grocery.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Grocery/
        public HttpResponseMessage DeleteGrocery()
        {
            var donelist = db.Db.Where(g => (g.Done));

            if (donelist == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            foreach (Object item in donelist)
            {
                db.Db.Remove((Grocery)item);
            }

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        // DELETE api/Grocery/5
        public HttpResponseMessage DeleteGrocery(int id)
        {
            Grocery grocery = db.Db.Find(id);
            if (grocery == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Db.Remove(grocery);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, grocery);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
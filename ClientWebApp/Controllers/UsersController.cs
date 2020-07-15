using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using SimpleCompanyDAL.EF;
using SimpleCompanyDAL.Models;

namespace ClientWebApp.Controllers
{
    public class UsersController : Controller
    {
        private string baseUrl = "https://localhost:44329/api/Users";

        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<User>>(await response.Content.ReadAsStringAsync());
                return View(items);
            }
            return HttpNotFound();
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                return View(user);
            }
            return HttpNotFound();
        }

        public ActionResult Create()
        {
            // Workaround to get values for DropDownList
            SimpleCompanyEntities db = new SimpleCompanyEntities();
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            //

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,DepartmentId,Department")] User user)
        {
            if (!ModelState.IsValid) return View(user);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(user);
                await client.PostAsync(baseUrl,
                    new StringContent(json, Encoding.UTF8, "application/json"));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

                // Workaround to get values for DropDownList
                SimpleCompanyEntities db = new SimpleCompanyEntities();
                ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", user.DepartmentId);
                //

                return View(user);
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,DepartmentId")] User user)
        {
            if (!ModelState.IsValid) return View(user);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(user);
            var response = await client.PutAsync($"{baseUrl}/{user.Id}",
                new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var client = new HttpClient();
            var response = await client.GetAsync($"{baseUrl}/{id.Value}");
            if (response.IsSuccessStatusCode)
            {
                var user = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
                return View(user);
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Id")] User user)
        {
            try
            {
                var client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}/{user.Id}")
                {
                    Content =
                        new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}

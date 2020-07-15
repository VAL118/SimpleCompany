using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using SimpleCompanyDAL.Models;

namespace ClientWebApp.Controllers
{
    public class DepartmentsController : Controller
    {
        private string baseUrl = "https://localhost:44385/api/Departments";

        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(baseUrl);
            if (response.IsSuccessStatusCode)
            {
                var items = JsonConvert.DeserializeObject<List<Department>>(await response.Content.ReadAsStringAsync());
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
                var department = JsonConvert.DeserializeObject<Department>(await response.Content.ReadAsStringAsync());
                return View(department);
            }
            return HttpNotFound();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Department department)
        {
            if (!ModelState.IsValid) return View(department);
            try
            {
                var client = new HttpClient();
                string json = JsonConvert.SerializeObject(department);
                await client.PostAsync(baseUrl, new StringContent(json, Encoding.UTF8, "application/json"));
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
                var department = JsonConvert.DeserializeObject<Department>(await response.Content.ReadAsStringAsync());
                return View(department);
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Department department)
        {
            if (!ModelState.IsValid) return View(department);
            var client = new HttpClient();
            string json = JsonConvert.SerializeObject(department);
            var response = await client.PutAsync($"{baseUrl}/{department.Id}",
                new StringContent(json, Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View(department);
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
                var department = JsonConvert.DeserializeObject<Department>(await response.Content.ReadAsStringAsync());
                return View(department);
            }
            return new HttpNotFoundResult();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete([Bind(Include = "Id")] Department department)
        {
            try
            {
                var client = new HttpClient();

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, $"{baseUrl}/{department.Id}")
                {
                    Content =
                        new StringContent(JsonConvert.SerializeObject(department), Encoding.UTF8, "application/json")
                };
                var response = await client.SendAsync(request);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Unable to create record: {ex.Message}");
            }
            return View(department);
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

using BikeMgrWeb.DTO;
using BikeMgrWeb.Loggers;
using BikeMgrWeb.Models;
using BikeMgrWeb.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;

namespace BikeMgrWeb.Controllers
{
    [ResourceAuthorize("Read", "Generic")]
    public class BikeController : Controller
    {
        private readonly HttpService _http;
        private readonly ILogService _log;

        private static string ApiUri = ConfigurationManager.AppSettings["BikeApi.Uri"];

        public BikeController(HttpService http, ILogService log)
        {
            _http = http;
            _log = log;
        }

        [HttpGet]
        public async Task<ActionResult> Index(string alertMsg = null, string sortOrder = null, string currentFilter = null, string search = null, int? page = null)
        {
            ViewBag.AlertMsg = alertMsg;
            if (search != null)
                page = 1;
            else 
            {
                search = currentFilter;
                if(String.IsNullOrEmpty(search) && HttpContext.Session["Bike.Search.search"] != null) search = HttpContext.Session["Bike.Search.search"].ToString();
            }
            if (String.IsNullOrEmpty(sortOrder) && HttpContext.Session["Bike.Search.sortOrder"] != null) sortOrder = HttpContext.Session["Bike.Search.sortOrder"].ToString();
            if (page == null && HttpContext.Session["Bike.Search.page"] != null) page = Convert.ToInt32(HttpContext.Session["Bike.Search.page"].ToString());
            try
            {
                var bikePage = await _http.Get<Page<Bike>>(HttpContext, $"/api/bike?sortOrder={sortOrder}&search={search}&page={page}&pageSize=10");
                BikeIndexView<Bike> bikeView = new BikeIndexView<Bike>(bikePage);
                bikeView.CurrentSort = sortOrder;
                bikeView.NameSort = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
                bikeView.BrandSort = sortOrder == "brand" ? "brand_desc" : "brand";
                bikeView.WheelSort = sortOrder == "wheel" ? "wheel_desc" : "wheel";
                bikeView.FrameSort = sortOrder == "frame" ? "frame_desc" : "frame";
                bikeView.TypeSort = sortOrder == "type" ? "type_desc" : "type";
                bikeView.PriceSort = sortOrder == "price" ? "price_desc" : "price";
                bikeView.CurrentFilter = search;
                HttpContext.Session["Bike.Search.sortOrder"] = sortOrder;
                HttpContext.Session["Bike.Search.search"] = search;
                HttpContext.Session["Bike.Search.page"] = page;
                return View(bikeView);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                ViewBag.AlertMsg = "Error retrieving bikes.";
                return View(BikeIndexView<Bike>.Create(new List<Bike>(), 0, 0));
            }
        }

        [ResourceAuthorize("Write", "Generic")]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            BikeDetailView bikeView = new BikeDetailView();
            bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
            return View(bikeView);
        }

        [ResourceAuthorize("Write", "Generic")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Bike bike, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(bike.ID.ToString()), "id");
                    content.Add(new StringContent(bike.Name), "name");
                    content.Add(new StringContent(bike.Brand), "brand");
                    content.Add(new StringContent(bike.Wheels.ToString()), "wheels");
                    content.Add(new StringContent(bike.FrameMaterial), "framematerial");
                    content.Add(new StringContent(bike.BikeType.ID.ToString()), "biketypeid");
                    content.Add(new StringContent(bike.Price.ToString()), "price");
                    if (file != null)
                        content.Add(new StreamContent(file.InputStream), "image", file.FileName);
                    var response = await _http.Post<BikeDTO>(HttpContext, "/api/bike", content);
                    return RedirectToAction("Index", new { alertMsg = String.Format("Bike {0} created successfully.", bike.Name) });
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, ex.Message);
                    ViewBag.SaveResult = "An error occured saving the bike. Contact the support team.";
                    BikeDetailView bikeView = new BikeDetailView();
                    bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
                    return View(bikeView);
                }
            }
            else
            {
                BikeDetailView bikeView = new BikeDetailView();
                bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
                return View(bikeView);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Details(int id)
        {
            return await GetBikeByID(id);
        }

        [ResourceAuthorize("Write", "Generic")]
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            return await GetBikeByID(id);
        }

        [ResourceAuthorize("Write", "Generic")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Bike bike, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (bike.ID == 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                try
                {
                    var content = new MultipartFormDataContent();
                    content.Add(new StringContent(bike.ID.ToString()), "id");
                    content.Add(new StringContent(bike.Name), "name");
                    content.Add(new StringContent(bike.Brand), "brand");
                    content.Add(new StringContent(bike.Wheels.ToString()), "wheels");
                    content.Add(new StringContent(bike.FrameMaterial), "framematerial");
                    content.Add(new StringContent(bike.BikeType.ID.ToString()), "biketypeid");
                    content.Add(new StringContent(bike.Price.ToString()), "price");
                    if(file != null)
                        content.Add(new StreamContent(file.InputStream), "image", file.FileName);
                    var response = await _http.Put<BikeDTO>(HttpContext, "/api/bike", content);
                    return RedirectToAction("Index", new { alertMsg = String.Format("Bike {0} updated successfully.", bike.Name) });
                }
                catch (Exception ex)
                {
                    _log.LogError(ex, ex.Message);
                    ViewBag.SaveResult = "An error occured saving the bike. Contact the support team.";
                    BikeDetailView bikeView = new BikeDetailView(bike);
                    bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
                    return View(bikeView);
                }
            }
            else
            {
                BikeDetailView bikeView = new BikeDetailView(bike);
                bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
                return View(bikeView);
            }
        }

        [ResourceAuthorize("Write", "Generic")]
        public async Task<ActionResult> Delete(int id)
        {
            string status = "Failed to delete bike";
            try
            {
                if (await _http.Delete(HttpContext, $"/api/bike/{id}"))
                    status = "Bike deleted successfully!";
            }
            catch (Exception ex)
            { _log.LogError(ex, ex.Message); }
            return RedirectToAction("Index", new { alertMsg = status });
        }

        private async Task<List<BikeType>> GetBikeTypes()
        {
            try
            { return await _http.Get<List<BikeType>>(HttpContext, "/api/biketype"); }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                return new List<BikeType>();
            }
        }

        private async Task<ActionResult> GetBikeByID(int id)
        {
            try
            {
                BikeDetailView bikeView = new BikeDetailView(await _http.Get<Bike>(HttpContext, $"/api/bike/{id}"));
                bikeView.BikeTypes = new SelectList(await GetBikeTypes(), "ID", "TypeName");
                if (!String.IsNullOrWhiteSpace(bikeView.ImageLocation)) bikeView.ImageLocation = ApiUri + bikeView.ImageLocation;
                if (Request.Path.Contains("Details"))
                    return View("Details", bikeView);
                else
                    return View("Edit", bikeView);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, ex.Message);
                return RedirectToAction("Index", new { alertMsg = "Error retrieving bike." });
            }
        }

    }
}
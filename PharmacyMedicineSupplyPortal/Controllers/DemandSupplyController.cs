using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PharmacyMedicineSupplyPortal.Models;
using PharmacyMedicineSupplyPortal.Repository;

namespace PharmacyMedicineSupplyPortal.Controllers
{
    public class DemandSupplyController : Controller
    {
        private IDemands repo;
        private ISupplies supplyrepo;
        public DemandSupplyController(IDemands repo, ISupplies supplyrepo)
        {
            this.repo = repo;
            this.supplyrepo = supplyrepo;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var stock = new List<MedicineStock>();

                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("https://localhost:44366/");
                    HttpResponseMessage res = await httpclient.GetAsync("MedicineStockInformation");
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        stock = JsonConvert.DeserializeObject<List<MedicineStock>>(result);
                    }
                }
                if(stock.Count==0)
                {
                    return RedirectToAction("Index","DemandSupply");
                }
                var list = new List<MedicineDemand>();
                foreach (var med in stock)
                {
                    list.Add(new MedicineDemand { Medicine = med.Name, Demand = 0 });
                }
                ViewBag.Demands = list;
                return View();
            }
            catch(Exception)
            {
                return RedirectToAction("Index","DemandSupply");
            }
        }

        [HttpPost]
        public IActionResult Add(MedicineDemand meds)
        {
            // string s = meds.Medicine + meds.Demand.ToString();
            try
            {
                if (meds == null)
                {
                    return RedirectToAction("Index", "DemandSupply");
                }
                Demands newdemand = new Demands()
                {
                    Medicine = meds.Medicine,
                    Demand = meds.Demand
                };
                int res = repo.AddDemand(newdemand);
                if (res > 0)
                {
                    return RedirectToAction("AddSupply", newdemand);
                }
                return RedirectToAction("Index", "Home");
            }
            catch(Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> AddSupply(Demands med)
        {
            if(med==null)
            {
                return RedirectToAction("Index", "Home");
            }
            try
            {
                var distributionOfStock = new List<PharmacyMedicineSupply>();

                using (var httpclient = new HttpClient())
                {
                    httpclient.BaseAddress = new Uri("https://localhost:44358/");
                    HttpResponseMessage res = await httpclient.GetAsync("api/MedicineSupply/GetSupplies/" + med.Medicine + "/" + med.Demand);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = res.Content.ReadAsStringAsync().Result;
                        distributionOfStock = JsonConvert.DeserializeObject<List<PharmacyMedicineSupply>>(result);
                    }
                }
                if (distributionOfStock.Count == 0)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var supply in distributionOfStock)
                {
                    supplyrepo.AddSupply(new Supplies { PharmacyName = supply.PharmacyName, MedicineName = supply.MedicineName, SupplyCount = supply.SupplyCount });
                }
                return View(distributionOfStock);
            }
            catch(Exception)
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }

}


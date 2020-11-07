using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MedicineStockApi.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace MedicineStockApi.Controllers
{
    public class MedicineStockController : Controller
    {
        private IConfiguration configuration;
        private readonly IMedicineStockService service;
        public MedicineStockController(IConfiguration config, IMedicineStockService service)
        {
            configuration = config;
            this.service = service;
        }
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(MedicineStockController));
        [HttpGet]
        [Route("MedicineStockInformation")]
        public IActionResult MedicineStockInformation()
        {
            _log4net.Info("Get Api Initiated");
            var MedicineData = service.MedicineStockInformation1();
            if (MedicineData == null)
            {
                return BadRequest();
            }
            return Ok(MedicineData);

        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MedicineSupplyMicroservice.Models;
using MedicineSupplyMicroservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using log4net;

namespace MedicineSupplyMicroservice.Controllers
{
    public class SupplyController : ControllerBase
    {
        static readonly ILog _log4net = LogManager.GetLogger(typeof(SupplyController));
        private readonly ISupplyRepo _supplyRepo;

        public SupplyController(ISupplyRepo supplyRepo)
        {
            _supplyRepo = supplyRepo;
        }


        // Retrieves the name of Pharmaci

        [HttpGet("ListOfPharmacy")]
        public IActionResult GetListOfPharmacy()
        {
            _log4net.Info("Get Pharmacy List Api Accessed");
            List<string> pharmacies =  _supplyRepo.GetListOfPharmacy();
            if (pharmacies != null)
            {
                _log4net.Info("List of pharmacies retrieved successfully");
                return Ok(pharmacies);
            }
            else
            {
                _log4net.Info("No Pharmacy data available");
                return BadRequest("No Pharmacy data available");
            }
        }


        // MAIN FUNCTIONALITY
        //Receives Demand and Accesses StockAPI and distributes the medicine Stock .

        [HttpPost("PharmacySupply")]
        [Authorize(Roles = "Supplier")]
        public Task<List<PharmacyMedicineSupply>> GetPharmacySupply([FromBody] List<MedicineDemand> medDemand)
        {
            _log4net.Info("Get Pharmacy Supply API Acessed");
            var pharmacySupply =  _supplyRepo.GetPharmacySupply(medDemand);
            if (pharmacySupply == null)
            {
                _log4net.Info("Couldn't provide Supply Count");
            }
            return pharmacySupply;
        }


        //Accesses the StockApi and forwards the data .

        [HttpGet("Stock")]
        [Authorize(Roles = "Supplier")]
        public Task<List<MedicineStock>> GetMedicineStock()
        {
            _log4net.Info("Get Medicine Stock Accessed");
            var stockList =  _supplyRepo.GetMedicineStock();
            if (stockList==null)
            {
                _log4net.Info("Couldn't access Stock Api");
            }
            return stockList;
            
        }
    }
}

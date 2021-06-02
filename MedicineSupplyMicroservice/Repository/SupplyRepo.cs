using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using MedicineSupplyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MedicineSupplyMicroservice.Repository
{
    public class SupplyRepo : ISupplyRepo
    {
        string Baseurl = "https://localhost:44364/";
        PharmacyData pd = new PharmacyData();

        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger(typeof(SupplyRepo));

        List<MedicineNameAndSupply> supCount = new List<MedicineNameAndSupply>();  //List of medicine name and its supply count for each pharmacy


        // Gets the list of Pharmacy from PharmacyData File.

        public List<String> GetListOfPharmacy()
        {
            _log4net.Info("Accessing PharmacyData File");
            return (pd.pharmacies);
        }



        // Accesses StockAPI and distributes the stock equally among the pharmacies.

        public async Task<List<PharmacyMedicineSupply>> GetPharmacySupply(List<MedicineDemand> lisMedDemand)
        {
            using (HttpClient client = new HttpClient())
            {
                List<MedicineStock> medStock = new List<MedicineStock>();
                List<PharmacyMedicineSupply> medSupply = new List<PharmacyMedicineSupply>();
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource MedicineDemand using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("MedicineStockInformation");
                if (Res.IsSuccessStatusCode)
                {
                    _log4net.Info("Stock API Accessed successfully");

                    //Storing the response details recieved from web api   
                    var StockResponse = Res.Content.ReadAsStringAsync().Result;


                    medStock = JsonConvert.DeserializeObject<List<MedicineStock>>(StockResponse);

                }

                for (int i = 0; i < medStock.Count; i++)
                {
                    supCount.Add(new MedicineNameAndSupply { medicineName = medStock[i].Name, supplyCount = (lisMedDemand[i].DemandCount > medStock[i].NumberOfTabletsInStock) ? medStock[i].NumberOfTabletsInStock / pd.pharmacies.Count : lisMedDemand[i].DemandCount / pd.pharmacies.Count });
                }

                for (int i = 0; i < pd.pharmacies.Count; i++)
                {
                    medSupply.Add(new PharmacyMedicineSupply { pharmacyName = pd.pharmacies[i], medicineAndSupply = supCount });
                }
                return medSupply;
            }

        }

        public async Task<List<MedicineStock>> GetMedicineStock()
        {

            List<MedicineStock> medicineStock = new List<MedicineStock>();
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource MedicineDemand using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("MedicineStockInformation");
                if (Res.IsSuccessStatusCode)
                {
                    _log4net.Info("Stock API Accessed successfully");

                    //Storing the response details recieved from web api   
                    var stockResponse = Res.Content.ReadAsStringAsync().Result;


                    medicineStock = JsonConvert.DeserializeObject<List<MedicineStock>>(stockResponse);

                }

                return medicineStock;
            }
        }
    }
}

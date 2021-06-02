using MedicineSupplyMicroservice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineSupplyMicroservice.Repository
{
    public interface ISupplyRepo
    {
        public Task<List<PharmacyMedicineSupply>> GetPharmacySupply(List<MedicineDemand> lisMedDemand);

        public Task<List<MedicineStock>> GetMedicineStock();

        public List<String> GetListOfPharmacy();

    }
}

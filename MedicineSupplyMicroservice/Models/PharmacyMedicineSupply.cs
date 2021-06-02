using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineSupplyMicroservice.Models
{
    public class PharmacyMedicineSupply
    {
        public string pharmacyName { get; set; }
        public List<MedicineNameAndSupply> medicineAndSupply { get; set; }
    }
}

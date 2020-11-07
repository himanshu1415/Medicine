
using PharmacyMedicineSupplyApi.Models;
using PharmacyMedicineSupplyApi.Respository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyApi.Service
{
    public class MedicineSupplyService : IMedicineSupplyService
    {
        private ISupply supplyRepo;
        public MedicineSupplyService(ISupply supplyrepo)
        {
            this.supplyRepo = supplyrepo;
        }
        public async Task<List<PharmacyMedicineSupply>> MedcineSupply(string medicine, int demand)
        {
            List<PharmacyMedicineSupply> medList = await supplyRepo.GetSupplies(medicine, demand);
            return medList;
        }
    }
}

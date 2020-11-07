using Newtonsoft.Json;
using PharmacyMedicineSupplyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PharmacyMedicineSupplyApi.Respository
{
    public class SupplyRepository : ISupply
    {
        /*   public List<PharmacyMedicineSupply> GetSupplies(List<MedicineDemand> demand)
           {
               //List of Supply to be sent
               List<PharmacyMedicineSupply> supplies = new List<PharmacyMedicineSupply>();

               //List of All the meds in stock
               var stock = new List<MedicineStock>() { 
               new MedicineStock{Name="A",ChemicalComposition="a,b,c",DateOfExpiry="02-11-2022",NumberOfTabletsInStock=200,TargetAilment="General"},
               new MedicineStock{Name="B",ChemicalComposition="a,b,c",DateOfExpiry="02-11-2022",NumberOfTabletsInStock=100,TargetAilment="General"},
               new MedicineStock{Name="C",ChemicalComposition="a,b,c",DateOfExpiry="02-11-2022",NumberOfTabletsInStock=200,TargetAilment="General"},
               new MedicineStock{Name="D",ChemicalComposition="a,b,c",DateOfExpiry="02-11-2022",NumberOfTabletsInStock=200,TargetAilment="General"},
               new MedicineStock{Name="E",ChemicalComposition="a,b,c",DateOfExpiry="02-11-2022",NumberOfTabletsInStock=400,TargetAilment="General"},
               };

               //Dictionary to store the Name of med and stock value as key value pair
               Dictionary<string, int> meds = new Dictionary<string, int>();
               foreach(var med in stock)
               {
                   meds.Add(med.Name, med.NumberOfTabletsInStock);
               }

               //List of Pharmacy the company does business with
               List<string> Pharmacies = new List<string>() {"Pharmacy1","Pharmacy2","Pharmacy3","Pharmacy4"};
               int totalPharmacies = Pharmacies.Count;
               PharmacyMedicineSupply medSupply;
               foreach(var med in demand)
               {
                   int inStock = meds[med.Medicine];
                   int demandStock = med.Demand;
                   if(inStock>med.Demand)
                   {
                       int medcount = demandStock / totalPharmacies;
                       for(int i=0;i<totalPharmacies-1;i++)
                       {
                           medSupply = new PharmacyMedicineSupply() { MedicineName = med.Medicine, PharmacyName = Pharmacies[i], SupplyCount = medcount };
                           supplies.Add(medSupply);
                           demandStock = demandStock - medcount;
                       }
                       medSupply = new PharmacyMedicineSupply() { MedicineName = med.Medicine, PharmacyName = Pharmacies[totalPharmacies-1], SupplyCount = demandStock };
                       supplies.Add(medSupply);
                   }
                   else
                   {
                       int medcount = inStock / totalPharmacies;
                       for (int i = 0; i < totalPharmacies - 1; i++)
                       {
                           medSupply = new PharmacyMedicineSupply() { MedicineName = med.Medicine, PharmacyName = Pharmacies[i], SupplyCount = medcount };
                           supplies.Add(medSupply);
                           inStock = inStock - medcount;
                       }
                       medSupply = new PharmacyMedicineSupply() { MedicineName = med.Medicine, PharmacyName = Pharmacies[totalPharmacies - 1], SupplyCount = inStock };
                       supplies.Add(medSupply);
                   }

               }



               return supplies;
           }*/

        public async Task<List<PharmacyMedicineSupply>> GetSupplies(string medicineName,int demand)
        {
            //List of Supply to be sent
            List<PharmacyMedicineSupply> supplies = new List<PharmacyMedicineSupply>();
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

            //Dictionary to store the Name of med and stock value as key value pair
            Dictionary<string, int> meds = new Dictionary<string, int>();
            foreach (var medicine in stock)
            {
                meds.Add(medicine.Name, medicine.NumberOfTabletsInStock);
            }

            //List of Pharmacy the company does business with
            List<string> Pharmacies = new List<string>() { "Pharmacy1", "Pharmacy2", "Pharmacy3", "Pharmacy4" };
            int totalPharmacies = Pharmacies.Count;
            PharmacyMedicineSupply medSupply;


            int inStock = meds[medicineName];
            int demandStock = demand;
            if (inStock > demandStock)
            {
                int medCount = demandStock / totalPharmacies;
                for (int i = 0; i < totalPharmacies - 1; i++)
                {
                    medSupply = new PharmacyMedicineSupply() { MedicineName = medicineName, PharmacyName = Pharmacies[i], SupplyCount = medCount };
                    supplies.Add(medSupply);
                    demandStock = demandStock - medCount;
                }
                medSupply = new PharmacyMedicineSupply() { MedicineName = medicineName, PharmacyName = Pharmacies[totalPharmacies - 1], SupplyCount = demandStock };
                supplies.Add(medSupply);
            }
            else
            {
                int medCount = inStock / totalPharmacies;
                for (int i = 0; i < totalPharmacies - 1; i++)
                {
                    medSupply = new PharmacyMedicineSupply() { MedicineName = medicineName, PharmacyName = Pharmacies[i], SupplyCount = medCount };
                    supplies.Add(medSupply);
                    inStock = inStock - medCount;
                }
                medSupply = new PharmacyMedicineSupply() { MedicineName = medicineName, PharmacyName = Pharmacies[totalPharmacies - 1], SupplyCount = inStock };
                supplies.Add(medSupply);
            }

            return supplies;
        }
    }
}

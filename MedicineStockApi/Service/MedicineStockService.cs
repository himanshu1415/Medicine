using MedicineStockApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MedicineStockApi.Service
{
    public class MedicineStockService : IMedicineStockService
    {
        public readonly IMedicineStockRepository repo;
        public MedicineStockService(IMedicineStockRepository repo)
        {
            this.repo = repo;
        }
        public dynamic MedicineStockInformation1()
        {
            var Result = repo.MedicineStockInformation();
            if (Result == null)
            { return null; }
            return Result;
        }
    }
}

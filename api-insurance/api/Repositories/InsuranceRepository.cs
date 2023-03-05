using System;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
	public class InsuranceRepository: IInsuranceRepository
	{
        protected InsuranceDBContext _dBContext { get; set; }

        public InsuranceRepository(InsuranceDBContext dBContext)
		{
            _dBContext = dBContext;
        }

        public async Task Create(Insurance insurance)
        {
            await _dBContext.Set<Insurance>().AddAsync(insurance);
        }

        public IQueryable<Insurance> getAll()
        {
            return _dBContext.Set<Insurance>().AsNoTracking();
        }

        public async Task Save()
        {
            await _dBContext.SaveChangesAsync();
        }

        public void Update(Insurance insurance)
        {
            _dBContext.Set<Insurance>().Update(insurance);
        }

        public void Delete(Insurance insurance)
        {
            _dBContext.Set<Insurance>().Remove(insurance);
        }

        public async Task<Insurance> getById(int id)
        {
            var insurance = await _dBContext.Set<Insurance>().Where(i => i.Id == id).FirstOrDefaultAsync();
            return insurance!;
        }
    }
}


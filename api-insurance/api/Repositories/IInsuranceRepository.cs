using System;
using api.Models;

namespace api.Repositories
{
    public interface IInsuranceRepository
    {
        IQueryable<Insurance> getAll();
        Task<Insurance> getById(int id);
        Task Create(Insurance insurance);
        void Update(Insurance insurance);
        void Delete(Insurance insurance);
        Task Save();

    }
}


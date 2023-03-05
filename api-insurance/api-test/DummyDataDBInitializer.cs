using System;
using api.Models;
using Microsoft.Extensions.Hosting;

namespace api_test
{
	public class DummyDataDBInitializer
	{
		public DummyDataDBInitializer()
		{
		}

        public void Seed(InsuranceDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Insurances!.AddRange(
                new Insurance() { Id = 1, Name = "Angloamericana de Seguros, S. A.", Fee = 0.10, Status = true },
                new Insurance() { Id = 2, Name = "Aseguradora Agropecuaria Dominicana, S.A.", Fee = 0.10, Status = true },
                new Insurance() { Id = 3, Name = "Atlántica Seguros, S.A.", Fee = 0.10, Status = true },
                new Insurance() { Id = 4, Name = "Atrio Seguros, S.A.", Fee = 0.10, Status = true },
                new Insurance() { Id = 5, Name = "Autoseguro, S.A.", Fee = 0.10, Status = true }
            );
            
            context.SaveChanges();
        }
    }
}


using System;
namespace api.DTOs
{
	public class InsuranceCreationDTO
	{
        public string? Name { get; set; }

        public double Fee { get; set; }

        public bool Status { get; set; }
    }
}


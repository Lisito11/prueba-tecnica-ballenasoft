using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
	public class Insurance
	{
		[Key]
		public int Id { get; set; }

		[MaxLength(45)]
		public string? Name { get; set; }

		[Range(0, 0.25)]
		public double Fee {get ; set; }

		public bool Status { get; set; }
    }
}


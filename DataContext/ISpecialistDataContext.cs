using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SpecialistTestApp1.Models;

namespace SpecialistTestApp1.DataContext
{
	public interface ISpecialistDataContext
	{
		DbSet<Customer> Customers { get; set; }

		DbSet<Course> Courses { get; set; }

	}
}

using Microsoft.EntityFrameworkCore;
using SpecialistTestApp1.Models;

namespace SpecialistTestApp1.DataContext
{
	public class TestDataContext : DbContext, ISpecialistDataContext
	{
		public DbSet<Customer> Customers { get; set; } = default!;

		public DbSet<Course> Courses { get; set; } = default!;

		public TestDataContext(DbContextOptions<TestDataContext> options) : base(options) 
		{
			if (Customers.Any() || Courses.Any()) 
				return;

			Customers.AddRange([
					new Customer() {
						Id = 1,
						Name = "John Smith",
						IsGraduate = false,
					},
					new Customer() {
						Id = 2,
						Name = "Иван Иванов",
						IsGraduate = true,
					},
				]);
			Courses.AddRange([
					new Course() {
						Id = 1,
						Name = "Линейная алгебра",
						Description = "Университетский курс линейной алгебры",
						StartDate = DateTime.Today.AddDays(4),
						Price = 2000.0M
					},
					new Course() {
						Id = 2,
						Name = "Тензорный анализ",
						Description = "Вводный курс тензорного анализа",
						StartDate = DateTime.Today.AddDays(10),
						Price = 1500.0M
					},
					new Course() {
						Id = 3,
						Name = "Русский язык",
						Description = "Курс русского языка для иностранцев",
						StartDate = DateTime.Today.AddDays(7),
						Price = 2000.0M
					},
					new Course() {
						Id = 4,
						Name = "Тригонометрия",
						Description = "Тригонометрия для школьников",
						StartDate = DateTime.Today.AddDays(1),
						Price = 500.0M
					},
				]);
			SaveChanges();
		}
	}
}

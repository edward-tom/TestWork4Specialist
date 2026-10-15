namespace SpecialistTestApp1.Models
{
	public class Order
	{
		public Customer Customer { get; set; }

		public IEnumerable<Course> Courses { get; set; }

		public decimal OrderSum { get; set; }

		public Order(Customer customer, IEnumerable<Course> courses)
		{
			Customer = customer;
			Courses = courses;
			OrderSum = 0;
		}
	}
}

namespace SpecialistTestApp1.Models
{
	public class Course
	{
		public required int Id { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public DateTime StartDate { get; set; }
		public decimal Price { get; set; }
	}
}

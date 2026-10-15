namespace SpecialistTestApp1.Models
{
	public class Customer
	{
		public required int Id { get; set; }
		public required string Name { get; set; }
		public bool IsGraduate { get; set; } = false;
	}
}

namespace SpecialistTestApp1.Models
{
	public enum ClientType
	{
		LeagalEntity,
		Individual
	}

	public class CallbackDTO
	{
		public string? Name { get; set; }
		public string? Phone { get; set; }
		public string? Email { get; set; }
		public int ClientType { get; set; } = 0;
	}
}

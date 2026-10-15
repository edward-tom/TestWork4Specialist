namespace SpecialistTestApp1.Services
{
	public interface IEmailService
	{
		Task<EmailStatus> Send(string email, string message);
		Task<EmailStatus> SendAlternative(string email, string message);
	}
}

using System.Threading.Tasks;

namespace SpecialistTestApp1.Services
{
	public class EmailService : IEmailService
	{
		public async Task<EmailStatus> Send(string email, string message)
		{
			if (email.Length > 20)
			{
				throw new NotImplementedException();
			}
			
			return EmailStatus.Success;
		}

		public async Task<EmailStatus> SendAlternative(string email, string message)
		{
			return EmailStatus.Success;
		}
	}
}

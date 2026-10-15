using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using SpecialistTestApp1.Models;
using SpecialistTestApp1.Services;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpecialistTestApp1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CallbackController : ControllerBase
	{
		private string LegalEntityAlterEmail;
		private string IndividualAlterEmail;

		private readonly IEmailService _emailService;
		private readonly ILogger<OrderController> _logger;

		public CallbackController(
			IEmailService emailService,
			ILogger<OrderController> logger)
		{
			_emailService = emailService;
			_logger = logger;

			//TODO: Здесь должна быть инициализация из настроек приложения
			LegalEntityAlterEmail = "specialist.corp@yandex.ru";
			IndividualAlterEmail = "specialist.ufl@yandex.ru";
		}


		// POST api/Callback/Send
		[HttpPost("send")]
		public async Task<IResult> Send(CallbackDTO data)
		{
			// TODO: текст сообщения должен браться из запроса или из настроек или как-то генерироваться
			string message = "Some text";

			if (data == null)
			{
				return Results.BadRequest();
			}

			if (!IsValidEmail(data.Email))
			{
				return Results.BadRequest("Нет или некорректный адрес email.");
			}

			if (!IsValidPhone(data.Phone))
			{
				return Results.BadRequest("Нет или некорректный номер телефона.");
			}

			var email = data.Email;
			EmailStatus result;
			try
			{
				result = await _emailService.Send(email, message);
			}
			catch (Exception ex) 
			{
				_logger.Log(LogLevel.Error, "Filed to send email to {0}:\n{1}", email, ex.Message);
				result = EmailStatus.NotSend;
			}

			if (result != EmailStatus.Success)
			{
				try
				{
					email = data.ClientType == (int)ClientType.Individual ? IndividualAlterEmail : LegalEntityAlterEmail;
					result = await _emailService.SendAlternative(email, message);
					if (result != EmailStatus.Success)
					{
						return Results.Problem("Не получилось отправить сообщение.");
					}
				}
				catch (Exception ex) 
				{
					_logger.Log(LogLevel.Error, "Filed to send email to {0}:\n{1}", email, ex.Message);
					return Results.Problem("Не получилось отправить сообщение. Ошибка при отправке на резервный email.");
				}
			}

			_logger.Log(LogLevel.Information, "Email sent to {0}", data.Email);
			return Results.Ok();
		}

		private bool IsValidEmail(string? email)
		{
			if (string.IsNullOrWhiteSpace(email))
			{
				return false;
			}

			try
			{
				return Regex.IsMatch(email,
					@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}

		private bool IsValidPhone(string? phone)
		{
			if (string.IsNullOrWhiteSpace(phone))
			{
				return false;
			}

			try
			{
				return Regex.IsMatch(phone,
					@"^\+?[0-9()-]{8,17}$",
					RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}

	}
}

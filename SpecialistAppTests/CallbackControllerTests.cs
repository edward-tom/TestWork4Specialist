using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using SpecialistTestApp1.Controllers;
using SpecialistTestApp1.Models;
using SpecialistTestApp1.Services;

namespace SpecialistAppUnitTests;

public class CallbackControllerTests
{
	private const string LegalEntityAlterEmail = "specialist.corp@yandex.ru";
	private const string IndividualAlterEmail = "specialist.ufl@yandex.ru";

	private ILogger<OrderController> _logger;

	public CallbackControllerTests()
	{
		_logger = new Mock<ILogger<OrderController>>().Object;
	}

	[Theory]
	[InlineData("asdasd_gmail.com", false, 400)]
	[InlineData("asdasd@gmail.com", true, 200)]
	public async void SendChecksEmailAddress(string email, bool expectSuccess, int expectedStatusCode)
	{
		var emailService = new Mock<IEmailService>();
		emailService.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(EmailStatus.Success);
		var callbackController = new CallbackController(emailService.Object, _logger);
		CallbackDTO data = new() { Name = "Some name", Email = email, Phone = "+7(910)123-45-67", ClientType = 1 };

		var actionResult = await callbackController.Send(data);

		if (expectSuccess)
		{
			var result = actionResult as Ok;
			Assert.NotNull(result);
			Assert.Equal(expectedStatusCode, result.StatusCode);
		}
		else
		{
			var result = actionResult as BadRequest<string>;
			Assert.NotNull(result);
			Assert.Equal(expectedStatusCode, result.StatusCode);
		}
	}

	[Fact]
    public async void SendToInvalidEmailNoServiceCall()
    {
		var badEmail = "asdasd_gmail.com";
		var emailService = new Mock<IEmailService>();
		//emailService.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(EmailStatus.Fail);
		var callbackController = new CallbackController(emailService.Object, _logger);
		CallbackDTO data = new() { Name = "Some name", Email = badEmail, Phone = "+7(910)123-45-67", ClientType = 1 };

		var actionResult = await callbackController.Send(data);

		var result = actionResult as BadRequest<string>;
		Assert.NotNull(result);
		emailService.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never, "Произошел вызов метода EmailService.Send()");
	}

	[Fact]
	public async void SendCallsEmailService()
	{
		var badEmail = "asdasd@gmail.com";
		var emailService = new Mock<IEmailService>();
		emailService.Setup(x => x.Send(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(EmailStatus.Success);
		var callbackController = new CallbackController(emailService.Object, _logger);
		CallbackDTO data = new() { Name = "Some name", Email = badEmail, Phone = "+7(910)123-45-67", ClientType = 1 };

		var actionResult = await callbackController.Send(data);

		var result = actionResult as Ok;
		Assert.NotNull(result);
		emailService.Verify(x => x.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once, "Не произошел вызов метода EmailService.Send()");
	}
}

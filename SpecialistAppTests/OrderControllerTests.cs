using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SpecialistTestApp1.Calculators;
using SpecialistTestApp1.Controllers;
using SpecialistTestApp1.DataContext;
using SpecialistTestApp1.Models;
using Xunit.Extensions;

namespace SpecialistAppTests
{
	public class OrderControllerTests
	{
		private TestDataContext _context;
		//private Mock<OrderCalculator> _calculator;
		private ILogger<OrderController> _logger;

		public OrderControllerTests() 
		{
			var dbOptions = new DbContextOptionsBuilder<TestDataContext>().UseInMemoryDatabase("TestDatabase").Options;
				//new Mock<DbContextOptions<TestDataContext>>().Object;
			_context = new TestDataContext(dbOptions);
			_logger = new Mock<ILogger<OrderController>>().Object;

			//_controller = new OrderController(_context, _calculator, _logger);
		}

		[Fact]
		public async void CalculatePriceCallsCalculator()
		{
			var calculator = new Mock<IOrderCalculator>();
			calculator.Setup(x => x.OrderSum(It.IsAny<Order>())).Returns(1000.0M);
			OrderController controller = new OrderController(_context, calculator.Object, _logger);
			var customerId = 1;
			var courseIds = new[] { 1, 2 };

			var actionResult = await controller.CalculatePrice(customerId, courseIds);

			var result = actionResult.Result as OkObjectResult;
			Assert.NotNull(result);
			Assert.Equal(200, result.StatusCode);
			Assert.Equal(1000.0M, result.Value);
			calculator.Verify(x => x.OrderSum(It.IsAny<Order>()), Times.Once, "Не произошло вызова метода OrderCalculator.OrderSum()");
		}

		[Fact]
		public async void CalculatePriceOfNotExistedCustomerReturnsFalse()
		{
			var calculator = new Mock<IOrderCalculator>();
			calculator.Setup(x => x.OrderSum(It.IsAny<Order>())).Returns(1000.0M);
			OrderController controller = new OrderController(_context, calculator.Object, _logger);
			var customerId = 100;
			var courseIds = new[] { 1, 2 };

			var actionResult = await controller.CalculatePrice(customerId, courseIds);

			var result = actionResult.Result as ObjectResult;
			Assert.NotNull(result);
			Assert.IsType<ValidationProblemDetails>(result.Value);
		}
	}
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SpecialistTestApp1.Calculators;
using SpecialistTestApp1.DataContext;
using SpecialistTestApp1.Models;

namespace SpecialistTestApp1.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class OrderController : ControllerBase
	{
		private readonly TestDataContext _context;
		private readonly IOrderCalculator _calculator;
		private readonly ILogger<OrderController> _logger;

		public OrderController(
			TestDataContext context,
			IOrderCalculator orderCalculator,
			ILogger<OrderController> logger) : base()
		{
			_context = context;
			_calculator = orderCalculator;
			_logger = logger;
		}


		// GET: api/Order/CalculatePrice
		[HttpGet("CalculatePrice")]
		public async Task<ActionResult<decimal>> CalculatePrice([FromQuery] int customerId, [FromQuery] int[] courseIds)
		{
			var customer = await _context.Customers.FindAsync(customerId);
			if (customer == null)
			{
				return ValidationProblem("Пользователь не найден"); //NotFound();
			}

			var courses = await _context.Courses.Where(x => courseIds.Contains(x.Id)).ToListAsync();

			var order = new Order(customer, courses);
			order.OrderSum = _calculator.OrderSum(order);

			return Ok(order.OrderSum);
		}

	}
}

using SpecialistTestApp1.Models;

namespace SpecialistTestApp1.Calculators
{
	public class OrderCalculator : IOrderCalculator
	{
		private int GraduateDiscount { get; set; }
		private int LastMinuteDiscount { get; set; }
		private int LastMinuteTerm { get; set; }
		public OrderCalculator()
		{
			//TODO: Параметры надо будет брать из настроек или из БД
			GraduateDiscount = 30;
			LastMinuteDiscount = 20;
			LastMinuteTerm = 5;
		}
		public decimal OrderSum(Order order)
		{
			decimal sum = 0;
			int discount = order.Customer.IsGraduate ? GraduateDiscount : 0;

			foreach (var course in order.Courses)
			{
				var courseDiscount = discount == GraduateDiscount ? GraduateDiscount : (course.StartDate.Date - DateTime.Today).TotalDays < LastMinuteTerm ? LastMinuteDiscount : 0;
				sum += course.Price * (100 - courseDiscount) / 100;
			}
			return sum;
		}
	}
}

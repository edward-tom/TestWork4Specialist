using SpecialistTestApp1.Models;

namespace SpecialistTestApp1.Calculators
{
	public interface IOrderCalculator
	{
		decimal OrderSum(Order order);
	}
}

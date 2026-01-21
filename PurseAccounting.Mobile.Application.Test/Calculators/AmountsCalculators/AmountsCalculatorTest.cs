using PurseAccounting.Mobile.Application.Calculators.AmountsCalculators;

namespace PurseAccounting.Mobile.Application.Test.Calculators.AmountsCalculators;

public class AmountsCalculatorTest
{
    private readonly AmountsCalculator _amountsCalculator;

    public AmountsCalculatorTest()
    {
        _amountsCalculator = new();
    }

    [Fact]
    public void CalculateAmounts_NegativeTotalAmount_ThrowsArgumentOutOfRangeException()
    {
        // Arrange & Act & Assert
        Assert.ThatCode(() => _amountsCalculator.CalculateAmounts(-1, 1)).Throws<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void CalculateAmounts_NonPositiveDaysCount_ThrowsArgumentOutOfRangeException(int daysCount)
    {
        // Arrange & Act & Assert
        Assert.ThatCode(() => _amountsCalculator.CalculateAmounts(1, daysCount)).Throws<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData(0, 1, 0, 0)]
    [InlineData(10, 1, 10, 0)]
    [InlineData(20, 4, 5, 15)]
    public void CalculateAmounts_ReturnsExpectedDayAmountAndRestAmount(
        int totalAmount,
        int daysCount,
        int expectedDayAmount,
        int expectedRestAmount)
    {
        // Arrange & Act
        var result = _amountsCalculator.CalculateAmounts(totalAmount, daysCount);

        // Assert
        Assert.That(result.DayAmount).IsEqualTo(expectedDayAmount);
        Assert.That(result.RestAmount).IsEqualTo(expectedRestAmount);
    }
}

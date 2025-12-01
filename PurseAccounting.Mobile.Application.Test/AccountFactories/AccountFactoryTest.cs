using AutoFixture;
using Moq;
using PurseAccounting.Mobile.Application.AccountFactories;
using System.Globalization;

namespace PurseAccounting.Mobile.Application.Test.AccountFactories;

public class AccountFactoryTest
{
    private static readonly Fixture _fixture = new();
    private readonly Mock<IDateTimeService> _dateTimeServiceMock = new(MockBehavior.Strict);

    private readonly AccountFactory _accountFactory;

    public AccountFactoryTest()
    {
        _accountFactory = new(_dateTimeServiceMock.Object);
    }

    [Theory]
    [InlineData("2025-11-27T10:00:00Z", "2025-11-28", 100L, 50L, 0, 150L, 2)]
    [InlineData("2025-11-26T23:59:59Z", "2025-11-28", 100L, 50L, 1, 150L, 2)]
    [InlineData("2025-11-27T10:00:00Z", "2025-11-27", 100L, 0L, 0, 100L, 1)]
    [InlineData("2025-11-27T10:00:00Z", "2025-11-26", 50L, -20L, 0, 30L, 0)]
    [InlineData("2025-11-27T00:00:00Z", "2025-12-01", 0L, 100L, 0, 100L, 5)]
    [InlineData("2025-11-27T12:00:00Z", "2025-11-20", 10L, -15L, 0, -5L, 0)]
    [InlineData("2025-11-30T23:59:59Z", "2025-11-25", 0L, -100L, 0, -100L, 0)]
    public void GetAccount_ValidAccount_ReturnsCorrectAccount(string nowString, string plannedDateString, long dayAmount, long restAmount, short timeZone, long expectedAvailableAmount, int expectedDaysCount)
    {
        // Arrange
        var now = DateTime.Parse(nowString, null, DateTimeStyles.AdjustToUniversal);
        var plannedDate = DateTime.Parse(plannedDateString);

        _dateTimeServiceMock.Setup(s => s.UtcNow).Returns(now);

        var infrastructureAccount = new Infrastructure.Accounting.Account
        {
            DayAmount = dayAmount,
            RestAmount = restAmount,
            PlannedDate = plannedDate,
            TimeZone = timeZone,
        };

        // Act
        var result = _accountFactory.GetAccount(infrastructureAccount);

        // Assert
        Assert.That(result.DayAmount).IsEqualTo(dayAmount);
        Assert.That(result.AvaliableAmount).IsEqualTo(expectedAvailableAmount);
        Assert.That(result.DaysCount).IsEqualTo(expectedDaysCount);
    }
}

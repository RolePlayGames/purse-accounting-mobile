using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.Distribution;

namespace PurseAccounting.Mobile.Application.Distribution;

internal class DistributionService : IDistributionService
{
    private readonly IDistributionClient _distributionClient;
    private readonly IApplicationContext _applicationContext;
    private readonly IAccountFactory _accountFactory;

    public DistributionService(IDistributionClient distributionClient, IApplicationContext applicationContext, IAccountFactory accountFactory)
    {
        _distributionClient = distributionClient;
        _applicationContext = applicationContext;
        _accountFactory = accountFactory;
    }

    public async Task<DistributionStrategyInfo> GetDistributionStrategy(CancellationToken cancellationToken)
    {
        var apiResult = await _distributionClient.GetDistributionStrategy(cancellationToken);

        return apiResult.Match(
            result => result,
            exception => throw new InvalidOperationException("Failed to get distribution strategy"));
    }

    public async Task<bool> DistributeAllToToday(CancellationToken cancellationToken)
    {
        var apiResult = await _distributionClient.DistributeAllToToday(cancellationToken);

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, new() { DayAmount = result.DayAmount, RestAmount = result.RestAmount });

                return true;
            },
            exception => false);
    }

    public async Task<bool> DistributeBetweenDays(CancellationToken cancellationToken)
    {
        var apiResult = await _distributionClient.DistributeBetweenDays(cancellationToken);

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, new() { DayAmount = result.DayAmount, RestAmount = result.RestAmount });

                return true;
            },
            exception => false);
    }
}

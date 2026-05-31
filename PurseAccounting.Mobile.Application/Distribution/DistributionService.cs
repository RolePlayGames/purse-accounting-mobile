using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.Distribution;
using PurseAccounting.Mobile.Infrastructure.ServerResults;

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

    public async Task<AvailableUserChoiceDistributionStrategyInfo?> GetAvailableUserChoiceDistributionStrategy(CancellationToken cancellationToken)
    {
        var apiResult = await _distributionClient.GetDistributionStrategy(cancellationToken);

        var strategyInfo = apiResult.Match(result => result, exception => throw new InvalidOperationException("Failed to get distribution strategy"));

        if (strategyInfo.Type == DistributionStrategyType.Automatic)
        {
            await DistributeAccount(_distributionClient.DistributeAutomatically, cancellationToken);
        }

        return strategyInfo is UserChoiceDistributionStrategyInfo userChoiceStrategy
            ? new AvailableUserChoiceDistributionStrategyInfo
            {
                AllToTodayDistributedDayAmount = userChoiceStrategy.AllToTodayDistributedDayAmount,
                BetweenDaysDistributedDayAmount = userChoiceStrategy.BetweenDaysDistributedDayAmount,
            }
            : null;
    }

    public Task<DistributionResult> DistributeAllToToday(CancellationToken cancellationToken)
    {
        return DistributeAccount(_distributionClient.DistributeAllToToday, cancellationToken);
    }

    public Task<DistributionResult> DistributeBetweenDays(CancellationToken cancellationToken)
    {
        return DistributeAccount(_distributionClient.DistributeBetweenDays, cancellationToken);
    }

    private async Task<DistributionResult> DistributeAccount(Func<CancellationToken, Task<ApiResult<DistributeAccountResponse>>> distributeAction, CancellationToken cancellationToken)
    {
        var apiResult = await distributeAction(cancellationToken);

        return apiResult.Match(
            result =>
            {
                _applicationContext.Account = _applicationContext.Account is null
                    ? _accountFactory.CreateAccount(new()
                    {
                        DayAmount = result.DayAmount,
                        RestAmount = result.RestAmount,
                        TimeZone = result.TimeZone,
                        PlannedDate = result.PlannedDate,
                    })
                    : _accountFactory.CreateAccount(_applicationContext.Account, new()
                    {
                        DayAmount = result.DayAmount,
                        RestAmount = result.RestAmount,
                    });

                return DistributionResult.Success;
            },
            exception =>
            {
                return exception switch
                {
                    ServerException<DistributionExceptionCode> ex when ex.NoticeType == DistributionExceptionCode.DistributionIsNotNeeded => DistributionResult.DoNotNeeded,
                    _ => DistributionResult.Failed,
                };
            });
    }
}

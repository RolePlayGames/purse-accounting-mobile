using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Models;
using PurseAccounting.Mobile.Infrastructure.ApiResults.Generics;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings;
using PurseAccounting.Mobile.Infrastructure.PlannedTransactions.Settings.ExceptionCodes;
using PurseAccounting.Mobile.Infrastructure.ServerResults;
using PurseAccounting.Mobile.Infrastructure.Transactions;

namespace PurseAccounting.Mobile.Application.PlannedTransactions;

internal class PlannedTransactionSettingsService : IPlannedTransactionSettingsService
{
    private readonly IPlannedTransactionSettingsClient _plannedTransactionSettingsClient;
    private readonly IApplicationContext _applicationContext;
    private readonly IAccountFactory _accountFactory;

    public PlannedTransactionSettingsService(
        IPlannedTransactionSettingsClient plannedTransactionSettingsClient,
        IApplicationContext applicationContext,
        IAccountFactory accountFactory)
    {
        _plannedTransactionSettingsClient = plannedTransactionSettingsClient;
        _applicationContext = applicationContext;
        _accountFactory = accountFactory;
    }

    public async Task<CreatePlannedTransactionSettingResult> Create(CreatePlannedTransactionSettingRequest request, CancellationToken cancellationToken)
    {
        var apiResult = await _plannedTransactionSettingsClient.Create(request, cancellationToken);

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, ToDailyDistributedAmount(result.AccountAmounts));

                return CreatePlannedTransactionSettingResult.Success;
            },
            exception =>
            {
                return exception switch
                {
                    ServerException<CreatePlannedTransactionSettingExceptionCode> ex when ex.NoticeType == CreatePlannedTransactionSettingExceptionCode.FirstAwaitingPlannedTransactionIsAlreadyExists => CreatePlannedTransactionSettingResult.FirstAwaitingPlannedTransactionIsAlreadyExists,
                    ServerException<CreatePlannedTransactionSettingExceptionCode> ex when ex.NoticeType == CreatePlannedTransactionSettingExceptionCode.PlannedTransactionSettingsNameAlreadyExists => CreatePlannedTransactionSettingResult.PlannedTransactionSettingsNameAlreadyExists,
                    _ => CreatePlannedTransactionSettingResult.Unknown,
                };
            });
    }

    public async Task<bool> Deactivate(long settingID, CancellationToken cancellationToken)
    {
        var apiResult = await _plannedTransactionSettingsClient.Deactivate(settingID, cancellationToken);

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, ToDailyDistributedAmount(result));

                return true;
            },
            exception =>
            {
                return false;
            });
    }

    public async Task<IReadOnlyCollection<PlannedTransactionSettingInfo>> GetInfo(CancellationToken cancellationToken)
    {
        var apiResult = await _plannedTransactionSettingsClient.GetInfo(cancellationToken);

        return apiResult.Match(
            result => result,
            exception => Array.Empty<PlannedTransactionSettingInfo>());
    }

    public async Task<bool> Update(long settingID, PlannedTransactionSettingInfo info, CancellationToken cancellationToken)
    {
        var apiResult = await _plannedTransactionSettingsClient.Update(settingID, info, cancellationToken);

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, ToDailyDistributedAmount(result));

                return true;
            },
            exception =>
            {
                return false;
            });
    }

    private static DailyDistributedAmount ToDailyDistributedAmount(AccountAmounts accountAmounts) =>
        new() { DayAmount = accountAmounts.DayAmount, RestAmount = accountAmounts.RestAmount, ReservedAmount = accountAmounts.ReservedAmount };
}

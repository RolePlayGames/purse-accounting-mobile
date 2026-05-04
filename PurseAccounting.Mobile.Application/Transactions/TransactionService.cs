using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Infrastructure.ServerResults;
using PurseAccounting.Mobile.Infrastructure.Transactions;
using PurseAccounting.Mobile.Infrastructure.Transactions.Daily;
using PurseAccounting.Mobile.Infrastructure.Transactions.Total;

namespace PurseAccounting.Mobile.Application.Transactions;

internal class TransactionService : ITransactionService
{
    private readonly IDailyTransactionClient _dailyTransactionClient;
    private readonly ITotalTransactionClient _totalTransactionClient;
    private readonly IApplicationContext _applicationContext;
    private readonly IAccountFactory _accountFactory;
    private readonly ITransactionsClient _transactionsClient;

    public TransactionService(IDailyTransactionClient dailyTransactionClient, ITotalTransactionClient totalTransactionClient, IApplicationContext applicationContext, IAccountFactory accountFactory, ITransactionsClient transactionsClient)
    {
        _dailyTransactionClient = dailyTransactionClient;
        _totalTransactionClient = totalTransactionClient;
        _applicationContext = applicationContext;
        _accountFactory = accountFactory;
        _transactionsClient = transactionsClient;
    }

    public async Task<MakeTransactionResult> MakeTransaction(Transaction transaction, CancellationToken cancellationToken)
    {
        if (transaction.ChangeAmountType == TransactionChangeAmountType.Total)
        {
            if (_applicationContext.Account?.DaysCount <= 1)
                return MakeTransactionResult.PlannedDateHasPassed;

            if (_applicationContext.Account?.AvaliableAmount <= 0)
                return MakeTransactionResult.NegativeRestAmount;
        }

        var request = new AddTransactionRequest { Amount = transaction.Amount, TransactionCategoryID = transaction.TransactionCategoryID };

        var addTrasactionTask = transaction.ChangeAmountType switch
        {
            TransactionChangeAmountType.Daily when transaction.ChangeType == TransactionChangeType.Income => _dailyTransactionClient.AddIncomeTransaction(request, cancellationToken),
            TransactionChangeAmountType.Daily when transaction.ChangeType == TransactionChangeType.Withdrawal => _dailyTransactionClient.AddWithdrawalTransaction(request, cancellationToken),
            TransactionChangeAmountType.Total when transaction.ChangeType == TransactionChangeType.Income => _totalTransactionClient.AddTotalIncomeTransaction(request, cancellationToken),
            TransactionChangeAmountType.Total when transaction.ChangeType == TransactionChangeType.Withdrawal => _totalTransactionClient.AddTotalWithdrawalTransaction(request, cancellationToken),
            _ => throw new NotImplementedException(),
        };

        var apiResult = await addTrasactionTask;

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, new() { DayAmount = result.DayAmount, RestAmount = result.RestAmount });

                return MakeTransactionResult.Success;
            },
            exception =>
            {
                return exception switch
                {
                    ServerException<AddTotalTransactoinsExceptionCode> ex when ex.NoticeType == AddTotalTransactoinsExceptionCode.PlannedDateHasPassed => MakeTransactionResult.PlannedDateHasPassed,
                    ServerException<AddTotalTransactoinsExceptionCode> ex when ex.NoticeType == AddTotalTransactoinsExceptionCode.NegativeRestAmount => MakeTransactionResult.NegativeRestAmount,
                    _ => MakeTransactionResult.Unknown,
                };
            });
    }

    public async Task<IReadOnlyCollection<TransactionGroup>> GetTransactionsByDate(IReadOnlyCollection<long> categoryIds, short timeZone, CancellationToken cancellationToken)
    {
        var transactions = await _transactionsClient.GetTransactions(categoryIds, cancellationToken);

        var transactionsWithTimeZone = transactions
            .Select(x => x with { Date = x.Date.AddHours(timeZone) })
            .ToList();

        var groupedByDate = transactionsWithTimeZone
            .GroupBy(x => x.Date.Date)
            .OrderByDescending(x => x.Key)
            .Select(x => new TransactionGroup
            {
                GroupDate = x.Key,
                Transactions = x.OrderByDescending(x => x.ID).ToList(),
            })
            .ToList();

        return groupedByDate;
    }

    public async Task<bool> CancelTransaction(long transactionId, TransactionChangeAmountType changeAmountType, CancellationToken cancellationToken)
    {
        var cancelTrasactionTask = changeAmountType switch
        {
            TransactionChangeAmountType.Daily => _dailyTransactionClient.CancelTransaction(transactionId, cancellationToken),
            TransactionChangeAmountType.Total => _totalTransactionClient.CancelTransaction(transactionId, cancellationToken),
            _ => throw new NotImplementedException(),
        };

        var apiResult = await cancelTrasactionTask;

        return apiResult.Match(
            result =>
            {
                if (_applicationContext.Account is not null)
                    _applicationContext.Account = _accountFactory.CreateAccount(_applicationContext.Account, new() { DayAmount = result.DayAmount, RestAmount = result.RestAmount });

                return true;
            },
            exception =>
            {
                return false;
            });
    }
}

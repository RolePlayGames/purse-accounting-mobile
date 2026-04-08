using PurseAccounting.Mobile.Application.AccountFactories;
using PurseAccounting.Mobile.Application.Context;
using PurseAccounting.Mobile.Application.Models;
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

    public async Task<IReadOnlyCollection<IGrouping<DateTime, DateWithTimeZone>>> GetTransactionsByDate(
        IReadOnlyCollection<long> categoryIds,
        short timeZone,
        CancellationToken cancellationToken)
    {
        var transactions = await _transactionsClient.GetTransactions(categoryIds, cancellationToken);

        var transactionsWithTimeZone = transactions
            .Select(t => new DateWithTimeZone 
            { 
                Value = t.Date.AddHours(timeZone), 
                TimeZone = timeZone,
                TransactionId = t.ID
            })
            .ToList();

        var groupedByDate = transactionsWithTimeZone
            .GroupBy(t => t.Value.Date)
            .OrderByDescending(g => g.Key)
            .Select(g => g.OrderByDescending(t => t.TransactionId).ToList() as IGrouping<DateTime, DateWithTimeZone>)
            .ToList();

        return groupedByDate;
    }
}

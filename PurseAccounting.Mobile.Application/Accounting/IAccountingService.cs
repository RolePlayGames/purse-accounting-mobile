namespace PurseAccounting.Mobile.Application.Accounting;

public interface IAccountingService
{
    /// <summary>
    /// Loads account
    /// </summary>
    /// <returns>Is loading succeed</returns>
    Task<bool> LoadAccount();
}

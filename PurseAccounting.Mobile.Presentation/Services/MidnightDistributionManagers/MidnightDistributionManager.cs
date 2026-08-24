using Polly;
using Polly.Retry;
using PurseAccounting.Mobile.Application.Distribution;
using PurseAccountinng.Mobile.Presentation.Pages.Authorized.Distribution;
using PurseAccountinng.Mobile.Presentation.Services.Tabs;

namespace PurseAccountinng.Mobile.Presentation.Services.MidnightDistributionManagers;

internal class MidnightDistributionManager : IMidnightDistributionManager
{
    private static readonly ResiliencePipeline _retryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true, // to add random (±20%) to avoid synchronizing requests from all clients.
        })
        .Build();

    private readonly IDistributionService _distributionService;
    private readonly ITabNavigator _tabNavigator;

    private CancellationTokenSource? _cancellationTokenSource;
    private Task? _backgroundTask;

    public MidnightDistributionManager(IDistributionService distributionService, ITabNavigator tabNavigator)
    {
        _distributionService = distributionService;
        _tabNavigator = tabNavigator;
    }

    public Task Start(short timezone, CancellationToken cancellationToken)
    {
        if (_backgroundTask is not null && !_backgroundTask.IsCompleted)
            return Task.CompletedTask;

        _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        _backgroundTask = Task.Run(async () =>
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var delay = GetDelayUntilNextMidnight(timezone);

                try
                {
                    await Task.Delay(delay, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                    break;

                try
                {
                    var strategy = await _retryPipeline.ExecuteAsync(async (token) => await _distributionService.GetAvailableUserChoiceDistributionStrategy(token), _cancellationTokenSource.Token);

                    if (strategy is not null)
                        await MainThread.InvokeOnMainThreadAsync(_tabNavigator.ChangeTabTo<DistributionTab>);
                }
                catch (Exception)
                {
                    // ignore
                }
            }
        }, _cancellationTokenSource.Token);

        return Task.CompletedTask;
    }

    public async Task Stop()
    {
        if (_cancellationTokenSource is not null)
        {
            await _cancellationTokenSource.CancelAsync();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        if (_backgroundTask is not null)
        {
            try
            {
                await _backgroundTask;
            }
            catch (TaskCanceledException)
            {
                // ignore
            }

            _backgroundTask = null;
        }
    }

    public async ValueTask DisposeAsync()
    {
        await Stop();
    }

    private static TimeSpan GetDelayUntilNextMidnight(short timezone)
    {
        var now = DateTime.UtcNow;
        var nextMidnight = now.Date.AddDays(1).AddHours(timezone);
        return nextMidnight - now;
    }
}

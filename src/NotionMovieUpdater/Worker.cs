using NotionMovieUpdater.Services;

namespace NotionMovieUpdater;

public class Worker: BackgroundService
{
    private readonly INotionUpdaterService _service;
    private readonly IHostApplicationLifetime _app;
    private readonly ILogger<Worker> _logger;

    public Worker(INotionUpdaterService service, IHostApplicationLifetime app, ILogger<Worker> logger)
    {
        _service = service;
        _app = app;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _service.Update(stoppingToken);

        _app.StopApplication();
    }
}
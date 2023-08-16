using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace influxdb_client_csharp
{
    class BookHostedService : BackgroundService
    {
        private readonly Random random = new();
        private readonly IServiceScopeFactory scopeFactory;
        private readonly ILogger<BookHostedService> logger;

        public BookHostedService(IServiceScopeFactory scopeFactory, ILogger<BookHostedService> logger)
        {
            this.scopeFactory = scopeFactory;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var count = 1000;
                var stopwatch = new Stopwatch();
                stopwatch.Start();

                var tasks = Enumerable.Range(0, count).Select(i => ReadAsync());
                await Task.WhenAll(tasks);

                stopwatch.Stop();
                this.logger.LogInformation($"并发读写各{count}次总耗时：{stopwatch.Elapsed}");
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "influxdb_client_csharp异常");
            }
        }

        private async Task ReadAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();
           
            var books = await service.GetBooksAsync();

            foreach (var book in books)
            {
                Console.WriteLine(JsonConvert.SerializeObject(book));
            }
        }

        private async Task WriteAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();
            var book = new Book2
            {
                Serie = "科幻",
                Name = $"{random.Next(1, 100)}体",
                Price = random.NextDouble() * 100d,
                SpecialOffer = random.NextDouble() < 0.5d,
                CreateTime = DateTime.UtcNow
            };

            await service.AddAsync(book);
           
        }

        private async Task ReadWriteAsync()
        {
            using var scope = scopeFactory.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<BookService>();
            var book = new Book2
            {
                Serie = "科幻",
                Name = $"{random.Next(1, 100)}体",
                Price = random.NextDouble() * 100d,
                SpecialOffer = random.NextDouble() < 0.5d,
                CreateTime = DateTime.UtcNow
            };

            await service.AddAsync(book);
            var books = await service.GetBooksAsync();
        }
    }
}

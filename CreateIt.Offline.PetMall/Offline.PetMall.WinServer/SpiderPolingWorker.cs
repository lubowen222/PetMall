using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using EasyFrame.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Offline.PetMall.WinServer
{
    public class SpiderPolingWorker : BackgroundService
    {
        //private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private static readonly TimeSpan _hotelUrlInterval = TimeSpan.FromMinutes(5);
        private static readonly TimeSpan _checkPriceInterval = TimeSpan.FromMinutes(5);

        #region .Ctor
        private readonly IConfiguration _configuration;
        private readonly ILogger<SpiderPolingWorker> _logger;

        public SpiderPolingWorker(IConfiguration configuration,
            ILogger<SpiderPolingWorker> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        /// <summary>
        /// 重写Start
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SpiderPolingWorker starting at: {time}", DateTimeOffset.Now);
            Console.WriteLine("...SpiderPolingWorker starting");
            await base.StartAsync(cancellationToken);
        }

        /// <summary>
        /// 重写Stop
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SpiderPolingWorker stopping at: {time}", DateTimeOffset.Now);
            await base.StopAsync(cancellationToken);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine("...SpiderPolingWorker Executing");
                _logger.LogInformation("SpiderPolingWorker execute at: {time}", DateTimeOffset.Now);

                // 启动时强制回收内存
                //ForceMemoryCleanup();

                var tasks = new List<Task>
                {
                    RunSpiderHotelUrl(stoppingToken),
                    RunSpiderHotelCheckPrice(stoppingToken),
                    RunSpiderPolingHotelPrice(stoppingToken)
                };

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError("SpiderPolingWorker Executing Error: {message}", ex.Message);
            }
        }

        /// <summary>
        /// 爬虫爬官网详情页url
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected async Task RunSpiderHotelUrl(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred in RunSpiderHotelUrl: {ex.Message}");
                }
                finally
                {
                    await Task.Delay(_hotelUrlInterval, stoppingToken);
                }
            }
        }

        /// <summary>
        /// 爬虫官网有价查询
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected async Task RunSpiderHotelCheckPrice(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred in RunSpiderHotelCheckPrice: {ex.Message}");
                }
                finally
                {
                    await Task.Delay(_checkPriceInterval, stoppingToken);
                }
            }
        }

        /// <summary>
        /// 爬虫有价酒店询价查询
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected async Task RunSpiderPolingHotelPrice(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred in RunSpiderPolingHotelPrice: {ex.Message}");
                }
            }
        }
    }
}

using Offline.PetMall.Application.StaticDownload;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Offline.PetMall.ConsoleServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("京东商品图片下载工具");
            Console.WriteLine("====================");

            // 示例参数，可以从命令行参数获取
            string url = "https://item.jd.com/10064770409766.html?spmTag=YTAyNDAuYjAwMjQ5My5jMDAwMDQwMjcuMyUyM3NrdV9jYXJk&pvid=ca4d8265e2a64ccba423b2dce55c374f";
            string saveDir = @"E:\JDImages";
            string areaId1 = "main-image"; // 缩略图区域ID
            string areaId2 = "detail-main"; // 详情图区域ID

            if (args.Length == 0)
            {
                Console.WriteLine("使用示例参数，您也可以通过命令行参数传入：");
                Console.WriteLine("用法: Offline.PetMall.Console.exe <URL> <保存目录> [缩略图区域ID] [详情图区域ID]");
                Console.WriteLine();
            }

            Console.WriteLine($"商品URL: {url}");
            Console.WriteLine($"保存目录: {saveDir}");
            if (!string.IsNullOrWhiteSpace(areaId1))
                Console.WriteLine($"缩略图区域ID: {areaId1}");
            if (!string.IsNullOrWhiteSpace(areaId2))
                Console.WriteLine($"详情图区域ID: {areaId2}");
            Console.WriteLine();

            try
            {
                // 如果HttpClient方式失败，可以尝试使用Selenium（需要Chrome浏览器）
                // 设置 useSelenium = true 来使用Selenium获取动态渲染的内容
                bool useSelenium = true;
                
                JDDownloadHelper helper;
                if (useSelenium)
                {
                    Console.WriteLine("使用Selenium模式（需要Chrome浏览器）...");
                    helper = new JDDownloadHelper(useSelenium: true);
                }
                else
                {
                    Console.WriteLine("使用HttpClient模式...");
                    helper = new JDDownloadHelper();
                }

                Console.WriteLine("开始下载...");
                var result = await helper.DownloadProductImagesAsync(url, saveDir, areaId1, areaId2);

                Console.WriteLine();
                Console.WriteLine("====================");
                if (result.Success)
                {
                    Console.WriteLine("下载完成！");
                    Console.WriteLine($"状态: {result.Message}");
                    
                    if (!string.IsNullOrWhiteSpace(result.ProductId))
                    {
                        Console.WriteLine($"商品编号: {result.ProductId}");
                    }

                    if (result.ThumbnailResult != null)
                    {
                        Console.WriteLine($"缩略图 - 成功: {result.ThumbnailResult.SuccessCount}, 失败: {result.ThumbnailResult.FailCount}");
                    }

                    if (result.DetailResult != null)
                    {
                        Console.WriteLine($"详情图 - 成功: {result.DetailResult.SuccessCount}, 失败: {result.DetailResult.FailCount}");
                    }
                }
                else
                {
                    Console.WriteLine("下载失败！");
                    Console.WriteLine($"错误信息: {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发生异常: {ex.Message}");
                Console.WriteLine($"堆栈跟踪: {ex.StackTrace}");
                
                // 如果HttpClient方式失败，提示可以尝试Selenium
                if (!args.Any(a => a.ToLower() == "selenium"))
                {
                    Console.WriteLine();
                    Console.WriteLine("提示：如果遇到反爬虫限制，可以尝试使用Selenium模式：");
                    Console.WriteLine("在命令行参数最后添加 'selenium' 参数");
                }
            }
            finally
            {
                // 确保资源释放
            }

            Console.WriteLine();
            Console.WriteLine("按任意键退出...");
            Console.ReadKey();
        }
    }
}

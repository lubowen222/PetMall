using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Offline.PetMall.Application.StaticDownload
{
    /// <summary>
    /// 京东静态数据下载
    /// </summary>
    public class JDDownloadHelper : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _useSelenium;
        private ChromeDriver _chromeDriver;
        private bool _disposed = false;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpClient">HttpClient实例（可选，如果不提供则创建新的）</param>
        /// <param name="useSelenium">是否使用Selenium获取动态内容（默认false）</param>
        public JDDownloadHelper(HttpClient httpClient = null, bool useSelenium = false)
        {
            _useSelenium = useSelenium;
            
            if (httpClient != null)
            {
                _httpClient = httpClient;
            }
            else
            {
                _httpClient = CreateHttpClient();
            }

            if (_useSelenium)
            {
                InitializeSelenium();
            }
        }

        /// <summary>
        /// 创建配置好的HttpClient
        /// </summary>
        private HttpClient CreateHttpClient()
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                MaxAutomaticRedirections = 5,
                // 启用自动解压，支持 gzip, deflate, br 压缩
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | 
                                         System.Net.DecompressionMethods.Deflate | 
                                         System.Net.DecompressionMethods.Brotli
            };

            var client = new HttpClient(handler);
            
            // 设置完整的浏览器请求头，模拟真实浏览器
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            client.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            client.DefaultRequestHeaders.Add("Connection", "keep-alive");
            client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");
            client.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
            
            client.Timeout = TimeSpan.FromSeconds(30);

            return client;
        }

        /// <summary>
        /// 初始化Selenium WebDriver
        /// </summary>
        private void InitializeSelenium()
        {
            int maxRetries = 3;
            int retryCount = 0;
            Exception lastException = null;

            while (retryCount < maxRetries)
            {
                try
                {
                    var options = new ChromeOptions();
                    
                    // 无头模式
                    options.AddArgument("--headless=new"); // 使用新的headless模式
                    options.AddArgument("--no-sandbox");
                    options.AddArgument("--disable-dev-shm-usage");
                    options.AddArgument("--disable-blink-features=AutomationControlled");
                    options.AddArgument("--disable-gpu");
                    options.AddArgument("--window-size=1920,1080");
                    
                    // 性能优化选项，加快启动速度
                    options.AddArgument("--disable-extensions");
                    options.AddArgument("--disable-plugins");
                    options.AddArgument("--disable-background-networking");
                    options.AddArgument("--disable-background-timer-throttling");
                    options.AddArgument("--disable-renderer-backgrounding");
                    options.AddArgument("--disable-backgrounding-occluded-windows");
                    options.AddArgument("--disable-ipc-flooding-protection");
                    options.AddArgument("--disable-features=TranslateUI");
                    options.AddArgument("--disable-features=BlinkGenPropertyTrees");
                    options.AddArgument("--disable-component-extensions-with-background-pages");
                    options.AddArgument("--disable-default-apps");
                    options.AddArgument("--disable-sync");
                    options.AddArgument("--metrics-recording-only");
                    options.AddArgument("--mute-audio");
                    options.AddArgument("--no-first-run");
                    options.AddArgument("--safebrowsing-disable-auto-update");
                    options.AddArgument("--disable-web-security");
                    options.AddArgument("--disable-features=VizDisplayCompositor");
                    
                    // 设置User-Agent
                    options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                    
                    // 禁用自动化标识
                    options.AddExcludedArgument("enable-automation");
                    options.AddAdditionalOption("useAutomationExtension", false);
                    
                    // 设置页面加载策略为eager（不等待所有资源）
                    options.PageLoadStrategy = PageLoadStrategy.Eager;

                    var service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true; // 隐藏命令行窗口
                    service.SuppressInitialDiagnosticInformation = true; // 抑制初始诊断信息
                    
                    // 使用更短的超时时间创建驱动，如果失败再重试
                    _chromeDriver = new ChromeDriver(service, options, TimeSpan.FromSeconds(30));
                    
                    // 设置超时时间
                    _chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                    _chromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                    _chromeDriver.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromSeconds(30);
                    
                    // 测试驱动是否正常工作
                    //_chromeDriver.Manage().Window.Size = new OpenQA.Selenium.SizSize(1920, 1080);
                    
                    return; // 成功初始化，退出
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    retryCount++;
                    
                    // 清理失败的驱动实例
                    try
                    {
                        _chromeDriver?.Quit();
                        _chromeDriver?.Dispose();
                        _chromeDriver = null;
                    }
                    catch { }
                    
                    if (retryCount < maxRetries)
                    {
                        // 等待后重试
                        System.Threading.Thread.Sleep(2000 * retryCount);
                    }
                }
            }
            
            // 所有重试都失败
            string errorMessage = $"初始化Selenium失败（已重试{maxRetries}次）: {lastException?.Message}";
            if (lastException != null && lastException.Message.Contains("timed out"))
            {
                errorMessage += "\n提示：Chrome浏览器可能启动过慢，请检查：";
                errorMessage += "\n1. Chrome浏览器是否正确安装";
                errorMessage += "\n2. ChromeDriver版本是否与Chrome浏览器版本匹配";
                errorMessage += "\n3. 系统资源是否充足（内存、CPU）";
                errorMessage += "\n4. 是否有其他Chrome进程占用资源";
            }
            
            throw new Exception(errorMessage, lastException);
        }

        /// <summary>
        /// 下载结果
        /// </summary>
        public class DownloadResult
        {
            /// <summary>
            /// 是否成功
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// 状态消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 商品编号
            /// </summary>
            public string ProductId { get; set; }

            /// <summary>
            /// 缩略图下载结果
            /// </summary>
            public ThumbnailResult ThumbnailResult { get; set; }

            /// <summary>
            /// 详情图下载结果
            /// </summary>
            public DetailResult DetailResult { get; set; }
        }

        /// <summary>
        /// 缩略图下载结果
        /// </summary>
        public class ThumbnailResult
        {
            public int SuccessCount { get; set; }
            public int FailCount { get; set; }
        }

        /// <summary>
        /// 详情图下载结果
        /// </summary>
        public class DetailResult
        {
            public int SuccessCount { get; set; }
            public int FailCount { get; set; }
        }

        /// <summary>
        /// 根据京东商品页URL下载商品图片
        /// </summary>
        /// <param name="url">京东商品页URL</param>
        /// <param name="saveDir">保存目录</param>
        /// <param name="areaId1">缩略图区域ID</param>
        /// <param name="areaId2">详情图区域ID</param>
        /// <param name="maxRetries">最大重试次数（默认3次）</param>
        /// <returns>下载结果</returns>
        public async Task<DownloadResult> DownloadProductImagesAsync(string url, string saveDir, string areaId1 = null, string areaId2 = null, int maxRetries = 3)
        {
            var result = new DownloadResult();

            try
            {
                // 验证参数
                if (string.IsNullOrWhiteSpace(url))
                {
                    result.Success = false;
                    result.Message = "URL不能为空！";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(saveDir))
                {
                    result.Success = false;
                    result.Message = "保存目录不能为空！";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(areaId1) && string.IsNullOrWhiteSpace(areaId2))
                {
                    result.Success = false;
                    result.Message = "请至少输入一个区域ID！";
                    return result;
                }

                // 获取HTML内容（带重试机制）
                result.Message = "正在获取页面内容...";
                string htmlContent = null;
                int retryCount = 0;
                Exception lastException = null;

                while (retryCount < maxRetries && string.IsNullOrWhiteSpace(htmlContent))
                {
                    try
                    {
                        if (_useSelenium && _chromeDriver != null)
                        {
                            htmlContent = await GetHtmlWithSeleniumAsync(url);
                        }
                        else
                        {
                            htmlContent = await GetHtmlWithHttpClientAsync(url);
                        }

                        if (!string.IsNullOrWhiteSpace(htmlContent))
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        lastException = ex;
                        retryCount++;
                        if (retryCount < maxRetries)
                        {
                            await Task.Delay(1000 * retryCount); // 递增延迟
                            result.Message = $"获取页面内容失败，正在重试 ({retryCount}/{maxRetries})...";
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(htmlContent))
                {
                    result.Success = false;
                    result.Message = $"获取页面内容失败：{lastException?.Message ?? "未知错误"}";
                    return result;
                }

                // 解析HTML
                result.Message = "正在解析HTML...";
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlContent);

                // 获取商品编号
                var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
                if (bodyNode != null)
                {
                    string classValue = bodyNode.GetAttributeValue("class", "");
                    var match = Regex.Match(classValue, @"item-(\d+)");
                    if (match.Success)
                    {
                        result.ProductId = match.Groups[1].Value;
                        saveDir = Path.Combine(saveDir, result.ProductId);
                    }
                }

                // 确保保存目录存在
                if (!Directory.Exists(saveDir))
                {
                    Directory.CreateDirectory(saveDir);
                }

                result.ThumbnailResult = new ThumbnailResult();
                result.DetailResult = new DetailResult();

                // 处理缩略图
                if (!string.IsNullOrWhiteSpace(areaId1))
                {
                    result.Message = "正在下载缩略图...";
                    var thumbnailResult = await DownloadThumbnailsAsync(doc, areaId1, saveDir);
                    result.ThumbnailResult = thumbnailResult;
                }

                // 处理详情图
                if (!string.IsNullOrWhiteSpace(areaId2))
                {
                    result.Message = "正在下载详情图...";
                    var detailResult = await DownloadDetailsAsync(doc, areaId2, saveDir);
                    result.DetailResult = detailResult;
                }

                // 构建状态消息
                var statusMsg = new StringBuilder();
                if (result.ThumbnailResult != null)
                {
                    statusMsg.Append($"缩略图：成功{result.ThumbnailResult.SuccessCount}，失败{result.ThumbnailResult.FailCount}。 ");
                }
                if (result.DetailResult != null)
                {
                    statusMsg.Append($"详情图：成功{result.DetailResult.SuccessCount}，失败{result.DetailResult.FailCount}。 ");
                }

                result.Success = true;
                result.Message = statusMsg.ToString();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"下载失败：{ex.Message}";
            }

            return result;
        }

        /// <summary>
        /// 使用HttpClient获取HTML内容
        /// </summary>
        private async Task<string> GetHtmlWithHttpClientAsync(string url)
        {
            // 设置Referer
            if (_httpClient.DefaultRequestHeaders.Contains("Referer"))
            {
                _httpClient.DefaultRequestHeaders.Remove("Referer");
            }
            _httpClient.DefaultRequestHeaders.Add("Referer", "https://www.jd.com/");

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            // 读取字节数组，然后根据响应头或HTML meta标签确定编码
            var bytes = await response.Content.ReadAsByteArrayAsync();
            
            // 尝试从Content-Type响应头获取编码
            string charset = null;
            if (response.Content.Headers.ContentType?.CharSet != null)
            {
                charset = response.Content.Headers.ContentType.CharSet.Trim('"', '\'');
            }
            
            // 如果响应头没有编码信息，尝试从HTML内容中提取
            if (string.IsNullOrEmpty(charset))
            {
                string htmlPreview = Encoding.UTF8.GetString(bytes, 0, Math.Min(1024, bytes.Length));
                var charsetMatch = Regex.Match(htmlPreview, @"charset\s*=\s*[""']?([^""'\s>]+)[""']?", RegexOptions.IgnoreCase);
                if (charsetMatch.Success)
                {
                    charset = charsetMatch.Groups[1].Value.Trim();
                }
            }
            
            // 根据编码解码，默认使用UTF-8
            Encoding encoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(charset))
            {
                try
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                catch
                {
                    // 如果编码名称无效，使用UTF-8
                    encoding = Encoding.UTF8;
                }
            }
            
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 使用Selenium获取HTML内容
        /// </summary>
        private async Task<string> GetHtmlWithSeleniumAsync(string url)
        {
            return await Task.Run(() =>
            {
                try
                {
                    // 设置页面加载超时
                    _chromeDriver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                    
                    // 导航到页面，使用eager策略不等待所有资源
                    _chromeDriver.Navigate().GoToUrl(url);
                    
                    // 使用更健壮的等待策略
                    var wait = new WebDriverWait(_chromeDriver, TimeSpan.FromSeconds(60))
                    {
                        PollingInterval = TimeSpan.FromMilliseconds(500)
                    };
                    
                    // 等待页面基本加载完成（不等待所有资源）
                    wait.Until(driver => 
                    {
                        try
                        {
                            var readyState = ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").ToString();
                            return readyState == "complete" || readyState == "interactive";
                        }
                        catch (WebDriverTimeoutException)
                        {
                            // 超时也返回true，至少获取部分内容
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                    });

                    // 等待一小段时间让关键内容加载
                    Thread.Sleep(6000); // 给JavaScript一些执行时间

                    // 尝试执行JavaScript来触发懒加载内容
                    try
                    {
                        ((IJavaScriptExecutor)_chromeDriver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                        Thread.Sleep(1000);
                        ((IJavaScriptExecutor)_chromeDriver).ExecuteScript("window.scrollTo(0, 0);");
                        Thread.Sleep(1000);
                    }
                    catch
                    {
                        // 忽略JS执行错误
                    }

                    return _chromeDriver.PageSource;
                }
                catch (WebDriverTimeoutException ex)
                {
                    // 即使超时，也尝试获取当前页面源码
                    try
                    {
                        return _chromeDriver?.PageSource ?? throw new Exception("无法获取页面源码");
                    }
                    catch
                    {
                        throw new Exception($"Selenium获取页面超时: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Selenium获取页面失败: {ex.Message}", ex);
                }
            });
        }

        /// <summary>
        /// 下载缩略图
        /// </summary>
        private async Task<ThumbnailResult> DownloadThumbnailsAsync(HtmlDocument doc, string areaId, string saveDir)
        {
            var result = new ThumbnailResult();
            var node = doc.DocumentNode.SelectSingleNode($"//*[@id='{areaId}']");

            if (node == null)
            {
                return result;
            }

            var imgNodes = node.Descendants("img")
                .Select(img => img.GetAttributeValue("src", null) ?? img.GetAttributeValue("data-lazy-img", null) ?? img.GetAttributeValue("data-src", null))
                .Where(src => !string.IsNullOrEmpty(src))
                .ToList();

            int index = 1;
            string thumbDir = Path.Combine(saveDir, "缩略图");

            foreach (var imgUrl in imgNodes)
            {
                try
                {
                    if (imgUrl.EndsWith(".avif", StringComparison.OrdinalIgnoreCase) || 
                        imgUrl.Contains(".avif", StringComparison.OrdinalIgnoreCase))
                    {
                        var newImgUrl = imgUrl;
                        if (newImgUrl.StartsWith("//"))
                        {
                            newImgUrl = "https:" + newImgUrl;
                        }
                        if (newImgUrl.EndsWith(".jpg.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".jpg.avif", ".jpg");
                        }
                        if (newImgUrl.EndsWith(".jpeg.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".jpeg.avif", ".jpeg");
                        }
                        if (newImgUrl.EndsWith(".png.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".png.avif", ".png");
                        }
                        if (newImgUrl.EndsWith(".gif.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".gif.avif", ".gif");
                        }

                        // 正则：n5/后面跟s开头的一串非/字符
                        var match = Regex.Match(newImgUrl, @"n5/(s\d+x\d+)");
                        var imgSize = "";
                        if (match.Success)
                        {
                            imgSize = match.Groups[1].Value;
                        }

                        if (!string.IsNullOrWhiteSpace(imgSize))
                        {
                            await DownloadImageWithRetryAsync(thumbDir, newImgUrl, index, imgSize);

                            var newSize = "s720x720";
                            newImgUrl = newImgUrl.Replace(imgSize, newSize);
                            await DownloadImageWithRetryAsync(thumbDir, newImgUrl, index, newSize);
                        }

                        index++;
                        result.SuccessCount++;
                        
                        // 添加延迟，避免请求过快
                        await Task.Delay(200);
                    }
                    else
                    {
                        result.FailCount++;
                    }
                }
                catch
                {
                    result.FailCount++;
                }
            }

            return result;
        }

        /// <summary>
        /// 下载详情图
        /// </summary>
        private async Task<DetailResult> DownloadDetailsAsync(HtmlDocument doc, string areaId, string saveDir)
        {
            var result = new DetailResult();
            var node = doc.DocumentNode.SelectSingleNode($"//*[@id='{areaId}']");

            if (node == null)
            {
                return result;
            }

            var imgNodes = node.Descendants("img")
                .Select(img => img.GetAttributeValue("src", null) ?? img.GetAttributeValue("data-lazy-img", null) ?? img.GetAttributeValue("data-src", null))
                .Where(src => !string.IsNullOrEmpty(src))
                .ToList();

            int index = 1;
            string detailDir = Path.Combine(saveDir, "详情图");

            if (!Directory.Exists(detailDir))
            {
                Directory.CreateDirectory(detailDir);
            }

            foreach (var imgUrl in imgNodes)
            {
                try
                {
                    if (imgUrl.EndsWith("avif", StringComparison.OrdinalIgnoreCase) || 
                        imgUrl.Contains(".avif", StringComparison.OrdinalIgnoreCase))
                    {
                        var newImgUrl = imgUrl;
                        if (newImgUrl.StartsWith("//"))
                        {
                            newImgUrl = "https:" + newImgUrl;
                        }
                        if (newImgUrl.EndsWith(".jpg.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".jpg.avif", ".jpg");
                        }
                        if (newImgUrl.EndsWith(".jpeg.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".jpeg.avif", ".jpeg");
                        }
                        if (newImgUrl.EndsWith(".png.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".png.avif", ".png");
                        }
                        if (newImgUrl.EndsWith(".gif.avif", StringComparison.OrdinalIgnoreCase))
                        {
                            newImgUrl = newImgUrl.Replace(".gif.avif", ".gif");
                        }

                        await DownloadImageWithRetryAsync(detailDir, newImgUrl, index, null);

                        index++;
                        result.SuccessCount++;
                        
                        // 添加延迟，避免请求过快
                        await Task.Delay(200);
                    }
                    else
                    {
                        result.FailCount++;
                    }
                }
                catch
                {
                    result.FailCount++;
                }
            }

            return result;
        }

        /// <summary>
        /// 下载图片到指定目录（带重试）
        /// </summary>
        private async Task DownloadImageWithRetryAsync(string saveDir, string imgUrl, int index, string size, int maxRetries = 3)
        {
            string finalSaveDir = saveDir;
            if (!string.IsNullOrWhiteSpace(size))
            {
                finalSaveDir = Path.Combine(saveDir, size);
            }

            if (!Directory.Exists(finalSaveDir))
            {
                Directory.CreateDirectory(finalSaveDir);
            }

            var ext = Path.GetExtension(imgUrl);
            if (string.IsNullOrEmpty(ext) || ext.Length > 5) ext = ".jpg";
            
            string fileName = string.IsNullOrWhiteSpace(size) 
                ? $"img_{index}{ext}" 
                : $"img_{size}_{index}{ext}";
            var savePath = Path.Combine(finalSaveDir, fileName);

            int retryCount = 0;
            while (retryCount < maxRetries)
            {
                try
                {
                    // 设置Referer为京东域名
                    var request = new HttpRequestMessage(HttpMethod.Get, imgUrl);
                    request.Headers.Add("Referer", "https://item.jd.com/");
                    request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");

                    var response = await _httpClient.SendAsync(request);
                    response.EnsureSuccessStatusCode();

                    var imgBytes = await response.Content.ReadAsByteArrayAsync();
                    await File.WriteAllBytesAsync(savePath, imgBytes);
                    return;
                }
                catch
                {
                    retryCount++;
                    if (retryCount < maxRetries)
                    {
                        await Task.Delay(500 * retryCount);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _chromeDriver?.Quit();
                _chromeDriver?.Dispose();
                _httpClient?.Dispose();
                _disposed = true;
            }
        }
    }
}

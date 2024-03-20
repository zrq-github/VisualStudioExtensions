using Microsoft.Web.WebView2.Core;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace LeetCodeVsExtension
{
    public partial class MyToolWindowControl : UserControl
    {
        static string WebView2UserDataFolderPath = $@"{Path.Combine(PackageGuids.LeetCodeVsExtensionString, "WebView2Runing")}";

        public MyToolWindowControl()
        {
            InitializeComponent();

            ThreadHelper.JoinableTaskFactory.Run(delegate {
                _ = InitializeWebViewAsync();
                return Task.CompletedTask;
            });
        }

        #region WebView2

        private void SetWebView2Color()
        {
            Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "0");
        }

        #endregion

        #region LeetCode

        /// <summary>
        /// LeetCode 跟随系统
        /// </summary>
        /// <remarks>
        /// 因为不知道设置 WebView2 本身默认的设置, 暂时先不使用
        /// </remarks>
        private void SetLeetCodeColorToSystem()
        {
            throw new NotImplementedException();
            _ = this.myWebView2.ExecuteScriptAsync("document.getElementByClassName('flex cursor-pointer items-center space-x-3 rounded px-2 py-[10px] hover:bg-fill-4 dark:hover:bg-dark-fill-4 text-text-secondary dark:text-text-secondary')[0].click()");
        }
        /// <summary>
        /// LeetCode 外观 浅色
        /// </summary>
        private async Task SetLeetCodeColorToLightAsync()
        {
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[9].click()");
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        }
        /// <summary>
        /// LeetCode 外观 深色
        /// </summary>
        private async Task SetLeetCodeColorToDarkAsync()
        {
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('grow text-left')[10].click()");
            _ = await this.myWebView2.ExecuteScriptAsync("document.getElementsByClassName('flex items-center focus:outline-none')[2].click()");
        }

        #endregion

        private void InitializeWebViewEvent()
        {
            this.myWebView2.CoreWebView2InitializationCompleted += MyWebView2_CoreWebView2InitializationCompleted;
            // 禁用打开新的窗口, 直接就在当前页面进行跳转
            this.myWebView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        }

        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            e.Handled = true;
            this.myWebView2.CoreWebView2.Navigate(e.Uri);
        }

        private void MyWebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            Console.WriteLine("WebView2 Initialization Completed");
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        private void HandleError(string message, Exception exception = null) => Console.WriteLine(message, exception);

        /// <summary>
        /// 初始化WebView2
        /// </summary>
        private async Task InitializeWebViewAsync()
        {
            try
            {
                var userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), WebView2UserDataFolderPath);
                var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
                await this.myWebView2.EnsureCoreWebView2Async(environment);

                SetWebView2Color();

                // 增加一些事件操作
                InitializeWebViewEvent();

                this.myWebView2.Source = new Uri(@"https://leetcode.cn/");
            }
            catch (Exception ex)
            {
                HandleError(nameof(InitializeWebViewAsync), ex);
            }
        }
        private void btnDarkColor_Click(object sender, RoutedEventArgs e)
        {
            _ = SetLeetCodeColorToDarkAsync();
        }

        private void btnLightColor_Click(object sender, RoutedEventArgs e)
        {
            _ = SetLeetCodeColorToLightAsync();
        }
    }
}
using Microsoft.VisualStudio.Shell;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

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

        /// <summary>
        /// 错误消息
        /// </summary>
        private void HandleError(string message, Exception exception = null) => Console.WriteLine(message, exception);

        private async Task InitializeWebViewAsync()
        {
            try
            {
                var userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), WebView2UserDataFolderPath);
                var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder, null);
                await this.myWebView2.EnsureCoreWebView2Async(environment);

                this.myWebView2.Source = new Uri(@"https://leetcode.cn/");
            }
            catch (Exception ex)
            {
                HandleError(nameof(InitializeWebViewAsync), ex);
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            VS.MessageBox.Show("LeetCodeVsExtension", "Button clicked");
        }
    }
}
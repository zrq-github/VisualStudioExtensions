using Microsoft.VisualStudio.Shell;
using Microsoft.Web.WebView2.Core;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace FirstWindow
{
    /// <summary>
    /// Interaction logic for MyWindowControl.
    /// </summary>
    public partial class MyWindowControl : UserControl, IComponentConnector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyWindowControl"/> class.
        /// </summary>
        public MyWindowControl()
        {
            this.InitializeComponent();

            ThreadHelper.JoinableTaskFactory.Run(async delegate {
                InitializeWebViewAsync();
            });
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        private void HandleError(string message, Exception exception = null) => Console.WriteLine(message, exception);

        /// <summary>
        /// 异步初始化WebView环境
        /// </summary>
        private async void InitializeWebViewAsync()
        {
            try
            {
                var userDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "VS2022WebBrowserExtension");
                var environment = await CoreWebView2Environment.CreateAsync(null, userDataFolder);
                await this.myWebView2.EnsureCoreWebView2Async(environment);

                this.myWebView2.Source = new Uri(@"https://www.baidu.com/");
            }
            catch (Exception ex)
            {
                HandleError(nameof(InitializeWebViewAsync), ex);
            }
            

            //// create webview2 environment and load the webview
            //string webviewDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ////webviewDirectory = Path.Combine(webviewDirectory, "MyWebView2Directory");
            //webviewDirectory = @"D:\source\repos\github\VisualStudioExtensions\LearingMicrosoft\VSIXProject\FirstWindow\bin\Debug\WebView2";
            ////Directory.CreateDirectory(webviewDirectory);
            //var env = await CoreWebView2Environment.CreateAsync(webviewDirectory);
        }

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("load webview2 and open leet code");

        }
    }
}
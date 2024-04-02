using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using LeetCodeVsExtension.Utils;
using Microsoft.Web.WebView2.Core;
using MessageBox = Community.VisualStudio.Toolkit.MessageBox;

namespace LeetCodeVsExtension;

public partial class MyToolWindowControl : UserControl
{
    public MyToolWindowControl()
    {
        InitializeComponent();

        ThreadHelper.JoinableTaskFactory.Run(delegate
        {
            _ = InitializeWebViewAsync();
            return Task.CompletedTask;
        });
    }

    private void BtnBack_Click(object sender, RoutedEventArgs e)
    {
        MyWebView2.GoBack();
    }

    private void BtnClearWebData_Click(object sender, RoutedEventArgs e)
    {
        _ = ClearBrowsingDataAsync();
    }

    private async void BtnCopyText_ClickAsync(object sender, RoutedEventArgs e)
    {
        var message = new MessageBox();
        var text = await GetSelectTestAsync();
        if (!text.Any() || text.Equals("\"\""))
        {
            _ = await message.ShowErrorAsync("请选择输出");
            return;
        }

        try
        {
            var code = LeetCodeTopicUtil.TestCase2CSharpCode(text);
            Clipboard.SetText(code);
        }
        catch (Exception ex)
        {
            _ = await message.ShowErrorAsync(ex.ToString());
        }
    }

    private void BtnDarkColor_Click(object sender, RoutedEventArgs e)
    {
        _ = LeetCodeWebView2Util.ColorToDarkAsync(MyWebView2);
    }

    private void BtnForward_Click(object sender, RoutedEventArgs e)
    {
        MyWebView2.GoForward();
    }

    private void BtnLightColor_Click(object sender, RoutedEventArgs e)
    {
        _ = LeetCodeWebView2Util.ColorToLightAsync(MyWebView2);
    }

    private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
    {
        e.Handled = true;
        MyWebView2.CoreWebView2.Navigate(e.Uri);
    }

    /// <summary>
    /// 错误消息
    /// </summary>
    private void HandleError(string message, Exception exception = null)
    {
        Console.WriteLine(message, exception);
    }

    /// <summary>
    /// 初始化WebView2
    /// </summary>
    private async Task InitializeWebViewAsync()
    {
        try
        {
            var environment = await CoreWebView2Environment.CreateAsync(null, VsExtensionUtil.WebView2UserDataFolderPath);
            await MyWebView2.EnsureCoreWebView2Async(environment);

            SetWebView2Color();

            // 增加一些事件操作
            InitializeWebViewEvent();

            MyWebView2.Source = new Uri(@"https://leetcode.cn/");
        }
        catch (Exception ex)
        {
            HandleError(nameof(InitializeWebViewAsync), ex);
        }
    }

    private void MyWebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        Console.WriteLine("WebView2 Initialization Completed");
    }

    #region WebView2

    private void SetWebView2Color()
    {
        Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "0");
    }

    /// <summary>
    /// 清理浏览器的全部数据
    /// </summary>
    private async Task ClearBrowsingDataAsync()
    {
        await MyWebView2.CoreWebView2.Profile.ClearBrowsingDataAsync();
    }

    private void InitializeWebViewEvent()
    {
        MyWebView2.CoreWebView2InitializationCompleted += MyWebView2_CoreWebView2InitializationCompleted;
        // 禁用打开新的窗口, 直接就在当前页面进行跳转
        MyWebView2.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        MyWebView2.CoreWebView2.WebMessageReceived += CoreWebView2_WebMessageReceived;
    }

    private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
    {
        var selectedTest = e.TryGetWebMessageAsString();
        Console.WriteLine(selectedTest);
    }

    private async Task<string> GetSelectTestAsync()
    {
        var selectTest = await MyWebView2.ExecuteScriptAsync("window.getSelection().toString()");
        return selectTest;
    }

    #endregion
}

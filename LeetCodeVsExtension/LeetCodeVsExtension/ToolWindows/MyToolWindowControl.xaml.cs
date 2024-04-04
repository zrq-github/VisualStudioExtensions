using System.IO;
using System.Linq;
using System.Text;
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
    /// 创建题目
    /// </summary>
    private async Task CreateClassFileAsync()
    {
        // 获取题目名字
        var topicName = await LeetCodeWebView2Util.GetTopicNameAsync(MyWebView2);
        var codeContent = await LeetCodeWebView2Util.GetCode(MyWebView2);

        // 获取激活项目
        var project = await VS.Solutions.GetActiveProjectAsync();
        if (project is null)
        {
            var model = new InfoBarModel("没有激活的项目");
            _ = await VS.InfoBar.CreateAsync(model);
            return;
        }

        // 获取项目目录路径
        var projectPath = project.FullPath;
        if (projectPath is null) return;
        var projectDir = Path.GetDirectoryName(projectPath);
        if (projectDir is null) return;
        var projectDirName = Path.GetFileName(projectDir);

        if (topicName is null or "") return;
        topicName = topicName
            .Replace(". ", "_")
            .Replace(".", "_")
            .Replace(" ", "_");

        var className = $"Topic_{topicName}";
        var classFilePath = Path.Combine(projectDir, $"{className}.cs");
        // 检测文件是否已经存在
        if (File.Exists(classFilePath))
        {
            return;
            var model = new InfoBarModel("文件已经存在");
            _ = await VS.InfoBar.CreateAsync(model);
        }

        var classContent = new StringBuilder();
        classContent.AppendLine($"namespace {projectDirName};");
        classContent.AppendLine($"{codeContent}");
        classContent.Replace("public class Solution", $"public class {className}");
        // 替换混合的行结束符为 CRLF
        classContent.Replace("\r\n", "\n") // 先将 CRLF 替换为 LF
            .Replace("\r", "\n")           // 将 CR 替换为 LF
            .Replace("\n", "\r\n");        // 最后将 LF 替换为 CRLF

        // 创建文件,写入文件信息
        File.Create(classFilePath).Close();
        File.WriteAllText(classFilePath, classContent.ToString(), Encoding.UTF8);

        // 将文件添加到项目中
        _ = await project.AddExistingFilesAsync(classFilePath);
    }

    /// <summary>
    /// 创建题目事件
    /// </summary>
    private void CreateLeetCodeTopic(object sender, RoutedEventArgs e)
    {
        _ = CreateClassFileAsync();
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

# VisualStudioExtensions
VS扩展的开发

## LeetCodeVsExtension 

使用WebView2, 将LeetCode的界面内置到 Visual Studio 中, 实现更加方便刷取每日一题的内容

### 计划列表 

- [x] 内嵌LeetCode网站
- [X] 禁止页面弹出新的标签页
- [X] 适配LeetCode VS 深色/浅色背景切换(模拟鼠标点击, 不保证时刻有效)
- [ ] 卸载VSIX的回收WebView2的资源文件
- [ ] 增加重置主页的操作
- [ ] 在VS中写好代码, 选中指定代码, 右键/快捷键自动上传提交

**有问题**
- 适配LeetCode的背景设置  
  找不到调用接口; 模拟鼠标点击不行, class id 是动态变换的
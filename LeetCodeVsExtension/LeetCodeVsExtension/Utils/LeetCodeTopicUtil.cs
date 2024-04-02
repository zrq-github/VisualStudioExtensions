using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LeetCodeVsExtension.Utils;

/// <summary>
/// LeetCode 题目相关的辅助函数
/// </summary>
public static class LeetCodeTopicUtil
{
    /// <summary>
    /// 将复制的JS格式的测试用例, 转换成CShapre代码
    /// </summary>
    /// <remarks>
    /// [3,2,2,3] => var input = new List
    /// <int>
    /// (){3,2,2,3};
    /// ["a","b","c"] => var input = new List<string>(){"a","b","c"};
    /// </remarks>
    public static string TestCase2CSharpCode(string testCase)
    {
        if (testCase == null) throw new ArgumentNullException(nameof(testCase));
        if (testCase.Equals(string.Empty) || testCase.Equals("")) return string.Empty;

        JsonDocument jsonDocument;
        var utf8Bytes = Encoding.UTF8.GetBytes(testCase);
        var utf8JsonReader = new Utf8JsonReader(utf8Bytes);
        if (!JsonDocument.TryParseValue(ref utf8JsonReader, out jsonDocument))
        {
            var cleanedStr = JsonSerializer.Deserialize<string>(testCase);
            jsonDocument = JsonDocument.Parse(cleanedStr);
        }

        var root = jsonDocument.RootElement;

        if (root.ValueKind != JsonValueKind.Array) throw new InvalidOperationException("不支持非数组格式");

        var enumerator = root.EnumerateArray();
        if (enumerator.MoveNext())
        {
            var firstElement = enumerator.Current;
            if (firstElement.ValueKind == JsonValueKind.String)
                return TestCase2CSharpCode(jsonDocument.Deserialize<List<string>>());
            if (firstElement.ValueKind == JsonValueKind.Number) return TestCase2CSharpCode(jsonDocument.Deserialize<List<double>>());
        }

        return string.Empty;
    }

    private static string TestCase2CSharpCode(in IEnumerable<string> list)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in list)
        {
            if (item == null)
                continue;

            stringBuilder.Append($" \"{item}\",");
        }

        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        var outStr = $"var input = new List<string>() {{{stringBuilder}}};";
        return outStr;
    }

    private static string TestCase2CSharpCode(in IEnumerable<double> list)
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in list) stringBuilder.Append($" {item},");
        stringBuilder.Remove(stringBuilder.Length - 1, 1);
        var outStr = $"var input = new List<double>() {{{stringBuilder}}};";
        return outStr;
    }
}

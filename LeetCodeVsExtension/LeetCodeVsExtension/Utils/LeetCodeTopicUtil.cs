using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace LeetCodeVsExtension.Utils
{
    /// <summary>
    /// LeetCode 题目相关的辅助函数
    /// </summary>
    public static class LeetCodeTopicUtil
    {
        /// <summary>
        /// 将复制的JS格式的测试用例, 转换成CShapre代码
        /// </summary>
        /// <remarks>
        /// [3,2,2,3] => var input = new List<int>(){3,2,2,3};
        /// ["a","b","c"] => var input = new List<string>(){"a","b","c"};
        /// </remarks>
        public static string TestCase2CShaperCode(string testCase)
        {
            if (testCase == null)
            {
                throw new ArgumentNullException(nameof(testCase));
            }
            if (testCase.Equals(string.Empty) || testCase.Equals(""))
            {
                return string.Empty;
            }

            try
            {
                JsonDocument jsonDocument;
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(testCase);
                Utf8JsonReader utf8JsonReader = new Utf8JsonReader(utf8Bytes);
                if (!JsonDocument.TryParseValue(ref utf8JsonReader, out jsonDocument))
                {
                    string cleanedStr = JsonSerializer.Deserialize<string>(testCase);
                    jsonDocument = JsonDocument.Parse(cleanedStr);
                }

                JsonElement root = jsonDocument.RootElement;

                if (root.ValueKind != JsonValueKind.Array)
                {
                    throw new InvalidOperationException("不支持非数组格式");
                }

                JsonElement.ArrayEnumerator enumerator = root.EnumerateArray();
                if (enumerator.MoveNext())
                {
                    JsonElement firstElement = enumerator.Current;
                    if (firstElement.ValueKind == JsonValueKind.String)
                    {
                        return TestCase2CShaperCode(jsonDocument.Deserialize<List<string>>());
                    }
                    else if (firstElement.ValueKind == JsonValueKind.Number)
                    {
                        return TestCase2CShaperCode(jsonDocument.Deserialize<List<double>>());
                    }
                }
            }
            catch
            {
                throw;
            }

            return string.Empty;
        }

        private static string TestCase2CShaperCode(in IEnumerable<string> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
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
        private static string TestCase2CShaperCode(in IEnumerable<double> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in list)
            {
                stringBuilder.Append($" {item},");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            var outStr = $"var input = new List<double>() {{{stringBuilder}}};";
            return outStr;
        }
    }
}

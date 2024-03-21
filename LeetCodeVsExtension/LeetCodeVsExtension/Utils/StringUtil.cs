using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LeetCodeVsExtension.Utils
{
    internal static class StringUtil
    {
        public class MyList<T> : List<T>
        {
            public override string ToString()
            {
                if (typeof(T) == typeof(string))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in this)
                    {
                        if (item == null)
                            continue;

                        stringBuilder.Append($" \"{item.ToString()}\",");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    var outStr = $"var input = new List<string>(){{ {stringBuilder} }};";
                    return outStr;
                }

                if (typeof(T) == typeof(int)
                    || typeof(T) == typeof(double)
                    || typeof(T) == typeof(float))
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    foreach (var item in this)
                    {
                        if (item == null)
                            continue;

                        stringBuilder.Append($" {item.ToString()},");
                    }
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    var outStr = $"var input = new List<double>(){{ {stringBuilder} }};";
                    return outStr;
                }

                return string.Empty;
            }
        }

        public static string ToListCode(in string inputString, out string errorMsg)
        {
            errorMsg = string.Empty;
            if(inputString == null)
            {
                return string.Empty;
            }

            string cleanedString = JsonSerializer.Deserialize<string>(inputString);

            JsonDocument jsonDocument = JsonDocument.Parse(cleanedString);
            JsonElement root = jsonDocument.RootElement;
            
            if(root.ValueKind != JsonValueKind.Array)
            {
                errorMsg = "不是数组";
                return string.Empty;
            }

            try
            {
                JsonElement.ArrayEnumerator enumerator = root.EnumerateArray();
                if (enumerator.MoveNext())
                {
                    JsonElement firstElement = enumerator.Current;
                    if (firstElement.ValueKind == JsonValueKind.String)
                    {
                        var list = JsonSerializer.Deserialize<MyList<string>>(cleanedString);
                        return list.ToString();
                    }
                    else if (firstElement.ValueKind == JsonValueKind.Number)
                    {
                        var list = JsonSerializer.Deserialize<MyList<double>>(cleanedString);
                        return list.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }
            
            return string.Empty;
        }
    }
}

using Newtonsoft;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text;

namespace TestProject1
{
    public class MyList<T>:List<T>
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

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestCopuInt()
        {
            string inputString = "[1,2,3,0,0,0]";
            dynamic? dyObject = JsonConvert.DeserializeObject<dynamic>(inputString);
            if (dyObject == null)
            {
                return;
            }
            if (dyObject is JArray jArray)
            {
                var list = new MyList<double>();
                foreach (var item in jArray)
                {
                    if (item.ToObject(typeof(double)) is double value)
                    {
                        list.Add(value);
                    }
                }
                Console.WriteLine(list.ToString());
            }
        }

        [TestMethod]
        public void TestCopyString()
        {
            string inputString = "[\"FrequencyTracker\", \"add\", \"add\", \"hasFrequency\"]";
            dynamic? dyObject = JsonConvert.DeserializeObject<dynamic>(inputString);
            if(dyObject == null )
            {
                return;
            }
            if(dyObject is JArray jArray)
            {
                var list = new MyList<string>();
                foreach (var item in jArray)
                {
                    var value = item.ToObject(typeof(string)) as string;
                    if (value != null)
                    {
                        list.Add(value);
                    }
                }
            }
        }
    }
}
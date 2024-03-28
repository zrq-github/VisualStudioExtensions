using LeetCodeVsExtension.Utils;
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
    public class LeetCodeTopicUtilTest
    {
        [TestMethod]
        public void TestCase2CShaperCode_Int()
        {
            // Arrange
            string inputString = "[1,2,3,0,0,0]";

            // Act
            var result = LeetCodeVsExtension.Utils.LeetCodeTopicUtil.TestCase2CShaperCode(inputString);

            // Assert
            Assert.AreEqual(result, "var input = new List<double>() { 1, 2, 3, 0, 0, 0};");
        }

        [TestMethod]
        public void TestCopyString()
        {
            // Arrange
            string inputString = "[\"FrequencyTracker\", \"add\", \"add\", \"hasFrequency\"]";

            // Act
            var result = LeetCodeVsExtension.Utils.LeetCodeTopicUtil.TestCase2CShaperCode(inputString);

            // Assert
            Assert.AreEqual(result, "var input = new List<string>() { \"FrequencyTracker\", \"add\", \"add\", \"hasFrequency\"};");
        }
    }
}
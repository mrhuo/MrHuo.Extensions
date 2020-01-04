using NUnit.Framework;
using System.Diagnostics;

namespace Test
{
    public class HttpClientHelperTest
    {
        class RestResult
        {
        }

        [Test]
        public void TestHttpGet()
        {
            Debug.WriteLine("http://www.baidu.com".HttpGet());
            var ex = Assert.Catch(() =>
            {
                "http://www.fwerwrxfwer.com".HttpGet(true);
            });
            Debug.WriteLine(ex.ToString());

            var html = "http://www.example.com".HttpPost(new System.Collections.Generic.Dictionary<string, object>()
            {
                ["userId"] = "xxx",
                ["userName"] = "xxx"
            });
            var restResult = "http://www.example.com".HttpPost<RestResult>(new System.Collections.Generic.Dictionary<string, object>()
            {
                ["userId"] = "xxx",
                ["userName"] = "xxx"
            });
            Assert.Pass();
        }
    }
}

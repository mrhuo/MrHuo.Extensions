using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Test
{
    public class StreamExtensionsTest
    {
        [Test]
        public void TestStreamToFile()
        {
            ("HelloWorld" + DateTime.Now.Ticks).ToFile(Path.GetTempFileName());
            var file = Path.GetTempFileName();
            ("HelloWorld" + DateTime.Now.Ticks).AppendToFile(file);
            ("HelloWorld" + DateTime.Now.Ticks).AppendToFile(file);

            Assert.Pass();
        }

        [Test]
        public void TestStreamToImage()
        {
            var fromFile = Path.Combine(Path.GetTempPath(), "my.png");
            var stream = fromFile.ToStream();
            var image = stream.ToImage(Path.GetTempFileName(), System.Drawing.Imaging.ImageFormat.Png);
            //fromFile.CopyFileTo(Path.GetTempFileName());
            Assert.Pass();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

/// <summary>
/// Stream 扩展方法
/// </summary>
public static class StreamExtensions
{
    /// <summary>
    /// 将可读的 Stream 写入到文件中
    /// </summary>
    /// <param name="stream">可读 Stream</param>
    /// <param name="fileName">需要写入的文件路径</param>
    /// <exception cref="IOException">Stream不可读或写入文件有问题会抛出异常</exception>
    public static void ToFile(this Stream stream, string fileName)
    {
        if (!stream.CanRead)
        {
            throw new IOException("该Stream不可读");
        }
        using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate,  FileAccess.Write))
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(bytes);
                binaryWriter.Close();
                fileStream.Close();
            }
        }
    }

    /// <summary>
    /// 将可读的 Stream 追加到文件，如果不存在，文件自动创建
    /// </summary>
    /// <param name="stream">可读 Stream</param>
    /// <param name="fileName">需要写入的文件路径</param>
    public static void AppendToFile(this Stream stream, string fileName)
    {
        if (!stream.CanRead)
        {
            throw new IOException("该Stream不可读");
        }
        using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write))
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            using (var binaryWriter = new BinaryWriter(fileStream))
            {
                binaryWriter.Write(bytes);
                binaryWriter.Close();
                fileStream.Close();
            }
        }
    }

    /// <summary>
    /// 写入图片 Stream 到一个文件，默认格式为 ImageFormat.Jpeg
    /// </summary>
    /// <param name="stream">可读 Stream</param>
    /// <param name="fileName">需要写入的文件路径</param>
    public static Image ToImage(this Stream stream, string fileName)
    {
        return ToImage(stream, fileName, ImageFormat.Jpeg);
    }

    /// <summary>
    /// 写入到一个图片，可传入 ImageFormat
    /// </summary>
    /// <param name="stream">可读 Stream</param>
    /// <param name="fileName">需要写入的文件路径</param>
    /// <param name="imageFormat">图片格式</param>
    public static Image ToImage(this Stream stream, string fileName, ImageFormat imageFormat)
    {
        var image = Image.FromStream(stream);
        image.Save(fileName, imageFormat);
        return image;
    }
}

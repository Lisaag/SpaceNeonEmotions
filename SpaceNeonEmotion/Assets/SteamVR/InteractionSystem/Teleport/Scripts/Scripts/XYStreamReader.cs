using UnityEngine;
using System;
using System.IO;
using System.Globalization;

public class XYStreamReader : IDisposable
{
    public int heartrate;
    public bool IsDisposed { get; private set; }

    private readonly Stream stream;
    private readonly StreamReader reader;
    private readonly CultureInfo culture;

    public XYStreamReader(Stream stream)
    {
        culture = CultureInfo.InvariantCulture;
        reader = new StreamReader(this.stream = stream);
    }

    public void Read()
    {
        if (IsDisposed)
            throw new ObjectDisposedException("stream");

        try
        {
            stream.Seek(0, SeekOrigin.Begin);
            heartrate = int.Parse(reader.ReadLine(), culture);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Dispose()
    {
        if (IsDisposed)
            return;

        stream.Dispose();
        reader.Dispose();
    }

    public static XYStreamReader FromFile(string path)
    {
        Stream stream = new FileStream(
            Application.dataPath + "/StreamingAssets/sample.txt",
            FileMode.Open,
            FileAccess.Read);

        return new XYStreamReader(stream);
    }

    public static int DoIt()
    {
        int heartrate = 70;
        using(StreamReader streamreader = new StreamReader(Application.dataPath + "/StreamingAssets/sample.txt"))
        {
            heartrate = int.Parse(streamreader.ReadToEnd());
        }
        return heartrate;
    }
    public static void Reset()
    {
        using (StreamWriter streamwriter = new StreamWriter(Application.dataPath + "/StreamingAssets/sample.txt"))
        {
            streamwriter.Write("0");
        }
    }
}
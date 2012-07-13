namespace Requester
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;

    internal class Program
    {
        private static readonly string IntervalFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "interval.txt");

        private static readonly string UrlFile =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "url.txt");

        private static CancellationToken _token;

        private static CancellationTokenSource _tokenSource;

        private static void Main()
        {
            if (File.Exists(UrlFile))
            {
                File.Delete(UrlFile);
            }

            File.WriteAllLines(
                UrlFile,
                new[]
                    {
                        "http://portfolio-14.apphb.com", "http://biblegravy.apphb.com", "http://instant.apphb.com",
                        "http://funnymoneycasino.apphb.com", "http://blackjackwcf.apphb.com/blackjack/blackjackservice.svc",
                        "http://blackjackwcf.apphb.com/chat/chatservice.svc"
                    });
            
            if (!File.Exists(IntervalFile))
            {
                File.WriteAllText(IntervalFile, "40");
            }

            foreach (string readLine in File.ReadAllLines(UrlFile))
            {
                string line = readLine;
                var t = new Thread(() => StartThread(line, int.Parse(File.ReadAllText(IntervalFile))));
                t.Start();
            }
        }

        private static void StartThread(string url, int interval)
        {
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;

            while (!_token.IsCancellationRequested)
            {
                try
                {
                    new HttpClient().GetAsync(url).Wait(20000, _token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(TimeSpan.FromSeconds(interval));
            }
        }
    }
}
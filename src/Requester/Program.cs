﻿namespace Requester
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading;
    using System.Web.Hosting;

    using Topshelf;

    using log4net.Config;

    internal class Program
    {
        private readonly static string IntervalFile = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "/interval.txt");

        private readonly static string UrlFile = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "/url.txt");

        private CancellationToken token;

        private CancellationTokenSource tokenSource;

        private static void Main(string[] args)
        {
            BasicConfigurator.Configure();

            //if (args.Length < 2)
            //{
            //    Console.WriteLine(args[0]);
            //    PrintHelp("args");
            //    Environment.Exit(-1);
            //}

            //bool isInstallOrUninstall = args[0].Contains("install");
            string interval = "4000";

            //int res;
            //if (!int.TryParse(interval, out res))
            //{
            //    PrintHelp("interval");
            //    Environment.Exit(-1);
            //}

            //if (!File.Exists(UrlFile))
            //{
            //    File.WriteAllText(UrlFile, "http://portfolio-14.apphb.com/");
            //}

            //if (!File.Exists(IntervalFile))
            //{
            //    File.WriteAllText(IntervalFile, "40");
            //}

            //Thread.CurrentThread.Name = "Requester Entrypoint Thread";

            //HostFactory.Run(
            //    x =>
            //        {
            //            x.Service<Program>(
            //                s =>
            //                    {
            //                        s.ConstructUsing(name => new Program());
            //                        s.WhenStarted(
            //                            p =>
            //                            p.Start(File.ReadAllText(UrlFile), int.Parse(File.ReadAllText(IntervalFile))));
            //                        s.WhenStopped(p => p.Stop());
            //                    });
            //            x.RunAsLocalSystem();
            //            x.BeforeInstall(
            //                () =>
            //                    {
            //                        File.WriteAllText(UrlFile, args[1]);
            //                        File.WriteAllText("inteval.txt", interval);
            //                    });
            //            x.SetDescription(string.Format("Requests an url every interval"));
            //            x.SetDisplayName("Requester");
            //            x.SetServiceName("Requester");
            //        });
        }

        private static void PrintHelp(string entryPoint)
        {
            Console.WriteLine(
                @"Too few args: requester install <url> <interval>

                Sample usage; request
                 url: ""https://github.com""
                 interval: 300 seconds

                requester install https://github.com 300

                "+ entryPoint );
        }

        private void Start(string url, int interval)
        {
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;

            while (!this.token.IsCancellationRequested)
            {
                try
                {
                    new HttpClient().GetAsync(url).Wait(20000, this.token);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Thread.Sleep(TimeSpan.FromSeconds(interval));
            }
        }

        private void Stop()
        {
            this.tokenSource.Cancel();
        }
    }
}
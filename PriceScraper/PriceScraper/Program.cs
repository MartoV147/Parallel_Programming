using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Linq;

namespace PriceScraper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<string> links = new List<string>
            {
                "https://www.olx.bg/d/ad/audi-a6-2-4i-lpg-CID360-ID6t7wQ.html",
                "https://www.olx.bg/d/ad/samsung-tablet-0168-CID632-ID8GKX4.html",
                "https://www.olx.bg/d/ad/lego-bionicle-onua-nuva-8566-CID618-ID8FX7l.html"
            };

            Stopwatch stopwatch = new Stopwatch();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            stopwatch.Start();
            var result = await InvokeAsync(links);
            stopwatch.Stop();

            PrintList(result);
            Console.WriteLine("This program ran for: " + stopwatch.ElapsedMilliseconds/1000.0 + " seconds"); 
        }

        static async Task<List<string>> InvokeAsync(List<string> links)
        {
            List<Task<string>> tasks = new List<Task<string>>();

            foreach (string link in links)
            {
                tasks.Add(Task.Run(() => LinkToStringWorker(link)));
            }

            var result = await Task.WhenAll(tasks);
            return result.ToList();
        }

        static HtmlDocument DownloadFromOlx(string link)
        {
            HtmlWeb web = new HtmlWeb();
            return web.Load(link);
        }

        static string LinkToStringWorker(string link)
        {
            try
            {
                HtmlNode htmlNode = DownloadFromOlx(link).DocumentNode;
                string price = htmlNode.SelectSingleNode(".//h3[@class='css-okktvh-Text eu5v0x0']").InnerText.Trim();
                string productTitle = htmlNode.SelectSingleNode(".//h1[@class='css-r9zjja-Text eu5v0x0']").InnerText.Trim();

                string result = "'" + productTitle + "' costs " + price;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

        }

        static void PrintList(List<string> list)
        {
            foreach (var item in list)
            {
                Console.WriteLine(item);
            }
        }
    }
}
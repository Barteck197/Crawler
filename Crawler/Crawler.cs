using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Crawler
{
    class Crawler
    {
        public async static Task Main(string[] args)
        {
            try
            {
                if (args.Length < 1)
                    throw new ArgumentNullException();

                var websiteUrl = args[0];

                Regex regexUrl = new Regex(@"(http|https)\:\/{2}[a-zA-Z0-9_.+-]+\.[a-zA-Z0-9\-]+");

                if (!regexUrl.IsMatch(websiteUrl))                
                    throw new ArgumentException();

                var httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Błąd podczas pobierania strony");

                var content = await response.Content.ReadAsStringAsync();

                httpClient.Dispose();

                Regex regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

                MatchCollection matchCollection = regex.Matches(content);

                var set = new HashSet<string>();

                foreach (var item in matchCollection)
                {
                    set.Add(item.ToString());
                }

                foreach (var item in set)
                {
                    Console.WriteLine(item);
                }
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine(ane.Message);
            }
            catch (ArgumentException ae)
            {
                Console.WriteLine(ae.Message);
            }




            //var list = new List<string>();
            //var dictionary = new Dictionary<string, string>();
            //var set = new HashSet<string>();

            //            var a = $"Content {content}";
            //            var b = "'";

            //Console.WriteLine(matchCollection);

            //Console.WriteLine(content);
        }
    }
}
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

                //regex na URL'e mógłby być lepszy
                Regex regexUrl = new Regex(@"(http|https)\:\/{2}[a-zA-Z0-9_.+-]+\.[a-zA-Z0-9\-]+");

                if (!regexUrl.IsMatch(websiteUrl))                
                    throw new ArgumentException();

                var httpClient = new HttpClient();

                HttpResponseMessage response = await httpClient.GetAsync(websiteUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Błąd podczas pobierania strony");

                var content = await response.Content.ReadAsStringAsync();

                httpClient.Dispose();

                //regex na maile mógłby być lepszy
                Regex regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+");

                MatchCollection matchCollection = regex.Matches(content);

                if (matchCollection.Count == 0)
                    throw new EmptyCollectionException();

                var set = new HashSet<string>();

                foreach (var item in matchCollection)
                {
                    string toAdd = item.ToString();
                    if (!set.Contains(toAdd))
                        set.Add(toAdd);
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
            catch (HttpRequestException hre)
            {
                Console.WriteLine("Błąd w czasie pobierania strony.");
            }
            catch (EmptyCollectionException ece)
            {
                Console.WriteLine("Nie znaleziono adresów email.");
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

    class EmptyCollectionException : Exception
    {
        public EmptyCollectionException() { }
    }
}
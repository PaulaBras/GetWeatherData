using System;
using System.IO;

namespace GetWeatherData
{
    class Program
    {
        static void Main(string[] args)
        {
            string vorher = "";
            Console.WriteLine("Please enter Airport ICAO code");
            string airport = Console.ReadLine().ToUpper();
            while(true)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData("https://tgftp.nws.noaa.gov/data/observations/metar/stations/" + airport + ".TXT");
                string webData = System.Text.Encoding.UTF8.GetString(raw);
                if(vorher != webData)
                {
                    Console.WriteLine(webData);
                    string data =
                        "<html>\n" +
                        "<head>\n" +
                        "       <meta http-equiv='refresh' content='60'>\n" +
                        "</head>\n" +
                        "   <body>\n" +
                        "       <pre style='word - wrap: break-word; white - space: pre - wrap; '>\n" +
                        webData +
                        "       </pre>\n" +
                        "   </body>\n" +
                        "</html>";
                    File.WriteAllTextAsync(airport + ".html", data);
                }
                System.Threading.Thread.Sleep(60 * 1000);
            }
        }
    }
}

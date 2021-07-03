using System;
using System.IO;
using System.Text;

namespace GetWeatherData
{
    class Program
    {
        static void Main(string[] args)
        {
            string before = "";
            Console.WriteLine("Please enter Airport ICAO code");
            string airport = Console.ReadLine().ToUpper();
            while(true)
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] raw = wc.DownloadData("https://tgftp.nws.noaa.gov/data/observations/metar/stations/" + airport + ".TXT");
                string webData = Encoding.UTF8.GetString(raw);

                var sampleLines = new StringBuilder();
                string filePath = AppDomain.CurrentDomain.BaseDirectory + @"\website\sample.html";
                string metarHtmlAirportPath = AppDomain.CurrentDomain.BaseDirectory + @"\website\Airports\" + airport + ".html";

                if (before != webData)
                {
                    Console.WriteLine(webData);

                    //Remove '\n' from the imported web string
                    string[] split = new string[2];
                    split[0] = webData.Substring(0, webData.IndexOf("\n"));
                    split[1] = webData.Substring(webData.IndexOf("\n") + 1, (webData.Length - 1) - (webData.IndexOf("\n")));
                    split[1] = split[1].Remove(split[1].LastIndexOf("\n"));

                    //Format Output: 2021/07/03
                    string[] date = new string[3];
                    date[0] = split[0].Substring(0, 4);
                    date[1] = split[0].Substring(5, 2);
                    date[2] = split[0].Substring(8, 2);

                    //Format change: 03.07.2021
                    string newDate = date[2] + "." + date[1] + "." + date[0];

                    //Apply Date format
                    split[0] = split[0].Remove(0, newDate.Length);
                    split[0] = split[0].Insert(0, newDate);

                    //Insert formatted text into the List
                    foreach (string s in File.ReadAllLines(filePath))
                    {
                        sampleLines.AppendLine(s.Replace("metar.innerText = '", "metar.innerText = '" + split[0] + @"\n" + split[1]));
                    }

                    //Write formatted text to file
                    using (var file = new StreamWriter(File.Create(metarHtmlAirportPath)))
                    {
                        file.Write(sampleLines.ToString());
                    }
                }
                before = webData;
                System.Threading.Thread.Sleep(60 * 1000);
            }
        }
    }
}

using System;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Threading;

namespace AggregateGDPPopulation
{
    public class Class1
    {

        public async Task<string[][]> importcsv()
        {
            StreamReader read= new StreamReader(@"../../../../AggregateGDPPopulation/data/datafile.csv");
            Task<string> dataTask = read.ReadToEndAsync();
            string data = await dataTask;
            string[] rows = data.Split('\n');
            string[][] sheet1rows = new string[rows.Length][];
            int r = 0;
            continent continent_name = new continent();
            foreach (string s in rows)
            {
                string trim = s.Replace("\"", "");
                sheet1rows[r++] = trim.Split(',');
            }
            return sheet1rows;
        }
        public async Task<JObject> importjson()
        {
            StreamReader read2 = new StreamReader(@"../../../../AggregateGDPPopulation/data/country-continent.json");
            string data2 = await read2.ReadToEndAsync();
            JObject sheet2rows = JObject.Parse(data2);
            return sheet2rows;
        }

        public void calculateaggregate()
        {
            Task<string[][]> task1 = importcsv();
            Task<JObject> task2 = importjson();
            Dictionary<string, continent> dict = new Dictionary<string, continent>();
            Task t = Task.WhenAll(task1, task2);
            t.Wait();
            string[][] sheet1rows = task1.Result;
            JObject sheet2rows = task2.Result;

            for (int i=1;i<sheet1rows.Length-2;i++)
            {
                if (!(dict.ContainsKey((string)sheet2rows[sheet1rows[i][0]])))
                {
                    dict[(string)sheet2rows[sheet1rows[i][0]]] = new continent();
                    dict[(string)sheet2rows[sheet1rows[i][0]]].GDP_2012 = double.Parse(sheet1rows[i][7]);
                    dict[(string)sheet2rows[sheet1rows[i][0]]].Population_2012 = double.Parse(sheet1rows[i][4]);
                }
                else
                {
                    dict[(string)sheet2rows[sheet1rows[i][0]]].GDP_2012 += double.Parse(sheet1rows[i][7]);
                    dict[(string)sheet2rows[sheet1rows[i][0]]].Population_2012 += double.Parse(sheet1rows[i][4]);
                }
            }          
            string json = JsonConvert.SerializeObject(dict, Formatting.Indented);
            System.IO.File.WriteAllText(@"../../../../output.json", json);
            Console.WriteLine(json);
            
        }
    }

    public class continent
    {
        public double GDP_2012;
        public double Population_2012;

    }

}

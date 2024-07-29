using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiAppTempoAgora.Models;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Java.Interop;
using System.Data;

namespace MauiAppTempoAgora.Service
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisaoDoTempo(string cidade) 
        {
            // https://openweathermap.org/current#current_JSON
            // https://home.openweathermap.org/api_keys
            string appId = "";

            string url = $"http://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={appId}";

            Tempo tempo = null;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                { 
                    string json = response.Content.ReadAsStringAsync().Result;

                    //var rascunho = JsonConvert.DeserializeObject(json);
                    var rascunho = JObject.Parse(json);

                    DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                    DataTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                    DataTime sunset = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();

                    // Ajustar de UTC para GMT
                    // https://stckoverflow.com/questions/6239976/how-to-set-a-time-zone-

                    tempo = new () 
                    { 
                        Humidity = (string)rascunho["main"]["humidity"],
                        Temperature = (string)rascunho["main"]["temp"],
                        Title = (string)rascunho["name"],
                        Visibility = (string)rascunho["visibility"],
                    }
                }
            }

        }
    }
}

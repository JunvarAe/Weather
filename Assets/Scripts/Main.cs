using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;

public class Main : MonoBehaviour
{
    public Text Description;
    public Text Temperature;
    public Text DateNow;
    public SpriteRenderer WeatherIcon;
    public Text Sity;

    public float lat;
    public float lon;
    public Weather weather;

    void SetValues(){
        var now = DateTime.Now;
        DateNow.text = $"{now:M}";
        Temperature.text = Convert.ToInt32(weather.main.temp).ToString();
        Description.text = weather.weather[0].description;
        WeatherIcon.sprite = LoadSprite(weather.weather[0].icon.Substring(0, 2));
    }

    private Sprite LoadSprite(string resourceName)
    {
        return Resources.Load<Sprite>(resourceName);
    }

    void Start()
    {
        GetSity("Сызрань");
    }

    async void GetSity(string sity)
    {
        Sity.text = sity;
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("http://api.openweathermap.org/geo/1.0/direct?q=" + sity + "&limit=5&appid=6df9619aa5cca620b3d6f38e9a9b4fd0"),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            lat = JsonConvert.DeserializeObject<List<Sity>>(body)[0].lat;
            lon = JsonConvert.DeserializeObject<List<Sity>>(body)[0].lon;
            GetWeather();
        }
    }

    async void GetWeather()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://api.openweathermap.org/data/2.5/weather?lat=" + lat + "&lon=" + lon + "&appid=6df9619aa5cca620b3d6f38e9a9b4fd0&lang=ru&units=metric"),
        };
        using (var response = await client.SendAsync(request))
        {
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();
            weather = JsonConvert.DeserializeObject<Weather>(body);
            SetValues();
        }
    }



    public void SetSity(string sity)
    {
        GetSity(sity);
    }
}

public class Sity
{
    public string name;
    public Language local_names;
    public float lat;
    public float lon;
    public string country;
    public string state;
}

public class Language
{
    public string ru;
}

public class Weather
{
    public MainWeather main;
    public int visibility;
    public List<Info> weather;
}

public class Info
{
    public string description;
    public string icon;
}

public class MainWeather
{
    public float temp;
}
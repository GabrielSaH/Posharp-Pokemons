// AUTO-GENERATED from src/pokete/data/weather.py - do not edit by hand.
using System.Collections.Generic;
using Pokete.Models;

namespace Pokete.Data.Generated;

public static class GeneratedWeather
{
    public static readonly Dictionary<string, WeatherInfo> All = new()
    {
        ["rain"] = new WeatherInfo("rain", "It's raining!", new Dictionary<string, double> { ["fire"] = 0.5, ["plant"] = 1.5, ["water"] = 1.5 }),
        ["thunderstorm"] = new WeatherInfo("thunderstorm", "There is a thunderstorm going on!", new Dictionary<string, double> { ["fire"] = 0.5, ["plant"] = 1.5, ["water"] = 1.5, ["electro"] = 2.0 }),
        ["foggy"] = new WeatherInfo("foggy", "It's foggy!", new Dictionary<string, double> { ["undead"] = 1.5, ["normal"] = 0.75 }),
        ["sunny"] = new WeatherInfo("sunny", "It's a hot sunny day!", new Dictionary<string, double> { ["fire"] = 1.5, ["water"] = 0.75, ["ice"] = 0.5, ["plant"] = 0.75 }),
    };
}
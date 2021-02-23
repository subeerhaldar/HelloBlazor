using HelloBlazor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloBlazor.Models
{
    public interface IWeatherForecastRepository
    {
        IEnumerable<WeatherForecast> GetWeatherForecasts();
        WeatherForecast GetWeatherForecastById(int Id);
        WeatherForecast AddWeatherForecast(WeatherForecast weatherForecast);
        WeatherForecast UpdateWeatherForecast(WeatherForecast weatherForecast);
        void DeleteWeatherForecast(int id);
    }
}

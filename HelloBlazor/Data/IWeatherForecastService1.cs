using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloBlazor.Data
{
    public interface IWeatherForecastService1
    {
        //IEnumerable<WeatherForecast> GetForecast();
        Task<IEnumerable<WeatherForecast>> GetForecastAsync();

        Task<WeatherForecast> AddWeatherForecast(WeatherForecast weatherForecast);
        Task UpdateWeatherForecast(WeatherForecast weatherForecast);
        Task DeleteWeatherForecast(int Id);
        Task<WeatherForecast> GetWeatherForecastDetails(int Id);
    }
}

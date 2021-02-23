using HelloBlazor.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloBlazor.Models
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly AppDbContext _appDbContext;
        public WeatherForecastRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<WeatherForecast> GetWeatherForecasts()
        {
            return _appDbContext.WeatherForecast.ToList();
        }

        public WeatherForecast AddWeatherForecast(WeatherForecast weatherForecast)
        {
            var result = _appDbContext.WeatherForecast.Add(weatherForecast);
            _appDbContext.SaveChanges();
            return result.Entity;
        }

        public WeatherForecast GetWeatherForecastById(int Id)
        {
            return _appDbContext.WeatherForecast.FirstOrDefault(c => c.Id == Id);
        }

        public void DeleteWeatherForecast(int id)
        {
            var foundWeather = _appDbContext.WeatherForecast.FirstOrDefault(e => e.Id == id);
            if (foundWeather == null) return;

            _appDbContext.WeatherForecast.Remove(foundWeather);
            _appDbContext.SaveChanges();
        }

        public WeatherForecast UpdateWeatherForecast(WeatherForecast weatherForecast)
        {
            var foundWeatherForecast = _appDbContext.WeatherForecast.FirstOrDefault(e => e.Id == weatherForecast.Id);

            if (foundWeatherForecast != null)
            {
                foundWeatherForecast.Date = weatherForecast.Date;
                foundWeatherForecast.TemperatureC = weatherForecast.TemperatureC;
                foundWeatherForecast.Summary = weatherForecast.Summary;
               
                _appDbContext.SaveChanges();

                return foundWeatherForecast;
            }

            return null;
        }
    }
}

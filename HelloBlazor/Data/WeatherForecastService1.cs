using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace HelloBlazor.Data
{
    public class WeatherForecastService1 : IWeatherForecastService1
    {
        private readonly HttpClient _httpClient;

        public WeatherForecastService1(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherForecast> AddWeatherForecast(WeatherForecast weatherForecast)
        {
            var employeeJson =
                new StringContent(JsonSerializer.Serialize(weatherForecast), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/WeatherForecast", employeeJson);

            if (response.IsSuccessStatusCode)
            {
                return await JsonSerializer.DeserializeAsync<WeatherForecast>(await response.Content.ReadAsStreamAsync());
            }

            return null;
        }

        public async Task DeleteWeatherForecast(int Id)
        {
            await _httpClient.DeleteAsync($"api/WeatherForecast/{Id}");
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecastAsync()
        {
            return await JsonSerializer.DeserializeAsync<IEnumerable<WeatherForecast>>
                (await _httpClient.GetStreamAsync($"api/WeatherForecast"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }

        public async Task UpdateWeatherForecast(WeatherForecast weatherForecast)
        {
            var weatherForecastJson =
               new StringContent(JsonSerializer.Serialize(weatherForecast), Encoding.UTF8, "application/json");

            await _httpClient.PutAsync("api/WeatherForecast", weatherForecastJson);
        }

        public async Task<WeatherForecast> GetWeatherForecastDetails(int Id)
        {
            return await JsonSerializer.DeserializeAsync<WeatherForecast>
                (await _httpClient.GetStreamAsync($"api/WeatherForecast/{Id}"), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}

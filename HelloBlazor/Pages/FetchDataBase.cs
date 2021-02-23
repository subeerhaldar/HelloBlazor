using ChartJs.Blazor.Common;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Util;
using HelloBlazor.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace HelloBlazor.Pages
{
    public class FetchDataBase : ComponentBase
    {
        [Inject]
        public WeatherForecastService ForecastService { get; set; }
        [Inject]
        public IWeatherForecastService1 ForecastService1 { get; set; }
        [Inject]
        public HttpClient HttpClient { get; set; }

        public WeatherForecast[] forecasts { get; set; }
        public IEnumerable<WeatherForecast> weatherForecasts { get; set; }

        protected override async Task OnInitializedAsync()
        {
            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);

            weatherForecasts = await ForecastService1.GetForecastAsync();


            var aa = weatherForecasts.Select(a => a.Date.ToString("yyyy-MM-dd")).ToArray();
            var bb = weatherForecasts.Select(a => a.TemperatureC).ToArray();

            LoadChart(aa, bb);
        }

        protected BarConfig _config;

        protected override void OnInitialized()
        {
            _config = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "ChartJs.Blazor Bar Chart"
                    }
                }
            };
        }

        protected void LoadChart(string[] aa, int[] bb)
        {
            foreach (string color in aa)
            {
                _config.Data.Labels.Add(color);
            }

            BarDataset<int> dataset = new BarDataset<int>(bb)
            {
                BackgroundColor = new[]
                {
                    ColorUtil.ColorHexString(255, 99, 132), // Slice 1 aka "Red"
                    ColorUtil.ColorHexString(255, 205, 86), // Slice 2 aka "Yellow"
                    ColorUtil.ColorHexString(75, 192, 192), // Slice 3 aka "Green"
                    ColorUtil.ColorHexString(54, 162, 235), // Slice 4 aka "Blue"
                    ColorUtil.ColorHexString(255, 99, 132), // Slice 1 aka "Red"
                    ColorUtil.ColorHexString(255, 205, 86), // Slice 2 aka "Yellow"
                    ColorUtil.ColorHexString(75, 192, 192), // Slice 3 aka "Green"
                    ColorUtil.ColorHexString(54, 162, 235), // Slice 4 aka "Blue"
                }
            };

            _config.Data.Datasets.Add(dataset);
        }
    }
}

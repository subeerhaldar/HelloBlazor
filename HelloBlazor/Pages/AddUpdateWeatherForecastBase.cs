using HelloBlazor.Data;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelloBlazor.Pages
{
    public class AddUpdateWeatherForecastBase : ComponentBase
    {
        [Inject]
        public IWeatherForecastService1 WeatherForecastService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string Id { get; set; }

        public WeatherForecast WeatherForecast { get; set; } = new WeatherForecast();

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected override async Task OnInitializedAsync()
        {
            Saved = false;

            int.TryParse(Id, out var weatherId);

            if (weatherId == 0)
            {
                //add some defaults
                WeatherForecast = new WeatherForecast { Date = DateTime.Now, TemperatureC = 100,  Summary = "Dummy"};
            }
            else
            {
                WeatherForecast = await WeatherForecastService.GetWeatherForecastDetails(int.Parse(Id));
            }
        }

        protected async Task HandleValidSubmit()
        {
            if (WeatherForecast.Id == 0)
            {
                var added = await WeatherForecastService.AddWeatherForecast(WeatherForecast);
                if (added != null)
                {
                    StatusClass = "alert-success";
                    Message = "New Weather added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = "Something went wrong adding the new Weather. Please try again.";
                    Saved = false;
                }
            }
            else
            {
                await WeatherForecastService.UpdateWeatherForecast(WeatherForecast);
                StatusClass = "alert-success";
                Message = "Employee updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteEmployee()
        {
            await WeatherForecastService.DeleteWeatherForecast(WeatherForecast.Id);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/fetchdata");
        }
    }
}

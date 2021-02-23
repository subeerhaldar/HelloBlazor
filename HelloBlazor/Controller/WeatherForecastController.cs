using HelloBlazor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HelloBlazor.Data
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : Controller
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        private readonly ILogger _logger;

        public WeatherForecastController(IWeatherForecastRepository weatherForecastRepository, ILogger<WeatherForecastController> logger)
        {
            this._weatherForecastRepository = weatherForecastRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Log message in the Get() method");
            _logger.LogWarning("Azure log message from Get() method");

            return Ok(_weatherForecastRepository.GetWeatherForecasts());
        }

        [HttpGet("{id}")]
        public IActionResult GetWeatherForecastById(int id)
        {
            return Ok(_weatherForecastRepository.GetWeatherForecastById(id));
        }

        //[Authorize]
        [HttpPost]
        public IActionResult CreateWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            if (weatherForecast == null)
                return BadRequest();

            if (weatherForecast.TemperatureC == 0 || weatherForecast.Summary == string.Empty)
            {
                ModelState.AddModelError("Temperature/Summary", "The Temperature/Summary shouldn't be empty");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdWeatherForecast = _weatherForecastRepository.AddWeatherForecast(weatherForecast);

            return Created("fetchdata", createdWeatherForecast);
        }

        [HttpPut]
        public IActionResult UpdateWeatherForecast([FromBody] WeatherForecast weatherForecast)
        {
            if (weatherForecast == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var weatherForecastToUpdate = _weatherForecastRepository.GetWeatherForecastById(weatherForecast.Id);

            if (weatherForecastToUpdate == null)
                return NotFound();

            _weatherForecastRepository.UpdateWeatherForecast(weatherForecast);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteWeatherForecast(int id)
        {
            if (id == 0)
                return BadRequest();

            var weatherForecastToDelete = _weatherForecastRepository.GetWeatherForecastById(id);
            if (weatherForecastToDelete == null)
                return NotFound();

            _weatherForecastRepository.DeleteWeatherForecast(id);

            return NoContent();
        }
    }
}
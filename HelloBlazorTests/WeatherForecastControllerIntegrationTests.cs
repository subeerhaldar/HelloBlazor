﻿using HelloBlazor;
using HelloBlazor.Data;
using HelloBlazor.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace HelloBlazorTests
{
    public class WeatherForecastControllerIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string WeatherForecastUriPath = "api/weatherforecast";

        private static HttpClient _httpClientWithFullIntegration;

        private readonly WebApplicationFactory<Startup> _webApplicationFactory;

        public WeatherForecastControllerIntegrationTests(WebApplicationFactory<Startup> webApplicationFactory)
        {
            _webApplicationFactory = webApplicationFactory;
            _httpClientWithFullIntegration ??= webApplicationFactory.CreateClient();
        }

        [Fact]
        public async void Get_WithFullyIntegratedServices_ReturnsOkWithExpectedResponse()
        {
            // Act
            var weatherForecastResponse = await _httpClientWithFullIntegration.GetAsync(WeatherForecastUriPath);

            // Assert
            Assert.Equal(HttpStatusCode.OK, weatherForecastResponse.StatusCode);

            var weatherForecastContent = await weatherForecastResponse.Content.ReadAsStringAsync();
            var weatherForecast = JsonConvert.DeserializeObject<List<WeatherForecast>>(weatherForecastContent);

            Assert.NotNull(weatherForecast);
            Assert.NotNull(weatherForecast.FirstOrDefault().Summary);
        }

        [Fact]
        public async void Get_WeatherForecastServiceThrowsException_ReturnsBadRequestWithExpectedErrorResponse()
        {
            // Arrange
            var randomExceptionMessage = Guid.NewGuid().ToString();
            var expectedException = new Exception(randomExceptionMessage);

            var weatherForecastServiceMock = new Mock<IWeatherForecastRepository>();
            weatherForecastServiceMock.Setup(weatherForecastService => weatherForecastService.GetWeatherForecasts())
                .Throws(expectedException);

            var webApplicationFactoryWithMockedServices = _webApplicationFactory.WithWebHostBuilder(builder =>
                builder.ConfigureTestServices(services =>
                    services.AddScoped(serviceProvider => weatherForecastServiceMock.Object)));

            var httpClientWithMockedService = webApplicationFactoryWithMockedServices.CreateClient();

            // Act
            var weatherForecastResponse = await httpClientWithMockedService.GetAsync(WeatherForecastUriPath);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, weatherForecastResponse.StatusCode);

            var errorResponse = await weatherForecastResponse.Content.ReadAsStringAsync();
            Assert.Equal(randomExceptionMessage, errorResponse);
        }
    }
}

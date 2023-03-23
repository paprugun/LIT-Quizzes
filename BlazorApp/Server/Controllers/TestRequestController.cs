using System;
using System.Threading.Tasks;
using BlazorApp.Services.Interfaces;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlazorApp.Server.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TestRequestController : ControllerBase
    {
        private readonly ILogger<TestRequestController> _logger;

        public TestRequestController(ILogger<TestRequestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<WeatherForecast> MethodGetRequest()
        {
            var response = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "Rain"
            };

            return response;
        }

        [HttpPost("")]
        public async Task<WeatherForecast> MethodPostRequest([FromBody] WeatherForecast model)
        {
            var response = new WeatherForecast()
            {
                Date = model.Date,
                TemperatureC = model.TemperatureC,
                Summary = model.Summary
            };

            return response;
        }

        [HttpPatch("")]
        public async Task<WeatherForecast> MethodPatchRequest()
        {
            var response = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "Rain"
            };

            //patch logic:
            response.Summary = "Cloudy";

            return response;
        }

        [HttpPut("")]
        public async Task<WeatherForecast> MethodPutRequest([FromBody] WeatherForecast model)
        {
            var response = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "Rain"
            };

            //put logic:
            response.Date = model.Date;
            response.TemperatureC = model.TemperatureC;
            response.Summary = model.Summary;

            return response;
        }

        [HttpDelete("")]
        public async Task<string> MethodDeleteRequest()
        {
            var model = new WeatherForecast()
            {
                Date = DateTime.Now,
                TemperatureC = 10,
                Summary = "Rain"
            };

            //delete logic:
            model = null;

            var response = "model was deleted!";

            return response;
        }
    }
}


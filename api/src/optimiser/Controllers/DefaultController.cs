using Optimiser.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using Amazon.Lambda.APIGatewayEvents;
using System.Collections.Generic;

namespace Optimiser.Controllers
{
    [Route("v1/[controller]")]
    public class DefaultController : ControllerBase
    {
        private const string ErrorMessage = "failed to process request";
        private readonly IProcessingService _calculationService;

        public DefaultController(IProcessingService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            int count = -1;
            var from = "var"; var to = "var";
            if (!string.IsNullOrEmpty(from) && !string.IsNullOrEmpty(to))
            {
                count = Convert.ToInt16(_calculationService.GetData());
            };

            if (count < 0) return BadRequest(ErrorMessage);

            return Ok(JsonConvert.SerializeObject(count));
        }
    }
}
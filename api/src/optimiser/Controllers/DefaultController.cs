using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Optimiser.Models;
using Optimiser.Services;

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
        public  IActionResult GetData()
        {
           var breaks =  _calculationService.GetDefaultData();
           if (breaks?.Result?.Count <= 0) return BadRequest(ErrorMessage);
          
            return Ok(JsonConvert.SerializeObject(breaks.Result));
        }


        [HttpPost]
        public  IActionResult GetData([FromBody]List<Break> breaks)
        {
            if(breaks == null || !breaks.Any()) return BadRequest(ErrorMessage);
            breaks =  _calculationService.GetOptimalRatings(breaks).Result;
            if (breaks == null || !breaks.Any()) return BadRequest(ErrorMessage);
          
            return Ok(JsonConvert.SerializeObject(new { breaksWithCommercials = breaks, total = breaks.Sum(d => d.Commercials.Sum(p => p.CurrentRating.Score))}));
        }


        [HttpPut]
        public IActionResult SeedData()
        {
            var success = _calculationService.SeedData();
            if (success) return Ok("Successfully seeded data");
            return BadRequest(ErrorMessage);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Optimiser.Models;
using Optimiser.Services;
using System.Threading.Tasks;

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

        /// <summary>
        /// returns the list of default/empty breaks with ratings
        /// but without commercials allocated to each break
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public  IActionResult GetData()
        {
           var breaks =  _calculationService.GetDefaultData();
           if (breaks?.Count <= 0) return BadRequest(ErrorMessage);
          
            return Ok(JsonConvert.SerializeObject(breaks));
        }

        /// <summary>
        /// this is the main api which takes in a list of brake objects
        /// posted by the client ui app and which contain ratings to be used 
        /// for calculations
        /// </summary>
        /// <param name="breaks"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Optimise([FromBody]List<Break> breaks)
        {
            if(breaks == null || !breaks.Any()) return BadRequest(ErrorMessage);
            breaks = await _calculationService.GetOptimalRatings(breaks);
            if (breaks == null || !breaks.Any()) return BadRequest(ErrorMessage);
          
            return Ok(JsonConvert.SerializeObject(new { breaksWithCommercials = breaks, total = breaks.Sum(d => d?.Commercials?.Sum(p => p.CurrentRating.Score))}));
        }

        /// <summary>
        /// should be called when the database has just been deployed
        /// and contains no data. This call will prepopulate the db with default data
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        public IActionResult SeedData()
        {
            var success = _calculationService.SeedData();
            if (success) return Ok("Successfully seeded data");
            return BadRequest(ErrorMessage);
        }
    }
}
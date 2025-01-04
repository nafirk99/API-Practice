using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    //  https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        // Get All Regions
        // GET: https://localhost:portnumber/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"},
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"},
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"}
            //};

            // Get Data From Domain Model
            var regionsDomain = _dbContext.Regions.ToList();

            // Map Domain Model To DTO
            var regionsDTO = new List<RegionDTO>();
            foreach (var region in regionsDomain) 
            {
                regionsDTO.Add(new RegionDTO
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImgUrl = region.RegionImgUrl,
                });
            }

            // Return The DTO
            return Ok(regionsDTO);
        }

        // Get Region By Id
        // GET: https://localhost:portnumber/api/regions/{id}
        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            var regionDomain = _dbContext.Regions.Find(id);      // Find Can Only be used in Primary Key
            //var region = _dbContext.Regions.Where(x => x.Id == id);
            //var region = _dbContext.Regions.FirstOrDefault(r => r.Id == id);
            if (regionDomain == null) 
            {
               return NotFound();
            }

            // Convert Region Domain Model to Redion DTO
            var regionsDTO = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImgUrl= regionDomain.RegionImgUrl
            };

            // Return The DT to the Client
            return Ok(regionDomain);
        }
    }
}

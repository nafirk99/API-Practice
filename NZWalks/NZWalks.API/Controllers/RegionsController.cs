using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    //  https://localhost:portnumber/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext _dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this._dbContext = dbContext;
            this.regionRepository = regionRepository;
        }


        // Get All Regions
        // GET: https://localhost:portnumber/api/regions
        //[HttpGet("getttt")]  // This is how you name your endpoint
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"},
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"},
            //    new Region{Id = Guid.NewGuid(),Name = "Awkland Walks",Code = "AW",RegionImgUrl = "aw.jpeg"}
            //};

            // Get Data From Domain Model
            //var regionsDomain = await _dbContext.Regions.ToListAsync();   // This is before using Repository pattern
            var regionsDomain = await regionRepository.GetAllAsync();

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
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // Get Region Domain Model From Database
            //var regionDomain = await _dbContext.Regions.FindAsync(id);      // Find Can Only be used in Primary Key
            //var regionDomain = _dbContext.Regions.Where(x => x.Id == id);
            //var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);   // this code was before Repository Pattern
            var regionDomain =  await regionRepository.GetByIdAsync(id);
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
                RegionImgUrl = regionDomain.RegionImgUrl
            };

            // Return The DT to the Client
            return Ok(regionDomain);
        }

        // POST To Create A New Region
        // POST: https://localhost:portnumber/api/regions/
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionDTO addRegionDTO)
        {
            // Map the DTO to Domain Model
            var regionDomain = new Region
            {
                Code = addRegionDTO.Code,
                Name = addRegionDTO.Name,
                RegionImgUrl = addRegionDTO.RegionImgUrl
            };

            // Use Domain Model to Create Region
            //await _dbContext.Regions.AddAsync(regionDomain);  // before Repository Pattern
            //await _dbContext.SaveChangesAsync();              // before Repository Pattern
            regionDomain = await regionRepository.CreateAsync(regionDomain);


            // Map region Domain Model back To DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImgUrl = regionDomain.RegionImgUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        // PUT To Update Region
        // PUT: https://localhost:portnumber/api/regions/{id}
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateRegionDTO updateRegionDTO)
        {
            // Map DTO to Domain Model
            var regionDomain = new Region
            {
                Code = updateRegionDTO.Code,
                Name = updateRegionDTO.Name,
                RegionImgUrl= updateRegionDTO.RegionImgUrl
            };

            // Check If the Region Exists
            //var regionDomain = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id); // This is happening in Repository
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

            if(regionDomain == null)
            {
                return NotFound();
            }

            //// Map DTO to Domain Model
            //regionDomain.Code = updateRegionDTO.Code;
            //regionDomain.Name = updateRegionDTO.Name;
            //regionDomain.RegionImgUrl = updateRegionDTO.RegionImgUrl;

            //await _dbContext.SaveChangesAsync();

            // Convert Domain Model To DTO
            var regionDTO = new RegionDTO
            {
                Id=regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImgUrl=regionDomain.RegionImgUrl
            };

            // Return The DTO to the Client
            return Ok(regionDTO);
        }

        // DELETE to Delete region
        // DELETE: https://localhost:portnumber/api/regions/{id}
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Check If The Region Exists
            //var regionDomainModel = await _dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id); // Repository is doing this 

            var regionDomainModel = await regionRepository.DeleteAsync(id);

            if (regionDomainModel == null) 
            {
                return NotFound();
            }

            // Delete The Region , Repository is doing this 
            //_dbContext.Regions.Remove(regionDomainModel);
            //await _dbContext.SaveChangesAsync();

            // Return Deleted Region Back
            // Map the Domain Model to DTO
            var regionDTO = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImgUrl = regionDomainModel.RegionImgUrl
            };

            return Ok(regionDTO);
        }
    }
}

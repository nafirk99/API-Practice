using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class InmemoryRegionRepository //: IRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>
            {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Name = "Name From InmemoryRegionRepository",
                    Code = "Code InmemoryRegionRepository",
                    RegionImgUrl = "url InmemoryRegionRepository"
                }
            };
        }

        public Task<Region> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}

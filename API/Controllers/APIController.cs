using API.Data;
using API.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : Controller
    {
        private readonly ApplicationDbContext _context;

        public APIController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var advert = _context.Advert.FirstOrDefault(advert => advert.Id == id);
            if (advert == null) return NotFound();
            _context.Advert.Remove(advert);
            _context.SaveChanges();
            return NoContent();
        }



        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(int id, UpdateAdvertDTO updateAdvertDto)
        {
            var advert = _context.Advert.FirstOrDefault(advert => advert.Id == id);
            if (advert == null) return NotFound();

            advert.Id = updateAdvertDto.Id;
            advert.Price = updateAdvertDto.Price;
            advert.Title = updateAdvertDto.Title;

            _context.SaveChanges();
            return NoContent();
        }


        [HttpPost]
        public IActionResult Create(CreateAdvertDTO CreateAdvertDto)
        {
            var advert = new Advert
            {
                Id = CreateAdvertDto.Id,
                Price = CreateAdvertDto.Price,
                Title = CreateAdvertDto.Title
            };
            _context.Advert.Add(advert);
            _context.SaveChanges();

            var advertDto = new AdvertDTO
            {
                Id = advert.Id,
                Price = advert.Price,
                Title = advert.Title,
                AdvertBundle = new List<AdvertBundleDTO>()
            };
            return CreatedAtAction(nameof(GetOne), new { id = advert.Id }, advertDto);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.Advert.Select(advert => new AdvertItemDTO
            {
                Id = advert.Id,
                Price = advert.Price,
                Title = advert.Title,
            }).ToList());
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetOne(int id)
        {
            var advert = _context.Advert.Include(advert => advert.AdvertBundle).FirstOrDefault(advert => advert.Id == id);
            if (advert == null)
                return NotFound();

            var advertDto = new AdvertDTO
            {
                Id = advert.Id,
                Price = advert.Price,
                Title = advert.Title,
                AdvertBundle = advert.AdvertBundle.Select(advert => new AdvertBundleDTO
                { 
                    Id = advert.Id,
                    Name = advert.Name 
                }).ToList(),             
            };
            return Ok(advertDto);
        }
    }
}

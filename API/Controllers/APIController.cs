using API.Data;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Update(int id, Advert advert)
        {
            var Advert = _context.Advert.FirstOrDefault(advert => advert.Id == id);
            if (advert == null) return NotFound();

            advert.Id = advert.Id;
            advert.Price = advert.Price;
            advert.Title = advert.Title;

            _context.SaveChanges();
            return NoContent();

        }


        [HttpPost]
        public IActionResult Create(Advert advert)
        {
            var Advert = new Advert
            {
                Id = advert.Id,
                Price = advert.Price,
                Title = advert.Title
            };
            _context.Advert.Add(advert);
            _context.SaveChanges();

            var Advert = new Advert
            {
                Id = advert.Id,
                Price = advert.Price,
                Title = advert.Title,
            };
            return CreatedAtAction(nameof(GetOne), new { id = advert.Id }, Advert);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_context.Advert.Select(advert => new Advert
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
            var advert = _context.Advert.Include(advert => advert.Id).FirstOrDefault(advert => advert.Id == id);
            if (advert == null)
                return NotFound();
            var Advert = new Advert
            {
                Id = advert.id,
                Price = advert.price,
                Title = advert.title,
                Advert = advert.Title.Select(advert => new Advert { Id = advert.Id, Price = advert.Price }).ToList(),             
            };
            return Ok(advert);
        }
    }
}

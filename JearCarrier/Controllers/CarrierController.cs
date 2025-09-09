using JearCarrier.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JearCarrier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarrierController : ControllerBase
    {
        private readonly DataCarrier _db;
        public CarrierController(DataCarrier db) => _db = db;

        // GET /api/Carrier?q=&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<PagedResult<CarrierDto>>> Get(
            [FromQuery] string? q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize is < 1 or > 200 ? 10 : pageSize;

            var baseQuery = _db.Carriers.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                baseQuery = baseQuery.Where(c =>
                    EF.Functions.Like(c.CarrierName ?? "", $"%{q}%") ||
                    EF.Functions.Like(c.City ?? "", $"%{q}%") ||
                    EF.Functions.Like(c.State ?? "", $"{q}%")   // prefix match for state
                );
            }

            var total = await baseQuery.CountAsync();

            var items = await baseQuery
                .OrderBy(c => c.Id)                           // oldest first
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CarrierDto
                {
                    Id = c.Id,
                    CarrierName = c.CarrierName ?? "",
                    Address = c.Address ?? "",
                    Address2 = c.Address2 ?? "",
                    City = c.City ?? "",
                    State = c.State ?? "",
                    Zip = c.Zip ?? "",
                    Contact = c.Contact ?? "",
                    Phone = c.Phone ?? "",
                    Fax = c.Fax ?? "",
                    Email = c.Email ?? ""
                })
                .ToListAsync();

            var result = new PagedResult<CarrierDto>
            {
                Items = items,
                Total = total,
                Page = page,
                PageSize = pageSize
            };

            return Ok(result);
        }

        // GET /api/Carrier/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CarrierDto>> GetOne(int id)
        {
            var c = await _db.Carriers.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            if (c == null) return NotFound();

            return Ok(new CarrierDto
            {
                Id = c.Id,
                CarrierName = c.CarrierName ?? "",
                Address = c.Address ?? "",
                Address2 = c.Address2 ?? "",
                City = c.City ?? "",
                State = c.State ?? "",
                Zip = c.Zip ?? "",
                Contact = c.Contact ?? "",
                Phone = c.Phone ?? "",
                Fax = c.Fax ?? "",
                Email = c.Email ?? ""
            });
        }

        // POST /api/Carrier
        [HttpPost]
        public async Task<ActionResult<CarrierDto>> Create([FromBody] Carrier carrier)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Carriers.Add(carrier);
            await _db.SaveChangesAsync();

            var dto = new CarrierDto
            {
                Id = carrier.Id,
                CarrierName = carrier.CarrierName ?? "",
                Address = carrier.Address ?? "",
                Address2 = carrier.Address2 ?? "",
                City = carrier.City ?? "",
                State = carrier.State ?? "",
                Zip = carrier.Zip ?? "",
                Contact = carrier.Contact ?? "",
                Phone = carrier.Phone ?? "",
                Fax = carrier.Fax ?? "",
                Email = carrier.Email ?? ""
            };

            return CreatedAtAction(nameof(GetOne), new { id = carrier.Id }, dto);
        }

        // PUT /api/Carrier/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Carrier carrier)
        {
            if (id != carrier.Id) return BadRequest("Id mismatch");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _db.Entry(carrier).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        // DELETE /api/Carrier/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _db.Carriers.FindAsync(id);
            if (c == null) return NotFound();

            _db.Carriers.Remove(c);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

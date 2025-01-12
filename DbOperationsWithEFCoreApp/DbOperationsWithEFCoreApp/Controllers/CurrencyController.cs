using DbOperationsWithEFCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public CurrencyController(AppDbContext appContext)
        {
            _appDbContext = appContext;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencyDataAsync()
        {
            //var result = await _appDbContext.CurrencyTypes.ToListAsync();
            var result1 = await (from currency in _appDbContext.CurrencyTypes select currency).ToListAsync();
            return Ok(result1);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAllCurrencyDataByIdAsync([FromRoute] int id)
        {
            var result = await _appDbContext.CurrencyTypes.FindAsync(id);
            //var result = await (from currency in _appDbContext.CurrencyTypes select currency).ToListAsync();
            return Ok(result);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetAllCurrencyDataByNameAsync([FromRoute] string name)
        {
            var result = await _appDbContext.CurrencyTypes.Where(x => x.Currency == name).FirstOrDefaultAsync();
            //var result1 = await _appDbContext.CurrencyTypes.FirstOrDefaultAsync(x => x.Currency == name);

            return Ok(result);
        }
        [HttpGet("data")]
        public async Task<IActionResult> GetAllCurrencyDataByNameDescriptionAsync([FromQuery] string name, [FromQuery] string description)
        {
            //var result = await _appDbContext.CurrencyTypes.Where(x => x.Currency == name && x.Description == description).FirstOrDefaultAsync();
            var result1 = await _appDbContext.CurrencyTypes.FirstOrDefaultAsync(x => x.Currency == name && (string.IsNullOrEmpty(description) || x.Description == description));

            return Ok(result1);
        }
    }
}

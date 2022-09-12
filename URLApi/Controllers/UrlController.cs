using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text;
using URLApi.DAL;
using URLApi.DAL.Models;

namespace URLApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _memoryCache;
        private readonly string _baseDomain = "https://url.cerint.org/Url/";
        public UrlController(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
        {
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
        }

        [HttpGet("{chuck}")]
        public async Task<HttpResponseMessage> GetByShort(string chuck)
        {
            try
            {
                if (_memoryCache.TryGetValue(chuck, out UrlMap result))
                {
                    Response.Redirect(result.LongUrl);
                }

                var dbValue = await _unitOfWork.Urls.GetByChunkAsync(chuck);
                if (dbValue != null)
                {
                    Response.Redirect(dbValue.LongUrl);
                }
            }
            catch (Exception)
            {

                throw;
            }


            return null;

        }

        [HttpPost]
        public async Task<IActionResult> GenerateLink([FromQuery] string longUrl)
        {
            //The code in here would be better placed in a service, but for the purpose of the test i think it is sufficient to have it in the controller. 
            try
            {

                //An additional Validation step can be added here to make sure that the incoming url is returning 200 code. 
                var hash = sha256(longUrl);
                if (_memoryCache.TryGetValue(hash, out UrlMap result))
                {
                    return Ok(new { url = _baseDomain + result.Chunk });
                }

                var dbValue = await _unitOfWork.Urls.GetByHashAsync(hash);
                if (dbValue != null)
                {
                    _memoryCache.Set(hash, dbValue);
                    _memoryCache.Set(dbValue.Chunk, dbValue);
                    return Ok(new { url = _baseDomain + dbValue.Chunk });
                }

                result = new UrlMap() { Hash = hash, LongUrl = longUrl };
                var insertId = await _unitOfWork.Urls.AddAsync(result);
                result.Id = insertId;
                result.Chunk = result.GetUrlChunk();

                _memoryCache.Set(hash, result);
                _memoryCache.Set(result.Chunk, result);
                _unitOfWork.Urls.UpdateAsync(result);

                return Ok(new { url = _baseDomain + result.Chunk });

            }
            catch (Exception ex)
            {
                return BadRequest();                
            }

        }

        //This would normally go into a helper class
        static string sha256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

    }
}

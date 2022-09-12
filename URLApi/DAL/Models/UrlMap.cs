using Microsoft.AspNetCore.WebUtilities;

namespace URLApi.DAL.Models
{
    public class UrlMap
    {
        public int Id { get; set; }        
        public string Chunk { get; set; }
        public string Hash { get; set; }
        public string LongUrl { get; set; }

        public string GetUrlChunk()
        {
            // Transform the "Id" property on this object into a short piece of text
            return WebEncoders.Base64UrlEncode(BitConverter.GetBytes(Id));
        }
        
    }
}

using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace MVC_FinalProject.Models.Product
{
    public class ProductImage
    {
        public int Id { get; set; }
        [JsonProperty("img")]
        public string Img { get; set; }
        public bool IsMain { get; set; }
    }
}

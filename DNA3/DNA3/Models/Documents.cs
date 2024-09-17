using Microsoft.AspNetCore.Http;

namespace DNA3.Models {

    public class Document {

        public IFormFile FormFile { get; set; }

        public byte[] bytes { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

    }

}
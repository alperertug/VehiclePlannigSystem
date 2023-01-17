using Core.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Car : EntityBase
    {
        public string Plate { get; set; }
        public int ModelId { get; set; }
        public Model? Model { get; set; }
        public int ColorId { get; set; }
        public Color? Color { get; set; }
        public int ProductionYear { get; set; }
        public int PersonCapacity { get; set; }
        public int LoadLimit { get; set; }
        public Maintenance? Maintenance { get; set; }

        [Column(TypeName ="nvarchar(50)")]
        public string? Title { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        [DisplayName("Image Name")]
        public string? ImageName { get; set; }

        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}

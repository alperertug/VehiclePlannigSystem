using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Concrete
{
    public class Model : EntityBase
    {        
        public string ModelName { get; set; }
        public string BrandName { get; set; }

    }
}

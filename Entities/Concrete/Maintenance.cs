using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Maintenance : EntityBase
    {
        public int CarId { get; set; }
        public string Title { get; set; }        
        public DateTime LastMaintenanceTime { get; set; }
        public long LastMaintenanceMileage { get; set; }
        public string WorkDone { get; set; }
        public DateTime NextMaintenanceTime { get; set; }
        public long NextMaintenanceMileage { get; set; }

        public Car? Car { get; set; }
    }
}

﻿using Core.Entities;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class ReasonForReservation : EntityBase
    {
        public string Reason { get; set; }
    }
}

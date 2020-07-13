﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models
{
    public class ZadanieRedact
    {
        public int Id { get; set; }
        public int Tip { get; set; }
        public string Text { get; set; }
        public double Ball { get; set; }
        public int KolStrTabOtv { get; set; }
        public List<Otvet> Otvets { get; set; }
    }
}
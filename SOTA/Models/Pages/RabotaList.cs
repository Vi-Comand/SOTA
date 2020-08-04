﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace SOTA.Models
{

    public class RabotaTabl
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdSpec { get; set; }
        public int Dliteln { get; set; }
        public string UrovenRabot { get; set; }
        public DateTime Nachalo { get; set; }
        public DateTime Konec { get; set; }
        public string ListUchasn { get; set; }
        public DateTime Sozd { get; set; }
        // public int SpecId { get; set; }
        public string SpecN { get; set; }
        public string TipN { get; set; }
        public string PredmN { get; set; }
        public int KlassR { get; set; }

    }

    public class RabotaTablList
    {
        public List<RabotaTabl> RabotaTabls { get; set; }
    }

    public class RabotaList
    {
       //
        public List<Rabota> Rabotas { get; set; }
  
      
       
    }

   
    }





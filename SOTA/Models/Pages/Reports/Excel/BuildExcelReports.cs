﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SOTA.Models.Pages.Reports.Excel.Tip;

namespace SOTA.Models.Pages.Reports
{
    public class BuildExcelReports
    {
        ProtokolProvedeniaMonitoringovoyRaboti Protokol;
        string Name;
        string Path;
        public BuildExcelReports(ProtokolProvedeniaMonitoringovoyRaboti Protokol)
        {
            this.Protokol = Protokol;
            Path = GetPath();
            Name = GetName();
}
        private string GetPath()
        { string path = Directory.GetCurrentDirectory() + "/wwwroot/Reports/FinishedReports/Tip1/Region/" + Protokol.Tables[1].MO;

               if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;

        }
        private string GetName()
        {
            Protokol.Tables= Protokol.Tables.OrderBy(x => x.OO).ToList();
           string name= Protokol.Tables[0].OO== Protokol.Tables[Protokol.Tables.Count-1].OO? Protokol.Tables[0].OO: Protokol.Tables[0].MO;

          

            return name;

        }
        public void CreateFile()
        {
            if (!Directory.Exists(Path+"/"+Name+".xlsx"))
            {
                ProtokolExcel Excel = new ProtokolExcel(Protokol, Path + "/" + Name + ".xlsx");
                Excel.Create();
            }

        }




        private void Save()
        {

        }


    }
}

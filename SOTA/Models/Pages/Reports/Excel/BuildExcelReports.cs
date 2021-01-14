using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SOTA.Models.Pages.Reports.Excel.Tip;
using System.Text.RegularExpressions;

namespace SOTA.Models.Pages.Reports
{
    public class BuildExcelReports
    {
        ProtokolProvedeniaMonitoringovoyRaboti Protokol;
        string Name;
        string Path;
        string PathFile;

        public BuildExcelReports(ProtokolProvedeniaMonitoringovoyRaboti Protokol)
        {
            this.Protokol = Protokol;
            Path = GetPathFolder();
            Name = GetName();
}

        private string GetPathFolder()
        {
         


            string path = Directory.GetCurrentDirectory() + "/wwwroot/Reports/FinishedReports/Tip1/Rabota/" + Regex.Replace(Protokol.NameR, @"[^\w\r\n]", "");
         

            if (!Directory.Exists(path))
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
            if (!File.Exists(Path+"/"+Name+".xlsx"))
            {
                ProtokolExcel Excel = new ProtokolExcel(Protokol, Path + "/" + Name + ".xlsx");
                Excel.Create();
            }


            PathFile = Path + "/" + Name + ".xlsx";


        }
      
        public string GetPath()
        {
            return PathFile;
        }




    }
}

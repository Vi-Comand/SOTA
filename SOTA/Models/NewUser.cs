using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOTA.Models
{

    public class RowExcel
    {
        public string MO { get; set; }
        public string OO { get; set; }
        public string F { get; set; }
        public string I { get; set; }
        public string O { get; set; }
        public string Klass { get; set; }
        public int NomKlass { get; set; }
        public string Login { get; set; }
    }

    public class ReadExcel
    {
        List<RowExcel> rowExcels;
        IFormFile File;
        public ReadExcel(IFormFile uploadedFile)
        {
            rowExcels = new List<RowExcel>();
            File = uploadedFile;

        }
        public List<RowExcel> ListExcel()
        {
            ReadFile();
            return rowExcels;
        }
        private void ReadFile()
        {
            using (var package = new ExcelPackage(File.OpenReadStream()))
            {

                var workSheet = package.Workbook.Worksheets[0];
                for (int i = 8; workSheet.Cells[i, 1].Value != null; i++)
                {
                    RowExcel rowExcel = new RowExcel();
                    rowExcel.MO = workSheet.Cells[i, 2].Value.ToString();
                    rowExcel.OO = workSheet.Cells[i, 3].Value.ToString();
                    rowExcel.F = workSheet.Cells[i, 6].Value.ToString();
                    rowExcel.I = workSheet.Cells[i, 7].Value == null ? string.Empty : workSheet.Cells[i, 7].Value.ToString();
                    rowExcel.O = workSheet.Cells[i, 8].Value == null ? string.Empty : workSheet.Cells[i, 8].Value.ToString();
                    rowExcel.Klass = workSheet.Cells[i, 5].Value.ToString();
                    rowExcel.NomKlass = Convert.ToInt32(workSheet.Cells[i, 4].Value);
                    //rowExcel.Login = workSheet.Cells[i, 9].Value.ToString();

                    rowExcels.Add(rowExcel);
                }

            }
        }
    }

    public class NewUser
    {

        private SotaContext db;
        private Random rnd;
        const string valid = "1234567890";
        private async void FormirListAndCreatOO(IFormFile uploadedFile)
        {
            using (var package = new ExcelPackage(uploadedFile.OpenReadStream()))
            {

                var workSheet = package.Workbook.Worksheets[0];

                Oo oo;
                Klass gruppa;
                Users user;
                if (workSheet.Cells[1, 3].Value != null)
                {

                }
            }
        }
        public void Added(IFormFile file)
        {

            FormirListAndCreatOO(file);

        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using MoreLinq;
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
                    rowExcel.MO = workSheet.Cells[i, 2].Value == null ? string.Empty : workSheet.Cells[i, 2].Value.ToString();
                    rowExcel.OO = workSheet.Cells[i, 3].Value == null ? string.Empty : workSheet.Cells[i, 3].Value.ToString();
                    rowExcel.F = workSheet.Cells[i, 6].Value == null ? string.Empty : workSheet.Cells[i, 6].Value.ToString();
                    rowExcel.I = workSheet.Cells[i, 7].Value == null ? string.Empty : workSheet.Cells[i, 7].Value.ToString();
                    rowExcel.O = workSheet.Cells[i, 8].Value == null ? string.Empty : workSheet.Cells[i, 8].Value.ToString();
                    rowExcel.Klass = workSheet.Cells[i, 5].Value == null ? string.Empty : workSheet.Cells[i, 5].Value.ToString();
                    rowExcel.NomKlass = Convert.ToInt32(workSheet.Cells[i, 4].Value);
                    //rowExcel.Login = workSheet.Cells[i, 9].Value.ToString();
                    rowExcels.Add(rowExcel);
                }
            }
        }
    }

    public class LoadVBD
    {
        List<Mo> ListMObd;
        List<Mo> ListMO;
        List<Oo> ListOObd;
        List<Oo> ListOO;
        List<Klass> ListKlassbd;
        List<Klass> ListKlass;
        List<Users> ListUserbd;
        List<Users> ListUser;
        List<RowExcel> rowExcels;
        SotaContext db;

        public LoadVBD(List<RowExcel> _rowExcel, SotaContext _db)
        {
            if (_rowExcel != null && _db != null)
            {
                rowExcels = _rowExcel;
                db = _db;
                ListMObd = new List<Mo>();
                ListMO = new List<Mo>();
                ListOObd = new List<Oo>();
                ListOO = new List<Oo>();
                ListKlassbd = new List<Klass>();
                ListKlass = new List<Klass>();
                ListUserbd = new List<Users>();
                ListUser = new List<Users>();
            }
        }

        public void CreateMO()
        {
            var predListMO = rowExcels.Select(x => new Mo { Name = x.MO });
            ListMObd = db.Mo.ToList();
            ListMO = predListMO.DistinctBy(x => x.Name).ToList();
            ListMO = ListMO.AsEnumerable().Where(x => !ListMObd.Any(y => y.Name == x.Name)).ToList();
            try
            {
                db.Mo.AddRange(ListMO);
                db.SaveChanges();
                ListMObd.AddRange(ListMO);
            }
            finally
            {
                IdMoVList();
            }

        }

        private void IdMoVList()
        {
            foreach (Mo mos in ListMObd)
            {
                rowExcels.Where(x => x.MO == mos.Name).AsParallel().ForAll(y => y.MO = mos.Id.ToString());
            }
            CreateOO();
        }

        private void CreateOO()
        {
            var predListOO = rowExcels.Select(x => new Oo { Name = x.OO, IdMo = Convert.ToInt32(x.MO), Tip = 1 });
            ListOObd = db.Oo.ToList();
            ListOO = predListOO.GroupBy(x => new { x.Name, x.IdMo }).Select(x => x.First()).ToList();
            ListOO = ListOO.AsEnumerable().Where(x => !ListOObd.Any(y => y.Name == x.Name && y.IdMo == x.IdMo)).ToList();
            try
            {
                db.Oo.AddRange(ListOO);
                db.SaveChanges();
                ListOObd.AddRange(ListOO);
            }
            finally
            {
                IdOoVList();
            }
        }

        private void IdOoVList()
        {
            foreach (Oo oos in ListOObd)
            {
                rowExcels.Where(x => x.OO == oos.Name && x.MO == oos.IdMo.ToString()).AsParallel().ForAll(y => y.OO = oos.Id.ToString());
            }
            CreateKlass();
        }

        private void CreateKlass()
        {
            var predListklass = rowExcels.Select(x => new Klass { Kod = x.Klass, IdOo = Convert.ToInt32(x.OO), KlassNom = Convert.ToInt32(x.NomKlass) });
            ListKlassbd = db.Klass.ToList();
            ListKlass = predListklass.GroupBy(x => new { x.Kod, x.IdOo, x.KlassNom }).Select(x => x.First()).ToList();
            ListKlass = ListKlass.AsEnumerable().Where(x => !ListKlassbd.Any(y => y.Kod == x.Kod && y.IdOo == x.IdOo && y.KlassNom == x.KlassNom)).ToList();
            try
            {
                db.Klass.AddRange(ListKlass);
                db.SaveChanges();
                ListKlassbd.AddRange(ListKlass);
            }
            finally
            {
                IdKlassVList();
            }
        }

        private void IdKlassVList()
        {
            foreach (Klass klass in ListKlassbd)
            {
                rowExcels.Where(x => x.Klass == klass.Kod && x.NomKlass == klass.KlassNom && x.OO == klass.IdOo.ToString()).AsParallel().ForAll(y => y.Klass = klass.Id.ToString());
            }
            CreateKlass();
        }

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

using Microsoft.EntityFrameworkCore;
using SOTA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SOTA.Models
{


    public class UsersAdmin
    {
        public int Id { get; set; }
        public int IdKlass { get; set; }
        public int IdOo { get; set; }
        public int IdMo { get; set; }
        public string Kod { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        //public string Mail { get; set; }
        public int Sogl { get; set; }
        public DateTime DateReg { get; set; }
        public string F { get; set; }
        public string I { get; set; }
        public string O { get; set; }
        //public string лн { get; set; }
        public string OO { get; set; }
        public string Klass { get; set; }
        public int NomKlass { get; set; }
        public string MO { get; internal set; }
        public int Zaverh { get; set; }
    }

    public class UsersPage
    {
        public List<UsersAdmin> LisrUsersMO;
        public List<UsersAdmin> LisrUsersOO;
        public List<UsersAdmin> LisrUsersKlass;
        public List<UsersAdmin> LisrUsersTest;

        public List<RabotaUchen> rabotaKlasss;
    }


    public class ListUsersAdmin
    {
        private SotaContext db;
        public List<UsersAdmin> LisrUsersAdm;
        public List<Rabota> Rabotas;
        public List<RabotaUchen> rabList;
        public ListUsersAdmin(SotaContext context)
        {
            db = context;
        }

        public void GetRab(int idmo, int idoo, int idklass)
        {
            Users user = new Users { IdMo = idmo, IdOo = idoo, IdKlass = idklass };
            FormirRabotaTablList formir = new FormirRabotaTablList(user, db);
            RabotaUchenList rabotaList = new RabotaUchenList();

            rabotaList = formir.GetSpisokRabotUchen();
            try
            {
                rabList = rabotaList.RabotaTabls.ToList();
            }
            catch
            {
                rabList = null;
            }
        }



        public void LisrUsersA()
        {
            LisrUsersAdm = (from user in db.Users

                            join MO in db.Mo on user.IdMo equals MO.Id into mo
                            from Mo in mo.DefaultIfEmpty()
                            join OO in db.Oo on user.IdOo equals OO.Id into oo
                            from Oo in oo.DefaultIfEmpty()
                            join KLASS in db.Klass on user.IdKlass equals KLASS.Id into klass
                            from Klass in klass.DefaultIfEmpty()

                            select new UsersAdmin
                            {
                                Id = user.Id,
                                Name = user.Name,
                                IdKlass = user.IdKlass,
                                IdOo = user.IdOo,
                                IdMo = user.IdMo,
                                Pass = user.Pass,
                                Role = user.Role,
                                Sogl = user.Sogl,
                                DateReg = user.DateReg,
                                F = user.F,
                                I = user.I,
                                O = user.O,
                                Kod = user.Kod,
                                MO = Mo.Name,
                                OO = Oo.Name,
                                Klass = Klass.Kod,
                                NomKlass = Klass == null ? 0 : Klass.KlassNom
                            }).ToList();
        }

        public void LisrUsersA(int idRab)
        {
            LisrUsersAdm = (from user in db.Users

                            join MO in db.Mo on user.IdMo equals MO.Id into mo
                            from Mo in mo.DefaultIfEmpty()
                            join OO in db.Oo on user.IdOo equals OO.Id into oo
                            from Oo in oo.DefaultIfEmpty()
                            join KLASS in db.Klass on user.IdKlass equals KLASS.Id into klass
                            from Klass in klass.DefaultIfEmpty()
                            join Rab in db.VariantUser.Where(y => y.IdRabota == idRab) on user.Id equals Rab.IdUser into rab
                            from Rab in rab.DefaultIfEmpty()
                            select new UsersAdmin
                            {
                                Id = user.Id,
                                Name = user.Name,
                                IdKlass = user.IdKlass,
                                IdOo = user.IdOo,
                                IdMo = user.IdMo,
                                Pass = user.Pass,
                                Role = user.Role,
                                // Sogl = user.Sogl,
                                DateReg = user.DateReg,
                                F = user.F,
                                I = user.I,
                                O = user.O,
                                // Kod = user.Kod,
                                MO = Mo.Name,
                                OO = Oo.Name,
                                Klass = Klass.Kod,
                                NomKlass = Klass == null ? 0 : Klass.KlassNom,
                                Zaverh = Rab.Konec == null ? 0 : Rab.Konec
                            }).ToList();
        }

    }

}
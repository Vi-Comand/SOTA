using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Rewrite.Internal.UrlActions;


namespace SOTA.Models.Pages.Reports
{
    public class BallUser
      {
    public int idUser { get; set; }
    public int idZad { get; set; }
        public int nZad { get; set; }
        public double balls { get; set; }
}
    public class GeneretionListBalls
    {
        SotaContext db;
        int idRabota;
        int idOO;
        List<BallUser> UsrBalls;
        List<RowProtokol> Tables;
        public GeneretionListBalls(SotaContext context,int idRabota)
        {
            db = context;
            this.idRabota = idRabota;
    
        }
        public List<RowProtokol>  Get(int idOO)
        {
            this.idOO = idOO;
            UploadingBalls();
            GenerationTables();
            return Tables;

        }
        private void UploadingBalls()
        {

            UsrBalls = (from UsBall in db.UsersBalls.Where(x => x.IdRabota == idRabota)

                        join User in db.Users.Where(x => x.IdOo == idOO) on UsBall.IdUser equals User.Id into us
                        from US in us
                        join Zad in db.Zadanie on UsBall.IdZadania equals Zad.Id into zad
                        from Zad in zad.DefaultIfEmpty()
                        select new BallUser
                        {
                            idUser = UsBall.IdUser,

                            idZad = UsBall.IdZadania,

                            balls = UsBall.Ball,
                            nZad = Zad.Nomer



                        }).OrderBy(x => x.nZad).ToList();
           
        }
        private void GenerationTables()
        {
           Tables = new List<RowProtokol>();
            int idspec = db.Rabota.Find(idRabota).IdSpec;
           int KolZadansVVar =  db.Zadanie.Count(x => x.Variant == 1 && x.IdSpec == idspec);
            int[] idUser = UsrBalls.Select(x => x.idUser).ToArray();
            idUser=idUser.Distinct().ToArray();
            for(int i=0;i< idUser.Length; i++)
            {
                var sp = UsrBalls.Where(x => x.idUser == idUser[i]).ToList();
                RowProtokol str = new RowProtokol();
                str.Id = idUser[i];
                str.Balls = new List<double>();
                for (int j=1;j<=KolZadansVVar;j++)
                {
                    try
                    {
                        double ball = sp.Where(x => x.nZad == j).First().balls;
                        str.Balls.Add(ball);
                    }
                    catch
                    {
                        str.Balls.Add(-1);
                    }


                }
                Tables.Add(str);
            }









        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SOTA.Models.Pages.Test;
using SOTA.Models.Pages.Test.TestRaschet;
using SOTA.Models.Pages.Test.TestRaschet.ConcreteStrategy;

namespace SOTA.Models.Pages.TestRaschet

{
    
    public class AnswerUserRascheta
    {
        public AnswerUser Otvet { get; set; }
        public int Tip { get; set; }
        public double Ball { get; set; }
    }

    public class Raschet
        {
        List<AnswerUserRascheta> OtvUsrPoschitano;
        SotaContext db;
        ContextStrategy strategy;
        List<Otvet> OtvVBD;
        public Raschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvetsVBD, SotaContext db)
        { 
            this.db=db;
            List<AnswerUserRascheta> OtvetsUsers =new List<AnswerUserRascheta>();

            
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 1).ToList();
            if (OtvUsr.Count != 0)
            {
                strategy = new ContextStrategy(new Tip1());
                
                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).ToList();
                OtvUsrPoschitano.AddRange(strategy.GetRaschet(OtvUsr, OtvVBD));
            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 2).ToList();
            if (OtvUsr.Count != 0)
            {

                strategy = new ContextStrategy(new Tip2());

                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).Where(x => x.Verno == 1).ToList();
                OtvUsrPoschitano.AddRange(strategy.GetRaschet(OtvUsr, OtvVBD));
            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 4).ToList();
            if (OtvUsr.Count != 0)
            {

                strategy = new ContextStrategy(new Tip4());

                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).Where(x => x.Verno == 1).ToList();
                OtvUsrPoschitano.AddRange(strategy.GetRaschet(OtvUsr, OtvVBD));
            }



        }

        private void SaveBalls()
        {
            db.UsersBalls.AddRange(OtvUsrPoschitano.Select(x => new UsersBalls() { Ball = x.Ball, Date = DateTime.Now, IdRabota = x.Otvet.IdRabota, IdUser = x.Otvet.IdUser, IdZadania = x.Otvet.IdZadan }));
            db.SaveChanges();
        }

}





    public class RaschetBallsUser: IRaschetBallsUser
    {
        private List<AnswerUserRascheta> OtvetsUsers;
        private SotaContext db;
        private List<Otvet> OtvetsVBD;
        private int idRabota;
        private int idUser;
      private int idSpec;
        public RaschetBallsUser(SotaContext context,int idRabota, int idUser)
        {
            db = context;
            List<UsersBalls> VBD =new List<UsersBalls>();
            VBD = db.UsersBalls.Where(x => x.IdRabota == idRabota && x.IdUser == idUser).ToList();
            this.idRabota = idRabota;
            this.idUser = idUser;
            ClearBD(VBD);
            this.idSpec=  db.Rabota.Find(idRabota).IdSpec;

            if (!StaticRabotsOtvVBD.Find(idSpec))
            {

                StaticRabotsOtvVBD.Add(idSpec, db);
            }
            

            FormirOtvetsUser();
            PodgotovkaRascheta();
        }

        private void ClearBD(List<UsersBalls> VBD)
        {
            if (VBD != null)
            {
                db.UsersBalls.RemoveRange(VBD);
                db.SaveChanges();
            }
        }

        private void FormirOtvetsUser()
        {
            var vrem = db.AnswerUser.Where(x => x.IdRabota == idRabota && x.IdUser == idUser).ToList();

            OtvetsUsers = vrem.Join(db.Zadanie, x => x.IdZadan, y => y.Id,
                (x, y) => new AnswerUserRascheta { Otvet =(AnswerUser)x, Tip = y.Tip}).ToList();

        }

        private void FormirOtvVBD()
        {
            
            int[] idZadans = OtvetsUsers.Select(x => x.Otvet.IdZadan).ToArray();
         
        }

        private void PodgotovkaRascheta()
        {
            List<AnswerUserRascheta> OtvUsr=new List<AnswerUserRascheta>();
            List<AnswerUserRascheta> OtvUsrPoschitano = new List<AnswerUserRascheta>();
            List<Otvet> OtvVBD= new List<Otvet>();

            OtvUsr = OtvetsUsers.Where(x => x.Tip == 1).ToList();
            if (OtvUsr.Count != 0)
            {
                //int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
               // OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).ToList();
                var OtvVBDn = StaticRabotsOtvVBD.staticOtvVBDs.Where(x => x.IDSpec == idSpec).First().ZadOtvVBD;
             foreach(var row in OtvUsr)
                {
                    row.Otvet.TextOtv = row.Otvet.TextOtv.Replace(" ", "").ToUpper();
        OtvUsrPoschitano.Add(RaschetaTip1(row, OtvVBDn.Where(x => x.ID == row.Otvet.IdZadan).First()));

                }
                       
            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 2).ToList();
            if (OtvUsr.Count != 0)
            {
                var OtvVBDn = StaticRabotsOtvVBD.staticOtvVBDs.Where(x => x.IDSpec == idSpec).First().ZadOtvVBD;
                foreach (var row in OtvUsr)
                {



                   OtvUsrPoschitano.Add(RaschetaTip2(row, OtvVBDn.Where(x => x.ID == row.Otvet.IdZadan).First()));

                }

            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 4).ToList();
            if (OtvUsr.Count != 0)
            {
                var OtvVBDn = StaticRabotsOtvVBD.staticOtvVBDs.Where(x => x.IDSpec == idSpec).First().ZadOtvVBD;
                foreach (var row in OtvUsr)
                {

                    row.Otvet.TextOtv = row.Otvet.TextOtv.Replace(" ", "").ToUpper();

                    OtvUsrPoschitano.Add(RaschetaTip4(row, OtvVBDn.Where(x => x.ID == row.Otvet.IdZadan).First()));

                }

            }
            db.UsersBalls.AddRange(OtvUsrPoschitano.Select(x=>new UsersBalls(){Ball = x.Ball,Date = DateTime.Now,IdRabota=x.Otvet.IdRabota,IdUser=x.Otvet.IdUser,IdZadania = x.Otvet.IdZadan}));
            db.SaveChanges();
        }

        private Dictionary<int, string> RazdlenieStrokiTip4(AnswerUserRascheta OtvUsr)
        {
            Dictionary<int, string> otvetsDictionary= new Dictionary<int, string>();
            string Otvet = OtvUsr.Otvet.TextOtv;
            while (Otvet.Length != 0)
            {
                int indexID = Otvet.IndexOf("ID");
                int indexNachStr = Otvet.IndexOf("{=|");
                int indexKonecStr = Otvet.IndexOf("|=}");
                int id=Int32.Parse(Otvet.Substring(indexID+2, length: indexNachStr- (indexID + 2)));

                string text = Otvet.Substring(indexNachStr+3, indexKonecStr-( indexNachStr + 3));
                otvetsDictionary.Add(id,text);
                Otvet= Otvet.Substring(indexKonecStr + 3 );
            }

            return otvetsDictionary;
        }
        private AnswerUserRascheta RaschetaTip4(AnswerUserRascheta OtvUsr, ZadOtvVBD OtvVBD)
        {
            
           
                
                bool obshBall = false;
                if (OtvVBD.Ball != 0)
                    obshBall = true;


                Dictionary<int, string> otvetsDictionary= RazdlenieStrokiTip4(OtvUsr);

            if (OtvVBD.Otvets.Select(x => x.Ball).Sum() != 0)
            {
                foreach (var row in otvetsDictionary)
                {
                    var param = OtvVBD.Otvets.Where(y => y.ID == row.Key).First().Param;
                    var VernOtv = OtvVBD.Otvets.Where(x => x.Param > param && x.Param < param + 0.9).ToList();

                    try
                    {
                        var otv = VernOtv.Where(x => x.Text == row.Value).First();
                        if (otv != null)
                        {
                            OtvUsr.Ball += OtvVBD.Otvets.Where(x=>x.ID== row.Key).First().Ball;
                        }
                    }
                    catch
                    {
                      
                    }
                }

                
            }
            else if (OtvVBD.PriceError != 0)
            {
                int Verno = 0;
                foreach (var row in otvetsDictionary)
                {
                    var param = OtvVBD.Otvets.Where(y => y.ID == row.Key).First().Param;
                    var VernOtv = OtvVBD.Otvets.Where(x => x.Param > param && x.Param < param + 0.9).ToList();

                    if (VernOtv.Where(x => x.Text == row.Value).Any())
                    {
                        Verno++;
                    }

                }


                OtvUsr.Ball = OtvVBD.Ball - OtvVBD.PriceError * (OtvVBD.Otvets.Where(x => Math.Round(x.Param % 1, 1) == 0.1).Count() - Verno);

                if (OtvUsr.Ball < 0)
                {
                    OtvUsr.Ball = 0;
                }
            }
            else
            {
                int Verno = 0;
                foreach (var row in otvetsDictionary)
                {
                    var param = OtvVBD.Otvets.Where(y => y.ID == row.Key).First().Param;
                    var VernOtv = OtvVBD.Otvets.Where(x => x.Param > param && x.Param < param + 0.9).ToList();

                    if (VernOtv.Where(x => x.Text == row.Value).Any())
                    {
                        Verno++;
                    }

                }

                if (OtvVBD.Otvets.Where(x => Math.Round(x.Param % 1, 1) == 0.1).Count() == Verno)
                {
                    OtvUsr.Ball = OtvVBD.Ball;
                }
                else
                {
                    OtvUsr.Ball = 0;
                }

            }




            OtvUsr.Ball = Math.Round(OtvUsr.Ball, 0, MidpointRounding.AwayFromZero);

            return OtvUsr;
        }

        private AnswerUserRascheta RaschetaTip2(AnswerUserRascheta OtvUsr, ZadOtvVBD OtvVBD)
        {
            
                    List<int> idChecks= RazdelenieStrokiChecks(';', OtvUsr.Otvet.TextOtv);

             if (OtvVBD.Otvets.Select(x=>x.Ball).Sum()!= 0)
            { 
                foreach(int id in idChecks)
                {
                     
                    try
                    {
                       var  otv = OtvVBD.Otvets.Where(x => x.ID == id).First();
                        if (otv != null)
                        {
                            OtvUsr.Ball += otv.Ball;
                        }
                    }
                    catch
                    {

                    }
                   
                   
                   
                }

            }
            
            else if (OtvVBD.PriceError != 0)
            {
                int sovpad = OtvVBD.Otvets.Where(x => idChecks.Contains(x.ID)).Count();
                int Error = (idChecks.Count - sovpad) + (OtvVBD.Otvets.Count - sovpad);
                OtvUsr.Ball = OtvVBD.Ball - Error * OtvVBD.PriceError;
                if (OtvUsr.Ball < 0)
                    OtvUsr.Ball = 0;
            }
            else
            {
                int sovpad = OtvVBD.Otvets.Where(x => idChecks.Contains(x.ID)).Count();
                if (sovpad == OtvVBD.Otvets.Count)
                {
                    OtvUsr.Ball = OtvVBD.Ball;



                }
                else
                {
                    OtvUsr.Ball = 0;
                }

            }
            OtvUsr.Ball = Math.Round(OtvUsr.Ball, 0, MidpointRounding.AwayFromZero);
            return OtvUsr;
        }
        private List<string> RazdlenieStrokiTip4(char Razdelitel, string Stroka)
        {
            List<string> strList = new List<string>();
            if (Stroka != "")
            {
                if (Stroka.Substring(Stroka.Length - 1) == ';'.ToString())
                {
                    Stroka = Stroka.Substring(0, Stroka.Length - 1);
                }

                
                string[] words = Stroka.Split(new char[] {Razdelitel});
                if (words.Length == 1)
                {

                    strList.Add(words[0]);
                }
                else
                    for (int i = 0; i < words.Length; i++)
                    {
                        strList.Add(item: words[i]);
                    }

            }

            return strList;
        }
        private List<int> RazdelenieStrokiChecks(char Razdelitel, string Stroka)
        {
            List<int> idChecks = new List<int>();
            string[] words = Stroka.Split(new char[] { Razdelitel });
            if (words.Length ==1)
            {

                idChecks.Add(Convert.ToInt32(words[0]));
            }
            else
            for (int i=0;i< words.Length-1;i++)
            {
                idChecks.Add(item: Convert.ToInt32(words[i]));
            }

            return idChecks;
        }

       
        private AnswerUserRascheta RaschetaTip1(AnswerUserRascheta OtvUsr, ZadOtvVBD OtvVBD)
        {
            if (OtvVBD != null)
               
                    try
                    {
                        OtvUsr.Ball = OtvVBD.Otvets.FirstOrDefault(x =>
                                x.Text == OtvUsr.Otvet.TextOtv) != null
                                ? OtvVBD.Ball
                                : 0;

                     
                    }
                    catch
                    {
                    OtvUsr.Ball = 0;
                    }
            OtvUsr.Ball = Math.Round(OtvUsr.Ball, 0, MidpointRounding.AwayFromZero);
            return OtvUsr;
        }

    }
}

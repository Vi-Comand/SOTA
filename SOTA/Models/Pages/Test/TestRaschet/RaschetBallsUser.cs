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
      
        public RaschetBallsUser(SotaContext context,int idRabota, int idUser)
        {
            db = context;
            List<UsersBalls> VBD =new List<UsersBalls>();
            VBD = db.UsersBalls.Where(x => x.IdRabota == idRabota && x.IdUser == idUser).ToList();
            this.idRabota = idRabota;
            this.idUser = idUser;
            ClearBD(VBD);
            int idSpec = db.Rabota.Find(idRabota).IdSpec;

            var fsdfs = StaticRabotsOtvVBD.staticOtvVBDs;


            StaticRabotsOtvVBD.Add(idSpec,db);
            

            FormirOtvetsUser();
            FormirOtvVBD();
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
            OtvetsVBD = db.Otvet.Where(x=>idZadans.Contains(x.IdZadan)).ToList();
        }

        private void PodgotovkaRascheta()
        {
            List<AnswerUserRascheta> OtvUsr=new List<AnswerUserRascheta>();
            List<AnswerUserRascheta> OtvUsrPoschitano = new List<AnswerUserRascheta>();
            List<Otvet> OtvVBD= new List<Otvet>();

            OtvUsr = OtvetsUsers.Where(x => x.Tip == 1).ToList();
            if (OtvUsr.Count != 0)
            {
                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).ToList();
                OtvUsrPoschitano.AddRange( RaschetaTip1(OtvUsr, OtvVBD));
            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 2).ToList();
            if (OtvUsr.Count != 0)
            {
                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).Where(x => x.Verno == 1).ToList();
                OtvUsrPoschitano.AddRange(RaschetaTip2(OtvUsr, OtvVBD));
            }
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 4).ToList();
            if (OtvUsr.Count != 0)
            {
                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).Where(x => x.Verno == 1).ToList();
                OtvUsrPoschitano.AddRange(RaschetaTip4(OtvUsr, OtvVBD));
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
        private List<AnswerUserRascheta> RaschetaTip4(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {
            foreach (var row in OtvUsr)
            {
                bool Verno = true;
                ;
                bool obshBall = false;
                if (row.Ball != 0)
                    obshBall = true;


                Dictionary<int, string> otvetsDictionary= RazdlenieStrokiTip4(row);
                if(otvetsDictionary!=null)
                    foreach (var str in otvetsDictionary)
                    {
                        List<string> otvets = RazdlenieStrokiTip4(';', str.Value);
                        Otvet first = null;
                        foreach (var x in OtvVBD)
                        {
                            if (x.Id == str.Key)
                            {
                                first = x;
                                break;
                            }
                        }

                        double numberOtv = first.Param1;
                        double numberOtv1 = numberOtv + 0.9;
                        var otvetVbdList = OtvVBD.Where(x => x.Param1 > numberOtv && x.Param1 < numberOtv1 && x.IdZadan==row.Otvet.IdZadan).ToList();
                        int kolVerno = 0;
                        foreach (var strokaOtvet in otvetVbdList)
                        {
                            int count = 0;
                            foreach (var x in otvets)
                            {
                                if (x == strokaOtvet.Text) count++;
                            }

                            if (count != 0)
                            {
                                kolVerno++;

                            }
                        }

                        if (kolVerno != otvetVbdList.Count && obshBall)
                        {
                            Verno = false;
                            break;
                        }
                        else if(!obshBall)
                            row.Ball += first.Ball;
                        

                    
                  
                }

                if (!Verno)
                    row.Ball = 0;
            }

            return OtvUsr;
        }

        private List<AnswerUserRascheta> RaschetaTip2(List<AnswerUserRascheta>OtvUsr,List<Otvet>OtvVBD)
        {
            if (OtvVBD != null)
                foreach (var row in OtvUsr)
                {
                    List<int> idChecks= RazdelenieStrokiChecks(';',row.Otvet.TextOtv);
                    List<Otvet> sovpad = OtvVBD.Where(x => idChecks.Contains(x.Id)).ToList();
                    if (row.Ball == 0)
                    {
                       
                        foreach (var str in sovpad)
                        {
                            row.Ball += str.Ball;
                        }

                        
                    }
                    else
                    {
                        if (sovpad.Count != 0)
                        {
                            int kolVerno = OtvVBD.Count(x => x.IdZadan == sovpad[0].IdZadan);
                            if (sovpad.Count != kolVerno)
                            {
                                row.Ball = 0;
                            }
                        }
                        else
                        {
                            row.Ball = 0;
                        }
                    }
                }
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

       
        private List<AnswerUserRascheta> RaschetaTip1(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {
            if (OtvVBD != null)
                foreach (var row in OtvUsr)
                {
                    try
                    {if(row.Ball==0)
                        row.Ball = OtvVBD.FirstOrDefault(x => x.IdZadan == row.Otvet.IdZadan && x.Text == row.Otvet.TextOtv)
                            .Ball;
                        else
                        {
                            row.Ball = OtvVBD.FirstOrDefault(x =>
                                x.IdZadan == row.Otvet.IdZadan && x.Text == row.Otvet.TextOtv) != null
                                ? row.Ball
                                : 0;

                        }
                    }
                    catch
                    {
                        row.Ball = 0;
                    }
                }
            return OtvUsr;
        }

    }
}

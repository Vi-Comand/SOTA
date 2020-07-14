using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SOTA.Models.Pages.TestRaschet
{
    public class AnswerUserRascheta
    {
        public AnswerUser Otvet { get; set; }
        public int Tip { get; set; }
        public double Ball { get; set; }
    }
    public class RaschetBallsUser: IRaschetBallsUser
    {
        private List<AnswerUserRascheta> OtvetsUsers;
        private SotaContext db;
        private List<Otvet> OtvetsVBD;
        private int idRabota;
        private int idUser;
        public RaschetBallsUser(SotaContext context,int _idRabota, int _idUser)
        {
            db = context;
            List<UsersBalls> VBD =new List<UsersBalls>();
            VBD = db.UsersBalls.Where(x => x.IdRabota == _idRabota && x.IdUser == _idUser).ToList();
            idRabota = _idRabota;
            idUser = _idUser;
            ClearBD(VBD);
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
                (x, y) => new AnswerUserRascheta { Otvet =(AnswerUser)x, Tip = y.Tip }).ToList();

        }

        private void FormirOtvVBD()
        {
            
            int[] idZadans = OtvetsUsers.Select(x => x.Otvet.IdZadan).ToArray();
            OtvetsVBD = db.Otvet.Where(x=>idZadans.Contains(x.IdZadan)).ToList();
        }

        private void PodgotovkaRascheta()
        {
            List<AnswerUserRascheta> OtvUsr=new List<AnswerUserRascheta>();
            List<Otvet> OtvVBD= new List<Otvet>();
            OtvUsr = OtvetsUsers.Where(x => x.Tip == 1).ToList();
            if (OtvUsr.Count != 0)
            {
                int[] mass = OtvUsr.Select(x => x.Otvet.IdZadan).ToArray();
                OtvVBD = OtvetsVBD.Where(x => mass.Contains(x.IdZadan)).ToList();
                RaschetaTip1(OtvUsr, OtvVBD);
            }
        }

        private void RaschetaTip1(List<AnswerUserRascheta>OtvUsr,List<Otvet>OtvVBD)
        {
            if (OtvVBD != null)
                foreach (var row in OtvUsr)
            {
                try
                {
                    row.Ball = OtvVBD.FirstOrDefault(x => x.IdZadan == row.Otvet.IdZadan && x.Text == row.Otvet.TextOtv)
                        .Ball;
                }
                catch
                {
                    row.Ball = 0;
                }
            }

        }

    }
}

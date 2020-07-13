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

    }
    public class RaschetBallsUser
    {
        private List<AnswerUserRascheta> OtvetsUsers;
        private SotaContext db;
        public RaschetBallsUser(SotaContext context,int _idRabota, int _idUser)
        {
            db = context;
            List<UsersBalls> VBD =new List<UsersBalls>();
            VBD = db.UsersBalls.Where(x => x.IdRabota == _idRabota && x.IdUser == _idUser).ToList();
            OtvetsUsers = db.AnswerUser.Where(x => x.IdRabota == _idRabota && x.IdUser == _idUser).Select(x => new AnswerUserRascheta { Otvet = x }).ToList();
            ClearBD(VBD);


        }

        private void ClearBD(List<UsersBalls> VBD)
        {
            if (VBD != null)
            {
                db.UsersBalls.RemoveRange(VBD);
                db.SaveChanges();
            }
        }

        private void Raschet()
        {

        }

    }
}

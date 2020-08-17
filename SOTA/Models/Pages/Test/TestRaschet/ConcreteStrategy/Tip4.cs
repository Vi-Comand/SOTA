using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SOTA.Models.Pages.TestRaschet;

namespace SOTA.Models.Pages.Test.TestRaschet.ConcreteStrategy
{
    public class Tip4:Strategy
    {
        public override List<AnswerUserRascheta> GetRaschet(List<AnswerUserRascheta> OtvUsr, List<Otvet> OtvVBD)
        {

            foreach (var row in OtvUsr)
            {
                bool Verno = true;
                ;
                bool obshBall = false;
                if (row.Ball != 0)
                    obshBall = true;


                Dictionary<int, string> otvetsDictionary = RazdlenieStrokiTip4(row);
                if (otvetsDictionary != null)
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
                        var otvetVbdList = OtvVBD.Where(x => x.Param1 > numberOtv && x.Param1 < numberOtv1 && x.IdZadan == row.Otvet.IdZadan).ToList();
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
                        else if (!obshBall)
                            row.Ball += first.Ball;




                    }

                if (!Verno)
                    row.Ball = 0;
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


                string[] words = Stroka.Split(new char[] { Razdelitel });
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
        private Dictionary<int, string> RazdlenieStrokiTip4(AnswerUserRascheta OtvUsr)
        {
            Dictionary<int, string> otvetsDictionary = new Dictionary<int, string>();
            string Otvet = OtvUsr.Otvet.TextOtv;
            while (Otvet.Length != 0)
            {
                int indexID = Otvet.IndexOf("ID");
                int indexNachStr = Otvet.IndexOf("{=|");
                int indexKonecStr = Otvet.IndexOf("|=}");
                int id = Int32.Parse(Otvet.Substring(indexID + 2, length: indexNachStr - (indexID + 2)));

                string text = Otvet.Substring(indexNachStr + 3, indexKonecStr - (indexNachStr + 3));
                otvetsDictionary.Add(id, text);
                Otvet = Otvet.Substring(indexKonecStr + 3);
            }

            return otvetsDictionary;
        }
    }
}

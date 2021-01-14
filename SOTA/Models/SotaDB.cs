using System;

namespace SOTA.Models
{
    public class Zadanie
    {
        public int Id
        {
            get; set;
        }
        public int IdSpec
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int Tip
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
        public int Variant
        {
            get; set;
        }
        public int Nomer
        {
            get; set;
        }
        public double Ball
        {
            get; set;
        }
        public string Tema
        {
            get; set;
        }
        public string Doptext
        {
            get; set;
        }
        public string Urov
        {
            get; set;
        }
    }
    public class Predm
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }
    public class Kriterocen
    {
        public int Id
        {
            get; set;
        }

        public int IdSpec
        {
            get; set;
        }

        public int MaxBall
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }
    }
    public class TipSpec
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }


    public class Specific
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int Tip
        {
            get; set;
        }
        public int Class
        {
            get; set;
        }
        public int Predm
        {
            get; set;
        }


    }

    public class Otvet
    {
        public int Id
        {
            get; set;
        }
        public int IdZadan
        {
            get; set;
        }
        public int Verno
        {
            get; set;
        }
        public double Param1
        {
            get; set;
        }
        public string Text
        {
            get; set;
        }
        public double Ball
        {
            get; set;
        }
        public int Ustar
        {
            get; set;
        }
        public DateTime DataIzm
        {
            get; set;
        }
    }

    public class Users
    {
        public int Id
        {
            get; set;
        }
        public int IdKlass
        {
            get; set;
        }
        public int IdOo
        {
            get; set;
        }
        public int IdMo
        {
            get; set;
        }
        public string Kod
        {
            get; set;
        }
        public int Role
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Pass
        {
            get; set;
        }
        public string Mail
        {
            get; set;
        }
        public int Sogl
        {
            get; set;
        }
        public DateTime DateReg
        {
            get; set;
        }
        public string F
        {
            get; set;
        }
        public string I
        {
            get; set;
        }
        public string O
        {
            get; set;
        }

    }

    public class Oo
    {
        public int Id
        {
            get; set;
        }
        public int IdMo
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int Tip
        {
            get; set;
        }
    }

    public class Mo
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
    }

    public class Klass
    {
        public int Id
        {
            get; set;
        }
        public int IdOo
        {
            get; set;
        }
        public string Kod
        {
            get; set;
        }
        public int KlassNom
        {
            get; set;
        }
    }
    public class AnswerUser
    {
        public int Id
        {
            get; set;
        }
        public int IdUser
        {
            get; set;
        }
        public int IdZadan
        {
            get; set;
        }
        public int IdRabota
        {
            get; set;
        }
        public string TextOtv
        {
            get; set;
        }
        public int Proveren
        {
            get; set;
        }
        public DateTime Date
        {
            get; set;
        }

    }

    public class VariantUser
    {
        public int Id
        {
            get; set;
        }
        public int IdRabota
        {
            get; set;
        }
        public int IdUser
        {
            get; set;
        }
        public int Variant
        {
            get; set;
        }
        public DateTime Date
        {
            get; set;
        }
        public int Konec
        {
            get; set;
        }
        public DateTime KonecDate
        {
            get; set;
        }

    }
    public class UsersBalls
    {
        public int Id
        {
            get; set;
        }
        public int IdRabota
        {
            get; set;
        }
        public int IdUser
        {
            get; set;
        }
        public int IdZadania
        {
            get; set;
        }
        public double Ball
        {
            get; set;
        }
        public DateTime Date
        {
            get; set;
        }


    }
    public class Rabota
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public int IdSpec
        {
            get; set;
        }
        public int Dliteln
        {
            get; set;
        }
        public string UrovenRabot
        {
            get; set;
        }
        public DateTime Nachalo
        {
            get; set;
        }
        public DateTime Konec
        {
            get; set;
        }
        public string ListUchasn
        {
            get; set;
        }
        public DateTime Sozd
        {
            get; set;
        }
        public int Klass
        {
            get; set;
        }

    }

    public class NaznachMo
    {
        public int Id
        {
            get; set;
        }
        public int IdRab
        {
            get; set;
        }
        public int IdMo
        {
            get; set;
        }
    }
    public class NaznachOo
    {
        public int Id
        {
            get; set;
        }
        public int IdRab
        {
            get; set;
        }
        public int IdOo
        {
            get; set;
        }
    }

    public class SaveImg
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Tip
        {
            get; set;
        }
    }
}

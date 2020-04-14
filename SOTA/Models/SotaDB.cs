using System;

namespace SOTA.Models
{
    public class Zadanie
    {
        public int Id { get; set; }
        public int IdSpec { get; set; }
        public string Name { get; set; }
        public int Tip { get; set; }
        public string Text { get; set; }
        public int Variant { get; set; }
        public int Nomer { get; set; }
        public double Ball { get; set; }
    }

    public class Otvet
    {
        public int Id { get; set; }
        public int IdZadan { get; set; }
        public int Verno { get; set; }
        public double Param1 { get; set; }
        public string Text { get; set; }
        public double Ball { get; set; }

    }

    public class Users
    {
        public int Id { get; set; }
        public int IdKlass { get; set; }
        public int IdOo { get; set; }
        public int IdMo { get; set; }
        public string Kod { get; set; }
        public int Role { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Mail { get; set; }
        public int Sogl { get; set; }
        public DateTime DateReg { get; set; }
        public string F { get; set; }
        public string I { get; set; }
        public string O { get; set; }

    }

    public class Oo
    {
        public int Id { get; set; }
        public int IdOo { get; set; }
        public string Name { get; set; }
        public int Tip { get; set; }
    }

    public class Mo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Klass
    {
        public int Id { get; set; }
        public int IdOo { get; set; }
        public string Kod { get; set; }
        public int KlassNom { get; set; }
    }
}
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
    public class Predm
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class TipSpec
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }


    public class Specific
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Tip { get; set; }
        public int Class { get; set; }
        public int Predm { get; set; }

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
}
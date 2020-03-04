using System;

namespace Sota.Models
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
    }
    public class Otvet
    {
        public int Id { get; set; }
        public int IdZadan{ get; set; }
        public int Verno { get; set; }
        public float Param1 { get; set; }
        public string Text { get; set; }
       
    }
}
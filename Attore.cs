using System;
using System.Collections.Generic;
using System.Text;

namespace DemoXML
{
    public class Attore
    {
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public DateTime DataNascita { get; set; }
        public int Oscar { get; set; }
        public int Nomination { get; set; }

        public override string ToString()
        {
            return $"{Nome} {Cognome} ha avuto {Nomination} nominations e ha vinto {Oscar} oscar";
        }
    }
}

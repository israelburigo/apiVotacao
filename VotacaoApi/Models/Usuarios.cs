using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models
{
    public class Usuario
    {
        public string Nome { get; set; }
        public Guid Id { get; set; }
        public int Senha { get; set; }
        public bool Bloqueado { get; set; }
        public string Ip { get; set; }
        public string Nomezinho { get; set; }
    }

    public class Usuarios : List<Usuario>
    {
        
    }
    
}
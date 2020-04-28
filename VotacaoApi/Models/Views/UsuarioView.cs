using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models.Views
{
    public class UsuarioView
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Nomezinho { get; set; }

        internal static UsuarioView Novo(Usuario usuario)
        {
            if (usuario == null) return null;

            return new UsuarioView
            {
                Id = usuario.Id.ToString(),
                Nome = usuario.Nome,
                Nomezinho = usuario.Nomezinho
            };
        }
    }
    
}
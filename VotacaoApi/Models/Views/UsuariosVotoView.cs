using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VotacaoApi.Models.Views
{
    public class UsuariosVotoView
    {
        public string Nomezinho { get; set; }
        public bool Bloqueado { get; set; }

        internal static UsuariosVotoView Novo(Usuario usuario)
        {
            return new UsuariosVotoView
            {
                Nomezinho = string.IsNullOrWhiteSpace(usuario.Nomezinho) ? usuario.Nome : usuario.Nomezinho,
                Bloqueado = usuario.Bloqueado
            };
        }
    }
}

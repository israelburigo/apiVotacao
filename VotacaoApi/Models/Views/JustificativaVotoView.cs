using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models.Views
{
    public class JustificativaVotoView
    {
        public Guid Guid { get; set; }
        public string Texto { get; set; }

        internal static JustificativaVotoView Novo(JustificativaVoto just)
        {
            return new JustificativaVotoView
            {
                Guid = just.Guid,
                Texto = just.Texto
            };
        }
    }
}
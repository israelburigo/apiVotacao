using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models.Views
{
    public class VotoView
    {
        public UsuariosVotoView Usuario { get; set; }
        public JustificativaVotoView Justificativa { get; set; }
        public EnumVoto Voto { get; set; }
        public Guid Guid { get; set; }

        internal static List<VotoView> Novo(List<Voto> votos)
        {
            var result = new List<VotoView>();

            foreach (var voto in votos)
            {
                var v = new VotoView
                {
                    Guid = voto.Guid,
                    Usuario = UsuariosVotoView.Novo(voto.Usuario),
                    Voto =  voto.TipoVoto,
                    Justificativa = JustificativaVotoView.Novo(voto.Justificativa)
                };
                result.Add(v);
            }

            return result;
        }
    }
}
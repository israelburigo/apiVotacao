using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Models.Views
{
    public class ListaVotoView
    {
        public int VotosSim { get; set; }
        public int VotosNao { get; set; }
        public int VotosTantoFaz { get; set; }
        public string Pergunta { get; set; }
        public string NomeDoSujeitoQueIniciouSessao { get; set; }
        public DateTime? InicioVotacao { get; set; }
        public string TempoVotacao { get; set; }
        public string Id { get; set; }
        public bool VotacaoEmAndamento { get; set; }
        public bool SessaoEmAndamento { get; set; }

        public List<VotoView> Votos { get; set; }
    }
}
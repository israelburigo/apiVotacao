using System;

namespace VotacaoApi.Models
{
    public enum EnumVoto
    {
        TantoFaz = 0,
        Sim = 1,
        Nao = 2
    }

    public class JustificativaVoto
    {
        public Guid Guid { get; set; }
        public string Texto { get; set; }
    }

    public class Voto
    {
        public Guid Guid { get; set; }
        public Usuario Usuario { get; set; }
        public EnumVoto TipoVoto { get; set; }
        public JustificativaVoto Justificativa { get; set; }
    }
}
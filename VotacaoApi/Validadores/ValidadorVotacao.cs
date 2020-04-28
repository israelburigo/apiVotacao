using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VotacaoApi.Models;
using VotacaoApi.Models.Consts;

namespace VotacaoApi.Validadores
{
    public class ValidadorVotacao
    {
        internal void ValidaAdms(string id)
        {
            var adms = new List<string>
            {
                "5e4d4ecb-f2a2-4437-8fa1-9a4c7f237dc1",
                "92c03ad3-5f25-412e-87ec-22174a6e30bf"
            };

            if(!adms.Contains(id))
                throw new Exception("Limpa daqui, só para adms.");
        }

        internal void ValidaBloqueadosPermissao(string id)
        {
            var adms = new List<string>
            {
                "5e4d4ecb-f2a2-4437-8fa1-9a4c7f237dc1",
                "92c03ad3-5f25-412e-87ec-22174a6e30bf"
            };

            if (!adms.Contains(id))
                throw new Exception("Limpa daqui, só para adms.");
        }

        internal void ValidaTempoInicioVotacao(DateTime? inicioVotacao)
        {
            if (inicioVotacao.HasValue && DateTime.Now < inicioVotacao.Value.AddMinutes(Consts.TempoVotacao))
                throw new Exception("Não cara, não vale iniciar uma nova sessão enquanto tem uma votação pau pega.");

            if (inicioVotacao.HasValue && DateTime.Now < inicioVotacao.Value.AddMinutes(Consts.TempoSessao))
            {
                var tempo = Consts.TempoSessao - Consts.TempoVotacao;
                throw new Exception("Seu malandro, só pode iniciar votação depois de " + tempo + "min a cada sessão.");
            }
        }
        
        internal void ValidaJair(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return;

            texto = texto.ToUpper();
            texto = texto.Replace('@', 'A');
            texto = texto.Replace('4', 'A');
            texto = texto.Replace('1', 'I');

            var ehEle = texto.Contains("JAIR");
            ehEle = ehEle || texto.Contains("JM");
            ehEle = ehEle || texto.Contains("JAJA");
            ehEle = ehEle || texto.Contains("JA JA");
            ehEle = ehEle || texto.Contains("MEZZARI");
            ehEle = ehEle || texto.Contains("MEZARI");
            ehEle = ehEle || texto.Contains("JÁ JÁ");

            if (ehEle)
                throw new Exception("Sua pergunta, nome ou justificativa contêm uma sequência de caracteres BEM suspeita. Safado!");

            var iJ = texto.IndexOf('J');
            var iA = texto.IndexOf('A');
            var iI = texto.IndexOf('I');
            var iR = texto.IndexOf('R');

            if (iR > 0 && iI > 0 && iA > 0 && iJ >= 0)
                if (iJ < iA && iA < iI && iI < iR)
                    throw new Exception(string.Format("Sua pergunta, nome ou justificativa contêm uma sequência de caracteres BEM suspeita. Safado!"));
        }

        internal void ValidaVotoSemSessao(DateTime? inicioVotacao)
        {
            if (!inicioVotacao.HasValue)
                throw new Exception("Não vale votar sem iniciar a sessão.");
        }

        internal void ValidaTerminoSessao(DateTime? inicioVotacao)
        {
            if (DateTime.Now > inicioVotacao.Value.AddMinutes(Consts.TempoVotacao))
                throw new Exception("Já acabou a sessão, só lamento!");
        }

        internal void ValidaVotos(string id, List<Voto> votos)
        {
            if (votos.Any(p => p.Usuario.Id.ToString() == id))
                throw new Exception("Não vale votar de novo.");
        }
        
        internal void ValidaTrabalho(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return;

            texto = texto.ToUpper();
            texto = texto.Replace('@', 'A');

            if (texto.Contains("TRABALH"))
                throw new Exception("Vai trabalhar tu.");
        }


        internal void ValidaBloqueados(Bloqueados bloqueados, Usuario usuario)
        {
            if (bloqueados.ContainsKey(usuario))
                throw new Exception(string.Format("Se ferrou, foi bloqueado pelo motivo: {0}", bloqueados[usuario]));
        }

       
    }
}
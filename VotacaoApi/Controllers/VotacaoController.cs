using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using VotacaoApi.Models;

namespace VotacaoApi.Controllers
{
    public class VotarDto
    {
        public string Id { get; set; }
        public EnumVoto Voto { get; set; }
        public string Justificativa { get; set; }
    }

    public class JustificarDto
    {
        public string Id { get; set; }
        public string Justificativa { get; set; }
    }

    public class BloquearDto
    {
        public string Id { get; set; }
        public string TargetId { get; set; }
        public string Motivo { get; set; }
    }

    public class GuidDto
    {
        public string Id { get; set; }
    }

    public class MudarNomeDto : GuidDto
    {
        public string Nome { get; set; }
    }

    public class NomeDto : GuidDto
    {
        public string Nome { get; set; }
        public string Nomezinho { get; set; }
    }

    public class LoguinhoDto
    {
        public string Nome { get; set; }
        public int Senha { get; set; }
    }

    public class IniciaVotacaoDto
    {
        public string Id { get; set; }
        public string Pergunta { get; set; }
    }

    public class VotacaoController : BaseController
    {
        [Route("api/Votacao/ConsultaVotos")]
        [HttpGet()]
        public HttpResponseMessage ConsultaVotos([FromUri]string id)
        {
            try
            {
                var view = ListaVotos.Instance.ConsultaVotos(id);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/IniciarVotacao")]
        [HttpPost]
        public HttpResponseMessage IniciarVotacao(IniciaVotacaoDto dto)
        {
            try
            {
                ListaVotos.Instance.IniciarVotacao(dto.Id, dto.Pergunta);
                return RetSucesso("Votação Iniciada");
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/Justificar")]
        [HttpPost]
        public HttpResponseMessage Justificar(JustificarDto dto)
        {
            try
            {
                ListaVotos.Instance.Justificar(dto.Id, dto.Justificativa);
                return RetSucesso("Justificadasso!!");
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/ListarBloqueados")]
        [HttpGet]
        public HttpResponseMessage ListarBloqueados([FromUri]string id)
        {
            try
            {
                var view = ListaVotos.Instance.ListarBloqueados(id);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/PegarSenha")]
        [HttpGet]
        public HttpResponseMessage PegarSenha([FromUri]string id)
        {
            try
            {
                var view = ListaVotos.Instance.PegarSenha(id);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/Loguinho")]
        [HttpPost]
        public HttpResponseMessage Loguinho(LoguinhoDto dto)
        {
            try
            {
                var view = ListaVotos.Instance.Loguinho(dto.Nome, dto.Senha);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/MostrarDados")]
        [HttpGet]
        public HttpResponseMessage MostrarDados([FromUri]string id)
        {
            try
            {
                var view = ListaVotos.Instance.MostrarDados(id);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/Bloquear")]
        [HttpPost]
        public HttpResponseMessage Bloquear(BloquearDto dto)
        {
            try
            {
                ListaVotos.Instance.Bloquear(dto.Id, dto.TargetId, dto.Motivo);
                return RetSucesso("Bloqueado!");
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/Cadastrar")]
        [HttpPost]
        public HttpResponseMessage Cadastrar(NomeDto dto)
        {
            try
            {
                var view = ListaVotos.Instance.Cadastrar(dto.Id, dto.Nome, dto.Nomezinho);
                return RetSucesso(view);
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/MudarNomezinho")]
        [HttpPost]
        public HttpResponseMessage MudarNomezinho(MudarNomeDto dto)
        {
            try
            {
                ListaVotos.Instance.MudarNomezinho(dto.Id, dto.Nome);
                return RetSucesso("");
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

        [Route("api/Votacao/Votar")]
        [HttpPost]
        public HttpResponseMessage Votar(VotarDto dto)
        {
            try
            {
                ListaVotos.Instance.Votar(dto.Id, dto.Voto, dto.Justificativa);
                return RetSucesso("Voto Computado!");
            }
            catch (Exception e)
            {
                return RetErro(e);
            }
        }

    }
}
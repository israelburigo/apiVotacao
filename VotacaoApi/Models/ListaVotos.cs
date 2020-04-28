using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using VotacaoApi.Extensoes;
using VotacaoApi.Geradores;
using VotacaoApi.Models.Views;
using VotacaoApi.Validadores;

namespace VotacaoApi.Models
{
    public sealed class ListaVotos
    {
        private string _nomeDoSujeitoQueIniciouSessao = string.Empty;
        private DateTime? _inicioVotacao = null;
        private string _pergunta = string.Empty;
        private Guid _guid = Guid.Empty;

        public Usuarios Usuarios { get; set; }
        public List<Voto> Votos { get; set; }
        public Bloqueados Bloqueados { get; set; }

        private static readonly ListaVotos instance = new ListaVotos();

        public static ListaVotos Instance { get { return instance; } }

        private ListaVotos()
        {
            Usuarios = Arquivos.Arquivos.Deserialize<Usuarios>(Consts.Consts.CadPath);
            if (Usuarios == null)
                Usuarios = new Usuarios();

            Votos = new List<Voto>();
            Bloqueados = new Bloqueados();
        }

        internal ListaVotoView ConsultaVotos(string id)
        {
            Usuarios.FirstOrDefault(p => p.Id.ToString() == id)
                .ExcecaoSeNull("Este gamelão não foi encontrado.");

            var tempoVotacao = string.Empty;

            if (_inicioVotacao.HasValue)
            {
                var calc = _inicioVotacao.Value.AddMinutes(Consts.Consts.TempoVotacao) - DateTime.Now;
                if (calc.TotalSeconds > 0)
                    tempoVotacao = "Tempo para votar " + Convert.ToDateTime(calc.ToString()).ToString("HH:mm:ss");
                else
                {
                    calc = _inicioVotacao.Value.AddMinutes(Consts.Consts.TempoSessao) - DateTime.Now;
                    tempoVotacao = calc.TotalSeconds > 0 ? "Tempo para proxima sessão " + Convert.ToDateTime(calc.ToString()).ToString("HH:mm:ss")
                                                         : "Pode iniciar uma nova votação!";
                }
            }

            return new ListaVotoView
            {
                VotosSim = Votos.Count(p => p.TipoVoto == EnumVoto.Sim),
                VotosNao = Votos.Count(p => p.TipoVoto == EnumVoto.Nao),
                VotosTantoFaz = Votos.Count(p => p.TipoVoto == EnumVoto.TantoFaz),
                Votos = VotoView.Novo(Votos),
                Pergunta = _pergunta,
                NomeDoSujeitoQueIniciouSessao = _nomeDoSujeitoQueIniciouSessao,
                InicioVotacao = _inicioVotacao,
                TempoVotacao = tempoVotacao,
                VotacaoEmAndamento = _inicioVotacao.HasValue && DateTime.Now < _inicioVotacao.Value.AddMinutes(Consts.Consts.TempoVotacao),
                SessaoEmAndamento = _inicioVotacao.HasValue,
                Id = _guid.ToString()
            };
        }

        internal void IniciarVotacao(string id, string pergunta)
        {
            var validador = new ValidadorVotacao();

            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == id)
                .ExcecaoSeNull("Este gamelão não foi encontrado.");

            validador.ValidaBloqueados(Bloqueados, usuario);
            validador.ValidaTempoInicioVotacao(_inicioVotacao);
            validador.ValidaJair(pergunta);
            validador.ValidaTrabalho(pergunta);

            _guid = Guid.NewGuid();
            _pergunta = pergunta;
            _inicioVotacao = DateTime.Now;

            _nomeDoSujeitoQueIniciouSessao = string.IsNullOrWhiteSpace(usuario.Nomezinho) ? usuario.Nome : usuario.Nomezinho;
            Votos.Clear();
        }

        internal void Votar(string id, EnumVoto voto, string just)
        {
            var validador = new ValidadorVotacao();

            validador.ValidaVotoSemSessao(_inicioVotacao);
            validador.ValidaTerminoSessao(_inicioVotacao);
            validador.ValidaVotos(id, Votos);

            validador.ValidaJair(just);
            validador.ValidaTrabalho(just);

            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == id)
                .ExcecaoSeNull("Este gamelão não foi encontrado.");

            var votoDto = new Voto
            {
                Guid = Guid.NewGuid(),
                Usuario = usuario,
                TipoVoto = voto,
                Justificativa = new JustificativaVoto
                {
                    Guid = Guid.NewGuid(),
                    Texto = just
                }
            };

            Votos.Add(votoDto);
        }

        internal void Justificar(string id, string just)
        {
            var validador = new ValidadorVotacao();

            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == id)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");

            validador.ValidaBloqueados(Bloqueados, usuario);

            var voto = Votos.FirstOrDefault(p => p.Usuario.Id.ToString() == id)
                .ExcecaoSeNull("Não votasse nessa sessão, só lamento.");

            validador.ValidaJair(just);
            validador.ValidaTrabalho(just);

            voto.Justificativa = new JustificativaVoto
            {
                Guid = Guid.NewGuid(),
                Texto = just
            };
        }

        internal void Bloquear(string id, string targetId, string motivo)
        {
            new ValidadorVotacao().ValidaBloqueadosPermissao(id);

            var usuarioAlvo = Usuarios.FirstOrDefault(p => p.Id.ToString() == targetId)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");

            if (Bloqueados.ContainsKey(usuarioAlvo)) return;
            Bloqueados.Add(usuarioAlvo, motivo);
            usuarioAlvo.Bloqueado = true;

            try
            {
                Arquivos.Arquivos.Serialize(Usuarios, Consts.Consts.CadPath);
            }
            catch
            {
                throw new Exception("Deu um erro massa que a gente sabe o que é, mas não vamos falar. Tente daqui a pouco");
            }
        }

        internal object ListarBloqueados(string id)
        {
            new ValidadorVotacao().ValidaBloqueadosPermissao(id);
            return Bloqueados;
        }

        internal UsuarioView Cadastrar(string id, string nome, string nomezinho)
        {
            new ValidadorVotacao().ValidaAdms(id);

            if (Usuarios.Any(p => p.Nome.ToUpper() == nome.ToUpper()))
                throw new Exception("O bixo, esse usuário já ta cadastrado");

            var usuario = new Usuario
            {
                Id = Guid.NewGuid(),
                Nome = nome,
                Nomezinho = nomezinho,
                Senha = new GeradorSenha().Gerar()
            };

            Usuarios.Add(usuario);

            try
            {
                Arquivos.Arquivos.Serialize(Usuarios, Consts.Consts.CadPath);
                return UsuarioView.Novo(usuario);
            }
            catch
            {
                Usuarios.Remove(usuario);
                throw new Exception("Deu um erro massa que a gente sabe o que é, mas não vamos falar. Tente daqui a pouco");
            }
        }

        internal UsuarioView MostrarDados(string guid)
        {
            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == guid)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");
            return UsuarioView.Novo(usuario);
        }

        internal object Loguinho(string nome, int senha)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new Exception("Ta de sacanagem? Escreve um nome aí bixo.");

            var usuario = Usuarios.FirstOrDefault(p => p.Nome.ToUpper() == nome.ToUpper() && p.Senha == senha)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");
            return UsuarioView.Novo(usuario);
        }

        internal object PegarSenha(string guid)
        {
            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == guid)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");
            return usuario.Senha;
        }

        internal void MudarNomezinho(string id, string nome)
        {
            Usuarios = Arquivos.Arquivos.Deserialize<Usuarios>(Consts.Consts.CadPath);

            var usuario = Usuarios.FirstOrDefault(p => p.Id.ToString() == id)
                .ExcecaoSeNull("Não achamos esse gamelão na nossa base baluda.");
            usuario.Nomezinho = nome;

            var voto = Votos.FirstOrDefault(p => p.Usuario.Id == usuario.Id);
            if (voto != null)
                voto.Usuario.Nomezinho = nome;

            try
            {
                Arquivos.Arquivos.Serialize(Usuarios, Consts.Consts.CadPath);
            }
            catch
            {
                throw new Exception("Deu um erro massa que a gente sabe o que é, mas não vamos falar. Tente daqui a pouco");
            }
        }
    }
}
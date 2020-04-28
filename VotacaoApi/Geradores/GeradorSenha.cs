using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Geradores
{
    public class GeradorSenha
    {
        internal int Gerar()
        {
            return new Random().Next(1111, 9999);
        }
    }
}
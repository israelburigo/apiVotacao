using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VotacaoApi.Extensoes
{
    public static class ExtensaoObjeto
    {
        public static T ExcecaoSeNull<T>(this T obj, string msg, params object[] p)  where T : class
        {
            if (obj == null)
                throw new Exception(string.Format(msg, p));
            return obj;
        }
    }
}
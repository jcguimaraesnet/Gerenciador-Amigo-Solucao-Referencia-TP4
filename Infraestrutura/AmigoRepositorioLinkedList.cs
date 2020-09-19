using Dominio;
using System.Collections.Generic;
using System.Linq;

namespace Infraestrutura
{
    public sealed class AmigoRepositorioLinkedList : IAmigoRepositorio
    {
        private static LinkedList<Amigo> amigoList = new LinkedList<Amigo>();

        public IList<Amigo> Pesquisar(string termoDePesquisa)
        {
            var amigosEncontrados = amigoList.Where(x => x.ObterNomeCompleto().ToLower().Contains(termoDePesquisa.ToLower()))
                                                 .ToList();
            return amigosEncontrados;
        }

        public void Adicionar(Amigo amigo)
        {
            amigoList.AddLast(amigo);
        }
    }
}

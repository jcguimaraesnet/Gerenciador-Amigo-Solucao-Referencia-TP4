using System.Collections.Generic;

namespace Dominio
{
    public interface IAmigoRepositorio
    {
        IList<Amigo> Pesquisar(string termoDePesquisa);
        void Adicionar(Amigo entidade);
    }
}

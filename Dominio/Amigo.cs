using System;

namespace Dominio
{
    public class Amigo
    {
        public Amigo(string primeiroNome, string sobreNome, DateTime nascimento, Parentesco parentesco)
        {
            Id = Guid.NewGuid();
            PrimeiroNome = primeiroNome ?? throw new ArgumentException("Primeiro nome não foi preenchido");
            Sobrenome = sobreNome ?? throw new ArgumentException("Sobrenome não foi preenchido");
            Nascimento = nascimento;
            TemParentesco = parentesco;
            DataCadastro = DateTime.Now.Date;
        }

        public Guid Id { get; private set; }
        public string PrimeiroNome { get; private set; }
        public string Sobrenome { get; private set; }
        public DateTime Nascimento { get; private set; }
        public Parentesco TemParentesco { get; private set; }
        public DateTime DataCadastro { get; private set; }

        public string ObterNomeCompleto() => string.Format("{0} {1}", PrimeiroNome, Sobrenome);

        public int ObterQtdeDeDiasParaOProximoAniversario()
        {
            var dataAniversarioAnoAtual = new DateTime(DateTime.Now.Year, Nascimento.Month, Nascimento.Day);
            var qtdeDiasDiff = dataAniversarioAnoAtual - DateTime.Now;
            return qtdeDiasDiff.Days;
        }
    }
}

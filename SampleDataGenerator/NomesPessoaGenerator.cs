using System.Linq;
using System;
using Microsoft.Data.Schema.Tools.DataGenerator;
using Microsoft.Data.Schema.Extensibility;
using Microsoft.Data.Schema.Sql;
using System.Text.RegularExpressions;

namespace SampleDataGenerator
{
    [DatabaseSchemaProviderCompatibility(typeof(SqlDatabaseSchemaProvider))]
    public class NomesPessoaGenerator : Generator
    {
        private string _nome;
        private string _sobrenome;
        private string _nomeCompleto;

        [Output(Description = "Gera aleatoriamente Nomes de pessoas fisicas.", Name = "Nome")]
        public string Nome
        {
            get { return _nome; }
        }

        [Output(Description = "Gera aleatoriamente Sobrenomes.", Name = "Sobrenome")]
        public string Sobrenome
        {
            get { return _sobrenome; }
        }

        [Output(Description = "Gera aleatoriamente Nomes Completos de Pessoas Fisicas.", Name = "NomeCompleto")]
        public string NomeCompleto
        {
            get { return _nomeCompleto; }
        }

        private string GerarNome()
        {
            var listaDeNomes = Regex.Split(Properties.Resources.nomes, "\r\n");
            return listaDeNomes.OrderBy(n => Guid.NewGuid()).First();
        }

        private string GerarSobrenome()
        {
            var listaDeSobrenomesSuja = Regex.Split(Properties.Resources.sobrenomes, " ").ToList();
            var listaDeSobrenomes = listaDeSobrenomesSuja.Where(item => !item.Contains("\r\n") && item.Length > 3).ToList();
            var sobrenome = listaDeSobrenomes.OrderBy(n => Guid.NewGuid()).First().ToLower();
            sobrenome = sobrenome.Substring(0, 1).ToUpper() + sobrenome.Substring(1, sobrenome.Length - 1);
            return sobrenome;
        }

        private string GerarNomeCompleto()
        {
            var sobrenome1 = GerarSobrenome();
            var sobrenome2 = GerarSobrenome();

            while (sobrenome1 == sobrenome2)
                sobrenome2 = GerarSobrenome();

            return string.Format("{0} {1} {2}", GerarNome(), sobrenome1, sobrenome2);
        }

        protected override void OnGenerateNextValues()
        {
            _nome = GerarNome();
            _sobrenome = GerarSobrenome();
            _nomeCompleto = GerarNomeCompleto();
        }
    }
}

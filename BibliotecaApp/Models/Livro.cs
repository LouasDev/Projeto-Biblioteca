using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//classe provisoria totalmente sujeita a alteraçoes (DESCONSIDERE)

namespace BibliotecaApp
{
    public class Livro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public string Genero { get; set; }
        public bool Disponibilidade { get; set; }
        public int Quantidade { get; set; }
        public string CodigoDeBarras { get; set; }

        

        public Livro() { }

       
        public Livro(string nome, string autor, string genero, bool disponivel, int quantidade, string codigoDeBarras)
        {
            Nome = nome;
            Autor = autor;
            Genero = genero;
            Disponibilidade = disponivel;
            Quantidade = quantidade;
            CodigoDeBarras = codigoDeBarras;
        }
    }
}

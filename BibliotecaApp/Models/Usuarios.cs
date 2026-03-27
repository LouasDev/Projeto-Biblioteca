using System;
using System.Collections.Generic;

//classe provisoria totalmente sujeita a alteraçoes (DESCONSIDERE)
public class Usuarios
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string TipoUsuario { get; set; }
    public string CPF { get; set; }
    public DateTime DataNascimento { get; set; }
    public string Telefone { get; set; }
    public string Email { get; set; }
    public string Turma { get; set; }
    public string Senha { get; set; }
    public string ConfirmarSenha { get; set; }

   
    public static List<Usuarios> ListaUsuarios { get; set; } = new List<Usuarios>();

    public Usuarios() { }

    public Usuarios(int id, string nome, string tipoUsuario, string cpf, DateTime dataNascimento,
                   string telefone, string email, string turma, string senha, string confirmarSenha)
    {
        Id = id;
        Nome = nome;
        TipoUsuario = tipoUsuario;
        CPF = cpf;
        DataNascimento = dataNascimento;
        Telefone = telefone;
        Email = email;
        Turma = turma;
        Senha = senha;
        ConfirmarSenha = confirmarSenha;
    }

    public bool SenhasCoincidem()
    {
        return Senha == ConfirmarSenha;
    }

    public bool CamposObrigatoriosPreenchidos()
    {
        return !string.IsNullOrWhiteSpace(Nome)
            && !string.IsNullOrWhiteSpace(TipoUsuario)
            && !string.IsNullOrWhiteSpace(CPF)
            && !string.IsNullOrWhiteSpace(Senha)
            && !string.IsNullOrWhiteSpace(ConfirmarSenha);
    }

    public override string ToString()
    {
        return $"{Nome} - {Turma}";
    }

    // Para Manipulação de dados SOMENTE dos Alunos 
    public class Aluno
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Turma { get; set; }
        public string Telefone { get; set; }
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
    }


}
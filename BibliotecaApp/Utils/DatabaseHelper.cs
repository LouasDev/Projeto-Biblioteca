using System;
using System.Data.SqlServerCe;
using System.IO;
using System.Windows.Forms;

namespace BibliotecaApp.Utils
{
    public static class DatabaseHelper
    {
        public static void EnsureDatabase()
        {
            var dbPath = Conexao.CaminhoBanco;
            var dbDir = Path.GetDirectoryName(dbPath);

            if (!Directory.Exists(dbDir))
                Directory.CreateDirectory(dbDir);

            if (!File.Exists(dbPath))
                CriarBanco();
        }

        public static void RecriarBanco()
        {
            var dbPath = Conexao.CaminhoBanco;

            if (File.Exists(dbPath))
                File.Delete(dbPath);

            CriarBanco();
        }

        private static void CriarBanco()
        {
            var connStr = Conexao.Conectar;

            var engine = new SqlCeEngine(connStr);
            engine.CreateDatabase();
            engine.Dispose();

            using (var conn = new SqlCeConnection(connStr))
            {
                conn.Open();
                CriarTabelas(conn);
                InserirAdminPadrao(conn);
            }
        }

        private static void CriarTabelas(SqlCeConnection conn)
        {
            var comandos = new[]
            {
                @"CREATE TABLE usuarios (
                    Id              int IDENTITY(1,1) PRIMARY KEY,
                    Nome            nvarchar(200)  NOT NULL,
                    Email           nvarchar(200)  NULL,
                    Senha_hash      nvarchar(200)  NULL,
                    Senha_salt      nvarchar(200)  NULL,
                    CPF             nvarchar(20)   NULL,
                    DataNascimento  datetime       NULL,
                    Turma           nvarchar(100)  NULL,
                    Telefone        nvarchar(20)   NULL,
                    TipoUsuario     nvarchar(50)   NOT NULL
                )",

                @"CREATE TABLE Livros (
                    Id              int IDENTITY(1,1) PRIMARY KEY,
                    Nome            nvarchar(300)  NOT NULL,
                    Autor           nvarchar(200)  NULL,
                    Genero          nvarchar(100)  NULL,
                    Quantidade      int            NOT NULL DEFAULT 0,
                    CodigoBarras    nvarchar(100)  NULL,
                    Disponibilidade bit            NOT NULL DEFAULT 1
                )",

                @"CREATE TABLE Emprestimo (
                    Id                  int IDENTITY(1,1) PRIMARY KEY,
                    Alocador            int            NOT NULL,
                    Livro               int            NOT NULL,
                    LivroNome           nvarchar(300)  NULL,
                    Responsavel         int            NOT NULL,
                    DataEmprestimo      datetime       NOT NULL,
                    DataDevolucao       datetime       NOT NULL,
                    DataProrrogacao     datetime       NULL,
                    DataRealDevolucao   datetime       NULL,
                    Status              nvarchar(50)   NOT NULL DEFAULT 'Ativo',
                    CodigoBarras        nvarchar(100)  NULL,
                    NotificadoLembrete  bit            NOT NULL DEFAULT 0,
                    NotificadoAtraso    bit            NOT NULL DEFAULT 0
                )",

                @"CREATE TABLE Admin (
                    Email   nvarchar(200) NOT NULL,
                    Senha   nvarchar(200) NOT NULL,
                    Ativo   bit          NOT NULL DEFAULT 1
                )",

                @"CREATE TABLE EmprestimoRapido (
                    Id                      int IDENTITY(1,1) PRIMARY KEY,
                    ProfessorId             int            NOT NULL,
                    LivroId                 int            NOT NULL,
                    LivroNome               nvarchar(300)  NULL,
                    Turma                   nvarchar(100)  NULL,
                    Quantidade              int            NOT NULL DEFAULT 1,
                    DataHoraEmprestimo      datetime       NOT NULL,
                    DataHoraDevolucaoReal   datetime       NULL,
                    Bibliotecaria           nvarchar(200)  NULL,
                    Status                  nvarchar(50)   NOT NULL DEFAULT 'Ativo'
                )",

                @"CREATE TABLE NotificacoesDisponibilidade (
                    Id          int IDENTITY(1,1) PRIMARY KEY,
                    UsuarioId   int            NOT NULL,
                    LivroId     int            NOT NULL,
                    Email       nvarchar(200)  NOT NULL,
                    Enviado     bit            NOT NULL DEFAULT 0
                )",

                @"CREATE TABLE LogProrrogacoes (
                    Id                  int IDENTITY(1,1) PRIMARY KEY,
                    EmprestimoId        int            NOT NULL,
                    DataDaAcao          datetime       NOT NULL,
                    NovaDataDevolucao   datetime       NOT NULL,
                    BibliotecariaNome   nvarchar(200)  NULL
                )"
            };

            foreach (var sql in comandos)
            {
                using (var cmd = new SqlCeCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static void InserirAdminPadrao(SqlCeConnection conn)
        {
            using (var cmd = new SqlCeCommand(
                "INSERT INTO Admin (Email, Senha, Ativo) VALUES (@email, @senha, 1)", conn))
            {
                cmd.Parameters.AddWithValue("@email", "admin@admin.com");
                cmd.Parameters.AddWithValue("@senha", "admin");
                cmd.ExecuteNonQuery();
            }
        }
    }
}

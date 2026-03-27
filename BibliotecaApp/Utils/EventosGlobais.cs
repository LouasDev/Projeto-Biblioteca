using System;

namespace BibliotecaApp.Utils
{
    public static class EventosGlobais
    {
        public static event EventHandler BibliotecariaCadastrada;
        public static void OnBibliotecariaCadastrada()
        {
            BibliotecariaCadastrada?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler ProfessorCadastrado;
        public static void OnProfessorCadastrado()
        {
            ProfessorCadastrado?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler LivroDidaticoCadastrado;
        public static void OnLivroDidaticoCadastrado()
        {
            LivroDidaticoCadastrado?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler LivroCadastradoOuAlterado;
        public static void OnLivroCadastradoOuAlterado()
        {
            LivroCadastradoOuAlterado?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler LivroDevolvido;
        public static void OnLivroDevolvido()
        {
            LivroDevolvido?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler EmprestimoRealizado;
        public static void OnEmprestimoRealizado()
        {
            EmprestimoRealizado?.Invoke(null, EventArgs.Empty);
        }

        public static event EventHandler EmprestimoProrrogado;
        public static void OnEmprestimoProrrogado()
        {
            EmprestimoProrrogado?.Invoke(null, EventArgs.Empty);
        }


    }
}
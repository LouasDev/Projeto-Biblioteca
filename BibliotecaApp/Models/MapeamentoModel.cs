using System;
using System.Collections.Generic;

namespace BibliotecaApp.Models
{
    public class MapeamentoRegistro
    {
        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string TurmaAtual { get; set; }
        public string Sugestao { get; set; }
        public string NovaTurma { get; set; }
        public string Observacao { get; set; }
    }

    public class MapeamentoAnualModel
    {
        public int Ano { get; set; }
        public string Status { get; set; } = "pendente"; // "pendente" | "concluido"
        public DateTime GeradoEm { get; set; }
        public List<MapeamentoRegistro> Registros { get; set; } = new List<MapeamentoRegistro>();
    }
}

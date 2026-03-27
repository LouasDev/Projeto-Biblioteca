# Biblioteca Gastão Valle

Sistema de gerenciamento de biblioteca desenvolvido como projeto acadêmico para a **Escola Estadual Professor Gastão Valle**.

## Sobre o Projeto

O **Biblioteca Gastão Valle** é um aplicativo desktop para controle de livros, empréstimos e usuários de uma biblioteca escolar. O sistema permite:

- **Cadastro e gerenciamento de livros** — adicionar, editar, remover e buscar livros por nome, gênero ou código de barras
- **Controle de empréstimos** — registrar empréstimos e devoluções com prazos, prorrogações e status (Ativo, Atrasado, Devolvido)
- **Empréstimo rápido** — fluxo simplificado para empréstimos de livros didáticos por turma
- **Gerenciamento de usuários** — cadastrar alunos, professores e bibliotecários com CPF, turma, telefone e data de nascimento
- **Leitura de código de barras** — suporte a leitores de código de barras para cadastro e empréstimo de livros
- **Geração de etiquetas** — criação e impressão de etiquetas com código de barras para os livros
- **Relatórios e exportação** — geração de relatórios com exportação para Excel (ClosedXML)
- **Notificações** — alertas de disponibilidade de livros e lembretes de devolução
- **Backup automático** — backup diário via Google Drive
- **Modo offline** — funcionamento local mesmo sem conexão com a internet
- **Modo escuro** — alternância entre tema claro e escuro
- **Mapeamento de turmas** — organização e visualização de alunos por turma

## Credenciais Padrão

Na primeira execução, o banco de dados é criado automaticamente com um administrador padrão:

| Campo    | Valor              |
|----------|--------------------|
| Email    | `admin@admin.com`  |
| Senha    | `admin`            |

> **Recomendação:** altere a senha do administrador após o primeiro acesso.

## Tecnologias Utilizadas

| Tecnologia | Descrição |
|---|---|
| **.NET Framework 4.8** | Framework principal da aplicação |
| **C# / WinForms** | Linguagem e interface gráfica |
| **SQL Server Compact Edition (SQL CE)** | Banco de dados local |
| **Google Drive API** | Backup automático na nuvem |
| **iTextSharp / PDFsharp** | Geração de PDFs e etiquetas |
| **ClosedXML** | Exportação de dados para Excel |
| **ZXing.Net** | Leitura e geração de códigos de barras |
| **Newtonsoft.Json** | Serialização JSON |
| **MaterialSkin** | Componentes de interface moderna |
| **MySql.Data** | Conexão com banco MySQL (remoto) |

## Pré-requisitos

- **Windows** 10 ou superior
- **.NET Framework 4.8** instalado
- **Visual Studio 2019+** (para desenvolvimento)

## Como Compilar e Executar

1. Clone o repositório:
   ```bash
   git clone https://github.com/LouasDev/Projeto-Biblioteca.git
   ```

2. Abra o arquivo `BibliotecaApp.sln` no Visual Studio.

3. Restaure os pacotes NuGet:
   ```
   Botão direito no projeto → Restore NuGet Packages
   ```

4. Compile e execute (F5).

5. O banco de dados será criado automaticamente na pasta `bin/Debug/bibliotecaDB/`.

## Estrutura do Projeto

```
BibliotecaApp/
├── Elements/          # Componentes de UI customizados (botões, textboxes arredondados)
├── Forms/
│   ├── Inicio/        # Tela inicial e formulário principal
│   ├── Livros/        # Cadastro, empréstimo, devolução e busca de livros
│   ├── Login/         # Tela de login e sobre
│   ├── Mapeamento/    # Mapeamento de turmas
│   ├── Relatorio/     # Relatórios
│   └── Usuario/       # Cadastro e edição de usuários
├── Models/            # Modelos de dados (Livro, Emprestimo, Usuario, Sessao)
├── Resources/         # Imagens, ícones e recursos
├── Utils/             # Utilitários (conexão DB, criptografia, backup, etiquetas)
├── Program.cs         # Ponto de entrada da aplicação
└── App.config         # Configurações e connection string
```

## Equipe

- [ManoLouas](https://github.com/ManoLouas)
- [Ithaloluzdepanela](https://github.com/Ithaloluzdepanela)
- [Renato](https://github.com/renato0x)
- [MatheusAlmeida10](https://github.com/MatheusAlmeida10)

---

## Versão Beta

> **Este software está em fase beta.** É um projeto acadêmico sujeito a bugs e melhorias. Funcionalidades podem ser adicionadas, alteradas ou removidas em versões futuras. Relate problemas e sugestões através das [Issues do GitHub](https://github.com/LouasDev/Projeto-Biblioteca/issues).

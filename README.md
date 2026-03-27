# Biblioteca Gastão Valle

Sistema de gerenciamento de biblioteca desenvolvido para a **Escola Estadual Gastão Valle (Bocaiúva/MG)**.

> **Nota sobre o repositório:** Este sistema foi desenvolvido para modernizar a gestão da biblioteca da **Escola Estadual Gastão Valle (Bocaiúva/MG)**. O projeto contou com o apoio e orientação do corpo docente, visando substituir registros manuais por uma solução digital eficiente.
>
> Este repositório é um *mirror/reupload* do projeto original para fins de portfólio pessoal, com o objetivo de demonstrar as minhas contribuições técnicas e facilitar a visualização do código.

## Sobre o Projeto

O **Biblioteca Gastão Valle** é um aplicativo desktop para controle de livros, empréstimos e usuários de uma biblioteca escolar. O sistema permite:

- **Cadastro e gerenciamento de livros** — adicionar, editar, remover e buscar livros por nome, gênero ou código de barras
- **Controle de empréstimos** — registrar empréstimos e devoluções com prazos, prorrogações e status (Ativo, Atrasado, Devolvido)
- **Empréstimo rápido** — fluxo simplificado para empréstimos de livros didáticos por turma
- **Gerenciamento de usuários** — cadastrar alunos, professores e bibliotecários com CPF, turma, telefone e data de nascimento
- **Leitura de código de barras** — suporte a leitores de código de barras para cadastro e empréstimo de livros
- **Geração de etiquetas** — criação e impressão de etiquetas com código de barras para os livros
- **Relatórios e exportação** — geração de relatórios com exportação para Excel
- **Notificações** — alertas de disponibilidade de livros e lembretes de devolução
- **Backup automático** — backup diário via Google Drive
- **Modo offline** — funcionamento local mesmo sem conexão com a internet
- **Modo escuro** — alternância entre tema claro e escuro
- **Mapeamento de turmas** — organização e visualização de alunos por turma

## Minhas Contribuições

Neste projeto, atuei focado na camada de **Back-end**, sendo responsável por:

- **Lógica de Negócios (C#)** — Implementação das regras de empréstimo, devolução e cálculo de prazos utilizando Programação Orientada a Objetos
- **Persistência de Dados (SQL)** — Criação das queries para cadastro e controle de estoque de livros
- **Interface (WinForms)** — Desenvolvimento de telas de livros, cadastro de livros, além da participação no design da tela de login

## Tecnologias Utilizadas

| Tecnologia | Descrição |
|---|---|
| **C# / .NET Framework 4.8** | Linguagem e framework principal |
| **Windows Forms (WinForms)** | Interface gráfica desktop |
| **SQL Server Compact Edition** | Banco de dados local |
| **Google Drive API** | Backup automático na nuvem |
| **iTextSharp / PDFsharp** | Geração de PDFs e etiquetas |
| **ClosedXML** | Exportação de dados para Excel |
| **ZXing.Net** | Leitura e geração de códigos de barras |
| **Newtonsoft.Json** | Serialização JSON |
| **MaterialSkin** | Componentes de interface moderna |
| **Git** | Controle de versão |

## Credenciais Padrão

Na primeira execução, o banco de dados é criado automaticamente com um administrador padrão:

| Campo    | Valor              |
|----------|--------------------|
| Email    | `admin@admin.com`  |
| Senha    | `admin`            |

> **Recomendação:** altere a senha do administrador após o primeiro acesso.

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
├── Elements/          # Componentes de UI customizados
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

- [LouasDev](https://github.com/LouasDev)
- [Ithaloluzdepanela](https://github.com/Ithaloluzdepanela)
- [Renato](https://github.com/renato0x)
- [MatheusAlmeida10](https://github.com/MatheusAlmeida10)

---

> **Versão Beta:** Este software está em fase beta. É um projeto acadêmico sujeito a bugs e melhorias. Funcionalidades podem ser adicionadas, alteradas ou removidas em versões futuras.

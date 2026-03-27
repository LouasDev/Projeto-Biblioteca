# Biblioteca Gastão Valle

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET_4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server_Compact-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)
![Status](https://img.shields.io/badge/Status-Beta-orange?style=for-the-badge)

Sistema de gerenciamento desktop desenvolvido para a **Escola Estadual Gastão Valle (Bocaiúva/MG)**. Este projeto moderniza a gestão bibliotecária escolar, substituindo registros manuais por uma solução digital robusta e segura.

> [!IMPORTANT]
> **Nota de Portfólio:** Este repositório é um *mirror/reupload* do projeto original desenvolvido em parceria com a instituição e apoio docente. O objetivo é demonstrar minhas contribuições técnicas e arquitetura de software.

---

## Sobre o Projeto

O **Biblioteca Gastão Valle** é um aplicativo desktop para controle de livros, empréstimos e usuários de uma biblioteca escolar. O sistema conta com:

- **Gestão de Acervo** — Cadastro completo com busca por nome, gênero ou código de barras
- **Controle de Empréstimos** — Registro de empréstimos e devoluções com prazos, prorrogações e status (Ativo, Atrasado, Devolvido)
- **Empréstimo Rápido** — Fluxo simplificado para empréstimos de livros didáticos por turma
- **Gerenciamento de Usuários** — Cadastro de alunos, professores e bibliotecários com CPF, turma, telefone e data de nascimento
- **Automação** — Leitura e geração de etiquetas com código de barras (ZXing.Net)
- **Relatórios** — Exportação de dados para Excel (ClosedXML) e geração de PDFs (iTextSharp)
- **Notificações** — Alertas de disponibilidade de livros e lembretes de devolução
- **Backup Automático** — Backup diário via Google Drive API
- **Modo Offline** — Funcionamento local mesmo sem conexão com a internet
- **Modo Escuro** — Alternância entre tema claro e escuro
- **Mapeamento de Turmas** — Organização e visualização de alunos por turma

---

## Minhas Contribuições (Back-end Focus)

Neste projeto, atuei focado na robustez da aplicação e na experiência do administrador:

- **Arquitetura & POO** — Implementação de toda a lógica de negócios em C#, garantindo regras de empréstimos, devoluções e prazos consistentes
- **Camada de Dados** — Criação das queries para cadastro e controle de estoque de livros utilizando SQL Server Compact Edition
- **Interface (WinForms)** — Desenvolvimento dos formulários de livros, cadastro de livros e participação no design da tela de login

---

## Tecnologias & Bibliotecas

| Categoria | Tecnologia | Uso no Projeto |
|:---|:---|:---|
| **Core** | .NET Framework 4.8 | Motor principal da aplicação |
| **Interface** | C# / WinForms | Interface gráfica desktop |
| **Interface** | MaterialSkin | Estilização moderna e componentes |
| **Banco de Dados** | SQL Server Compact Edition | Armazenamento local |
| **Arquivos** | iTextSharp / PDFsharp | Geração de PDFs e etiquetas |
| **Arquivos** | ClosedXML | Exportação de relatórios Excel |
| **Automação** | ZXing.Net | Processamento de códigos de barras |
| **Cloud** | Google Drive API | Backup automático na nuvem |
| **Dados** | Newtonsoft.Json | Serialização JSON |
| **Versão** | Git | Controle de versão |

---

## Credenciais Padrão

Na primeira execução, o banco de dados é criado automaticamente com um administrador padrão:

| Campo | Valor |
|:---|:---|
| **Email** | `admin@admin.com` |
| **Senha** | `admin` |

> [!WARNING]
> Altere a senha do administrador após o primeiro acesso.

---

## Pré-requisitos

- **Windows** 10 ou superior
- **.NET Framework 4.8** instalado
- **Visual Studio 2019+** (para desenvolvimento)

---

## Como Executar

1. **Clone o repositório:**
   ```bash
   git clone https://github.com/LouasDev/Projeto-Biblioteca.git
   ```

2. **Abra no Visual Studio:**
   ```
   BibliotecaApp/BibliotecaApp.sln
   ```

3. **Restaure os pacotes NuGet:**
   ```
   Botão direito no projeto → Restore NuGet Packages
   ```

4. **Compile e execute (F5)**

5. O banco de dados será criado automaticamente na pasta `bin/Debug/bibliotecaDB/`.

---

## Estrutura do Projeto

```
BibliotecaApp/
├── Elements/              # Componentes de UI customizados
├── Forms/
│   ├── Inicio/            # Tela inicial e formulário principal
│   ├── Livros/            # Cadastro, empréstimo, devolução e busca
│   ├── Login/             # Tela de login e sobre
│   ├── Mapeamento/        # Mapeamento de turmas
│   ├── Relatorio/         # Relatórios
│   └── Usuario/           # Cadastro e edição de usuários
├── Models/                # Modelos de dados
├── Resources/             # Imagens, ícones e recursos
├── Utils/                 # Utilitários (DB, criptografia, backup)
├── Program.cs             # Ponto de entrada
└── App.config             # Configurações
```

---

## Equipe

| Integrante | Perfil |
|:---|:---|
| ManoLouas | [GitHub](https://github.com/ManoLouas) |
| Ithaloluzdepanela | [GitHub](https://github.com/Ithaloluzdepanela) |
| Renato | [GitHub](https://github.com/renato0x) |
| MatheusAlmeida10 | [GitHub](https://github.com/MatheusAlmeida10) |

---

> [!NOTE]
> **Versão Beta:** Este software está em fase beta. É um projeto acadêmico sujeito a bugs e melhorias. Funcionalidades podem ser adicionadas, alteradas ou removidas em versões futuras.

# Product Backlog - Library Management System

## Visão do Produto
Um sistema desktop em C# (Windows Forms) com MySQL para gerenciamento de bibliotecas, permitindo controle de usuários, livros, empréstimos, devoluções e relatórios gerenciais. O sistema suporta autenticação, diferentes tipos de usuário (administrador, aluno, bibliotecário, professor, outros), senhas criptografadas com BCrypt, permissões por tipo de usuário, e funcionalidades específicas como empréstimo rápido e acesso remoto para a secretaria.

## Estrutura do Backlog
- **Épicos**: Grandes áreas funcionais do sistema.
- **User Stories**: Funcionalidades do ponto de vista do usuário, com critérios de aceitação.
- **Tarefas Técnicas**: Atividades técnicas para suportar as funcionalidades.
- **Priorização**: Alinhada às sprints descritas e dependências entre funcionalidades.
- **Responsáveis**: Mapeados conforme a divisão de tarefas e a reunião de 23/04/2025.

## Épicos
1. **Autenticação e Segurança**: Login, permissões por tipo de usuário e criptografia de senhas.
2. **Gerenciamento de Usuários**: Cadastro, edição e exclusão de usuários.
3. **Gerenciamento de Livros**: Cadastro, edição, pesquisa por gênero e controle de disponibilidade.
4. **Empréstimos e Devoluções**: Controle de empréstimos, devoluções, reservas e empréstimo rápido.
5. **Relatórios Gerenciais**: Relatórios de empréstimos, livros expirados e mais emprestados.
6. **Acesso Remoto**: Funcionalidade para a secretaria visualizar alunos com atrasos.
7. **Infraestrutura Técnica**: Configuração do banco, Entity Framework e estrutura do projeto.

## Backlog por Sprint

### Sprint 1: Base do Projeto
**Objetivo**: Configurar a infraestrutura inicial e o banco de dados.

#### User Stories
- **US001**: Como gerente de projeto, quero configurar o repositório Git para versionamento e colaboração da equipe.
  - Critérios de Aceitação:
    - Repositório criado no GitHub ou similar.
    - Estrutura de branches definida (main, develop, feature branches).
  - Responsável: Ithalo Pimentel (Gerente de Projeto)
  - Estimativa: 2 horas
- **US002**: Como desenvolvedor, quero configurar o projeto no Visual Studio com a estrutura de pastas para iniciar o desenvolvimento.
  - Critérios de Aceitação:
    - Projeto C# Windows Forms criado.
    - Pastas criadas: Data, Models, Repositories, Services, Forms, Utils.
  - Responsável: Ithalo Pimentel (Frontend - Tela Principal)
  - Estimativa: 4 horas

#### Tarefas Técnicas
- **TT001**: Criar o banco de dados MySQL com as tabelas Usuario, Livro e Emprestimo, incluindo o campo IdBibliotecaria.
  - Critérios de Aceitação:
    - Script SQL atualizado com tabelas conforme especificação.
    - Campo IdBibliotecaria (INT, NOT NULL) adicionado à tabela Emprestimo.
    - E-mail e CPF não obrigatórios na tabela Usuario.
    - Chaves primárias, estrangeiras e índices configurados.
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 6 horas
- **TT002**: Configurar o Entity Framework para conexão com o MySQL.
  - Critérios de Aceitação:
    - Pacotes NuGet do Entity Framework e MySQL instalados.
    - Contexto de dados (DbContext) configurado com entidades Usuario, Livro e Emprestimo.
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 6 horas
- **TT003**: Atualizar classes de modelo (Usuario.cs, Livro.cs, Emprestimo.cs) com validações e o campo IdBibliotecaria.
  - Critérios de Aceitação:
    - Classes refletem a estrutura do banco (E-mail e CPF não obrigatórios).
    - Emprestimo.cs inclui IdBibliotecaria com anotação [Required].
    - Relacionamentos configurados (ex.: Emprestimo com Usuario e Livro).
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 5 horas

### Sprint 2: Cadastro de Usuários e Login
**Objetivo**: Implementar autenticação e cadastro de usuários.

#### User Stories
- **US003**: Como usuário, quero fazer login com e-mail e senha para acessar o sistema com base no meu tipo de usuário.
  - Critérios de Aceitação:
    - Tela de login com campos para e-mail e senha.
    - Validação de credenciais no banco de dados.
    - Redirecionamento para a tela principal com permissões baseadas em TipoUsuario (Aluno, Bibliotecário, Professor, Administrador, Outros).
    - Mensagem de erro para credenciais inválidas.
  - Responsável: Luis Gustavo (Frontend - Login e Devoluções)
  - Estimativa: 8 horas
- **US004**: Como administrador, quero cadastrar novos usuários para gerenciar alunos, bibliotecários, professores e outros.
  - Critérios de Aceitação:
    - Formulário com campos: Nome, Email (opcional), Senha, CPF (opcional), Data de Nascimento, Turma, Telefone, TipoUsuario.
    - Validações: Campos obrigatórios (Nome, Senha, Data de Nascimento, Turma, Telefone, TipoUsuario).
    - Senha criptografada com BCrypt antes de salvar.
  - Responsável: Renato Monteiro (Frontend - Cadastro de Usuários)
  - Estimativa: 10 horas

#### Tarefas Técnicas
- **TT004**: Implementar criptografia de senhas com BCrypt.
  - Critérios de Aceitação:
    - Função de hash para senhas no cadastro.
    - Função de verificação de senha no login.
  - Responsável: Luis Gustavo (Backend - Regras de Negócio)
  - Estimativa: 4 horas
- **TT005**: Configurar permissões por tipo de usuário no backend.
  - Critérios de Aceitação:
    - Lógica para restringir acesso a telas/funções com base em TipoUsuario (ex.: apenas Administrador cadastra usuários).
  - Responsável: Luis Gustavo (Backend - Regras de Negócio)
  - Estimativa: 5 horas

### Sprint 3: Cadastro de Livros
**Objetivo**: Implementar o gerenciamento de livros com pesquisa por gênero.

#### User Stories
- **US005**: Como bibliotecário, quero cadastrar novos livros para disponibilizá-los no sistema.
  - Critérios de Aceitação:
    - Formulário com campos: Nome, Autor, Gênero, Quantidade, Código de Barras, Disponibilidade.
    - Validações: Código de Barras único, campos obrigatórios.
    - Livro salvo com Disponibilidade = true por padrão.
  - Responsável: Luis Gustavo (Frontend - Cadastro de Livros)
  - Estimativa: 8 horas
- **US006**: Como bibliotecário, quero visualizar, editar e excluir livros, com pesquisa por gênero, para gerenciar o acervo.
  - Critérios de Aceitação:
    - Lista de livros com filtros por gênero e nome.
    - Botões para editar e excluir em cada livro.
    - Formulário de edição pré-preenchido.
  - Responsável: Luis Gustavo (Frontend - Cadastro de Livros)
  - Estimativa: 10 horas

#### Tarefas Técnicas
- **TT006**: Implementar repositório para CRUD de livros.
  - Critérios de Aceitação:
    - Métodos para criar, ler, atualizar e excluir livros.
    - Suporte a filtro por gênero literário.
    - Integração com Entity Framework.
  - Responsável: Luis Gustavo (Backend - Regras de Negócio)
  - Estimativa: 6 horas

### Sprint 4: Empréstimos, Devoluções e Empréstimo Rápido
**Objetivo**: Implementar controle de empréstimos, devoluções e funcionalidade de empréstimo rápido.

#### User Stories
- **US007**: Como bibliotecário, quero registrar um empréstimo para um usuário, incluindo o campo Bibliotecária, para controlar o acervo.
  - Critérios de Aceitação:
    - Formulário com seleção de usuário, livro, data de empréstimo, data de devolução prevista e IdBibliotecaria.
    - Validações: Livro disponível, regras de empréstimo (1 livro por aluno, ilimitado para professores/funcionários).
    - Atualização de Quantidade e Disponibilidade do livro.
    - Prazo padrão de 7 dias, prorrogável por mais 7 dias.
  - Responsável: Renato Monteiro (Frontend - Empréstimos e Reservas)
  - Estimativa: 12 horas
- **US008**: Como bibliotecário, quero registrar a devolução de um livro, pesquisando por aluno ou livro, para atualizar o status.
  - Critérios de Aceitação:
    - Tela com lista de empréstimos em aberto (Status = "Aberto" ou "Atrasado").
    - Pesquisa por nome do aluno ou título do livro.
    - Botão para marcar devolução, atualizando DataRealDevolucao, Status e Disponibilidade do livro.
  - Responsável: Luis Gustavo (Frontend - Devoluções)
  - Estimativa: 8 horas
- **US009**: Como bibliotecário, quero reservar um livro para um usuário quando ele estiver indisponível.
  - Critérios de Aceitação:
    - Opção de reserva para livros com Disponibilidade = false.
    - Registro da reserva no sistema com notificação futura.
  - Responsável: Renato Monteiro (Frontend - Empréstimos e Reservas)
  - Estimativa: 6 horas
- **US010**: Como bibliotecário, quero usar a funcionalidade de Empréstimo Rápido para registrar empréstimos de professores de forma simplificada.
  - Critérios de Aceitação:
    - Botão no menu principal para Empréstimo Rápido.
    - Formulário com campos: Nome do Professor, Nome do Livro, Série, Quantidade, IdBibliotecaria.
    - Validações: Livro disponível, IdBibliotecaria preenchido.
  - Responsável: Bryan Emanuel (Frontend - Empréstimo Rápido)
  - Estimativa: 8 horas

#### Tarefas Técnicas
- **TT007**: Implementar regras de negócio para empréstimos e punições.
  - Critérios de Aceitação:
    - Alunos: 1 livro por vez, 7 dias de prazo, prorrogável por 7 dias.
    - Professores/Funcionários: Sem limite de empréstimos.
    - Atraso: Após 1 mês, Status = "Atrasado"; após 2 meses, notificação.
    - Punições: Aluno com atraso não pode pegar outro livro; após 2 meses, carta de notificação e bloqueio de documentos administrativos.
  - Responsável: Luis Gustavo (Backend - Regras de Negócio)
  - Estimativa: 8 horas
- **TT008**: Implementar repositório para CRUD de empréstimos.
  - Critérios de Aceitação:
    - Métodos para criar, ler, atualizar e excluir empréstimos.
    - Suporte ao campo IdBibliotecaria.
    - Integração com Entity Framework.
  - Responsável: Luis Gustavo (Backend - Regras de Negócio)
  - Estimativa: 6 horas

### Sprint 5: Relatórios e Acesso Remoto
**Objetivo**: Implementar relatórios e funcionalidade para a secretaria.

#### User Stories
- **US011**: Como administrador, quero visualizar relatórios de empréstimos em aberto, expirados e livros mais emprestados.
  - Critérios de Aceitação:
    - Relatório de empréstimos em aberto com filtros por usuário, livro ou data.
    - Relatório de livros expirados (atraso > 1 mês).
    - Relatório de livros mais emprestados, incluindo aluno com maior número de empréstimos.
  - Responsável: Matheus Felipe (Frontend - Relatórios)
  - Estimativa: 10 hours
- **US012**: Como funcionário da secretaria, quero acessar remotamente a lista de alunos com livros em atraso para gerenciar pendências.
  - Critérios de Aceitação:
    - Interface específica para a secretaria com lista de alunos com Status = "Atrasado".
    - Filtros por nome do aluno ou período de atraso.
  - Responsável: Matheus Felipe (Frontend - Relatórios)
  - Estimativa: 8 horas

#### Tarefas Técnicas
- **TT009**: Criar consultas SQL otimizadas para relatórios.
  - Critérios de Aceitação:
    - Queries para empréstimos em aberto, expirados, livros mais emprestados e aluno com mais empréstimos.
    - Integração com Entity Framework.
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 5 horas
- **TT010**: Configurar acesso remoto para a secretaria.
  - Critérios de Aceitação:
    - Lógica de autenticação para usuários da secretaria.
    - Endpoint ou tela específica para consulta de atrasos.
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 6 horas

### Sprint 6: Testes e Refino Final
**Objetivo**: Validar o sistema, incorporar feedback e preparar a entrega.

#### User Stories
- **US013**: Como testador, quero validar todas as telas e fluxos para garantir que o sistema funcione sem erros.
  - Critérios de Aceitação:
    - Testes manuais de login, cadastros, empréstimos, devoluções, relatórios e acesso remoto.
    - Relatório de bugs identificado e corrigido.
    - Validação das regras de negócio (ex.: limite de 1 livro por aluno, punições por atraso).
  - Responsável: Bryan Emanuel (Testes e Qualidade)
  - Estimativa: 12 horas
- **US014**: Como gerente de projeto, quero apresentar o sistema às bibliotecárias e incorporar o feedback delas.
  - Critérios de Aceitação:
    - Demonstração do sistema com todas as funcionalidades.
    - Relatório com feedback das bibliotecárias (ex.: quantidade de livros por aluno, prazos, punições).
    - Ajustes implementados com base no feedback.
  - Responsável: Ithalo Pimentel (Gerente de Projeto)
  - Estimativa: 8 horas
- **US015**: Como gerente de projeto, quero uma documentação final para orientar o uso e manutenção do sistema.
  - Critérios de Aceitação:
    - Documentação com instruções de instalação, uso, diagrama de telas e regras de negócio.
    - Tutorial para usuários finais (bibliotecários e secretaria).
  - Responsável: Ithalo Pimentel (Gerente de Projeto)
  - Estimativa: 8 horas

#### Tarefas Técnicas
- **TT011**: Ajustar a interface com base em feedback dos testes e das bibliotecárias.
  - Critérios de Aceitação:
    - Melhorias na usabilidade (ex.: mensagens de erro claras).
    - Correção de bugs identificados.
  - Responsável: Renato Monteiro (Frontend - Empréstimos e Reservas)
  - Estimativa: 6 horas
- **TT012**: Otimizar o desempenho do sistema.
  - Critérios de Aceitação:
    - Índices adicionais no banco para consultas frequentes (ex.: relatórios, pesquisa por gênero).
    - Redução do tempo de carregamento das telas.
  - Responsável: Matheus Felipe (Backend - Banco de Dados)
  - Estimativa: 4 horas

## Notas
- **Priorização**: As sprints seguem a ordem descrita, com dependências claras (ex.: cadastro de usuários antes de empréstimos).
- **Estimativas**: Baseadas em horas, considerando a complexidade e a experiência da equipe.
- **Alterações Incorporadas**:
  - Campo IdBibliotecaria na tabela e classe Emprestimo.
  - E-mail e CPF não obrigatórios na tabela e classe Usuario.
  - Empréstimo Rápido no menu principal.
  - Pesquisa de devoluções por aluno ou livro.
  - Pesquisa de livros por gênero literário.
  - Regras de empréstimo: 1 livro por aluno, ilimitado para professores/funcionários, prazos e punições.
  - Acesso remoto para a secretaria.
- **Ações Pendentes**:
  - Consulta com Bruno sobre o design do site.
  - Apresentação às bibliotecárias e incorporação de feedback.
  - Validação das regras de empréstimo (quantidade, prazos, punições) com as bibliotecárias.
- **Testes**: Bryan Emanuel valida cada sprint incrementalmente.
- **Segurança**: Criptografia de senhas (BCrypt) e permissões por tipo de usuário são obrigatórias.
- **Documentação**: Atualizada a cada sprint por Ithalo Pimentel.
- **Revisão**: O backlog será revisado no início de cada sprint para ajustes com base em feedback ou mudanças de escopo.
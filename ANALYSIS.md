# Análise Comparativa: Arquiteturas Razor Pages e MVC

## 1\. Introdução

Este documento apresenta uma análise comparativa entre as arquiteturas Razor Pages e MVC (Model-View-Controller) no ecossistema ASP.NET Core. A análise foi baseada na implementação de uma aplicação web de gerenciamento acadêmico, seguindo os tutoriais oficiais da Microsoft, para a disciplina de Programação WEB II do IFS. O objetivo é destacar as diferenças práticas na estrutura do projeto, fluxo de dados e produtividade do desenvolvedor.

## 2\. Visão Geral da Arquitetura Razor Pages

* **Filosofia:** Orientada a páginas (Page-centric), onde a interface do usuário (UI) e a lógica de back-end para uma página específica são mantidas juntas, promovendo maior coesão para funcionalidades focadas em páginas.

* **Estrutura de Arquivos:** O projeto foi criado utilizando o template **"Aplicativo Web ASP.NET Core"** no Visual Studio 2022, resultando em uma estrutura centrada na pasta `Pages`. Cada página consiste em um arquivo `.cshtml` (a view) e um arquivo code-behind `.cshtml.cs` (o `PageModel`), que contém a lógica e os manipuladores de requisição. A solução foi configurada para colocar o projeto e a solução no mesmo diretório.

* **Configuração do Projeto:**

  * **Framework:** .NET 8.0
  * **Autenticação:** Nenhuma
  * **HTTPS:** Configurado
  * **Contêineres:** Suporte não habilitado
  * **Instruções de Nível Superior:** Utilizadas para uma configuração mais concisa no arquivo `Program.cs`.

* **Vantagens Observadas:**

  * *(Preencha com sua análise sobre simplicidade, organização para CRUD, etc.)*

* **Desvantagens Observadas:**

  * *(Preencha com sua análise sobre cenários mais complexos, reutilização de lógica, etc.)*

## 3\. Visão Geral da Arquitetura MVC (Model-View-Controller)

* **Filosofia:** Orientada pela separação de responsabilidades (Separation of Concerns), onde a aplicação é dividida em três componentes interconectados: **Model** (lógica de negócio e dados), **View** (a interface do usuário - UI) e **Controller** (o intermediário que manipula a entrada do usuário). Essa abordagem promove um baixo acoplamento e facilita a manutenção e o teste de cada parte da aplicação de forma independente.

* **Estrutura de Arquivos:** O projeto foi criado utilizando o template "Aplicativo Web do ASP.NET Core (Model-View-Controller)" no Visual Studio 2022. A estrutura resultante organiza o código em pastas distintas para cada responsabilidade:

  * **Controllers:** Contém as classes C\# que recebem as requisições, processam a lógica de entrada e retornam uma View.
  * **Views:** Contém os arquivos `.cshtml` que definem a UI, organizados em subpastas correspondentes a cada Controller.
  * **Models:** Destinada às classes que representam os dados da aplicação e a lógica de negócio.

    A solução foi configurada para colocar o projeto e a solução no mesmo diretório.

* **Configuração do Projeto:**

  * **Framework:** .NET 8.0
  * **Autenticação:** Nenhuma
  * **HTTPS:** Configurado
  * **Contêineres:** Suporte não habilitado
  * **Instruções de Nível Superior:** Não utilizadas, resultando em uma estrutura mais tradicional com os arquivos `Program.cs` e `Startup.cs` (ou a configuração explícita no `Program.cs` com os métodos `CreateBuilder` e `Build`).

* **Vantagens Observadas:**

  * *(Preencha com sua análise sobre testabilidade, flexibilidade para APIs, clareza na separação, etc.)*

* **Desvantagens Observadas:**

  * *(Preencha com sua análise sobre a quantidade de arquivos para tarefas simples, complexidade inicial, etc.)*

## 4\. Camada de Acesso a Dados com Entity Framework Core (Tópico Comum)

Nesta seção, analisamos os conceitos e técnicas aplicados na camada de dados com o Entity Framework Core, que são fundamentais e compartilhados por ambas as arquiteturas.

### 4.1. Análise Prática dos Atributos (Data Annotations)

Os atributos (Data Annotations) são usados para configurar o comportamento do EF Core e para adicionar regras de validação e formatação que são aproveitadas pela aplicação.

* **Mapeamento e Estrutura do Banco de Dados:**

  * `[Table("NomeDaTabela")]`: Especifica explicitamente o nome da tabela no banco de dados.
  * `[Column("NomeDaColuna")]`: Define o nome da coluna no banco, permitindo nomes diferentes na classe C\# e no banco.
  * `[Column(TypeName = "...")]`: Otimiza o armazenamento ao definir um tipo de dado específico.
  * `[NotMapped]`: Exclui uma propriedade da criação de colunas, ideal para propriedades calculadas.
  * `[ForeignKey("NomeDaChave")]`: Liga explicitamente uma propriedade de navegação à sua chave estrangeira.
  * `[DatabaseGenerated(DatabaseGeneratedOption.None)]`: Indica que a aplicação, e não o banco, fornecerá o valor da chave primária.

* **Integridade dos Dados:**

  * `[Index(..., IsUnique = true)]`: Cria um índice único no banco de dados para garantir que não haverá valores duplicados.

* **Regras de Validação do Modelo:**

  * `[Required]`, `[StringLength]`, `[Range]`: Atributos para validação de entrada de dados no servidor e no cliente.

* **Formatação e Metadados para a UI:**

  * `[Display]`, `[DataType]`, `[DisplayFormat]`: Controlam como os dados são exibidos na interface do usuário (nomes de campos, formatação de datas, etc.).

### 4.2. Geração de Código com Scaffolding e Desafios de Implementação

Durante o desenvolvimento, a ferramenta de scaffolding do Visual Studio foi utilizada para automatizar a criação das páginas CRUD (Create, Read, Update, Delete), um passo fundamental que acelera a construção da aplicação. Este processo prático, no entanto, revelou alguns desafios importantes.

#### O Processo Automatizado de Scaffolding

A ferramenta "Razor Pages usando Entity Framework (CRUD)" foi utilizada para gerar a base da funcionalidade de gerenciamento de estudantes. As configurações padrão foram selecionadas para:

* **Gerar páginas completas**, garantindo que cada funcionalidade (Listar, Criar, etc.) tivesse sua própria URL e página dedicada.
* **Incluir bibliotecas de scripts de referência**, habilitando a validação de formulários no lado do cliente para uma melhor experiência do usuário.
* **Utilizar a página de layout do site**, mantendo a consistência visual com o restante da aplicação.

Este processo resultou na criação automática de todas as páginas Razor necessárias, na geração da classe `DbContext` e na configuração da injeção de dependência e da string de conexão, demonstrando a alta produtividade da ferramenta.

#### Desafios Encontrados e Soluções

1. **Incompatibilidade de Versões de Pacotes:** A primeira tentativa de scaffolding falhou com um erro (`Could not load type '...AdHocMapper'`). A investigação revelou que o projeto fazia referência a uma versão de pré-lançamento do `Microsoft.EntityFrameworkCore` (v9.0) enquanto o restante do projeto e o framework de destino eram baseados no .NET 8.

      * **Solução:** O problema foi corrigido editando-se o arquivo `.csproj` para alinhar todas as versões dos pacotes do Entity Framework à versão estável `8.0.x`, compatível com o projeto. Esta etapa reforça a importância crítica da gestão consistente de dependências.

2. **Normalização de Terminações de Linha:** Após a geração bem-sucedida do código, o Visual Studio emitiu um aviso de "Terminações de Linha Inconsistentes".

      * **Causa:** Este aviso ocorre devido a diferenças na forma como sistemas operacionais (Windows vs. Linux/macOS) marcam o final de uma linha em arquivos de texto.
      * **Solução:** O aviso foi resolvido de forma segura aceitando a sugestão do IDE para normalizar os arquivos para o padrão do Windows (CR LF), uma ação de formatação que garante a consistência dos arquivos sem alterar a lógica do código.

3. **Tipos de Referência Anuláveis:** Foi necessária a desativação da diretiva `<Nullable>enable</Nullable>` no arquivo `.csproj` devido a limitações das ferramentas de scaffolding do Visual Studio, que não suportam completamente este recurso moderno do C\#.

### 4.3. Evolução do Contexto de Dados e Refatoração

Após a geração automática do código, o próximo passo crucial foi evoluir o `SchoolContext` de um contexto que gerenciava apenas a entidade `Student` para um que abrange todo o modelo de dados da aplicação. Esta etapa envolveu uma refatoração significativa e a introdução de configurações mais explícitas.

As principais mudanças foram:

* **Expansão do DbContext:** Foram adicionadas as propriedades `DbSet<Course>` e `DbSet<Enrollment>` ao `SchoolContext`, informando ao Entity Framework para gerenciar também as tabelas de Cursos e Matrículas.

* **Refatoração para o Plural (Singular to Plural):** Seguindo uma convenção de codificação comum, a propriedade `DbSet<Student> Student` foi renomeada para `DbSet<Student> Students`. Como um `DbSet` representa uma coleção de entidades, o uso do plural torna o código mais legível e intuitivo.

* **Aplicação da Refatoração no Código Existente:** A renomeação da propriedade `DbSet` quebrou todo o código gerado pelo scaffolding que a utilizava. Foi necessário executar uma busca e substituição em toda a solução (usando a ferramenta "Substituir em Arquivos" do Visual Studio) para trocar todas as ocorrências de `_context.Student` por `_context.Students`, garantindo que o projeto voltasse a compilar.

* **Configuração Explícita com `OnModelCreating`:** Foi introduzido o método `OnModelCreating`, que permite configurar o modelo de dados usando a Fluent API. Isso oferece um controle mais detalhado sobre o esquema do banco de dados do que os Data Annotations sozinhos.

#### Destaque: Resolução de Conflito de Nomenclatura de Tabela

Um ponto de divergência importante em relação ao tutorial genérico ocorreu aqui. O projeto já utilizava o atributo `[Table("Estudantes")]` no modelo `Student` para definir o nome da tabela em português. O código do tutorial, por outro lado, sugeria `modelBuilder.Entity<Student>().ToTable("Student")` no método `OnModelCreating`, criando um conflito direto.

* **Solução Adotada:** A decisão foi adaptar o método `OnModelCreating` para que ele respeitasse os nomes já definidos nos modelos via atributos. A linha foi alterada para `modelBuilder.Entity<Student>().ToTable("Estudantes")`. Esta abordagem resolveu o conflito e manteve a consistência do esquema do banco de dados, demonstrando na prática que a Fluent API (`OnModelCreating`) tem precedência sobre os Data Annotations e deve ser usada para reforçar — e não contradizer — o design do modelo.

## 5\. Comparativo Lado a Lado

| Critério | Razor Pages | MVC (Model-View-Controller) |
| :--- | :--- | :--- |
| **Organização do Código** | Coesão por funcionalidade (a View e sua lógica estão juntas na pasta `Pages`). | Separação por responsabilidade (lógica nos `Controllers`, UI nas `Views`, dados nos `Models`). |
| **Roteamento** | Baseado na estrutura de arquivos e pastas. A URL corresponde diretamente ao caminho do arquivo em `Pages`. | Baseado em convenções e configurações explícitas, geralmente no formato `{controller}/{action}/{id}`. |
| **Fluxo de Requisição** | Requisição -\> Roteamento para o arquivo `.cshtml` -\> Execução dos Handlers do `PageModel` (`OnGet`, `OnPost`) -\> Renderização da Página. | Requisição -\> Roteamento para a `Action` de um `Controller` -\> Processamento da lógica -\> Seleção e renderização da `View`. |
| **Complexidade Inicial** | Menor. Mais direto para criar páginas e formulários simples. | Moderada. Requer a compreensão da interação entre os três componentes. |
| **Ideal para...** | Aplicações centradas em formulários, operações CRUD e cenários onde a página é a unidade principal de funcionalidade. | Aplicações complexas com regras de negócio ricas, APIs web, e cenários que exigem alta testabilidade e flexibilidade. |
| **Reutilização de Lógica** | *(Preencha com sua análise, ex: via View Components, classes base para PageModel)* | *(Preencha com sua análise, ex: Controllers podem servir múltiplas Actions e ser usados para APIs e UI)* |

## 6\. Conclusão

*Aqui você escreverá sua conclusão pessoal sobre qual abordagem se encaixou melhor para este tipo de projeto e por quê, considerando os pontos levantados na organização do código, na camada de dados e no comparativo geral.*

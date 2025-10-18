# Análise Comparativa: Arquiteturas Razor Pages e MVC

## 1. Introdução

Este documento apresenta uma análise comparativa entre as arquiteturas Razor Pages e MVC (Model-View-Controller) no ecossistema ASP.NET Core. A análise foi baseada na implementação de uma aplicação web de gerenciamento acadêmico, seguindo os tutoriais oficiais da Microsoft, para a disciplina de Programação WEB II do IFS. O objetivo é destacar as diferenças práticas na estrutura do projeto, fluxo de dados e produtividade do desenvolvedor.

## 2. Visão Geral da Arquitetura Razor Pages

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

## 3. Visão Geral da Arquitetura MVC (Model-View-Controller)

* **Filosofia:** Orientada pela separação de responsabilidades (Separation of Concerns), onde a aplicação é dividida em três componentes interconectados: **Model** (lógica de negócio e dados), **View** (a interface do usuário - UI) e **Controller** (o intermediário que manipula a entrada do usuário). Essa abordagem promove um baixo acoplamento e facilita a manutenção e o teste de cada parte da aplicação de forma independente.

* **Estrutura de Arquivos:** O projeto foi criado utilizando o template "Aplicativo Web do ASP.NET Core (Model-View-Controller)" no Visual Studio 2022. A estrutura resultante organiza o código em pastas distintas para cada responsabilidade:
  * **Controllers:** Contém as classes C# que recebem as requisições, processam a lógica de entrada e retornam uma View.
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

## 4. Camada de Acesso a Dados com Entity Framework Core (Tópico Comum)

Nesta seção, analisamos os conceitos e técnicas aplicados na camada de dados com o Entity Framework Core, que são fundamentais e compartilhados por ambas as arquiteturas. Os atributos (Data Annotations) são usados para configurar o comportamento do EF Core e para adicionar regras de validação e formatação que são aproveitadas pela aplicação.

### 4.1. Análise Prática dos Atributos (Data Annotations)

A seguir, detalhamos os atributos utilizados nos modelos da aplicação (`Student`, `Course`, `Enrollment`) e suas funções específicas.

* **Mapeamento e Estrutura do Banco de Dados:**
  * `[Table("NomeDaTabela")]`: Especifica explicitamente o nome da tabela no banco de dados para a qual a classe será mapeada. Ex: `[Table("Estudantes")]` na classe `Student`.
  * `[Column("NomeDaColuna")]`: Define o nome da coluna no banco de dados para uma propriedade específica, permitindo que o nome na classe C# seja diferente do nome no banco. Ex: `[Column("FirstName")]` mapeia a propriedade `FirstMidName`.
  * `[Column(TypeName = "varchar(50)")]`: Otimiza o armazenamento no banco de dados ao definir um tipo de dados específico para a coluna, como visto na propriedade `Title` da classe `Course`.
  * `[NotMapped]`: Exclui uma propriedade da criação de colunas no banco de dados. É ideal para propriedades calculadas que existem apenas na lógica da aplicação. Ex: A propriedade `FullName` em `Student`.
  * `[ForeignKey("NomeDaChave")]`: Liga explicitamente uma propriedade de navegação à sua chave estrangeira correspondente, tornando o relacionamento mais claro, embora o EF Core possa inferi-lo por convenção. Ex: `[ForeignKey("CourseID")]` na classe `Enrollment`.
  * `[DatabaseGenerated(DatabaseGeneratedOption.None)]`: Indica que o valor da chave primária não será gerado pelo banco de dados (auto-incremento). A aplicação se torna responsável por fornecer um valor único. Ex: `CourseID` na classe `Course`.

* **Integridade dos Dados:**
  * `[Index(nameof(Propriedade), IsUnique = true)]`: Cria um índice único no banco de dados para a coluna especificada, garantindo que não haverá valores duplicados. Ex: `[Index(nameof(Title), IsUnique = true)]` impede que dois cursos tenham o mesmo título.

* **Regras de Validação do Modelo:**
  * `[Required(ErrorMessage = "...")]`: Torna o preenchimento de um campo obrigatório. A `ErrorMessage` personaliza a mensagem de erro exibida ao usuário.
  * `[StringLength(max, MinimumLength = min, ...)]`: Valida o comprimento de uma string, definindo um tamanho máximo e, opcionalmente, um mínimo.
  * `[Range(min, max, ...)]`: Garante que um valor numérico esteja dentro de um intervalo específico. Ex: `[Range(0, 5)]` para a propriedade `Credits` do `Course`.

* **Formatação e Metadados para a Interface do Usuário (UI):**
  * `[Display(Name = "...")]`: Define o texto que será exibido para o usuário em labels e cabeçalhos de tabelas, facilitando a internacionalização e a clareza da UI.
  * `[DataType(DataType.Date)]`: Informa à engine de renderização do ASP.NET Core sobre o tipo de dado, permitindo que ela gere um controle de UI mais apropriado, como um seletor de datas (`<input type="date">`).
  * `[DisplayFormat(...)]`: Controla a formatação de exibição de um valor.
    * `DataFormatString = "{0:dd/MM/yyyy}"`: Formata a data para o padrão brasileiro.
    * `ApplyFormatInEditMode = true`: Aplica a formatação também em campos de edição.
    * `NullDisplayText = "Sem nota"`: Define um texto a ser exibido quando o valor da propriedade for nulo.

### 4.2. Desafios no Processo de Desenvolvimento

* **Tipos de Referência Anuláveis:** Análise da necessidade de desativar a diretiva `<Nullable>enable</Nullable>` no arquivo `.csproj` devido a limitações das ferramentas de scaffolding do Visual Studio, e as implicações disso (como a remoção do `?` da propriedade `RequestId`).

## 5. Comparativo Lado a Lado

| Critério | Razor Pages | MVC (Model-View-Controller) |
| :--- | :--- | :--- |
| **Organização do Código** | Coesão por funcionalidade (a View e sua lógica estão juntas na pasta `Pages`). | Separação por responsabilidade (lógica nos `Controllers`, UI nas `Views`, dados nos `Models`). |
| **Roteamento** | Baseado na estrutura de arquivos e pastas. A URL corresponde diretamente ao caminho do arquivo em `Pages`. | Baseado em convenções e configurações explícitas, geralmente no formato `{controller}/{action}/{id}`. |
| **Fluxo de Requisição** | Requisição -> Roteamento para o arquivo `.cshtml` -> Execução dos Handlers do `PageModel` (`OnGet`, `OnPost`) -> Renderização da Página. | Requisição -> Roteamento para a `Action` de um `Controller` -> Processamento da lógica -> Seleção e renderização da `View`. |
| **Complexidade Inicial** | Menor. Mais direto para criar páginas e formulários simples. | Moderada. Requer a compreensão da interação entre os três componentes. |
| **Ideal para...** | Aplicações centradas em formulários, operações CRUD e cenários onde a página é a unidade principal de funcionalidade. | Aplicações complexas com regras de negócio ricas, APIs web, e cenários que exigem alta testabilidade e flexibilidade. |
| **Reutilização de Lógica** | *(Preencha com sua análise, ex: via View Components, classes base para PageModel)* | *(Preencha com sua análise, ex: Controllers podem servir múltiplas Actions e ser usados para APIs e UI)* |

## 6. Conclusão

*Aqui você escreverá sua conclusão pessoal sobre qual abordagem se encaixou melhor para este tipo de projeto e por quê, considerando os pontos levantados na organização do código, na camada de dados e no comparativo geral.*

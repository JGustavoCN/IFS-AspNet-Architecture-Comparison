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

### 4.4. Inicialização da Aplicação e Criação do Banco de Dados

O arquivo `Program.cs` é o coração da configuração de uma aplicação ASP.NET Core. Nele, foram realizados passos cruciais para integrar o Entity Framework e garantir um ambiente de desenvolvimento robusto.

* **Injeção de Dependência (DI):** O princípio de DI é fundamental no ASP.NET Core. O serviço `SchoolContext` foi registrado no contêiner de DI com o comando `builder.Services.AddDbContext<SchoolContext>(...)`. Isso permite que qualquer outra parte da aplicação (como uma Razor Page) receba uma instância pronta do contexto do banco de dados através de seu construtor, sem precisar se preocupar em como criá-la.

* **Middleware de Diagnóstico:** Para facilitar a depuração de erros relacionados ao banco de dados, o pacote `Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore` foi adicionado. O serviço `AddDatabaseDeveloperPageExceptionFilter()` foi registrado para fornecer páginas de erro detalhadas especificamente para falhas do Entity Framework durante o desenvolvimento.

* **Criação Automática do Banco de Dados:** Foi implementado um bloco de código que, na inicialização da aplicação, obtém uma instância do `SchoolContext` e chama o método `context.Database.EnsureCreated()`. Esta é uma abordagem útil no início do desenvolvimento para criar o banco de dados e seu esquema a partir dos modelos de dados, sem a necessidade de executar migrações manualmente.

* **Localização da UI:** Como um passo inicial para adaptar a aplicação ao contexto do IFS, os links de navegação no arquivo de layout principal (`_Layout.cshtml`) foram traduzidos para o português, alterando textos como "Students" para "Estudantes".

### 4.5. Propagação de Dados Iniciais (Database Seeding)

Para garantir que a aplicação pudesse ser testada com um conjunto de dados consistente, foi implementado um mecanismo de "seeding". Esta técnica consiste em popular o banco de dados com dados iniciais logo após sua criação.

* **Classe `DbInitializer`:** Foi criada uma classe estática (`DbInitializer.cs`) dedicada exclusivamente a essa responsabilidade. Centralizar essa lógica em uma classe separada mantém o arquivo `Program.cs` mais limpo e organizado.

* **Lógica de Preenchimento:** O método `Initialize` dentro desta classe executa as seguintes ações:
    1. **Verificação de Existência:** Primeiramente, ele verifica se já existem registros na tabela de estudantes com `context.Students.Any()`. Isso torna a operação **idempotente**, ou seja, ela pode ser executada várias vezes sem o risco de duplicar os dados. Se o banco já foi populado, o método simplesmente encerra.
    2. **Criação de Dados:** Caso o banco esteja vazio, são criados arrays de `Student`, `Course` e `Enrollment`.
    3. **Persistência:** Os dados são salvos no banco de dados em lote para cada entidade usando `context.AddRange()` seguido de `context.SaveChanges()`.

* **Integração com a Aplicação:** A chamada para o inicializador foi ativada no arquivo `Program.cs`, removendo o comentário da linha `DbInitializer.Initialize(context)`. Isso garante que o processo de seeding ocorra automaticamente durante a inicialização da aplicação, logo após a criação do banco de dados pelo `EnsureCreated()`.

### 4.6. Adoção de Programação Assíncrona no Acesso a Dados

Um ponto de prática moderna fundamental, aplicado em ambas as implementações, é o uso de programação assíncrona para todas as operações de banco de dados.

* **Por que usar Assincronia?** Em uma aplicação web, as threads do servidor são um recurso limitado. Operações de E/S (Entrada/Saída), como consultar um banco de dados, são lentas. No modelo síncrono, uma thread fica bloqueada, aguardando a resposta do banco sem fazer nada. No modelo **assíncrono**, a thread é liberada de volta para o servidor enquanto a operação do banco de dados está em andamento, permitindo que ela atenda outras requisições. Isso aumenta a eficiência e a capacidade da aplicação de lidar com um volume maior de tráfego.

* **Implementação Prática:** A assincronia foi implementada através de um conjunto de palavras-chave e métodos padrão no C# e Entity Framework Core:
  * **`async` e `await`:** A palavra-chave `async` no método (ex: `public async Task OnGetAsync()`) permite o uso do operador `await`, que é o ponto onde o método "pausa" e libera a thread até que a operação de longa duração (a consulta ao banco) seja concluída.
  * **`Task`:** Os métodos assíncronos retornam um objeto `Task` (ou `Task<T>`), que representa o trabalho em andamento.
  * **Métodos do EF Core:** Foram utilizadas as versões assíncronas dos métodos do Entity Framework, como `ToListAsync()`, `FirstOrDefaultAsync()` e `SaveChangesAsync()`, que são projetados para trabalhar com o padrão `async/await`.

A adoção consciente dessa abordagem é crucial para o desenvolvimento de aplicações web modernas, responsivas e escaláveis.

### 4.7. Carregamento de Dados Relacionados (Eager Loading) na Página de Detalhes

Para enriquecer a página de detalhes do estudante, foi necessário exibir a lista de suas matrículas e os respectivos cursos. Isso foi alcançado através da técnica de **Eager Loading** (carregamento adiantado) do Entity Framework Core, que consiste em carregar os dados relacionados em uma única consulta ao banco de dados.

* **Implementação no PageModel (`Details.cshtml.cs`):** A consulta original, que buscava apenas o estudante (`_context.Students.FirstOrDefaultAsync(...)`), foi aprimorada.
  * **`Include()` e `ThenInclude()`:** Foram encadeados os métodos `.Include(s => s.Enrollments)` para carregar a coleção de matrículas do estudante, e `.ThenInclude(e => e.Course)` para, dentro de cada matrícula, carregar a entidade do curso associado. Isso evita o problema de múltiplas consultas ao banco (conhecido como "N+1 query problem").
  * **Otimização com `AsNoTracking()`:** Foi adicionado o método `.AsNoTracking()`, uma otimização de performance crucial para cenários de apenas leitura. Ele informa ao EF Core para não "rastrear" as entidades retornadas, pois elas não serão alteradas nesta página. Isso reduz a sobrecarga de gerenciamento de estado do `DbContext`.

* **Exibição na View (`Details.cshtml`):** A página de detalhes foi atualizada para incluir uma tabela. Foi utilizado um loop `@foreach` para iterar sobre a propriedade `Model.Student.Enrollments` (que agora está populada) e exibir o título do curso (`item.Course.Title`) e a nota (`item.Grade`) de cada matrícula.

### 4.8. Segurança na Criação de Entidades e Prevenção de Overposting

Um ponto crítico de segurança foi abordado ao refatorar o método `OnPostAsync` da página de criação de estudantes. O código gerado inicialmente era vulnerável a ataques de **overposting**. Isso ocorre quando um usuário mal-intencionado envia mais dados do que o formulário exibe, potencialmente alterando propriedades sensíveis que não deveriam ser modificadas pelo usuário (como um campo `IsAdmin`, por exemplo).

* **Solução com `TryUpdateModelAsync`:** Para mitigar essa vulnerabilidade, o método foi reescrito para usar `TryUpdateModelAsync`. Esta abordagem é mais segura porque:
    1. **Cria uma Entidade Vazia:** Uma nova instância `emptyStudent` é criada em vez de usar a propriedade `Student` vinculada diretamente (`[BindProperty]`).
    2. **Lista de Permissões Explícita:** `TryUpdateModelAsync` foi chamado com uma lista explícita das propriedades que são permitidas para atualização a partir dos dados do formulário (`s => s.FirstMidName, s => s.LastName, s => s.EnrollmentDate`).
    3. **Segurança Garantida:** Qualquer outro campo enviado na requisição que não esteja nesta lista de permissões é simplesmente ignorado. Isso garante que apenas os dados esperados sejam mapeados para a nova entidade antes de serem salvos no banco de dados, fechando a brecha de segurança de overposting.

### 4.9. Padrão Alternativo de Segurança: ViewModels (VMs)

Além da abordagem com `TryUpdateModelAsync`, uma prática de segurança e arquitetura ainda mais robusta para prevenir o overposting é o uso de **ViewModels** (também conhecidos como DTOs - Data Transfer Objects).

* **O Problema:** A vulnerabilidade de overposting ocorre porque o modelo de domínio (a classe `Student`, que representa a tabela do banco) pode conter propriedades sensíveis (`Secret`, `IsAdmin`, etc.) que não estão presentes no formulário da UI. Um atacante pode forjar uma requisição HTTP para enviar valores para esses campos ocultos, e um mecanismo de *model binding* ingênuo poderia salvá-los diretamente no banco de dados.

* **Solução com ViewModels:** O padrão ViewModel resolve isso criando uma camada de abstração. Em vez de expor o modelo de domínio diretamente para a UI, cria-se uma classe específica para a tela em questão (ex: `StudentVM`).
  * **desacoplamento:** Esta `StudentVM` contém **apenas** as propriedades que a UI precisa exibir ou editar (`LastName`, `FirstMidName`, `EnrollmentDate`).
  * **Mapeamento Explícito:** No back-end, os dados são recebidos na `StudentVM`. Em seguida, o código mapeia manualmente (ou com uma ferramenta como o AutoMapper) os valores da VM para uma nova instância da entidade de domínio (`Student`) antes de salvá-la.

Essa abordagem garante que apenas os dados estritamente necessários transitem entre a UI e o back-end, oferecendo o mais alto nível de segurança contra overposting e promovendo um design mais limpo e de baixo acoplamento.

### 4.10. Refatoração da Página de Edição para Segurança e Performance

A página de edição de estudantes (`Edit.cshtml.cs`) foi refatorada para aplicar as mesmas boas práticas de segurança vistas na página de criação, além de introduzir otimizações de performance.

* **Prevenção de Overposting na Edição:** O método `OnPostAsync` original era vulnerável, pois anexava diretamente a entidade vinda do formulário ao `DbContext`. A nova implementação é mais segura:
    1. Primeiro, a entidade original do estudante (`studentToUpdate`) é buscada no banco de dados usando seu `id`.
    2. Em seguida, o método `TryUpdateModelAsync` é utilizado para aplicar as alterações do formulário **apenas** nas propriedades permitidas (`FirstMidName`, `LastName`, `EnrollmentDate`) sobre a entidade que foi carregada do banco. Isso garante que um atacante não possa modificar propriedades que não estão no formulário.

* **Otimização de Consulta com `FindAsync`:** No método `OnGetAsync`, a consulta para buscar o estudante foi alterada de `FirstOrDefaultAsync` para `FindAsync(id)`. O método `FindAsync` é otimizado especificamente para buscar uma entidade pela sua chave primária. Ele primeiro verifica se a entidade já está sendo rastreada pelo `DbContext` e, se estiver, a retorna sem fazer uma nova consulta ao banco, tornando a operação mais eficiente para este cenário.

### 4.11. Gerenciamento de Estado de Entidades no EF Core

O Entity Framework Core, através do `DbContext`, atua como uma unidade de trabalho que rastreia o estado de cada entidade que ele gerencia. Esse rastreamento é o que permite ao método `SaveChangesAsync` saber exatamente quais comandos SQL (INSERT, UPDATE, DELETE) devem ser executados no banco de dados.

* **O Modelo Desconectado da Web:** Em aplicações web, cada requisição HTTP (ex: um GET para carregar a página de edição, seguido por um POST para salvar) cria uma nova instância do `DbContext`. Isso significa que o contexto que carregou os dados é destruído, e um novo contexto é criado para salvar os dados. A aplicação é "desconectada". Por causa disso, o desenvolvedor precisa informar explicitamente ao novo `DbContext` qual é o estado da entidade recebida do formulário.

* **Principais Estados de uma Entidade:**
  * **`Detached` (Desanexado):** O `DbContext` não está rastreando a entidade. Este é o estado de qualquer objeto recém-criado que ainda não foi adicionado ao contexto.
  * **`Added` (Adicionado):** A entidade foi marcada para ser inserida no banco. `SaveChanges` gerará um `INSERT`.
  * **`Unchanged` (Inalterado):** A entidade existe no banco e não sofreu modificações desde que foi carregada.
  * **`Modified` (Modificado):** Pelo menos uma das propriedades da entidade foi alterada. `SaveChanges` gerará um `UPDATE`.
  * **`Deleted` (Excluído):** A entidade foi marcada para ser removida do banco. `SaveChanges` gerará um `DELETE`.

Compreender e gerenciar esses estados é essencial para implementar corretamente as operações de CRUD em um ambiente web.

### 4.12. Tratamento Robusto de Erros na Operação de Exclusão

Para tornar a aplicação mais resiliente, a página de exclusão (`Delete.cshtml.cs`) foi refatorada para gerenciar falhas de banco de dados de forma elegante.

* **Injeção de Dependência para Logging:** O serviço `ILogger` foi injetado no `PageModel`. Isso permite que exceções e outros eventos importantes sejam registrados, o que é fundamental para a depuração e monitoramento da aplicação em produção.

* **Tratamento de `DbUpdateException`:** A lógica de exclusão no método `OnPostAsync` foi envolvida em um bloco `try-catch`. Isso captura especificamente a `DbUpdateException`, que pode ocorrer por várias razões, como problemas de rede transitórios ou violações de integridade referencial (tentar excluir um estudante que ainda tem matrículas, por exemplo).

* **Feedback ao Usuário:** Em caso de falha na exclusão, em vez de travar, a aplicação agora:
    1. Registra o erro detalhado usando o `_logger`.
    2. Redireciona o usuário de volta para a mesma página de exclusão, mas passando um parâmetro na URL (`saveChangesError = true`).
    3. O método `OnGetAsync` utiliza esse parâmetro para definir uma mensagem de erro amigável, que é então exibida na UI, instruindo o usuário a tentar novamente.

Essa abordagem melhora significativamente a experiência do usuário e a estabilidade da aplicação ao lidar com falhas inesperadas na camada de dados.

## 5. Implementação de Funcionalidades Avançadas na UI

Nesta seção, detalhamos a implementação de funcionalidades que melhoram a experiência do usuário na manipulação e visualização de dados, focando nas técnicas aplicadas na camada de apresentação.

### 5.1. Classificação Dinâmica de Dados na Listagem de Estudantes

A página de listagem de estudantes foi aprimorada para permitir que o usuário classifique os dados clicando nos cabeçalhos das colunas (Sobrenome e Data de Matrícula).

* **Execução Adiada com `IQueryable`:** A chave para esta implementação é o uso de `IQueryable<Student>`. Em vez de buscar todos os dados do banco imediatamente, um `IQueryable` representa a *consulta* em si. A lógica no `PageModel` (`Index.cshtml.cs`) constrói dinamicamente a consulta adicionando cláusulas `OrderBy` ou `OrderByDescending` com base no parâmetro `sortOrder` recebido da URL. A consulta só é enviada ao banco de dados e executada no último momento, quando o método `.ToListAsync()` é chamado. Isso é extremamente eficiente, pois garante que uma única consulta otimizada seja gerada, contendo todas as regras de classificação.

* **Gerenciamento de Estado na UI:**
  * **No PageModel:** Propriedades como `NameSort` e `DateSort` foram adicionadas para controlar o estado atual da classificação. Elas usam operadores ternários para determinar qual será o próximo estado de classificação (ascendente ou descendente) quando um link for clicado.
  * **Na View (`Index.cshtml`):** O `asp-route-sortOrder` Tag Helper foi utilizado nos links dos cabeçalhos das colunas. Ele passa o valor de `NameSort` ou `DateSort` como um parâmetro de consulta na URL, que é então lido pelo método `OnGetAsync` na próxima requisição, completando o ciclo.

### 5.2. Filtragem de Dados do Lado do Servidor

Para aprimorar a usabilidade da página, foi adicionada a funcionalidade de filtragem, permitindo ao usuário buscar estudantes por nome ou sobrenome.

* **Implementação na View (`Index.cshtml`):** Um formulário (`<form>`) foi adicionado contendo uma caixa de texto (`<input type="text" name="SearchString">`) e um botão de envio. Crucialmente, o formulário utiliza `method="get"`, o que faz com que o termo de busca seja enviado como um parâmetro na query string da URL (ex: `.../Students?SearchString=Anand`). Essa abordagem é uma boa prática para operações de busca, pois não modifica dados no servidor e permite que os usuários salvem ou compartilhem a URL com os resultados da pesquisa.

* **Lógica no PageModel (`Index.cshtml.cs`):**
  * O método `OnGetAsync` foi atualizado para receber o parâmetro `searchString` da URL.
  * A lógica de filtragem foi aplicada diretamente sobre o `IQueryable`, antes da lógica de classificação e da chamada final a `.ToListAsync()`.
  * Foi utilizada uma cláusula `Where()` condicional (`if (!String.IsNullOrEmpty(searchString))`) que filtra os estudantes cujo `LastName` ou `FirstMidName` contenha a string de busca.

* **Vantagem de Performance (`IQueryable` vs. `IEnumerable`):** Ao aplicar o filtro `.Where()` sobre o `IQueryable`, garantimos que a filtragem ocorra **no lado do servidor de banco de dados**. O Entity Framework traduz a cláusula `Where` para uma cláusula `WHERE` em SQL. A alternativa, que seria buscar todos os dados e filtrá-los na memória da aplicação (com `IEnumerable`), seria drasticamente menos performática, especialmente com grandes volumes de dados.

### 5.3. Paginação de Dados do Lado do Servidor

Para completar a funcionalidade da listagem, foi implementada a paginação, garantindo que a aplicação permaneça performática mesmo com milhares de registros.

* **Classe Reutilizável `PaginatedList<T>`:** Foi criada uma classe genérica, `PaginatedList<T>`, para encapsular a lógica de paginação. Esta classe herda de `List<T>` e adiciona propriedades essenciais para a UI, como `PageIndex`, `TotalPages`, `HasPreviousPage` e `HasNextPage`. O seu método estático `CreateAsync` é o responsável por executar a consulta paginada.

* **Paginação Eficiente no Banco de Dados:** A eficiência da paginação é garantida pelo uso dos métodos `Skip()` e `Take()` do LINQ, que são aplicados diretamente ao `IQueryable`. Isso é traduzido pelo Entity Framework em uma consulta SQL otimizada (usando `OFFSET` e `FETCH` no SQL Server), garantindo que **apenas a página de dados solicitada** seja transferida do banco de dados para a aplicação.

* **Integração com a UI:**
  * **No PageModel (`Index.cshtml.cs`):** A lógica foi atualizada para receber o `pageIndex` da URL. O método `PaginatedList.CreateAsync` é chamado no final, após a aplicação dos filtros e da classificação, para obter a página de dados correta. O tamanho da página foi tornado configurável através do `appsettings.json`.
  * **Na View (`Index.cshtml`):** Foram adicionados os botões "Previous" e "Next". Eles são habilitados ou desabilitados dinamicamente com base nas propriedades `HasPreviousPage` and `HasNextPage`. Crucialmente, os Tag Helpers `asp-route-*` foram usados para garantir que os parâmetros de **classificação (`sortOrder`) e filtro (`currentFilter`) atuais sejam preservados** ao navegar entre as páginas.

### 5.4. Agregação de Dados para a Página de Estatísticas (Sobre)

Para exibir estatísticas sobre o corpo discente, foi criada a página "Sobre", que agrupa os estudantes pela data de matrícula e exibe a contagem de cada grupo.

* **ViewModel Específico para a Tela (`EnrollmentDateGroup`):** Em vez de retornar os dados brutos, foi criado um ViewModel (`EnrollmentDateGroup.cs`) contendo apenas as duas propriedades necessárias para a exibição: `EnrollmentDate` e `StudentCount`. Essa abordagem (usar um modelo específico para a view) desacopla a camada de apresentação do modelo de domínio e é uma prática recomendada para clareza e segurança.

* **Agregação Eficiente com LINQ `group by`:** A lógica de agregação foi implementada no `PageModel` (`About.cshtml.cs`) usando uma consulta LINQ com a cláusula `group student by student.EnrollmentDate`. Essa operação é traduzida pelo Entity Framework em uma consulta SQL `GROUP BY` eficiente, que é executada inteiramente no servidor de banco de dados que é o SQLServer. Como resultado, apenas os dados já agrupados e contados são transferidos para a aplicação, o que é extremamente performático, independentemente do número de estudantes no banco.

## 6. Evolução do Banco de Dados com EF Core Migrations

Inicialmente, o projeto utilizou o método `EnsureCreated()` para criar o banco de dados. Embora seja útil para o desenvolvimento rápido e prototipagem, essa abordagem tem uma limitação crítica: ela não consegue evoluir um banco de dados existente. Qualquer alteração no modelo de dados exigiria a exclusão manual do banco, resultando na perda de todos os dados.

Para preparar a aplicação para um cenário mais realista, onde os dados precisam ser preservados, foi adotado o **EF Core Migrations**.

* **Por que usar Migrations?** Migrations é o recurso que permite que o esquema do banco de dados evolua junto com as alterações no modelo de dados da aplicação, **sem perder os dados existentes**. Ele funciona como um sistema de controle de versão para o banco de dados.

* **Processo de Implementação:** A transição foi feita em três passos:
    1. **Remoção do `EnsureCreated()`:** A chamada `context.Database.EnsureCreated()` foi removida do arquivo `Program.cs`, pois é incompatível com o Migrations.
    2. **Criação da Migração Inicial:** O comando `Add-Migration InitialCreate` foi executado. Ele inspecionou os modelos de dados (`Student`, `Course`, etc.) e gerou um arquivo de migração contendo o código C# necessário para criar o esquema de banco de dados correspondente.
    3. **Aplicação da Migração:** O comando `Update-Database` foi executado para aplicar a migração, criando o banco de dados e, crucialmente, uma tabela especial chamada `__EFMigrationsHistory`. Esta tabela é usada pelo EF Core para rastrear quais migrações já foram aplicadas ao banco, garantindo que cada alteração seja aplicada apenas uma vez.

Com o Migrations configurado, a aplicação está agora pronta para evoluir de forma segura e controlada.

### 6.1. Anatomia de uma Migração

Cada migração gerada pelo EF Core é composta por vários artefatos que trabalham em conjunto para garantir a consistência entre o código e o banco de dados.

* **Métodos `Up()` e `Down()`:** O coração de uma migração está no arquivo `<timestamp>_<MigrationName>.cs`.
  * **`Up()`:** Contém o código para **aplicar** as alterações ao banco de dados (criar tabelas, adicionar colunas, etc.). É executado pelo comando `Update-Database`.
  * **`Down()`:** Contém o código para **reverter** as alterações, restaurando o esquema ao estado anterior. Isso permite "desfazer" uma migração, se necessário.

* **Tabela de Histórico (`__EFMigrationsHistory`):** Ao aplicar a primeira migração, o EF Core cria esta tabela no banco de dados. Ela funciona como um log, registrando o ID de cada migração que já foi aplicada com sucesso. Isso impede que a mesma migração seja executada mais de uma vez e garante que as migrações sejam aplicadas na ordem correta.

* **O Instantâneo do Modelo (`SchoolContextModelSnapshot.cs`):** Este arquivo é um registro do estado atual do seu modelo de dados no momento em que a última migração foi criada. Quando você executa `Add-Migration`, o EF Core compara o seu modelo de dados atual com este arquivo de snapshot para detectar as alterações e gerar o código para os métodos `Up()` e `Down()` da nova migração.

### 6.2. Refinando o Modelo de Dados com Anotações e Migração

Para aprimorar o modelo de dados, foram aplicados diversos atributos de "Data Annotation" na classe `Student`. Essas anotações adicionam metadados que controlam a validação, a formatação da UI e o mapeamento do banco de dados.

* **Regras de Validação:** Atributos como `[Required]` e `[StringLength(50)]` foram adicionados para impor regras de negócio, como campos obrigatórios e comprimentos máximos de string. Isso habilita a validação automática tanto no lado do servidor quanto no lado do cliente.

* **Formatação e Exibição na UI:**
  * **`[Display(Name = "...")]`:** Foi usado para definir rótulos amigáveis para os campos nos formulários (ex: "Sobrenome" em vez de "LastName").
  * **`[DataType(DataType.Date)]` e `[DisplayFormat]`:** Foram usados em conjunto na propriedade `EnrollmentDate` para indicar ao navegador que ele deve renderizar um seletor de data e para formatar a data explicitamente, sem exibir a hora.

* **Mapeamento do Banco de Dados:**
  * **`[Column("FirstName")]`:** Este atributo foi usado para renomear a coluna do banco de dados de `FirstMidName` para `FirstName`, melhorando a clareza do esquema sem alterar o nome da propriedade no código C#.
  * **Propriedade Calculada:** A propriedade `FullName` foi mantida como uma propriedade calculada (sem um `set`) para exibir o nome completo na UI, sem a necessidade de criar uma coluna correspondente no banco de dados.

* **Aplicação das Alterações com uma Nova Migração:** Como essas anotações alteraram a estrutura esperada do banco de dados, o modelo de dados ficou fora de sincronia. Para resolver isso, uma nova migração (`Add-Migration ColumnFirstName`) foi criada e aplicada (`Update-Database`), atualizando o esquema do banco de dados (alterando o tipo de `nvarchar(MAX)` para `nvarchar(50)` e renomeando a coluna) sem perda de dados.

### 6.3. Expansão do Modelo com a Entidade Instructor

O modelo de dados foi expandido com a adição da entidade `Instructor`, que foi aprimorada com os mesmos padrões de validação e localização em português das outras entidades. Esta nova classe introduz dois tipos importantes de relacionamentos, representados por suas propriedades de navegação:

* **Relação Um-para-Muitos (`ICollection<Course>`):** A propriedade `Courses` é uma coleção, estabelecendo que um instrutor pode ministrar vários cursos.

* **Relação Um-para-Um (`OfficeAssignment`):** A propriedade `OfficeAssignment` representa uma relação onde um instrutor pode ter no máximo um escritório. A propriedade conterá uma única entidade `OfficeAssignment` ou será nula se nenhum escritório for atribuído, modelando um relacionamento opcional de um para um.

### 6.4. Modelando a Relação Um-para-Um com OfficeAssignment

A entidade `OfficeAssignment` foi criada para modelar uma relação de **um-para-zero-ou-um** com a entidade `Instructor`. Isso significa que um instrutor pode ter um, e apenas um, escritório, ou pode não ter nenhum.

* **Chave Primária como Chave Estrangeira:** A chave para implementar este relacionamento está no design da classe `OfficeAssignment`.
  * O atributo `[Key]` foi aplicado à propriedade `InstructorID`. Isso a designa como a **chave primária (PK)** da tabela `OfficeAssignment`.
  * Como o nome `InstructorID` corresponde à convenção de chave estrangeira para a entidade `Instructor`, o Entity Framework Core a configura simultaneamente como uma **chave estrangeira (FK)**.
  * Quando a PK de uma tabela é também a FK para outra, o EF Core estabelece uma relação de um-para-um. Isso garante a integridade dos dados, pois não é possível criar uma `OfficeAssignment` sem que ela esteja vinculada a um `Instructor` existente.

### 6.5. Aprimoramento da Entidade Course e Relação Muitos-para-Muitos

A entidade `Course` foi atualizada para refletir relacionamentos mais complexos e para otimizar as operações de dados.

* **Chave Primária Gerada pela Aplicação:** O atributo `[DatabaseGenerated(DatabaseGeneratedOption.None)]` foi aplicado à chave primária `CourseID`. Isso informa ao Entity Framework que os valores para esta chave serão fornecidos pela aplicação (por exemplo, um código de curso definido pelo usuário, como "MAT101"), em vez de serem gerados automaticamente pelo banco de dados.

* **Chave Estrangeira Explícita (`DepartmentID`):** A propriedade `DepartmentID` foi adicionada explicitamente ao modelo. Embora o EF Core possa criar "propriedades de sombra" para chaves estrangeiras, incluí-las explicitamente no modelo torna as operações de atualização mais simples e eficientes, pois permite alterar a relação de um curso com um departamento apenas definindo o valor do ID, sem a necessidade de carregar a entidade `Department` inteira.

* **Relação Muitos-para-Muitos:** A adição da propriedade de navegação `ICollection<Instructor>` na classe `Course`, em conjunto com a propriedade `ICollection<Course>` na classe `Instructor`, estabelece uma relação **muitos-para-muitos**. O Entity Framework Core gerencia essa relação de forma inteligente, criando automaticamente uma tabela de junção (join table) no banco de dados (por exemplo, `CourseInstructor`) para conectar as duas entidades.

### 6.6. Finalização do Modelo com a Entidade Department e Controle de Concorrência

A entidade `Department` foi a última peça a ser adicionada ao modelo de dados, trazendo consigo conceitos importantes de mapeamento e segurança.

* **Mapeamento de Tipo de Dados Específico:** Na propriedade `Budget`, o atributo `[Column(TypeName = "money")]` foi utilizado para instruir o Entity Framework a mapear o tipo `decimal` do C# para o tipo de dados `money` do SQL Server. Essa é uma prática recomendada para campos monetários, garantindo a precisão e a semântica correta no banco de dados.

* **Relacionamento Opcional:** A propriedade de chave estrangeira `InstructorID` foi definida como um tipo anulável (`int?`). Isso modela corretamente a regra de negócio de que um departamento pode ter um administrador, mas não é obrigatório.

* **Controle de Concorrência Otimista:** Como uma melhoria de robustez, o atributo `[ConcurrencyCheck]` foi adicionado à propriedade `Budget`. Este atributo instrui o EF Core a verificar se o valor do orçamento no banco de dados mudou desde que foi lido, antes de salvar uma nova alteração. Isso previne que dois usuários editem o mesmo dado simultaneamente e um sobrescreva o trabalho do outro (conflito de concorrência).

### 6.7. Modelando a Entidade de Associação com Payload (Enrollment)

A entidade `Enrollment` (Matrícula) desempenha um papel crucial no modelo, atuando como a **entidade de associação** que resolve a relação muitos-para-muitos entre `Student` e `Course`.

* **Join Entity com Payload:** Diferente de uma relação muitos-para-muitos pura (onde o EF Core pode criar uma tabela de junção oculta), a relação entre estudantes e cursos precisava armazenar uma informação adicional: a **Nota (`Grade`)**. Por isso, foi necessário criar a entidade `Enrollment` explicitamente. Ela não apenas conecta `Student` e `Course` através das chaves estrangeiras `StudentID` e `CourseID`, mas também carrega esse dado extra, conhecido como "payload".

* **Enumeração para `Grade`:** Para garantir a consistência dos dados de notas, foi utilizada uma enumeração (`enum Grade { A, B, C, D, F }`), restringindo os valores possíveis e tornando o código mais legível e seguro contra erros de digitação.

### 6.8. Configuração Final do DbContext com Fluent API

Com todas as entidades do modelo de dados criadas, o `SchoolContext` foi finalizado para incluir os `DbSet`s de todas as novas entidades (`Department`, `Instructor`, `OfficeAssignment`) e para realizar configurações de relacionamento avançadas usando a **Fluent API** no método `OnModelCreating`.

* **Fluent API vs. Atributos:** Enquanto os Atributos (Data Annotations) são úteis para aplicar regras diretamente nas classes de modelo, a Fluent API oferece um local centralizado e mais poderoso para configurar o mapeamento. A abordagem adotada no projeto foi híbrida: usar atributos para validações e formatações simples e a Fluent API para configurações de relacionamento complexas que os atributos não suportam.

* **Configuração de Relação Muitos-para-Muitos:** A relação muitos-para-muitos "pura" (sem uma entidade de junção com payload) entre `Course` e `Instructor` foi configurada explicitamente no `OnModelCreating`. Isso instrui o EF Core a criar a tabela de junção necessária automaticamente.

* **Configuração de Comportamento de Exclusão:** Para aumentar a integridade dos dados, a Fluent API foi usada para configurar o comportamento de exclusão em cascata. Com a regra `.OnDelete(DeleteBehavior.Restrict)`, o sistema agora impede que um `Department` seja excluído se houver `Course`s associados a ele, evitando a perda acidental de dados.

### 6.11. Atualização da Propagação de Dados (Seeding) e Recriação do Banco

Com o modelo de dados completo, a classe `DbInitializer` foi expandida para popular todas as novas entidades, incluindo `Instructors`, `Departments`, `OfficeAssignments`, e as relações entre elas (como atribuições de cursos a instrutores). Para garantir que o banco de dados refletisse perfeitamente o modelo final e fosse populado com os dados corretos, um ciclo de recriação foi executado:

1. O banco de dados de desenvolvimento foi descartado (`Drop-Database`).
2. A migração inicial existente foi removida (`Remove-Migration`) para limpar o histórico.
3. Uma nova migração `InitialCreate` foi gerada, capturando o estado final de todo o modelo de dados.
4. O banco de dados foi recriado a partir desta nova migração (`Update-Database`), garantindo um esquema 100% sincronizado.

Ao executar a aplicação, o `DbInitializer` populou com sucesso o banco de dados recém-criado com um conjunto de dados completo e realistas.

## 7. Estratégias de Carregamento de Dados Relacionados

O Entity Framework Core oferece diferentes estratégias para carregar dados de entidades relacionadas (por exemplo, carregar o `Department` de um `Course`). A escolha da estratégia correta tem um impacto direto na performance da aplicação, principalmente no número de consultas executadas no banco de dados.

* **Carregamento Adiantado (Eager Loading):** Esta é a abordagem mais comum e, geralmente, a mais recomendada para aplicações web. Os dados relacionados são carregados na mesma consulta inicial da entidade principal, utilizando os métodos `.Include()` e `.ThenInclude()`. Isso resulta em uma única consulta (geralmente com `JOIN`s) que traz todos os dados necessários de uma só vez, evitando o problema de múltiplas idas e vindas ao banco. Foi a técnica utilizada na página de detalhes do estudante para carregar suas matrículas e cursos.

* **Carregamento Explícito (Explicit Loading):** Neste cenário, os dados relacionados não são carregados inicialmente. Eles são buscados "sob demanda" através de uma chamada explícita no código, como `_context.Entry(course).Reference(c => c.Department).LoadAsync()`. Isso resulta em uma segunda consulta ao banco de dados. É útil em cenários onde os dados relacionados só são necessários condicionalmente.

* **Carregamento Lento (Lazy Loading):** Com o Lazy Loading, os dados relacionados são carregados automaticamente na primeira vez que a propriedade de navegação é acessada no código. Embora pareça conveniente, esta abordagem é **perigosa** em aplicações web, pois pode levar facilmente ao problema de "N+1 queries" (uma consulta para a lista de entidades + N consultas para os dados relacionados de cada entidade dentro de um loop), causando sérios problemas de performance. Por padrão, o Lazy Loading vem desabilitado no EF Core e seu uso deve ser feito com muito cuidado.

### 7.1. Implementando Eager Loading na Listagem de Cursos

Após gerar o scaffold para as páginas de `Course`, a listagem inicial exibia apenas o `DepartmentID`, que não é uma informação útil para o usuário. Para corrigir isso e exibir o nome do departamento, a estratégia de **Eager Loading** foi aplicada.

* **Implementação no PageModel (`Courses/Index.cshtml.cs`):** A consulta LINQ no método `OnGetAsync` foi modificada para incluir explicitamente os dados do departamento relacionado.
  * **`.Include(c => c.Department)`:** Este método foi encadeado à consulta, instruindo o Entity Framework a carregar a entidade `Department` associada a cada `Course` na mesma consulta ao banco de dados.
  * **Otimização com `.AsNoTracking()`:** Como a página de listagem é um cenário de apenas leitura (os dados não são alterados), o método `.AsNoTracking()` foi adicionado. Isso informa ao EF Core para não rastrear as entidades retornadas, resultando em uma consulta mais rápida e com menor consumo de memória.

* **Exibição na View (`Courses/Index.cshtml`):** Com a entidade `Department` agora carregada, a view foi atualizada para exibir a propriedade `Name` do departamento, em vez do ID. A alteração foi simples, mudando a exibição para `@Html.DisplayFor(modelItem => item.Department.Name)`.

### 7.2. Otimização com Projeção para um ViewModel (usando .Select())

Como uma alternativa mais performática ao Eager Loading com `.Include()`, foi implementada a técnica de **projeção** na listagem de cursos. Essa abordagem consiste em transformar os resultados de uma consulta LINQ diretamente em um objeto de transferência de dados (DTO), ou **ViewModel**, que contém apenas os dados necessários para a tela.

* **Benefícios de Performance:**
    1. **Consultas mais Leves:** Em vez de carregar a entidade `Course` inteira e a entidade `Department` inteira, a consulta com `.Select()` gera um SQL que busca **apenas** as colunas necessárias (CourseID, Title, Credits e Department.Name). Isso reduz a quantidade de dados trafegados do banco para a aplicação.
    2. **Sem Rastreamento (No-Tracking):** Como o resultado da consulta não é uma entidade de domínio (é um `CourseViewModel`), o `DbContext` não precisa rastrear as mudanças. Isso elimina a sobrecarga de gerenciamento de estado, tornando a consulta inerentemente mais rápida, similar ao efeito de usar `.AsNoTracking()`.

* **Implementação:** Foi criado um `CourseViewModel` contendo somente as propriedades a serem exibidas. A consulta no `PageModel` foi reescrita para usar `.Select(p => new CourseViewModel { ... })`, projetando os dados das entidades `Course` e `Department` diretamente para a nova classe antes de materializar a lista com `.ToListAsync()`.

### 7.3. Implementação da Página Mestre-Detalhe de Instrutores

A página de Instrutores é a mais complexa da aplicação, implementando um padrão de UI "mestre-detalhe-detalhe" que exibe três níveis de informação: a lista de instrutores, os cursos ministrados pelo instrutor selecionado e os alunos matriculados no curso selecionado.

* **ViewModel Agregador (`InstructorIndexData`):** Para gerenciar os dados das três tabelas (`Instructors`, `Courses`, `Enrollments`) em uma única página, foi criado o ViewModel `InstructorIndexData`. Essas classes atuam como um contêiner para transportar todas as informações necessárias entre o `PageModel` e a `View`.

* **Estratégia de Carregamento Híbrida:** Uma estratégia de carregamento de dados em múltiplos estágios foi utilizada para otimizar a performance:
    1. **Carregamento Adiantado Inicial:** Na carga inicial da página, uma única consulta complexa é executada para buscar **todos** os instrutores. Essa consulta utiliza `Include` e `ThenInclude` para carregar adiantadamente os dados relacionados de `OfficeAssignment` e `Courses` (incluindo o `Department` de cada curso).
    2. **Filtragem em Memória:** Quando um instrutor é selecionado, a lista de seus cursos é obtida **filtrando os dados já carregados em memória**, evitando uma nova consulta ao banco de dados para buscar os cursos.
    3. **Carregamento Sob Demanda:** Apenas quando um curso é selecionado, uma **nova consulta** é executada no banco de dados para buscar especificamente as matrículas (`Enrollments`) daquele curso, incluindo os dados dos alunos (`Student`) relacionados.

* **Melhorias na Interface do Usuário:**
  * **Rotas Amigáveis:** A diretiva `@page "{id:int?}"` foi utilizada para transformar os parâmetros da URL de query strings (ex: `?id=1`) para segmentos de rota (ex: `/Instructors/1`), resultando em URLs mais limpas.
  * **Feedback Visual:** Foi implementada uma lógica na View que aplica a classe CSS `table-success` à linha (`<tr>`) do instrutor e do curso atualmente selecionados, fornecendo um feedback visual claro para os usuários.

## 8. Refatoração da UI e Boas Práticas de Apresentação

Com as funcionalidades básicas implementadas, o foco se voltou para a melhoria da qualidade do código e da experiência do usuário, aplicando padrões de reutilização e gerenciando a atualização de dados relacionados diretamente na interface.

### 8.1. Reutilização de Código com Classes Base de PageModel

Para evitar a duplicação de lógica em diferentes páginas, foi implementado o padrão de classe base para os PageModels.

* **Para Cursos (`DepartmentNamePageModel`):** A funcionalidade de popular a lista suspensa de departamentos, necessária tanto na página de Criação quanto na de Edição de Cursos, foi centralizada. As classes `CreateModel` e `EditModel` da pasta `Courses` foram modificadas para herdar desta classe base, eliminando código repetido (princípio DRY).

* **Para Instrutores (`InstructorCoursesPageModel`):** De forma similar, para gerenciar a lista de cursos que um instrutor pode ministrar (funcionalidade necessária na criação e edição de instrutores), foi criada uma classe base. Este `PageModel` contém a lógica `PopulateAssignedCourseData`, que prepara a lista de cursos com checkboxes para a UI.

### 8.2. Uso de Modelos Fortemente Tipados na UI

Para aumentar a segurança e a manutenibilidade, a passagem de dados para elementos de formulário foi alterada de métodos fracamente tipados para fortemente tipados. Em vez de usar `ViewData` (que depende de strings "mágicas"), as Views (`.cshtml`) agora se vinculam diretamente a propriedades no `Model`, como `DepartmentNameSL`. Essa abordagem permite que o compilador verifique se a propriedade existe, prevenindo erros que só seriam descobertos em tempo de execução.

### 8.3. Gerenciamento da Relação Muitos-para-Muitos na UI (Instrutores)

A parte mais complexa foi a atualização da relação muitos-para-muitos entre Instrutores e Cursos. Como o `model binding` não atualiza automaticamente uma coleção a partir de checkboxes, uma lógica manual foi implementada no `OnPostAsync` da página de edição de instrutores.

* **Lógica de Sincronização:** Um método auxiliar (`UpdateInstructorCourses`) foi criado para comparar a lista de cursos selecionados no formulário com a lista de cursos já associados ao instrutor no banco de dados, adicionando ou removendo as relações conforme necessário. O uso de `HashSet` tornou as operações de comparação mais eficientes.

### 8.4. Gerenciamento de Relacionamentos Dependentes na Exclusão

Para manter a integridade referencial, a página de Exclusão de Instrutor (`DeleteModel`) foi aprimorada. Antes de excluir um instrutor, o código agora verifica se ele é administrador de algum departamento. Se for, a referência (`InstructorID`) nesses departamentos é definida como `null` antes da exclusão, evitando erros de chave estrangeira no banco de dados.

## 9. Tratamento de Conflitos de Concorrência

Para garantir a integridade dos dados em um ambiente multiusuário, foi implementado o controle de **concorrência otimista**. Essa estratégia permite que conflitos aconteçam (quando dois usuários tentam editar o mesmo registro ao mesmo tempo), mas fornece um mecanismo para detectá-los e tratá-los de forma adequada, evitando que as alterações de um usuário sejam sobrescritas silenciosamente por outro.

### 9.1. Implementação com Token de Concorrência (`rowversion`)

A abordagem escolhida, recomendada para SQL Server, foi o uso de um token de concorrência no nível da linha.

* **Modificação do Modelo:** Foi adicionada uma nova propriedade à entidade `Department`:

    ```csharp
    [Timestamp]
    public byte[] ConcurrencyToken { get; set; }
    ```

* **Funcionamento:** O atributo `[Timestamp]` instrui o Entity Framework a mapear esta propriedade para uma coluna do tipo `rowversion` no banco de dados. O próprio SQL Server atualiza automaticamente o valor desta coluna a cada `UPDATE` na linha.
* **Detecção do Conflito:** Quando o EF Core executa um comando `UPDATE` ou `DELETE`, ele inclui o valor original do `ConcurrencyToken` na cláusula `WHERE`. Se a linha foi alterada por outro usuário nesse meio tempo, os tokens não corresponderão, o comando não afetará nenhuma linha, e o EF Core lançará uma `DbUpdateConcurrencyException`.

* **Aplicação via Migração:** A adição da propriedade `ConcurrencyToken` alterou o modelo de dados, exigindo a criação (`Add-Migration RowVersion`) e aplicação (`Update-Database`) de uma nova migração para adicionar a coluna `rowversion` à tabela `Departamentos`.

### 9.2. Tratamento da `DbUpdateConcurrencyException` na Interface do Usuário

A lógica para lidar com a exceção foi implementada nas páginas de Edição e Exclusão de Departamentos.

* **Captura da Exceção:** No método `OnPostAsync` de ambas as páginas, a chamada `await _context.SaveChangesAsync()` foi envolvida em um bloco `try-catch` para capturar a `DbUpdateConcurrencyException`.

* **Feedback ao Usuário na Edição:** Quando um conflito é detectado na página `EditModel`, a aplicação:
    1. Obtém os valores que o usuário tentou salvar (valores do cliente) e os valores que estão atualmente no banco de dados.
    2. Adiciona uma mensagem de erro geral ao `ModelState`, explicando o conflito.
    3. Para cada campo conflitante, adiciona uma mensagem de erro específica, mostrando o valor atual no banco (ex: "Valor atual no banco: R$ 123.456,78").
    4. Recarrega a página, permitindo que o usuário veja as diferenças e decida se deseja salvar suas alterações novamente.

* **Feedback ao Usuário na Exclusão:** Quando um conflito é detectado na página `DeleteModel`, a aplicação redireciona o usuário de volta para a mesma página de exclusão, passando um parâmetro de erro na URL, que exibe uma mensagem clara informando que o registro foi alterado e a exclusão foi cancelada.

* **Interface do Usuário (`Edit.cshtml` e `Delete.cshtml`):** Para que o controle de concorrência funcione, um campo oculto (`<input type="hidden">`) para o `ConcurrencyToken` foi adicionado aos formulários, garantindo que o valor original do token seja enviado de volta ao servidor no `POST`.

## 10\. Comparativo Lado a Lado

| Critério | Razor Pages | MVC (Model-View-Controller) |
| :--- | :--- | :--- |
| **Organização do Código** | Coesão por funcionalidade (a View e sua lógica estão juntas na pasta `Pages`). | Separação por responsabilidade (lógica nos `Controllers`, UI nas `Views`, dados nos `Models`). |
| **Roteamento** | Baseado na estrutura de arquivos e pastas. A URL corresponde diretamente ao caminho do arquivo em `Pages`. | Baseado em convenções e configurações explícitas, geralmente no formato `{controller}/{action}/{id}`. |
| **Fluxo de Requisição** | Requisição -\> Roteamento para o arquivo `.cshtml` -\> Execução dos Handlers do `PageModel` (`OnGet`, `OnPost`) -\> Renderização da Página. | Requisição -\> Roteamento para a `Action` de um `Controller` -\> Processamento da lógica -\> Seleção e renderização da `View`. |
| **Complexidade Inicial** | Menor. Mais direto para criar páginas e formulários simples. | Moderada. Requer a compreensão da interação entre os três componentes. |
| **Ideal para...** | Aplicações centradas em formulários, operações CRUD e cenários onde a página é a unidade principal de funcionalidade. | Aplicações complexas com regras de negócio ricas, APIs web, e cenários que exigem alta testabilidade e flexibilidade. |
| **Reutilização de Lógica** | *(Preencha com sua análise, ex: via View Components, classes base para PageModel)* | *(Preencha com sua análise, ex: Controllers podem servir múltiplas Actions e ser usados para APIs e UI)* |

## 11\. Conclusão

*Aqui você escreverá sua conclusão pessoal sobre qual abordagem se encaixou melhor para este tipo de projeto e por quê, considerando os pontos levantados na organização do código, na camada de dados e no comparativo geral.*
'''
'''

# Análise Comparativa: Arquiteturas Razor Pages e MVC

## 1. Introdução

Este documento apresenta uma análise comparativa entre as arquiteturas Razor Pages e MVC (Model-View-Controller) no ecossistema ASP.NET Core. A análise foi baseada na implementação de uma aplicação web de gerenciamento acadêmico, seguindo os tutoriais oficiais da Microsoft, para a disciplina de Programação WEB II do IFS. O objetivo é destacar as diferenças práticas na estrutura do projeto, filosofia de desenvolvimento, fluxo de dados e produtividade do desenvolvedor.

## 2. Estrutura e Filosofia do Projeto

### 2.1. Razor Pages

* **Filosofia:** Orientada a páginas (Page-Centric). A Microsoft criou esta arquitetura para cenários onde o modelo de interação é centrado em páginas individuais (como formulários e listagens). A ideia principal é a **coesão**: a marcação da UI (`.cshtml`) e o seu código de lógica (`.cshtml.cs`) são mantidos juntos, simplificando o desenvolvimento para funcionalidades focadas.

* **Estrutura de Arquivos:** O projeto utiliza uma estrutura centrada na pasta `Pages`. Cada página consiste em dois ficheiros acoplados: um ficheiro `.cshtml` (a View) e um ficheiro code-behind `.cshtml.cs` que contém a classe `PageModel` para lidar com as requisições.

* **Configuração do Projeto:**
  * **Framework:** .NET 8.0
  * **Autenticação:** Nenhuma
  * **HTTPS:** Configurado
  * **Configuração:** Utiliza instruções de nível superior no `Program.cs` para uma configuração mais concisa.

### 2.2. MVC (Model-View-Controller)

* **Filosofia:** Orientada pela **Separação de Responsabilidades (Separation of Concerns)**. A aplicação é dividida em três componentes: **Model** (dados e regras de negócio), **View** (a UI) e **Controller** (o intermediário que orquestra a interação). Esta abordagem promove baixo acoplamento, facilitando a manutenção e os testes de forma independente.

* **Estrutura de Arquivos:** O projeto organiza o código em pastas distintas para cada responsabilidade: `Controllers` (para a lógica de requisição), `Views` (para a UI, organizada por subpastas de controller) e `Models` (para as entidades de dados).

* **Configuração do Projeto:**
  * **Framework:** .NET 8.0
  * **Autenticação:** Nenhuma
  * **HTTPS:** Configurado
  * **Configuração:** Com a adoção do "modelo de hospedagem mínimo" a partir do .NET 6, a configuração também é unificada no `Program.cs`, abolindo o antigo ficheiro `Startup.cs`.

### 2.3. Análise Comparativa da Estrutura

A diferença fundamental reside na organização. Razor Pages adota uma **organização vertical**: todos os ficheiros relativos a uma funcionalidade (ex: `Students`) estão juntos na mesma pasta. O MVC adota uma **organização horizontal**: os ficheiros são agrupados pelo seu papel técnico (`Controllers`, `Views`), o que espalha uma única funcionalidade por várias pastas.

## 3. Camada de Dados com Entity Framework Core

Um dos primeiros e mais importantes pontos observados é que a camada de dados é **completamente agnóstica** à arquitetura de UI.

* **Implementação:** As pastas `Models` (com as entidades) e `Data` (com o `SchoolContext` e `DbInitializer`) do projeto Razor Pages foram diretamente reutilizadas no projeto MVC sem **nenhuma alteração**. O registo do `DbContext` em ambos os projetos é feito de forma semelhante no `Program.cs`.

* **Análise:** Isto demonstra um pilar da boa arquitetura: o Entity Framework Core gere o acesso a dados de forma independente de como esses dados serão exibidos. Esta abordagem acelera o desenvolvimento, promove a reutilização de código e garante a consistência da lógica de negócio entre diferentes interfaces da mesma aplicação.

## 4. Operações CRUD (Create, Read, Update, Delete)

A implementação das operações básicas de dados expõe as diferenças práticas no fluxo de cada arquitetura.

### 4.1. Listar Entidades (Read)

* **Implementação em Razor Pages:**
    1. A funcionalidade está encapsulada nos ficheiros `Pages/Students/Index.cshtml` e `Pages/Students/Index.cshtml.cs`.
    2. Uma requisição GET a `/Students` executa o método `OnGetAsync()` no `PageModel`.
    3. Os dados são obtidos da base de dados e atribuídos a uma **propriedade pública** do `PageModel` (ex: `public IList<Student> Student { get; set; }`).
    4. A View (`.cshtml`) acede aos dados através desta propriedade (`Model.Student`).

* **Implementação em MVC:**
    1. A funcionalidade está dividida entre `Controllers/StudentsController.cs` e `Views/Students/Index.cshtml`.
    2. Uma requisição GET a `/Students` é roteada para a `Action` `Index()` no `StudentsController`.
    3. Os dados são obtidos da base de dados e **passados como argumento** para a View através do método `return View(dados)`.
    4. A View (`.cshtml`) recebe a lista de dados diretamente no seu `@model`.

* **Análise Comparativa:**

| Característica | Razor Pages | MVC (Model-View-Controller) |
| :--- | :--- | :--- |
| **Localização da Lógica** | No `PageModel`, fortemente acoplado à sua View. | No `Controller`, separado da View. |
| **Fluxo de Dados para a UI** | A lógica preenche uma **propriedade** do `PageModel`. | A `Action` do Controller **passa o modelo** como parâmetro. |
| **Acoplamento** | **Alto:** A View e o `PageModel` são duas metades da mesma unidade. | **Baixo:** A `View` é independente e pode ser reutilizada por outra `Action`. |

### 4.2. Criar Entidades (Create)

Ambas as arquiteturas separam a lógica de "mostrar o formulário" da de "processar os dados".

* **Implementação em Razor Pages:**
    1. A página `Create.cshtml.cs` tem dois *handlers*: `OnGet()` e `OnPostAsync()`.
    2. `OnGet()`: Apenas exibe a página (`return Page();`).
    3. `OnPostAsync()`:
        * Os dados do formulário são vinculados à propriedade `[BindProperty] public Student Student { get; set; }`.
        * Usa `_context.Students.Add(Student)` para marcar a entidade como `Added`.
        * Salva na base de dados e redireciona.
    4. A segurança contra *overposting* (excesso de postagem) é feita implicitamente no tutorial, pois o `[BindProperty]` cria um novo `Student` vazio e o `Add` apenas insere os campos que foram vinculados.

* **Implementação em MVC:**
    1. O `StudentsController.cs` tem duas *Actions*: `Create()` (com `[HttpGet]`) e `Create()` (com `[HttpPost]`).
    2. `Create()` [GET]: Apenas exibe a *View* (`return View();`).
    3. `Create()` [POST]:
        * Recebe os dados como um *parâmetro* da *Action*: `public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student)`.
        * O `Model Binding` cria o objeto `student` a partir do formulário.
        * A segurança contra *overposting* é explícita, usando o atributo `[Bind(...)]` para listar *exatamente* quais campos são permitidos.
        * Usa `_context.Add(student)` (estado `Added`) e salva.

* **Análise Comparativa:**
  * A lógica é quase idêntica, mas a implementação difere. Razor Pages usa *Handlers* na mesma classe, enquanto MVC usa *Actions* separadas no *Controller*.
  * Ambos usam o atributo `[ValidateAntiForgeryToken]` para prevenir ataques CSRF, com o *token* a ser gerado automaticamente pelo `<form>`.
  * A principal diferença é no `Model Binding` e segurança: Razor Pages vincula a uma propriedade da classe, enquanto MVC vincula a um parâmetro de método. O tutorial de MVC introduz imediatamente o `[Bind]` como uma medida de segurança explícita contra *overposting*.

### 4.3. Exibir Detalhes (Details)

Esta operação é focada apenas em leitura e é muito semelhante à listagem (4.1).

* **Implementação em Razor Pages:**
  * O `PageModel` (`Details.cshtml.cs`) usa `OnGetAsync(int? id)` para receber o ID.
  * Os dados são buscados (com `FirstOrDefaultAsync`) e atribuídos à propriedade `public Student Student { get; set; }`.
  * A *View* acede aos dados via `Model.Student.LastName`.

* **Implementação em MVC:**
  * O `Controller` usa a *Action* `Details(int? id)`.
  * Os dados são buscados (com `FirstOrDefaultAsync`) e passados diretamente para a *View*: `return View(student)`.
  * A *View* acede aos dados diretamente via `Model.LastName`.

* **Análise Comparativa:**
  * A lógica é idêntica. A única diferença, tal como na listagem, é que Razor Pages usa uma propriedade no `PageModel` para expor os dados, enquanto MVC passa o modelo diretamente para a `View()`.
  * *Nota*: Ambos os tutoriais usam `FirstOrDefaultAsync`. Uma otimização possível em ambos seria usar `FindAsync(id)`, que primeiro verifica se a entidade já está na memória (cache do DbContext) antes de consultar a base de dados, sendo ideal para buscas por chave primária.

### 4.4. Editar Entidades (Update)

Esta é a operação mais complexa e onde as diferenças de abordagem ficam mais evidentes, especialmente em relação ao tratamento do estado "desconectado" da web.

* **Implementação em Razor Pages:**
    1. `OnGetAsync(int? id)`: Busca o estudante (com `FindAsync`) e preenche a propriedade `[BindProperty] Student` para exibir no formulário.
    2. `OnPostAsync(int id)`:
        * **Solução (B) "Buscar Original e Comparar":** O método primeiro busca a entidade original do banco: `var studentToUpdate = await _context.Students.FindAsync(id)`.
        * Em seguida, usa `await TryUpdateModelAsync<Student>(studentToUpdate, "Student", s => s.FirstMidName, s => s.LastName, ...)` para aplicar *apenas* os campos permitidos do formulário na entidade que o `DbContext` está a rastrear.
        * Isto previne *overposting* e gera um SQL de `UPDATE` eficiente, que atualiza apenas os campos alterados (estado `Modified`).

* **Implementação em MVC:**
    1. `Edit(int? id)` [GET]: Busca o estudante (com `FindAsync`) e passa-o para a `View`: `return View(student)`.
    2. `Edit(int id, [Bind("ID,LastName,...")] Student student)` [POST]:
        * **Solução (A) "Trocar Tudo" (com segurança):** O `Model Binder` cria um *novo* objeto `student` com os dados do formulário, graças ao `[Bind]`.
        * O código valida o ID (`id == student.ID`).
        * Em seguida, usa `_context.Update(student)`. Isto anexa a entidade "desconectada" ao `DbContext` e marca *todas* as suas propriedades (listadas no `[Bind]`) como `Modified`.
        * Gera um SQL de `UPDATE` que atualiza *todas* as colunas, mesmo as que não foram alteradas.

* **Análise Comparativa:**
  * Ambas as abordagens separam GET e POST e previnem *overposting* (CSRF e `[Bind]`/`TryUpdateModelAsync`).
  * **Diferença Chave de Estratégia:** O tutorial de Razor Pages opta por uma leitura extra da base de dados no `POST` (`FindAsync`) para usar `TryUpdateModelAsync`. Isto é mais eficiente a nível de SQL (só atualiza o que mudou).
  * O tutorial de MVC opta por usar `_context.Update()` no objeto vindo do `Model Binder`. Isto evita a leitura extra no `POST`, mas é menos eficiente a nível de SQL (atualiza todos os campos).
  * Ambas as plataformas *podem* usar ambas as estratégias (`TryUpdateModelAsync` existe no MVC e `_context.Update` existe no Razor Pages), mas os tutoriais mostram as abordagens mais comuns para cada uma.

### 4.5. Excluir Entidades (Delete)

* **Implementação em Razor Pages:**
    1. `OnGetAsync(int? id)`: Busca o estudante e armazena na propriedade `Student` para exibir os dados de confirmação.
    2. `OnPostAsync(int id)`: Busca o estudante com `FindAsync(id)`, o remove com `_context.Students.Remove(student)` (marcando-o como `Deleted`), e salva as alterações.

* **Implementação em MVC:**
    1. `Delete(int? id)` [GET]: Busca o estudante e passa-o para a `View` de confirmação.
    2. `DeleteConfirmed(int id)` [POST]: Uma *Action* separada (geralmente com `[HttpPost, ActionName("Delete")]`) é usada para o POST.
    3. A lógica é idêntica: busca com `FindAsync(id)`, remove com `_context.Students.Remove(student)` (estado `Deleted`), e salva.

* **Análise Comparativa:**
  * A lógica e a abordagem são **praticamente idênticas** em ambas as arquiteturas.

### 4.6. Análise de Conceitos-Chave do CRUD

Vários conceitos são fundamentais para o funcionamento do CRUD em ASP.NET Core e aplicam-se a *ambas* as arquiteturas.

* **Separação GET vs. POST:**
  * **Porquê?** Como explicaste, esta é uma convenção fundamental da web.
  * **GET (Buscar):** Deve ser uma operação segura e *idempotente* (pode ser repetida sem causar efeitos colaterais). É usada para *mostrar* a página (o formulário de criação, o formulário de edição pré-preenchido, a página de confirmação de exclusão).
  * **POST (Alterar):** É usada para enviar dados que *modificam* o estado no servidor (criar um registo, atualizar um registo, excluir um registo). Não é idempotente.

* **Auxiliares de Marcação (Tag Helpers):**
  * Ambas as arquiteturas usam exatamente os mesmos *Tag Helpers* nas *Views* (`.cshtml`).
  * `label`, `input`, `span asp-validation-for`: Geram o HTML para os campos do modelo e exibem mensagens de validação.
  * `asp-action`, `asp-controller`, `asp-page`, `asp-route-id`: Geram os links (`<a>`) e URLs de *action* do formulário (`<form>`) de forma correta, garantindo que o roteamento funcione.
  * **Conclusão:** A camada de *View* é partilhada e idêntica.

* **O Problema dos Dados Desconectados (Estados da Entidade):**
  * Como a tua analogia da "fotocópia" descreveu, o `DbContext` tem um ciclo de vida *por requisição* (é *Scoped*). O `DbContext` do `GET` é destruído. Um `DbContext` *novo* é criado para o `POST`.
  * Este novo `DbContext` não sabe nada sobre a entidade original ("fotocópia rabiscada").
  * **Estados do EF Core:** Para resolver isto, temos de dizer ao novo `DbContext` o que fazer:
    * `_context.Add(student)`: Diz "Esta entidade é nova" (estado `Added`). O `SaveChanges` fará um `INSERT`.
    * `_context.Update(student)`: Diz "Esta entidade existe mas foi modificada" (estado `Modified`). O `SaveChanges` fará um `UPDATE` para *todas* as colunas.
    * `_context.Remove(student)`: Diz "Esta entidade deve ser apagada" (estado `Deleted`). O `SaveChanges` fará um `DELETE`.
    * `TryUpdateModelAsync(studentToUpdate, ...)`: Este é o método mais inteligente. Ele usa uma entidade *já rastreada* (`studentToUpdate` que veio do `FindAsync`) e atualiza apenas os campos necessários, marcando-os individualmente como `Modified`.

* **Segurança (CSRF e Overposting):**
  * **CSRF (Cross-Site Request Forgery):**
    * O `[ValidateAntiForgeryToken]` (no `PageModel` ou na *Action* `[HttpPost]`) trabalha em conjunto com o `<form>` (que usa o `FormTagHelper`) para gerar um *token* oculto.
    * Isto garante que o pedido `POST` veio de um formulário gerado pela tua própria aplicação.
    * A implementação é idêntica em ambas as arquiteturas.
  * **Overposting (Excesso de Postagem):**
    * Ocorre quando um utilizador mal-intencionado envia mais campos do que o formulário exibe (ex: `StudentID=1, IsAdmin=true`).
    * Ambos os tutoriais previnem isto, mas com as estratégias diferentes que vimos na secção 4.4:
            1. **MVC (Tutorial):** Usa `[Bind("Prop1", "Prop2")]` no parâmetro da *Action* `[POST]`.
            2. **Razor Pages (Tutorial):** Usa `TryUpdateModelAsync(entidade, "prefixo", e => e.Prop1, e => e.Prop2)` no *handler* `[POST]`.
    * Ambas são defesas eficazes. `TryUpdateModelAsync` é frequentemente considerado mais robusto, pois está mais próximo da lógica de atualização do que da definição do método.

## 5. Funcionalidades Avançadas de Listagem

Aqui, analisamos a implementação de ordenação, filtragem (pesquisa) e paginação. Esta secção destaca a importância crucial do **LINQ to Entities** e da diferença entre `IQueryable` e `IEnumerable`.

### 5.1. Ordenação, Filtro e Paginação

* **O Conceito: `IQueryable` e a Execução Diferida**
  * Ambos os tutoriais (Razor Pages e MVC) baseiam-se num conceito fundamental: construir a consulta à base de dados passo a passo, mas sem a executar.
  * Quando escrevemos `var students = _context.Students.AsQueryable();`, não estamos a ir à base de dados. Estamos a criar um objeto `IQueryable<Student>`, que é uma **árvore de expressão** (expression tree) que representa uma *intenção* de consulta.
  * Cada método que adicionamos (como `.Where(s => s.LastName.Contains(...))` ou `.OrderBy(s => s.LastName)`) apenas *modifica* essa árvore de expressão. A consulta **ainda não foi executada**.
  * Isto é o **LINQ to Entities**: o Entity Framework Core analisa esta árvore de expressão C# e traduz tudo numa única e otimizada consulta SQL.
  * A consulta só é finalmente enviada à base de dados quando um método de "materialização" é chamado, como `ToListAsync()`, `FirstOrDefaultAsync()` ou `CountAsync()`.

* **O Anti-Padrão (`IEnumerable`)**
  * Se, por engano, chamássemos `ToListAsync()` *antes* de aplicar os filtros (ex: `var students = await _context.Students.ToListAsync(); var filtered = students.Where(...);`), estaríamos a usar **LINQ to Objects**.
  * Isto traria **todos** os registos da tabela `Students` para a memória do servidor web e só *depois* aplicaria o filtro. Numa tabela com milhares de linhas, isto seria desastroso para a performance.
  * **Conclusão:** O sucesso desta funcionalidade depende de manter a consulta como `IQueryable` o máximo de tempo possível.

* **Implementação em Razor Pages:**
    1. O método `OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex)` recebe todos os parâmetros da URL.
    2. O `searchString` (termo de pesquisa) é frequentemente associado a uma propriedade `[BindProperty(SupportsGet = true)]` para repopular o campo de busca.
    3. A lógica reside no `OnGetAsync`:
        * Inicia com `IQueryable<Student> studentsIQ = _context.Students.AsQueryable();`
        * Aplica o filtro: `if (!String.IsNullOrEmpty(searchString)) { studentsIQ = studentsIQ.Where(...); }`
        * Aplica a ordenação: `switch (sortOrder) { ... studentsIQ = studentsIQ.OrderBy(...); }`
        * Finalmente, a paginação (que usa `.Skip()` e `.Take()`) e a materialização (`ToListAsync()`) são encapsuladas na classe `PaginatedList.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, ...)`
    4. O resultado (`PaginatedList`) é atribuído a uma propriedade pública do `PageModel` para a View usar.

* **Implementação em MVC:**
    1. A `Action` `Index(string sortOrder, string currentFilter, string searchString, int? pageIndex)` recebe os mesmos parâmetros.
    2. Valores como `ViewData["CurrentSort"]` são usados para passar o estado atual para a *View*, para que os links de ordenação e paginação possam ser construídos corretamente.
    3. A lógica reside na `Action Index`:
        * Inicia com `var students = _context.Students.AsQueryable();` (O `var` aqui é inferido como `IQueryable<Student>`).
        * Aplica o filtro: `if (!String.IsNullOrEmpty(searchString)) { students = students.Where(...); }`
        * Aplica a ordenação: `switch (sortOrder) { ... students = students.OrderBy(...); }`
        * A mesma classe `PaginatedList.CreateAsync(students.AsNoTracking(), pageIndex ?? 1, ...)` é usada para executar a consulta.
    4. O resultado (`PaginatedList`) é passado diretamente para a *View*: `return View(await PaginatedList.CreateAsync(...));`.

* **Análise Comparativa:**
  * A lógica de negócio é **absolutamente idêntica**. O código dentro do `OnGetAsync()` (Razor Pages) e da `Action Index()` (MVC) é o mesmo em 99%. A classe `PaginatedList.cs` é partilhada e reutilizada sem qualquer modificação.
  * A única diferença é o "encanamento": Razor Pages armazena o resultado numa propriedade do `PageModel`, enquanto MVC passa o resultado como o modelo no `return View()`.

* **A Implementação do Filtro (Pesquisa) e o `method="get"`**
  * Ambos os tutoriais implementam o formulário de pesquisa (`<form>`) usando o **método HTTP GET** (`<form method="get">`).
  * Isto é uma prática recomendada pelas **diretrizes do W3C** para operações que são *idempotentes* (ou seja, seguras de repetir, como uma pesquisa, que não altera dados).
  * Ao usar `method="get"`, os parâmetros do formulário (ex: `searchString=...`) são adicionados à URL como *query strings*.
  * **Vantagem:** O utilizador pode marcar a página de resultados da pesquisa, copiar o link e partilhá-lo com outros, e o link conterá os termos da pesquisa.
  * O `FormTagHelper` (`<form asp-page="/Students/Index" method="get">` ou `<form asp-action="Index" method="get">`) gera este HTML corretamente em ambas as arquiteturas.

## 6. Gestão do Esquema da Base de Dados com Migrations

Assim como a definição dos modelos e do `DbContext` (Secção 3), o processo de gestão das alterações do **esquema da base de dados** é uma responsabilidade exclusiva do Entity Framework Core e é **100% idêntico** em ambas as arquiteturas.

### 6.1. O Processo de Migrations

O EF Core Migrations é a ferramenta usada para manter o esquema da base de dados sincronizado com o modelo de dados C# (as tuas classes de entidade).

* **Ferramentas:** O processo requer o pacote NuGet `Microsoft.EntityFrameworkCore.Tools`. Os comandos podem ser executados de duas formas, que são funcionalmente idênticas:
    1. **PMC (Package Manager Console):** Uma consola dentro do Visual Studio (ex: `Add-Migration ...`).
    2. **CLI (Command-line Interface):** A linha de comandos do .NET (ex: `dotnet ef migrations add ...`).

* **Alternativa (`EnsureCreated`):** O método `context.Database.EnsureCreated()` é uma alternativa que *cria* a base de dados se ela não existir, mas **não pode atualizá-la** se o modelo C# mudar. Os tutoriais de ambos os projetos evitam este método, pois ele é mutuamente exclusivo com as Migrations, que é a abordagem profissional e preferida para a evolução do esquema.

### 6.2. Análise Comparativa do Fluxo

O fluxo de trabalho é o mesmo, quer se esteja num projeto Razor Pages ou MVC:

1. **`Add-Migration <NomeDaMigracao>`** (ex: `Add-Migration InitialCreate`):
    * Este comando compara o estado atual das classes do modelo (lido a partir do `DbContext`) com o último **instantâneo** do esquema.
    * Como é a primeira migração, ele gera o ficheiro `<timestamp>_InitialCreate.cs`, que contém o código C# nos métodos `Up()` (para criar todas as tabelas) e `Down()` (para apagá-las).
    * Ele também cria (ou atualiza) o ficheiro `Migrations/SchoolContextModelSnapshot.cs`. Este ficheiro é um **instantâneo do esquema de base de dados atual** e é fundamental; é contra ele que a *próxima* migração será comparada para detetar alterações.

2. **`Update-Database`**:
    * Este comando executa todas as migrações pendentes.
    * Ele verifica a tabela `__EFMigrationsHistory` na base de dados para saber quais migrações já foram aplicadas.
    * Como a `InitialCreate` ainda não foi aplicada, ele executa o método `Up()` desse ficheiro, criando fisicamente todas as tabelas no SQL Server.
    * Após a conclusão bem-sucedida, ele adiciona um registo na tabela `__EFMigrationsHistory`.

* **Verificação:** Em ambos os projetos, o **Pesquisador de Objetos do SQL Server (SSOX)** é usado para verificar visualmente se as tabelas foram criadas corretamente, incluindo a tabela `__EFMigrationsHistory`.

* **Outros Comandos:** Comandos como `Remove-Migration` (que remove o último ficheiro de migração e reverte o instantâneo) também funcionam da mesma forma em ambos os projetos.

**Conclusão:** Esta etapa reforça que o EF Core é uma camada de infraestrutura completamente desacoplada da UI. A forma como geres, crias e atualizas a tua base de dados não é afetada pela tua escolha entre Razor Pages e MVC.

## 7. Modelagem de Dados Complexos e Relacionamentos

Esta etapa da implementação consistiu em evoluir o modelo de dados simples para um modelo mais robusto, que reflete as complexas regras de negócio de uma universidade. Isto envolveu a adição de novas entidades (`Instructor`, `OfficeAssignment`, `Department`, `CourseAssignment`) e a atualização das existentes.

### 7.1. Análise Comparativa

Esta é a secção mais importante da nossa análise até agora, juntamente com as Secções 3 e 6: **A implementação é 100% idêntica e partilhada entre os projetos MVC e Razor Pages.**

Toda a lógica de negócio, regras de validação, relacionamentos e mapeamento da base de dados residem nos ficheiros da pasta `Models/` e no `Data/SchoolContext.cs`. O projeto MVC reutiliza esta camada sem qualquer alteração.

Isto prova que a escolha entre MVC e Razor Pages é puramente uma decisão sobre a **arquitetura da camada de apresentação (UI)**. A lógica de negócio e de dados subjacente permanece independente, reutilizável e agnóstica.

### 7.2. Análise dos Conceitos de Modelagem (Comum a Ambos)

Ambos os projetos utilizam uma combinação de **Convenções do EF Core**, **Atributos (Data Annotations)** e **API Fluente** para construir o modelo.

* **Atributos (Data Annotations):** São usados para fornecer metadados ao EF Core e ao ASP.NET Core.
  * **Validação e Schema (`System.ComponentModel.DataAnnotations`):**
    * `[Required]`: Indica que a propriedade não pode ser nula. Afeta tanto a validação do lado do cliente (nos formulários) como o esquema da base de dados (coluna `NOT NULL`).
    * `[StringLength(50)]`, `[MaxLength(50)]`: Define um tamanho máximo. Usado para validação e para definir o tamanho da coluna (ex: `nvarchar(50)`).
    * `[Range(0, 5)]`: Define um intervalo numérico para validação.
    * `[RegularExpression(...)]`: Valida o formato da string.
  * **Apenas UI/Formatação (`System.ComponentModel.DataAnnotations`):**
    * `[Display(Name = "Last Name")]`: Controla o texto exibido pelos auxiliares de marcação (ex: `<label>`).
    * `[DataType(DataType.Date)]`: Especifica o tipo de dados, ajudando os *Tag Helpers* a renderizar o controlo HTML correto (ex: `type="date"`).
    * `[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]`: Formata a exibição dos dados.
  * **Apenas Schema (`System.ComponentModel.DataAnnotations.Schema`):**
    * `[Column("NomeDaColuna")]`: Especifica um nome de coluna diferente do nome da propriedade.
    * `[DatabaseGenerated(DatabaseGeneratedOption.None)]`: Informa ao EF Core que a chave primária (ex: `CourseID`) será fornecida pela aplicação, em vez de ser gerada pela base de dados.

* **Propriedades de Navegação e Relacionamentos:**
  * O EF Core interpreta os relacionamentos com base nas **propriedades de navegação** (`ICollection<T>`, `List<T>`, ou tipos de entidade simples como `Department`).
  * **Chaves Primárias (PK):** São detetadas por convenção (`ID` ou `classnameID`) ou explicitamente com o atributo `[Key]`.
  * **Chaves Estrangeiras (FK):** O EF Core pode criar "propriedades de sombra" (shadow properties) para FKs, mas é uma boa prática incluí-las explicitamente (ex: `public int DepartmentID { get; set; }`). O uso de `int?` (um inteiro que permite valor nulo) indica que o relacionamento é opcional.
  * **Relacionamento Muitos-para-Muitos:**
    * Ambos os tutoriais implementam a relação N-N entre `Instructor` e `Course` criando uma **tabela de junção explícita com conteúdo** (`CourseAssignment`).
    * Isto é feito criando a entidade `CourseAssignment` com uma **chave primária composta** (`CourseID` e `InstructorID`). Esta chave é definida usando a **API Fluente**, pois os atributos não conseguem definir chaves compostas de forma limpa.

* **API Fluente (`OnModelCreating`) vs. Atributos:**
  * Embora a maioria da modelagem tenha sido feita com atributos (Annotations), a **API Fluente** (configurando o `modelBuilder` no `OnModelCreating` do `DbContext`) é usada para cenários mais complexos que os atributos não suportam. **A API Fluente sempre substitui os atributos.**
  * **Exemplo 1 (Chave Composta):** `modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseID, c.InstructorID });`
  * **Exemplo 2 (Exclusão em Cascata):** Por convenção, o EF Core habilita a exclusão em cascata (cascade delete) para chaves estrangeiras que não permitem nulos. Como explicado, isto pode criar *ciclos* de exclusão. Para corrigir isto (ex: impedir que a exclusão de um `Instructor` exclua um `Department` do qual ele é administrador), a API Fluente é usada para definir o comportamento como restrito: `modelBuilder.Entity<Department>().HasOne(d => d.Administrator).WithMany().OnDelete(DeleteBehavior.Restrict)`.

### 7.3. O Desafio da Migração: Adicionando Colunas Obrigatórias

Como o modelo de dados evoluiu, a migração (`Add-Migration ComplexDataModel`) precisou de uma intervenção manual.

* **O Problema:** A entidade `Course` foi modificada para ter uma `DepartmentID` obrigatória (não nula). No entanto, a base de dados já continha registos de `Course` (criados pelo `DbInitializer`). Ao aplicar a migração, o SQL Server não saberia qual valor colocar na nova coluna `DepartmentID` para os cursos que já existiam, resultando num erro.
* **A Solução (Comum a Ambos):**
    1. A migração gerada foi editada manualmente.
    2. Primeiro, um `migrationBuilder.Sql("INSERT INTO Department (Name, Budget, StartDate) VALUES ('Temp', 0.00, GETDATE())")` foi adicionado para criar um departamento "fantasma" temporário.
    3. Segundo, o comando `migrationBuilder.AddColumn<int>(...)` para `DepartmentID` foi modificado para incluir um `defaultValue: 1` (assumindo que 1 é o ID do departamento "Temp").
    4. Isto permitiu que o `Update-Database` fosse executado com sucesso, associando todos os cursos existentes ao departamento temporário, satisfazendo a restrição `NOT NULL`.

## 8. Leitura de Dados Relacionados

Uma vez que o modelo de dados complexo está definido, o próximo passo é consultar e exibir esses dados relacionados. Um **ORM** como o Entity Framework Core facilita isto ao "mapear" as linhas da base de dados para objetos C# e preencher as suas **propriedades de navegação**.

### 8.1. Estratégias de Carregamento (Conceitos Comuns)

Ambos os projetos dependem das mesmas estratégias de carregamento de dados do EF Core:

* **Carregamento Adiantado (Eager Loading):** Esta é a estratégia principal usada em ambos os tutoriais.
  * **Como funciona:** Os dados relacionados são recuperados juntamente com a entidade principal numa única consulta. Isto é feito usando os métodos `.Include()` (para o primeiro nível de relacionamento) e `.ThenInclude()` (para níveis subsequentes).
  * **Vantagem:** É muito eficiente. Evita o "problema N+1", onde uma consulta inicial é seguida por N consultas separadas (uma para cada entidade relacionada).
* **Carregamento Explícito (Explicit Loading):**
  * **Como funciona:** Os dados relacionados são carregados *após* a entidade principal já ter sido recuperada, mas através de um comando explícito (ex: `_context.Entry(student).Collection(s => s.Enrollments).LoadAsync()`).
  * **Vantagem:** Útil quando os dados relacionados só são necessários condicionalmente.
* **Carregamento Lento (Lazy Loading):**
  * **Como funciona:** Os dados relacionados são carregados automaticamente da base de dados no momento em que a propriedade de navegação é acedida pela primeira vez.
  * **Análise:** Embora suportado pelo EF Core (com pacotes extra), não é usado nos tutoriais, pois pode levar facilmente ao problema N+1 se não for usado com cuidado.

### 8.2. Rastreamento vs. Sem Rastreamento (Performance)

* **Consultas de Acompanhamento (Tracking):** Por padrão, o `DbContext` "rastreia" as entidades que recupera. Ele guarda um instantâneo delas para poder detetar alterações e gerar o `UPDATE` correto quando `SaveChanges()` é chamado.
* **Consultas Sem Acompanhamento (`AsNoTracking()`):**
  * Para páginas de **apenas leitura** (como as páginas `Index` e `Details`), este rastreamento é um desperdício de memória e processamento.
  * Ambos os tutoriais usam `.AsNoTracking()` nestas páginas. Isto diz ao EF Core: "Apenas me dê os dados; não precisas de os rastrear para futuras atualizações."
  * **Resultado:** A consulta é executada significativamente mais rápido e consome menos memória.

### 8.3. Ponto de Divergência: Relações Muitos-para-Muitos (N-N)

Este é um ponto crucial na nossa análise. Os dois tutoriais oficiais da Microsoft abordam o relacionamento N-N (Instrutores <-> Cursos) de formas diferentes:

1. **Tutorial Razor Pages (e o teu Projeto):** Usa a abordagem moderna (EF Core 5+), onde a **tabela de junção é implícita**. O modelo tem apenas `ICollection<Course>` em `Instructor` e `ICollection<Instructor>` em `Course`. O EF Core cria a tabela de junção automaticamente.
2. **Tutorial MVC (Oficial):** Usa a abordagem mais antiga (ou necessária quando a tabela de junção tem *conteúdo*), onde a **tabela de junção é explícita**. Ou seja, existe uma entidade `CourseAssignment` que representa manualmente a ligação.

**Decisão do Projeto:** Para manter uma comparação justa entre as arquiteturas de UI, **o nosso projeto MVC reutiliza o modelo de dados moderno (implícito) do projeto Razor Pages**. Portanto, a nossa análise compara como o MVC e o Razor Pages consomem o **mesmo** modelo de dados.

### 8.4. Implementação e Análise Comparativa

#### Funcionalidade: Página de Cursos (Carregamento Nível 1)

* **Objetivo:** Listar Cursos e exibir o nome do `Department` relacionado (relacionamento 1-N).
* **Implementação em Razor Pages:**
  * O `PageModel` (`Pages/Courses/Index.cshtml.cs`) usa Eager Loading para buscar o departamento.
  * A consulta é: `_context.Courses.Include(c => c.Department).AsNoTracking()`.
  * O resultado é atribuído a uma propriedade: `public IList<Course> Course { get; set; }`.

* **Implementação em MVC:**
  * O `Controller` (`Controllers/CoursesController.cs`) usa a *mesma* consulta na `Action Index`.
  * A consulta é: `_context.Courses.Include(c => c.Department).AsNoTracking()`.
  * O resultado é passado para a View: `return View(await courses.ToListAsync())`.

* **Análise:** A lógica de acesso a dados é **100% idêntica**. A única diferença é como os dados chegam à View (`PageModel` Property vs. `return View(model)`).

#### Funcionalidade: Página de Instrutores (Carregamento Nível Múltiplo)

* **Objetivo:** Criar uma página mestre-detalhe complexa:
    1. Mostrar a lista de Instrutores.
    2. Ao selecionar um Instrutor, mostrar os Cursos que ele leciona (N-N).
    3. Ao selecionar um Curso, mostrar os Alunos matriculados (1-N).
* **Necessidade:** Esta UI complexa não pode ser representada por uma única entidade (`Instructor`). Ela requer um **ViewModel** dedicado (`InstructorIndexData`) para conter as três listas (`IEnumerable<Instructor>`, `IEnumerable<Course>`, `IEnumerable<Enrollment>`).
* **Implementação em Razor Pages:**
  * O `PageModel` (`Pages/Instructors/Index.cshtml.cs`) é responsável por orquestrar esta lógica.
  * No `OnGetAsync()`, ele usa uma consulta complexa de Eager Loading com `Include` e `ThenInclude` para trazer todos os dados necessários de uma só vez:

    ```csharp
    viewModel.Instructors = await _context.Instructors
        .Include(i => i.OfficeAssignment)
        .Include(i => i.Courses) // Nível 1
            .ThenInclude(c => c.Department) // Nível 2
        .Include(i => i.Courses) // Nível 1 (de novo)
            .ThenInclude(c => c.Enrollments) // Nível 2
                .ThenInclude(e => e.Student) // Nível 3
        .AsNoTracking()
        .ToListAsync();
    ```

  * O `PageModel` então preenche o `ViewModel` (`InstructorIndexData`) e expõe-no como uma propriedade pública para a View.

* **Implementação em MVC:**
  * O `Controller` (`Controllers/InstructorsController.cs`) tem a **exata mesma responsabilidade**.
  * A `Action Index()` reutiliza o mesmo `InstructorIndexData` ViewModel.
  * A lógica de consulta dentro da `Action` é **idêntica** à do `OnGetAsync()` do Razor Pages (a mesma consulta com `Include` e `ThenInclude`).
  * O `Controller` preenche o `ViewModel` e passa-o para a View: `return View(viewModel)`.

* **Análise Comparativa:**
  * Esta é a prova mais forte de que a complexidade da lógica de consulta de dados **não depende** da arquitetura de UI.
  * Ambas as arquiteturas identificaram a necessidade de um `ViewModel` para servir uma UI complexa.
  * Ambas usaram a mesma consulta EF Core de alta performance (Eager Loading) para preencher esse `ViewModel`.
  * Mais uma vez, a única diferença foi o "encanamento" de como o `ViewModel` preenchido chegou ao ficheiro `.cshtml`.

## 9. Atualização de Dados Relacionados

Se a *leitura* de dados relacionados (Secção 8) demonstrou a eficiência do EF Core, a *atualização* desses dados demonstra como cada arquitetura lida com operações complexas e transacionais. Esta é a funcionalidade mais complexa do tutorial.

### 9.1. Cenário 1: Atualizar Relação 1-N (Curso e Departamento)

* **Objetivo:** Na página de edição de um `Course` (Curso), permitir a alteração do seu `Department` (Departamento) através de uma lista *dropdown*.
* **Implementação em Razor Pages:**
  * O `PageModel` (`Pages/Courses/Edit.cshtml.cs`) herda de uma classe base (`DepartmentNamePageModel`) que tem a lógica para criar um `SelectList` dos departamentos.
  * `OnGetAsync()`: Carrega o curso e chama a lógica para popular o `SelectList`, que é armazenado numa propriedade (ex: `ViewData["DepartmentID"]` ou uma propriedade tipada).
  * `OnPostAsync()`: Usa `TryUpdateModelAsync` para aplicar de forma segura as alterações ao `Course` que foi carregado, incluindo a `DepartmentID` selecionada no *dropdown*.

* **Implementação em MVC:**
  * O `Controller` (`CoursesController.cs`) é responsável por esta lógica.
  * `Edit()` [GET]: Carrega o curso e chama um método auxiliar (ou fá-lo diretamente) para criar o `SelectList` de departamentos, que é passado para a *View* através do `ViewData["DepartmentID"]`.
  * `Edit()` [POST]: Usa `TryUpdateModelAsync` (ou `[Bind]`) para aplicar as alterações.

* **Análise:** A lógica é idêntica. Ambos precisam de carregar dados "extra" (a lista de departamentos) para a *View*. Ambos usam a mesma estratégia de `POST` (carregar, tentar atualizar, salvar). A única diferença é *onde* a lógica para popular o *dropdown* reside (classe base do `PageModel` vs. método privado ou `Action` no `Controller`).

### 9.2. Cenário 2: Atualizar Relações 1-1 e N-N (Instrutor)

* **Objetivo:** Na página de edição de um `Instructor` (Instrutor), permitir:
    1. A edição do `OfficeAssignment` (Lotação) (relação 1-1).
    2. A seleção dos `Courses` (Cursos) que ele leciona (relação N-N) através de *checkboxes*.

* **Análise da Implementação (Quase Idêntica):**
    Esta funcionalidade é tão complexa que ambas as arquiteturas adotam exatamente a mesma abordagem técnica, provando que, para lógica de negócio complexa, a arquitetura de UI torna-se apenas um "invólucro".

    1. **ViewModel (`AssignedCourseData`):**
        * Ambas as arquiteturas reutilizam o mesmo *ViewModel*, `AssignedCourseData`, para representar a lista de *checkboxes* (contendo `CourseID`, `Title` e o *booleano* `Assigned`).

    2. **Lógica `GET` (Popular a Página):**
        * **Razor Pages:** O `OnGetAsync()` no `PageModel` (`Pages/Instructors/Edit.cshtml.cs`) carrega o `Instructor` (com `Include` dos seus `OfficeAssignment` e `Courses`) e chama um método auxiliar (`PopulateAssignedCourseData`) para preencher a lista de *checkboxes*. O `ViewModel` é armazenado como uma propriedade no `PageModel`.
        * **MVC:** A `Action` `Edit()` [GET] no `Controller` (`InstructorsController.cs`) faz *exatamente o mesmo*: carrega o `Instructor` (com `Include`), chama o *mesmo* método auxiliar (`PopulateAssignedCourseData`) e passa a lista de *checkboxes* para a *View* (ex: via `ViewData["Courses"]`).

    3. **Renderização na View (`.cshtml`):**
        * As *Views* são quase idênticas. Ambas usam os *Tag Helpers* `label asp-for`, `input asp-for` e `span asp-validation-for` para os campos simples (como `LastName`).
        * Ambas contêm um ciclo `foreach` para renderizar a lista de *checkboxes* do `ViewModel` (`AssignedCourseData`).
        * *(Nota: A observação do tutorial sobre `Ctrl+Z` para corrigir a formatação do Razor é um *bug* de editor de versões mais antigas do Visual Studio, mas destaca a sintaxe complexa de renderização que é partilhada por ambos.)*

    4. **Lógica `POST` (Processar a Atualização):**
        * **Razor Pages:** O `OnPostAsync(int? id, string[] selectedCourses)` recebe os dados. Ele primeiro carrega a entidade `Instructor` original (com `.Include(i => i.Courses)`). Em seguida, usa `TryUpdateModelAsync` para aplicar as alterações simples (como `LastName` e `OfficeAssignment.Location`). Finalmente, chama um método auxiliar (`UpdateInstructorCourses`) que recebe o array `selectedCourses`, compara-o com os cursos atuais do instrutor e **adiciona ou remove manualmente** as entidades da coleção `instructor.Courses`.
        * **MVC:** A `Action` `Edit(int? id, string[] selectedCourses)` [POST] faz **exatamente a mesma coisa**. Carrega, usa `TryUpdateModelAsync`, e chama o *mesmo* método auxiliar (`UpdateInstructorCourses`) para manipular a coleção N-N.

### 9.3. Análise de Conceitos-Chave da Atualização

* **`TryUpdateModelAsync` (ou `TryUpdateModel`):**
  * Este método é a estrela desta secção. Como vimos no CRUD (4.4), ele é a forma mais segura de aplicar valores de um formulário a uma entidade *já rastreada* pelo `DbContext` (a que carregámos com `FindAsync` ou `FirstOrDefaultAsync`).
  * Ele previne *overposting* (excesso de postagem) e aplica de forma inteligente as alterações, marcando apenas as propriedades modificadas.

* **Transações (Implícitas):**
  * A operação `POST` da edição do Instrutor altera múltiplas tabelas: `Instructor` (dados pessoais), `OfficeAssignment` (lotação) e a tabela de junção `CourseInstructor` (cursos).
  * O Entity Framework Core é inteligente o suficiente para saber disto. Quando `await _context.SaveChangesAsync()` é chamado, o EF Core **automaticamente envolve todas estas operações numa única transação de base de dados**.
  * Se a atualização do nome do `Instructor` funcionar, mas a adição de um `Course` falhar, a base de dados **reverte (rollback) tudo**. A atualização do nome *não* é guardada. Isto garante a integridade dos dados sem que precisemos de escrever código de transação manualmente.

* **`ActionName`:**
  * Este atributo (mencionado nos conceitos) é usado em MVC quando o nome do método C# não corresponde ao nome da *Action* que o roteamento espera.
  * É mais comum na funcionalidade `Delete`, onde temos uma `Action` `Delete(int id)` [GET] e outra `DeleteConfirmed(int id)` [POST]. Para que a segunda `Action` responda ao `POST` de `/Delete/5`, usamos `[HttpPost, ActionName("Delete")]`.
  * Em Razor Pages, isto não é necessário, pois os *handlers* já são nomeados por convenção (`OnGet`, `OnPost`).

## 10. Tratamento de Conflitos de Simultaneidade

**Conflitos de simultaneidade** ocorrem quando um utilizador exibe os dados de uma entidade para editá-los e, em seguida, outro utilizador atualiza os dados dessa mesma entidade antes que o primeiro utilizador salve as suas alterações.

### 10.1. Conceitos: Simultaneidade Otimista vs. Pessimista

* **Simultaneidade Pessimista (Bloqueio):** Envolve bloquear os dados no momento em que são lidos, impedindo que outros utilizadores os alterem. O EF Core não oferece suporte interno para isto, e não é uma abordagem prática para aplicações web.
* **Simultaneidade Otimista (Deteção):** Permite que qualquer pessoa leia os dados. No momento de salvar (`SaveChanges()`), o sistema *deteta* se os dados foram alterados por outra pessoa desde que foram lidos. Esta é a abordagem padrão para a web.
  * **Estratégia "O Cliente Vence" (O último vence):** As alterações do utilizador atual sobrescrevem quaisquer alterações anteriores. Este é o comportamento padrão do EF Core *sem* tratamento de simultaneidade e pode levar à perda de dados.
  * **Estratégia "O Armazenamento Vence" (Store Wins):** As alterações do utilizador atual são descartadas se a base de dados tiver sido alterada. O utilizador é notificado com uma mensagem de erro. Esta é a estratégia implementada por ambos os tutoriais.

### 10.2. A Solução (Comum a Ambos): O Token de Acompanhamento

Ambos os tutoriais implementam a simultaneidade otimista usando uma coluna de acompanhamento na base de dados.

* **Implementação no Modelo:**
    1. Como reutilizámos a pasta `Models` do projeto Razor Pages, a entidade `Department` (e outras) possui a propriedade:

        ```csharp
        [Timestamp]
        public byte[] ConcurrencyToken { get; set; }
        ```

    2. O atributo `[Timestamp]` (apesar do nome, que vem de versões antigas do SQL Server) mapeia esta propriedade para uma coluna `rowversion` no SQL Server. Esta coluna é atualizada automaticamente pela base de dados sempre que a linha sofre um `UPDATE`.
    3. Isto também pode ser configurado via **API Fluente** no `DbContext` com `.Property(p => p.ConcurrencyToken).IsRowVersion()`.

* **Fluxo de Deteção (Idêntico):**
    1. **GET:** A página/view de Edição carrega o `Department` e inclui o valor do `ConcurrencyToken` num campo oculto (`<input type="hidden" asp-for="ConcurrencyToken" />`).
    2. **POST:** O formulário é enviado com os novos valores (ex: `Budget`) e o valor *original* do `ConcurrencyToken` (de quando a página foi carregada).
    3. **SaveChanges():** O EF Core gera um comando `UPDATE` que inclui o token na cláusula `WHERE`:
        `UPDATE Department SET Budget = @p0, ... WHERE DepartmentID = @p1 AND ConcurrencyToken = @p2`
    4. **Conflito:** Se outro utilizador (Alice) salvou alterações depois de o utilizador atual (Júlio) ter carregado a página, o `ConcurrencyToken` na base de dados será diferente do valor em `@p2`. O `UPDATE` falha em encontrar uma linha (0 linhas afetadas).
    5. **Exceção:** O EF Core vê que 0 linhas foram afetadas (quando esperava 1) e lança uma `DbUpdateConcurrencyException`.

### 10.3. Implementação e Análise Comparativa (Tratando a Exceção)

Ambas as arquiteturas implementam a mesma lógica "Store Wins" dentro de um bloco `try...catch`.

* **Implementação em Razor Pages (`Pages/Departments/Edit.cshtml.cs`):**
    1. O `OnPostAsync()` envolve a lógica de salvamento num `try...catch`.
    2. No `catch (DbUpdateConcurrencyException ex)`:
        * Obtém a entrada com falha: `var entry = ex.Entries.Single();`
        * Obtém os valores atuais da base de dados: `var databaseValues = entry.GetDatabaseValues();`
        * Recarrega o `ConcurrencyToken` do `PageModel` com o novo valor da base de dados: `entry.Property("ConcurrencyToken").OriginalValue = databaseValues["ConcurrencyToken"];`
        * **Ponto-chave:** Usa `ModelState.Remove("Department.ConcurrencyToken")` para forçar o *Tag Helper* (`asp-for`) a usar o novo valor da propriedade do modelo, em vez de usar o valor antigo que veio no `POST` e está no `ModelState`.
    3. Re-exibe a página com a mensagem de erro: `return Page();`.

* **Implementação em MVC (`Controllers/DepartmentsController.cs`):**
    1. A `Action Edit(int id, byte[] concurrencyToken)` [POST] envolve a lógica num `try...catch`.
    2. No `catch (DbUpdateConcurrencyException ex)`:
        * Faz o mesmo: obtém `entry`, `databaseValues`, etc.
        * Obtém o `Department` com os valores da base de dados.
        * Define o *novo* valor do token no modelo que será enviado de volta para a View: `departmentFromDb.ConcurrencyToken = (byte[])databaseValues["ConcurrencyToken"];`
        * **Ponto-chave:** Não precisa de `ModelState.Remove`. Ao passar o `departmentFromDb` atualizado para `return View(departmentFromDb)`, o *Tag Helper* (`asp-for`) lê o valor diretamente deste novo objeto de modelo.
    3. Re-exibe a *View* com a mensagem de erro: `return View(departmentFromDb);`.

* **Análise:** A lógica de negócio é idêntica. A única diferença técnica é como cada arquitetura limpa o valor antigo do `ConcurrencyToken` do `ModelState` para evitar um *loop* de erros: Razor Pages manipula o `ModelState` diretamente (`ModelState.Remove`), enquanto o MVC passa um objeto de modelo completamente novo para a `View`.

### 10.4. O Desafio Prático: Model Binding (Análise Específica do Projeto)

Durante a implementação do MVC, surgiu um problema prático crucial que não existe no Razor Pages.

* **O Problema:** O nosso modelo partilhado usa a propriedade `ConcurrencyToken`. A *View* MVC (`Edit.cshtml`) gera, portanto, um campo oculto com `name="ConcurrencyToken"`. No entanto, o tutorial original do MVC sugere uma assinatura de *Action* [POST] como:
    `public async Task<IActionResult> Edit(int id, [Bind(...)] Department department, byte[] rowVersion)`
* **A Falha:** O *Model Binder* do MVC falhou. Ele via o valor `ConcurrencyToken` vindo do formulário, mas não encontrava nenhum parâmetro de *Action* chamado `concurrencyToken`. O parâmetro `rowVersion` permanecia `null`, e o `[Bind]` no `department` não incluía o token.
* **A Solução:** A assinatura da *Action* [POST] do MVC teve que ser alterada para corresponder *exatamente* aos dados que chegam do formulário. A forma mais limpa (usada no tutorial de MVC) é adicionar o token como um parâmetro de método com o nome correto:
    `public async Task<IActionResult> Edit(int id, byte[] concurrencyToken)`
    *(E, claro, usar `concurrencyToken` dentro do `try...catch`)*
* **Conclusão Comparativa:** Isto expõe uma diferença fundamental. O `[BindProperty]` do Razor Pages lida com o *binding* de todas as propriedades do modelo (incluindo o token) de forma mais automática. O MVC, ao usar parâmetros de *Action* e o atributo `[Bind]`, exige uma correspondência manual mais explícita entre os nomes dos campos do formulário (definidos pelo `asp-for` no modelo) e os nomes dos parâmetros no método do `Controller`.

## 11. Padrões de Herança no Modelo de Dados

Embora esta secção seja exclusiva do tutorial de MVC, a implementação da herança no modelo de dados foi aplicada a *ambos* os projetos (MVC e Razor Pages) para manter a consistência da camada de dados partilhada.

### 11.1. Conceitos: Estratégias de Herança do EF Core

O EF Core suporta várias estratégias para mapear uma hierarquia de classes C# (ex: `Person` como classe base, `Student` e `Instructor` como classes derivadas) para uma base de dados SQL:

1. **TPH (Tabela por Hierarquia):** É a estratégia padrão e a demonstrada neste tutorial. Uma *única tabela* (ex: `People`) armazena os dados de *todas* as classes da hierarquia. Uma coluna especial, chamada "Discriminador", é usada para identificar a que classe (`Student` ou `Instructor`) cada linha pertence.
2. **TPT (Tabela por Tipo):** Cada classe (base e derivadas) tem a sua própria tabela. A tabela `Students` teria apenas colunas de `Student` e uma FK para a tabela `People`, que teria os campos comuns. (Suportado no EF Core 5+).
3. **TPC (Tabela por Classe Concreta):** Cada classe *concreta* (ex: `Student` e `Instructor`) tem a sua própria tabela completa, com todas as suas propriedades, incluindo as herdadas. A classe base (`Person`) não tem uma tabela.

### 11.2. O Desafio: Corrigindo uma Falha no Tutorial

Durante a implementação, foi identificada uma falha crítica no tutorial oficial do MVC, que mistura perigosamente as estratégias TPT e TPH:

* **A Dessincronização do Tutorial:**
    1. **Configuração do `DbContext`:** O tutorial instrui a manter mapeamentos de tabelas separados (`modelBuilder.Entity<Student>().ToTable("Estudantes");`, `...ToTable("Instrutores")`). Isto configura o `DbContext` para esperar uma estratégia **TPT (Tabela por Tipo)**.
    2. **Script de Migração:** Em seguida, o tutorial manda substituir a migração automática por um *script* SQL manual que renomeia `Instrutores` para `Pessoas`, move os dados de `Estudantes` para `Pessoas`, adiciona uma coluna `Discriminator` e apaga a tabela `Estudantes`. Isto implementa manualmente uma estratégia **TPH (Tabela por Hierarquia)**.

* **O Risco:** Esta dessincronização quebraria a aplicação. O `DbContext` (pensando em TPT) tentaria fazer consultas a uma tabela `Estudantes` que o *script* de migração (forçando TPH) teria apagado.

### 11.3. A Solução (Aplicada em Ambos os Projetos)

Para implementar corretamente a herança TPH num banco de dados com dados existentes, a seguinte correção em duas etapas foi aplicada (de forma idêntica) em ambos os projetos:

1. **Correção no `DbContext` (`SchoolContext.cs`):**
    * Para alinhar o `DbContext` com a estratégia TPH, os mapeamentos de tabelas das classes filhas foram **removidos** do `OnModelCreating`.
    * **Removido:** `modelBuilder.Entity<Student>().ToTable("Estudantes");`
    * **Removido:** `modelBuilder.Entity<Instructor>().ToTable("Instrutores");`
    * **Mantido:** `modelBuilder.Entity<Person>().ToTable("Pessoas");` (ou `Person`, dependendo da convenção).
    * Isto força o EF Core a entender que `Student` e `Instructor` são parte da tabela `Pessoas`.

2. **Manutenção da Migração Manual (`Migrations/..._Inheritance.cs`):**
    * Mesmo com o `DbContext` corrigido, a migração gerada automaticamente (com `Add-Migration`) apenas tentaria apagar a tabela `Estudantes`, causando **perda total de dados**.
    * Portanto, foi essencial manter o *script* de migração manual do tutorial. Este *script* atua como o "camião de mudanças", executando `migrationBuilder.Sql(...)` para *mover* os dados de `Estudantes` para a nova tabela `Pessoas` e corrigir as chaves estrangeiras (`Enrollment`) *antes* de, com segurança, apagar a tabela `Estudantes` original.

### 11.4. Análise Comparativa

Esta é a prova final da tese central deste comparativo: a camada de dados é agnóstica à UI.

* **Implementação Idêntica:** A implementação deste padrão de herança complexo foi **100% idêntica** em ambos os projetos.
* **Separação de Responsabilidades:** Toda a lógica residiu exclusivamente na camada de dados (`Models/`, `Data/SchoolContext.cs`, `Migrations/`).
* **Transparência para a UI:** A UI (seja o `StudentsController` do MVC ou o `PageModel` `Students/Index` do Razor Pages) continua a fazer a mesma consulta: `_context.Students.ToListAsync()`. Ela não precisa de saber que, "debaixo do capô", o EF Core está agora a consultar a tabela `Pessoas` e a filtrar por `WHERE Discriminator = 'Student'`.

## 12. Tópicos Avançados (Exclusivos do Tutorial MVC)

O tutorial de MVC conclui com uma secção de "Tópicos Avançados" que não tem um equivalente direto no tutorial de Razor Pages. Isto sugere que a Microsoft direciona o MVC para cenários que podem exigir uma "fuga" do alto nível de abstração do EF Core para otimizações de baixo nível.

No entanto, como esta análise demonstra, estas são funcionalidades do **Entity Framework Core**, não do MVC. Elas podem ser usadas de forma 100% idêntica num `PageModel` de Razor Pages.

### 12.1. Execução de Consultas SQL Brutas (Raw SQL)

O EF Core permite executar consultas SQL diretamente contra a base de dados, o que é útil para consultas complexas que o LINQ não consegue expressar facilmente.

* **O Desafio: Erros Históricos no Tutorial**
  * Durante a implementação, foram descobertas várias falhas na documentação do tutorial, que mistura métodos obsoletos com modernos.
    1. **Erro em `FromSql`:** O tutorial sugere `_context.Departments.FromSql(query, id)`. Isto falha, pois (como a análise do código-fonte do EF Core provou) a sobrecarga de `FromSql` no EF Core 8 espera **um** argumento `FormattableString` (interpolação), enquanto a sobrecarga `FromSqlRaw` espera `(string sql, params object[] parameters)`. O tutorial misturou o nome de um com os argumentos do outro.
    2. **Erro em `ExecuteSqlCommandAsync`:** O tutorial sugere `_context.Database.ExecuteSqlCommandAsync(...)` para atualizações em massa. Este método está obsoleto desde o EF Core 3.0.

* **A Solução e Análise (Comum a Ambos):**
    A forma correta (e segura contra **Injeção de SQL**) de executar SQL bruto é através de consultas parametrizadas, que existem em dois sabores:

    1. **Método "Raw" (Placeholders):**
        * **Para Consultas (`SELECT`):** `FromSqlRaw("... WHERE ID = {0}", id)`
        * **Para Comandos (`UPDATE`):** `ExecuteSqlRawAsync("UPDATE ... SET Credits = Credits * {0}", multiplier)`
        * *Análise:* Esta é a correção direta para o código do tutorial. Envia a consulta e os parâmetros separadamente para a base de dados, garantindo a segurança.

    2. **Método "Interpolado" (Preferido):**
        * **Para Consultas (`SELECT`):** `FromSql($"... WHERE ID = {id}")`
        * **Para Comandos (`UPDATE`):** `ExecuteSqlAsync($"UPDATE ... SET Credits = Credits * {multiplier}")`
        * *Análise:* Esta é a forma moderna. O C# converte a string interpolada (`$""`) num `FormattableString`, que o EF Core usa para criar uma consulta parametrizada segura. `FromSql` é agora um alias para `FromSqlInterpolated`.

* **Conclusão Comparativa:** A capacidade de executar SQL bruto é uma funcionalidade do `DbContext`, totalmente disponível e idêntica em ambas as arquiteturas.

### 12.2. O Risco do SQL Bruto: Contornar a Validação

Uma descoberta crucial ao implementar os comandos de atualização em massa (como `ExecuteSqlRawAsync`) é que eles contornam completamente as regras de negócio e validação definidas no modelo C#.

* **O Problema:** O modelo `Course` tem o atributo `[Range(0, 5)]` para a propriedade `Credits`. No entanto, ao executar o comando `ExecuteSqlRawAsync("UPDATE Cursos SET Credits = Credits * {0}", 3)`, os cursos com 3 créditos foram atualizados para 9.
* **Análise (Comum a Ambos):**
  * Os atributos de validação (`[Range]`, `[Required]`, etc.) são verificados apenas quando o EF Core está a usar o seu *Change Tracker* (Detetor de Alterações), tipicamente durante um `await _context.SaveChangesAsync()`.
  * Comandos SQL brutos (como `ExecuteSql...`) são enviados diretamente para a base de dados, "contornando" o `DbContext`. A base de dados não tem conhecimento das regras de atributos C#.
  * **Conclusão:** Esta é uma ferramenta poderosa que, se usada incorretamente, pode facilmente corromper a base de dados com "dados sujos" que violam as próprias regras da aplicação. A responsabilidade da validação é transferida para o programador.

### 12.3. LINQ Dinâmico (`EF.Property`)

* **O Problema:** A lógica de ordenação na página `Index` de Estudantes (Secção 5.1) usa um grande e verboso bloco `switch` para traduzir o parâmetro `sortOrder` (uma string) numa expressão LINQ (ex: `s => s.LastName`). Adicionar uma nova coluna para ordenação exige a alteração deste `switch`.

* **A Solução (Comum a Ambos):**
  * O tutorial de MVC introduz o método `EF.Property<object>(entity, stringPropertyName)`.
  * Isto permite reescrever todo o bloco `switch` de forma dinâmica e extensível:

    ```csharp
    // Ex: sortOrder = "LastName"
    students = students.OrderBy(e => EF.Property<object>(e, sortOrder));
    ```

  * **Análise:** Esta é uma otimização de código fantástica. A lógica de ordenação no `Controller` (MVC) ou `PageModel` (Razor Pages) torna-se universal. Para adicionar ordenação a uma nova coluna, basta modificar a *View* (`.cshtml`) para passar o nome correto da propriedade na string `sortOrder`, sem necessidade de alterar o código C# do *backend*.

* **Conclusão Comparativa:** Embora apenas o tutorial de MVC o mostre, esta técnica `EF.Property` é uma funcionalidade do EF Core e pode (e deve) ser aplicada de forma idêntica ao `PageModel` `Students/Index.cshtml.cs` no projeto Razor Pages, para obter a mesma simplificação de código.

### 12.4. Outros Conceitos (Comuns a Ambos)

* **Performance (`AutoDetectChangesEnabled`):** O tutorial menciona que, para operações em *batch* (lote) de muitas entidades, desativar temporariamente o detetor de alterações (`_context.ChangeTracker.AutoDetectChangesEnabled = false;`) pode dar um grande ganho de performance, evitando que o EF Core verifique por alterações desnecessariamente.
* **Engenharia Reversa (`scaffold-dbcontext`):** É mencionado o fluxo de trabalho "Database First" (onde as classes C# são geradas a partir de uma base de dados existente).

* **Análise Final:** Todos os "Tópicos Avançados" são, na verdade, tópicos do **Entity Framework Core**. A sua inclusão no tutorial de MVC e omissão no de Razor Pages reforça a perceção de que o MVC é a arquitetura "preferida" para cenários que exigem personalização de baixo nível, embora, tecnicamente, ambas as arquiteturas tenham acesso às mesmas ferramentas.

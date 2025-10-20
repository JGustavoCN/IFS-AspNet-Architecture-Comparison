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

* **ViewModel Agregador (`InstructorIndexData`):** Para gerenciar os dados das três tabelas (`Instructors`, `Courses`, `Enrollments`) em uma única página, foi criado o ViewModel `InstructorIndexData`. Essa classe atua como um contêiner para transportar todas as informações necessárias entre o `PageModel` e a `View`.

* **Estratégia de Carregamento Híbrida:** Uma estratégia de carregamento de dados em múltiplos estágios foi utilizada para otimizar a performance:
    1. **Carregamento Adiantado Inicial:** Na carga inicial da página, uma única consulta complexa é executada para buscar **todos** os instrutores. Essa consulta utiliza `Include` e `ThenInclude` para carregar adiantadamente os dados relacionados de `OfficeAssignment` e `Courses` (incluindo o `Department` de cada curso).
    2. **Filtragem em Memória:** Quando um instrutor é selecionado, a lista de seus cursos é obtida **filtrando os dados já carregados em memória**, evitando uma nova consulta ao banco de dados para buscar os cursos.
    3. **Carregamento Sob Demanda:** Apenas quando um curso é selecionado, uma **nova consulta** é executada no banco de dados para buscar especificamente as matrículas (`Enrollments`) daquele curso, incluindo os dados dos alunos (`Student`) relacionados.

* **Melhorias na Interface do Usuário:**
  * **Rotas Amigáveis:** A diretiva `@page "{id:int?}"` foi utilizada para transformar os parâmetros da URL de query strings (ex: `?id=1`) para segmentos de rota (ex: `/Instructors/1`), resultando em URLs mais limpas.
  * **Feedback Visual:** Foi implementada uma lógica na View que aplica a classe CSS `table-success` à linha (`<tr>`) do instrutor e do curso atualmente selecionados, fornecendo um feedback visual claro para os usuários.

## 8\. Comparativo Lado a Lado

| Critério | Razor Pages | MVC (Model-View-Controller) |
| :--- | :--- | :--- |
| **Organização do Código** | Coesão por funcionalidade (a View e sua lógica estão juntas na pasta `Pages`). | Separação por responsabilidade (lógica nos `Controllers`, UI nas `Views`, dados nos `Models`). |
| **Roteamento** | Baseado na estrutura de arquivos e pastas. A URL corresponde diretamente ao caminho do arquivo em `Pages`. | Baseado em convenções e configurações explícitas, geralmente no formato `{controller}/{action}/{id}`. |
| **Fluxo de Requisição** | Requisição -\> Roteamento para o arquivo `.cshtml` -\> Execução dos Handlers do `PageModel` (`OnGet`, `OnPost`) -\> Renderização da Página. | Requisição -\> Roteamento para a `Action` de um `Controller` -\> Processamento da lógica -\> Seleção e renderização da `View`. |
| **Complexidade Inicial** | Menor. Mais direto para criar páginas e formulários simples. | Moderada. Requer a compreensão da interação entre os três componentes. |
| **Ideal para...** | Aplicações centradas em formulários, operações CRUD e cenários onde a página é a unidade principal de funcionalidade. | Aplicações complexas com regras de negócio ricas, APIs web, e cenários que exigem alta testabilidade e flexibilidade. |
| **Reutilização de Lógica** | *(Preencha com sua análise, ex: via View Components, classes base para PageModel)* | *(Preencha com sua análise, ex: Controllers podem servir múltiplas Actions e ser usados para APIs e UI)* |

## 9\. Conclusão

*Aqui você escreverá sua conclusão pessoal sobre qual abordagem se encaixou melhor para este tipo de projeto e por quê, considerando os pontos levantados na organização do código, na camada de dados e no comparativo geral.*

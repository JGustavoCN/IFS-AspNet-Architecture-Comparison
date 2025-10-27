Análise Comparativa: Arquiteturas Razor Pages e MVC

1. Introdução
Este documento apresenta uma análise comparativa entre as arquiteturas Razor Pages e MVC (Model-View-Controller) no ecossistema ASP.NET Core. A análise foi baseada na implementação de uma aplicação web de gerenciamento acadêmico, seguindo os tutoriais oficiais da Microsoft, para a disciplina de Programação WEB II do IFS. O objetivo é destacar as diferenças práticas na estrutura do projeto, filosofia de desenvolvimento, fluxo de dados e produtividade do desenvolvedor.

2. Estrutura e Filosofia do Projeto
2.1. Razor Pages
Filosofia: Orientada a páginas (Page-Centric). A Microsoft criou esta arquitetura para cenários onde o modelo de interação é centrado em páginas individuais (como formulários e listagens). A ideia principal é a coesão: a marcação da UI (.cshtml) e o seu código de lógica (.cshtml.cs) são mantidos juntos, simplificando o desenvolvimento para funcionalidades focadas.

Estrutura de Arquivos: O projeto utiliza uma estrutura centrada na pasta Pages. Cada página consiste em dois ficheiros acoplados: um ficheiro .cshtml (a View) e um ficheiro code-behind .cshtml.cs que contém a classe PageModel para lidar com as requisições.

Configuração do Projeto:

Framework: .NET 8.0
Autenticação: Nenhuma
HTTPS: Configurado
Configuração: Utiliza instruções de nível superior no Program.cs para uma configuração mais concisa.
2.2. MVC (Model-View-Controller)
Filosofia: Orientada pela Separação de Responsabilidades (Separation of Concerns). A aplicação é dividida em três componentes: Model (dados e regras de negócio), View (a UI) e Controller (o intermediário que orquestra a interação). Esta abordagem promove baixo acoplamento, facilitando a manutenção e os testes de forma independente.

Estrutura de Arquivos: O projeto organiza o código em pastas distintas para cada responsabilidade: Controllers (para a lógica de requisição), Views (para a UI, organizada por subpastas de controller) e Models (para as entidades de dados).

Configuração do Projeto:

Framework: .NET 8.0
Autenticação: Nenhuma
HTTPS: Configurado
Configuração: Com a adoção do "modelo de hospedagem mínimo" a partir do .NET 6, a configuração também é unificada no Program.cs, abolindo o antigo ficheiro Startup.cs.
2.3. Análise Comparativa da Estrutura
A diferença fundamental reside na organização. Razor Pages adota uma organização vertical: todos os ficheiros relativos a uma funcionalidade (ex: Students) estão juntos na mesma pasta. O MVC adota uma organização horizontal: os ficheiros são agrupados pelo seu papel técnico (Controllers, Views), o que espalha uma única funcionalidade por várias pastas.

3. Camada de Dados com Entity Framework Core
Um dos primeiros e mais importantes pontos observados é que a camada de dados é completamente agnóstica à arquitetura de UI.

Implementação: As pastas Models (com as entidades) e Data (com o SchoolContext e DbInitializer) do projeto Razor Pages foram diretamente reutilizadas no projeto MVC sem nenhuma alteração. O registo do DbContext em ambos os projetos é feito de forma semelhante no Program.cs.

Análise: Isto demonstra um pilar da boa arquitetura: o Entity Framework Core gere o acesso a dados de forma independente de como esses dados serão exibidos. Esta abordagem acelera o desenvolvimento, promove a reutilização de código e garante a consistência da lógica de negócio entre diferentes interfaces da mesma aplicação.

4. Operações CRUD (Create, Read, Update, Delete)
A implementação das operações básicas de dados expõe as diferenças práticas no fluxo de cada arquitetura.

4.1. Listar Entidades (Read)
Implementação em Razor Pages:

A funcionalidade está encapsulada nos ficheiros Pages/Students/Index.cshtml e Pages/Students/Index.cshtml.cs.
Uma requisição GET a /Students executa o método OnGetAsync() no PageModel.
Os dados são obtidos da base de dados e atribuídos a uma propriedade pública do PageModel (ex: public IList<Student> Student { get; set; }).
A View (.cshtml) acede aos dados através desta propriedade (Model.Student).
Implementação em MVC:

A funcionalidade está dividida entre Controllers/StudentsController.cs e Views/Students/Index.cshtml.
Uma requisição GET a /Students é roteada para a Action Index() no StudentsController.
Os dados são obtidos da base de dados e passados como argumento para a View através do método return View(dados).
A View (.cshtml) recebe a lista de dados diretamente no seu @model.
Análise Comparativa:

Característica Razor Pages MVC (Model-View-Controller)
Localização da Lógica No PageModel, fortemente acoplado à sua View. No Controller, separado da View.
Fluxo de Dados para a UI A lógica preenche uma propriedade do PageModel. A Action do Controller passa o modelo como parâmetro.
Acoplamento Alto: A View e o PageModel são duas metades da mesma unidade. Baixo: A View é independente e pode ser reutilizada por outra Action.
4.2. Criar Entidades (Create)
Ambas as arquiteturas separam a lógica de "mostrar o formulário" da de "processar os dados".

Implementação em Razor Pages:

A página Create.cshtml.cs tem dois handlers: OnGet() e OnPostAsync().
OnGet(): Apenas exibe a página (return Page();).
OnPostAsync():
Os dados do formulário são vinculados à propriedade [BindProperty] public Student Student { get; set; }.
Usa _context.Students.Add(Student) para marcar a entidade como Added.
Salva na base de dados e redireciona.
A segurança contra overposting (excesso de postagem) é feita implicitamente no tutorial, pois o [BindProperty] cria um novo Student vazio e o Add apenas insere os campos que foram vinculados.
Implementação em MVC:

O StudentsController.cs tem duas Actions: Create() (com [HttpGet]) e Create() (com [HttpPost]).
Create() [GET]: Apenas exibe a View (return View();).
Create() [POST]:
Recebe os dados como um parâmetro da Action: public async Task<IActionResult> Create([Bind("LastName,FirstMidName,EnrollmentDate")] Student student).
O Model Binding cria o objeto student a partir do formulário.
A segurança contra overposting é explícita, usando o atributo [Bind(...)] para listar exatamente quais campos são permitidos.
Usa _context.Add(student) (estado Added) e salva.
Análise Comparativa:

A lógica é quase idêntica, mas a implementação difere. Razor Pages usa Handlers na mesma classe, enquanto MVC usa Actions separadas no Controller.
Ambos usam o atributo [ValidateAntiForgeryToken] para prevenir ataques CSRF, com o token a ser gerado automaticamente pelo <form>.
A principal diferença é no Model Binding e segurança: Razor Pages vincula a uma propriedade da classe, enquanto MVC vincula a um parâmetro de método. O tutorial de MVC introduz imediatamente o [Bind] como uma medida de segurança explícita contra overposting.
4.3. Exibir Detalhes (Details)
Esta operação é focada apenas em leitura e é muito semelhante à listagem (4.1).

Implementação em Razor Pages:

O PageModel (Details.cshtml.cs) usa OnGetAsync(int? id) para receber o ID.
Os dados são buscados (com FirstOrDefaultAsync) e atribuídos à propriedade public Student Student { get; set; }.
A View acede aos dados via Model.Student.LastName.
Implementação em MVC:

O Controller usa a Action Details(int? id).
Os dados são buscados (com FirstOrDefaultAsync) e passados diretamente para a View: return View(student).
A View acede aos dados diretamente via Model.LastName.
Análise Comparativa:

A lógica é idêntica. A única diferença, tal como na listagem, é que Razor Pages usa uma propriedade no PageModel para expor os dados, enquanto MVC passa o modelo diretamente para a View().
Nota: Ambos os tutoriais usam FirstOrDefaultAsync. Uma otimização possível em ambos seria usar FindAsync(id), que primeiro verifica se a entidade já está na memória (cache do DbContext) antes de consultar a base de dados, sendo ideal para buscas por chave primária.
4.4. Editar Entidades (Update)
Esta é a operação mais complexa e onde as diferenças de abordagem ficam mais evidentes, especialmente em relação ao tratamento do estado "desconectado" da web.

Implementação em Razor Pages:

OnGetAsync(int? id): Busca o estudante (com FindAsync) e preenche a propriedade [BindProperty] Student para exibir no formulário.
OnPostAsync(int id):
Solução (B) "Buscar Original e Comparar": O método primeiro busca a entidade original do banco: var studentToUpdate = await _context.Students.FindAsync(id).
Em seguida, usa await TryUpdateModelAsync<Student>(studentToUpdate, "Student", s => s.FirstMidName, s => s.LastName, ...) para aplicar apenas os campos permitidos do formulário na entidade que o DbContext está a rastrear.
Isto previne overposting e gera um SQL de UPDATE eficiente, que atualiza apenas os campos alterados (estado Modified).
Implementação em MVC:

Edit(int? id) [GET]: Busca o estudante (com FindAsync) e passa-o para a View: return View(student).
Edit(int id, [Bind("ID,LastName,...")] Student student) [POST]:
Solução (A) "Trocar Tudo" (com segurança): O Model Binder cria um novo objeto student com os dados do formulário, graças ao [Bind].
O código valida o ID (id == student.ID).
Em seguida, usa _context.Update(student). Isto anexa a entidade "desconectada" ao DbContext e marca todas as suas propriedades (listadas no [Bind]) como Modified.
Gera um SQL de UPDATE que atualiza todas as colunas, mesmo as que não foram alteradas.
Análise Comparativa:

Ambas as abordagens separam GET e POST e previnem overposting (CSRF e [Bind]/TryUpdateModelAsync).
Diferença Chave de Estratégia: O tutorial de Razor Pages opta por uma leitura extra da base de dados no POST (FindAsync) para usar TryUpdateModelAsync. Isto é mais eficiente a nível de SQL (só atualiza o que mudou).
O tutorial de MVC opta por usar _context.Update() no objeto vindo do Model Binder. Isto evita a leitura extra no POST, mas é menos eficiente a nível de SQL (atualiza todos os campos).
Ambas as plataformas podem usar ambas as estratégias (TryUpdateModelAsync existe no MVC e_context.Update existe no Razor Pages), mas os tutoriais mostram as abordagens mais comuns para cada uma.
4.5. Excluir Entidades (Delete)
Implementação em Razor Pages:

OnGetAsync(int? id): Busca o estudante e armazena na propriedade Student para exibir os dados de confirmação.
OnPostAsync(int id): Busca o estudante com FindAsync(id), o remove com _context.Students.Remove(student) (marcando-o como Deleted), e salva as alterações.
Implementação em MVC:

Delete(int? id) [GET]: Busca o estudante e passa-o para a View de confirmação.
DeleteConfirmed(int id) [POST]: Uma Action separada (geralmente com [HttpPost, ActionName("Delete")]) é usada para o POST.
A lógica é idêntica: busca com FindAsync(id), remove com _context.Students.Remove(student) (estado Deleted), e salva.
Análise Comparativa:

A lógica e a abordagem são praticamente idênticas em ambas as arquiteturas.
4.6. Análise de Conceitos-Chave do CRUD
Vários conceitos são fundamentais para o funcionamento do CRUD em ASP.NET Core e aplicam-se a ambas as arquiteturas.

Separação GET vs. POST:

Porquê? Como explicaste, esta é uma convenção fundamental da web.
GET (Buscar): Deve ser uma operação segura e idempotente (pode ser repetida sem causar efeitos colaterais). É usada para mostrar a página (o formulário de criação, o formulário de edição pré-preenchido, a página de confirmação de exclusão).
POST (Alterar): É usada para enviar dados que modificam o estado no servidor (criar um registo, atualizar um registo, excluir um registo). Não é idempotente.
Auxiliares de Marcação (Tag Helpers):

Ambas as arquiteturas usam exatamente os mesmos Tag Helpers nas Views (.cshtml).
label, input, span asp-validation-for: Geram o HTML para os campos do modelo e exibem mensagens de validação.
asp-action, asp-controller, asp-page, asp-route-id: Geram os links (<a>) e URLs de action do formulário (<form>) de forma correta, garantindo que o roteamento funcione.
Conclusão: A camada de View é partilhada e idêntica.
O Problema dos Dados Desconectados (Estados da Entidade):

Como a tua analogia da "fotocópia" descreveu, o DbContext tem um ciclo de vida por requisição (é Scoped). O DbContext do GET é destruído. Um DbContext novo é criado para o POST.
Este novo DbContext não sabe nada sobre a entidade original ("fotocópia rabiscada").
Estados do EF Core: Para resolver isto, temos de dizer ao novo DbContext o que fazer:
_context.Add(student): Diz "Esta entidade é nova" (estado Added). O SaveChanges fará um INSERT.
_context.Update(student): Diz "Esta entidade existe mas foi modificada" (estado Modified). O SaveChanges fará um UPDATE para todas as colunas.
_context.Remove(student): Diz "Esta entidade deve ser apagada" (estado Deleted). O SaveChanges fará um DELETE.
TryUpdateModelAsync(studentToUpdate, ...): Este é o método mais inteligente. Ele usa uma entidade já rastreada (studentToUpdate que veio do FindAsync) e atualiza apenas os campos necessários, marcando-os individualmente como Modified.
Segurança (CSRF e Overposting):

CSRF (Cross-Site Request Forgery):
O [ValidateAntiForgeryToken] (no PageModel ou na Action [HttpPost]) trabalha em conjunto com o <form> (que usa o FormTagHelper) para gerar um token oculto.
Isto garante que o pedido POST veio de um formulário gerado pela tua própria aplicação.
A implementação é idêntica em ambas as arquiteturas.
Overposting (Excesso de Postagem):
Ocorre quando um utilizador mal-intencionado envia mais campos do que o formulário exibe (ex: StudentID=1, IsAdmin=true).
Ambos os tutoriais previnem isto, mas com as estratégias diferentes que vimos na secção 4.4:

1. MVC (Tutorial): Usa [Bind("Prop1", "Prop2")] no parâmetro da Action [POST].
2. Razor Pages (Tutorial): Usa TryUpdateModelAsync(entidade, "prefixo", e => e.Prop1, e => e.Prop2) no handler [POST].
Ambas são defesas eficazes. TryUpdateModelAsync é frequentemente considerado mais robusto, pois está mais próximo da lógica de atualização do que da definição do método.
5. Funcionalidades Avançadas de Listagem
Aqui, analisamos a implementação de ordenação, filtragem (pesquisa) e paginação. Esta secção destaca a importância crucial do LINQ to Entities e da diferença entre IQueryable e IEnumerable.

5.1. Ordenação, Filtro e Paginação
O Conceito: IQueryable e a Execução Diferida

Ambos os tutoriais (Razor Pages e MVC) baseiam-se num conceito fundamental: construir a consulta à base de dados passo a passo, mas sem a executar.
Quando escrevemos var students = _context.Students.AsQueryable();, não estamos a ir à base de dados. Estamos a criar um objeto IQueryable<Student>, que é uma árvore de expressão (expression tree) que representa uma intenção de consulta.
Cada método que adicionamos (como .Where(s => s.LastName.Contains(...)) ou .OrderBy(s => s.LastName)) apenas modifica essa árvore de expressão. A consulta ainda não foi executada.
Isto é o LINQ to Entities: o Entity Framework Core analisa esta árvore de expressão C# e traduz tudo numa única e otimizada consulta SQL.
A consulta só é finalmente enviada à base de dados quando um método de "materialização" é chamado, como ToListAsync(), FirstOrDefaultAsync() ou CountAsync().
O Anti-Padrão (IEnumerable)

Se, por engano, chamássemos ToListAsync() antes de aplicar os filtros (ex: var students = await _context.Students.ToListAsync(); var filtered = students.Where(...);), estaríamos a usar LINQ to Objects.
Isto traria todos os registos da tabela Students para a memória do servidor web e só depois aplicaria o filtro. Numa tabela com milhares de linhas, isto seria desastroso para a performance.
Conclusão: O sucesso desta funcionalidade depende de manter a consulta como IQueryable o máximo de tempo possível.
Implementação em Razor Pages:

O método OnGetAsync(string sortOrder, string currentFilter, string searchString, int? pageIndex) recebe todos os parâmetros da URL.
O searchString (termo de pesquisa) é frequentemente associado a uma propriedade [BindProperty(SupportsGet = true)] para repopular o campo de busca.
A lógica reside no OnGetAsync:
Inicia com IQueryable<Student> studentsIQ = _context.Students.AsQueryable();
Aplica o filtro: if (!String.IsNullOrEmpty(searchString)) { studentsIQ = studentsIQ.Where(...); }
Aplica a ordenação: switch (sortOrder) { ... studentsIQ = studentsIQ.OrderBy(...); }
Finalmente, a paginação (que usa .Skip() e .Take()) e a materialização (ToListAsync()) são encapsuladas na classe PaginatedList.CreateAsync(studentsIQ.AsNoTracking(), pageIndex ?? 1, ...)
O resultado (PaginatedList) é atribuído a uma propriedade pública do PageModel para a View usar.
Implementação em MVC:

A Action Index(string sortOrder, string currentFilter, string searchString, int? pageIndex) recebe os mesmos parâmetros.
Valores como ViewData["CurrentSort"] são usados para passar o estado atual para a View, para que os links de ordenação e paginação possam ser construídos corretamente.
A lógica reside na Action Index:
Inicia com var students = _context.Students.AsQueryable(); (O var aqui é inferido como IQueryable<Student>).
Aplica o filtro: if (!String.IsNullOrEmpty(searchString)) { students = students.Where(...); }
Aplica a ordenação: switch (sortOrder) { ... students = students.OrderBy(...); }
A mesma classe PaginatedList.CreateAsync(students.AsNoTracking(), pageIndex ?? 1, ...) é usada para executar a consulta.
O resultado (PaginatedList) é passado diretamente para a View: return View(await PaginatedList.CreateAsync(...));.
Análise Comparativa:

A lógica de negócio é absolutamente idêntica. O código dentro do OnGetAsync() (Razor Pages) e da Action Index() (MVC) é o mesmo em 99%. A classe PaginatedList.cs é partilhada e reutilizada sem qualquer modificação.
A única diferença é o "encanamento": Razor Pages armazena o resultado numa propriedade do PageModel, enquanto MVC passa o resultado como o modelo no return View().
A Implementação do Filtro (Pesquisa) e o method="get"

Ambos os tutoriais implementam o formulário de pesquisa (<form>) usando o método HTTP GET (<form method="get">).
Isto é uma prática recomendada pelas diretrizes do W3C para operações que são idempotentes (ou seja, seguras de repetir, como uma pesquisa, que não altera dados).
Ao usar method="get", os parâmetros do formulário (ex: searchString=...) são adicionados à URL como query strings.
Vantagem: O utilizador pode marcar a página de resultados da pesquisa, copiar o link e partilhá-lo com outros, e o link conterá os termos da pesquisa.
O FormTagHelper (<form asp-page="/Students/Index" method="get"> ou <form asp-action="Index" method="get">) gera este HTML corretamente em ambas as arquiteturas.
6. Gestão do Esquema da Base de Dados com Migrations
Assim como a definição dos modelos e do DbContext (Secção 3), o processo de gestão das alterações do esquema da base de dados é uma responsabilidade exclusiva do Entity Framework Core e é 100% idêntico em ambas as arquiteturas.

6.1. O Processo de Migrations
O EF Core Migrations é a ferramenta usada para manter o esquema da base de dados sincronizado com o modelo de dados C# (as tuas classes de entidade).

Ferramentas: O processo requer o pacote NuGet Microsoft.EntityFrameworkCore.Tools. Os comandos podem ser executados de duas formas, que são funcionalmente idênticas:

PMC (Package Manager Console): Uma consola dentro do Visual Studio (ex: Add-Migration ...).
CLI (Command-line Interface): A linha de comandos do .NET (ex: dotnet ef migrations add ...).
Alternativa (EnsureCreated): O método context.Database.EnsureCreated() é uma alternativa que cria a base de dados se ela não existir, mas não pode atualizá-la se o modelo C# mudar. Os tutoriais de ambos os projetos evitam este método, pois ele é mutuamente exclusivo com as Migrations, que é a abordagem profissional e preferida para a evolução do esquema.

6.2. Análise Comparativa do Fluxo
O fluxo de trabalho é o mesmo, quer se esteja num projeto Razor Pages ou MVC:

Add-Migration <NomeDaMigracao> (ex: Add-Migration InitialCreate):

Este comando compara o estado atual das classes do modelo (lido a partir do DbContext) com o último instantâneo do esquema.
Como é a primeira migração, ele gera o ficheiro <timestamp>_InitialCreate.cs, que contém o código C# nos métodos Up() (para criar todas as tabelas) e Down() (para apagá-las).
Ele também cria (ou atualiza) o ficheiro Migrations/SchoolContextModelSnapshot.cs. Este ficheiro é um instantâneo do esquema de base de dados atual e é fundamental; é contra ele que a próxima migração será comparada para detetar alterações.
Update-Database:

Este comando executa todas as migrações pendentes.
Ele verifica a tabela __EFMigrationsHistory na base de dados para saber quais migrações já foram aplicadas.
Como a InitialCreate ainda não foi aplicada, ele executa o método Up() desse ficheiro, criando fisicamente todas as tabelas no SQL Server.
Após a conclusão bem-sucedida, ele adiciona um registo na tabela__EFMigrationsHistory.
Verificação: Em ambos os projetos, o Pesquisador de Objetos do SQL Server (SSOX) é usado para verificar visualmente se as tabelas foram criadas corretamente, incluindo a tabela __EFMigrationsHistory.

Outros Comandos: Comandos como Remove-Migration (que remove o último ficheiro de migração e reverte o instantâneo) também funcionam da mesma forma em ambos os projetos.

Conclusão: Esta etapa reforça que o EF Core é uma camada de infraestrutura completamente desacoplada da UI. A forma como geres, crias e atualizas a tua base de dados não é afetada pela tua escolha entre Razor Pages e MVC.

7. Modelagem de Dados Complexos e Relacionamentos
Esta etapa da implementação consistiu em evoluir o modelo de dados simples para um modelo mais robusto, que reflete as complexas regras de negócio de uma universidade. Isto envolveu a adição de novas entidades (Instructor, OfficeAssignment, Department, CourseAssignment) e a atualização das existentes.

7.1. Análise Comparativa
Esta é a secção mais importante da nossa análise até agora, juntamente com as Secções 3 e 6: A implementação é 100% idêntica e partilhada entre os projetos MVC e Razor Pages.

Toda a lógica de negócio, regras de validação, relacionamentos e mapeamento da base de dados residem nos ficheiros da pasta Models/ e no Data/SchoolContext.cs. O projeto MVC reutiliza esta camada sem qualquer alteração.

Isto prova que a escolha entre MVC e Razor Pages é puramente uma decisão sobre a arquitetura da camada de apresentação (UI). A lógica de negócio e de dados subjacente permanece independente, reutilizável e agnóstica.

7.2. Análise dos Conceitos de Modelagem (Comum a Ambos)
Ambos os projetos utilizam uma combinação de Convenções do EF Core, Atributos (Data Annotations) e API Fluente para construir o modelo.

Atributos (Data Annotations): São usados para fornecer metadados ao EF Core e ao ASP.NET Core.

Validação e Schema (System.ComponentModel.DataAnnotations):
[Required]: Indica que a propriedade não pode ser nula. Afeta tanto a validação do lado do cliente (nos formulários) como o esquema da base de dados (coluna NOT NULL).
[StringLength(50)], [MaxLength(50)]: Define um tamanho máximo. Usado para validação e para definir o tamanho da coluna (ex: nvarchar(50)).
[Range(0, 5)]: Define um intervalo numérico para validação.
[RegularExpression(...)]: Valida o formato da string.
Apenas UI/Formatação (System.ComponentModel.DataAnnotations):
[Display(Name = "Last Name")]: Controla o texto exibido pelos auxiliares de marcação (ex: <label>).
[DataType(DataType.Date)]: Especifica o tipo de dados, ajudando os Tag Helpers a renderizar o controlo HTML correto (ex: type="date").
[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]: Formata a exibição dos dados.
Apenas Schema (System.ComponentModel.DataAnnotations.Schema):
[Column("NomeDaColuna")]: Especifica um nome de coluna diferente do nome da propriedade.
[DatabaseGenerated(DatabaseGeneratedOption.None)]: Informa ao EF Core que a chave primária (ex: CourseID) será fornecida pela aplicação, em vez de ser gerada pela base de dados.
Propriedades de Navegação e Relacionamentos:

O EF Core interpreta os relacionamentos com base nas propriedades de navegação (ICollection<T>, List<T>, ou tipos de entidade simples como Department).
Chaves Primárias (PK): São detetadas por convenção (ID ou classnameID) ou explicitamente com o atributo [Key].
Chaves Estrangeiras (FK): O EF Core pode criar "propriedades de sombra" (shadow properties) para FKs, mas é uma boa prática incluí-las explicitamente (ex: public int DepartmentID { get; set; }). O uso de int? (um inteiro que permite valor nulo) indica que o relacionamento é opcional.
Relacionamento Muitos-para-Muitos:
Ambos os tutoriais implementam a relação N-N entre Instructor e Course criando uma tabela de junção explícita com conteúdo (CourseAssignment).
Isto é feito criando a entidade CourseAssignment com uma chave primária composta (CourseID e InstructorID). Esta chave é definida usando a API Fluente, pois os atributos não conseguem definir chaves compostas de forma limpa.
API Fluente (OnModelCreating) vs. Atributos:

Embora a maioria da modelagem tenha sido feita com atributos (Annotations), a API Fluente (configurando o modelBuilder no OnModelCreating do DbContext) é usada para cenários mais complexos que os atributos não suportam. A API Fluente sempre substitui os atributos.
Exemplo 1 (Chave Composta): modelBuilder.Entity<CourseAssignment>().HasKey(c => new { c.CourseID, c.InstructorID });
Exemplo 2 (Exclusão em Cascata): Por convenção, o EF Core habilita a exclusão em cascata (cascade delete) para chaves estrangeiras que não permitem nulos. Como explicado, isto pode criar ciclos de exclusão. Para corrigir isto (ex: impedir que a exclusão de um Instructor exclua um Department do qual ele é administrador), a API Fluente é usada para definir o comportamento como restrito: modelBuilder.Entity<Department>().HasOne(d => d.Administrator).WithMany().OnDelete(DeleteBehavior.Restrict).
7.3. O Desafio da Migração: Adicionando Colunas Obrigatórias
Como o modelo de dados evoluiu, a migração (Add-Migration ComplexDataModel) precisou de uma intervenção manual.

O Problema: A entidade Course foi modificada para ter uma DepartmentID obrigatória (não nula). No entanto, a base de dados já continha registos de Course (criados pelo DbInitializer). Ao aplicar a migração, o SQL Server não saberia qual valor colocar na nova coluna DepartmentID para os cursos que já existiam, resultando num erro.
A Solução (Comum a Ambos):
A migração gerada foi editada manualmente.
Primeiro, um migrationBuilder.Sql("INSERT INTO Department (Name, Budget, StartDate) VALUES ('Temp', 0.00, GETDATE())") foi adicionado para criar um departamento "fantasma" temporário.
Segundo, o comando migrationBuilder.AddColumn<int>(...) para DepartmentID foi modificado para incluir um defaultValue: 1 (assumindo que 1 é o ID do departamento "Temp").
Isto permitiu que o Update-Database fosse executado com sucesso, associando todos os cursos existentes ao departamento temporário, satisfazendo a restrição NOT NULL.
8. Leitura de Dados Relacionados
Uma vez que o modelo de dados complexo está definido, o próximo passo é consultar e exibir esses dados relacionados. Um ORM como o Entity Framework Core facilita isto ao "mapear" as linhas da base de dados para objetos C# e preencher as suas propriedades de navegação.

8.1. Estratégias de Carregamento (Conceitos Comuns)
Ambos os projetos dependem das mesmas estratégias de carregamento de dados do EF Core:

Carregamento Adiantado (Eager Loading): Esta é a estratégia principal usada em ambos os tutoriais.
Como funciona: Os dados relacionados são recuperados juntamente com a entidade principal numa única consulta. Isto é feito usando os métodos .Include() (para o primeiro nível de relacionamento) e .ThenInclude() (para níveis subsequentes).
Vantagem: É muito eficiente. Evita o "problema N+1", onde uma consulta inicial é seguida por N consultas separadas (uma para cada entidade relacionada).
Carregamento Explícito (Explicit Loading):
Como funciona: Os dados relacionados são carregados após a entidade principal já ter sido recuperada, mas através de um comando explícito (ex: _context.Entry(student).Collection(s => s.Enrollments).LoadAsync()).
Vantagem: Útil quando os dados relacionados só são necessários condicionalmente.
Carregamento Lento (Lazy Loading):
Como funciona: Os dados relacionados são carregados automaticamente da base de dados no momento em que a propriedade de navegação é acedida pela primeira vez.
Análise: Embora suportado pelo EF Core (com pacotes extra), não é usado nos tutoriais, pois pode levar facilmente ao problema N+1 se não for usado com cuidado.
8.2. Rastreamento vs. Sem Rastreamento (Performance)
Consultas de Acompanhamento (Tracking): Por padrão, o DbContext "rastreia" as entidades que recupera. Ele guarda um instantâneo delas para poder detetar alterações e gerar o UPDATE correto quando SaveChanges() é chamado.
Consultas Sem Acompanhamento (AsNoTracking()):
Para páginas de apenas leitura (como as páginas Index e Details), este rastreamento é um desperdício de memória e processamento.
Ambos os tutoriais usam .AsNoTracking() nestas páginas. Isto diz ao EF Core: "Apenas me dê os dados; não precisas de os rastrear para futuras atualizações."
Resultado: A consulta é executada significativamente mais rápido e consome menos memória.
8.3. Ponto de Divergência: Relações Muitos-para-Muitos (N-N)
Este é um ponto crucial na nossa análise. Os dois tutoriais oficiais da Microsoft abordam o relacionamento N-N (Instrutores <-> Cursos) de formas diferentes:

Tutorial Razor Pages (e o teu Projeto): Usa a abordagem moderna (EF Core 5+), onde a tabela de junção é implícita. O modelo tem apenas ICollection<Course> em Instructor e ICollection<Instructor> em Course. O EF Core cria a tabela de junção automaticamente.
Tutorial MVC (Oficial): Usa a abordagem mais antiga (ou necessária quando a tabela de junção tem conteúdo), onde a tabela de junção é explícita. Ou seja, existe uma entidade CourseAssignment que representa manualmente a ligação.
Decisão do Projeto: Para manter uma comparação justa entre as arquiteturas de UI, o nosso projeto MVC reutiliza o modelo de dados moderno (implícito) do projeto Razor Pages. Portanto, a nossa análise compara como o MVC e o Razor Pages consomem o mesmo modelo de dados.

8.4. Implementação e Análise Comparativa
Funcionalidade: Página de Cursos (Carregamento Nível 1)
Objetivo: Listar Cursos e exibir o nome do Department relacionado (relacionamento 1-N).

Implementação em Razor Pages:

O PageModel (Pages/Courses/Index.cshtml.cs) usa Eager Loading para buscar o departamento.
A consulta é: _context.Courses.Include(c => c.Department).AsNoTracking().
O resultado é atribuído a uma propriedade: public IList<Course> Course { get; set; }.
Implementação em MVC:

O Controller (Controllers/CoursesController.cs) usa a mesma consulta na Action Index.
A consulta é: _context.Courses.Include(c => c.Department).AsNoTracking().
O resultado é passado para a View: return View(await courses.ToListAsync()).
Análise: A lógica de acesso a dados é 100% idêntica. A única diferença é como os dados chegam à View (PageModel Property vs. return View(model)).

Funcionalidade: Página de Instrutores (Carregamento Nível Múltiplo)
Objetivo: Criar uma página mestre-detalhe complexa:

Mostrar a lista de Instrutores.
Ao selecionar um Instrutor, mostrar os Cursos que ele leciona (N-N).
Ao selecionar um Curso, mostrar os Alunos matriculados (1-N).
Necessidade: Esta UI complexa não pode ser representada por uma única entidade (Instructor). Ela requer um ViewModel dedicado (InstructorIndexData) para conter as três listas (IEnumerable<Instructor>, IEnumerable<Course>, IEnumerable<Enrollment>).

Implementação em Razor Pages:

O PageModel (Pages/Instructors/Index.cshtml.cs) é responsável por orquestrar esta lógica.

No OnGetAsync(), ele usa uma consulta complexa de Eager Loading com Include e ThenInclude para trazer todos os dados necessários de uma só vez:

viewModel.Instructors = await _context.Instructors
    .Include(i => i.OfficeAssignment)
    .Include(i => i.Courses) // Nível 1
        .ThenInclude(c => c.Department) // Nível 2
    .Include(i => i.Courses) // Nível 1 (de novo)
        .ThenInclude(c => c.Enrollments) // Nível 2
            .ThenInclude(e => e.Student) // Nível 3
    .AsNoTracking()
    .ToListAsync();
O PageModel então preenche o ViewModel (InstructorIndexData) e expõe-no como uma propriedade pública para a View.

Implementação em MVC:

O Controller (Controllers/InstructorsController.cs) tem a exata mesma responsabilidade.
A Action Index() reutiliza o mesmo InstructorIndexData ViewModel.
A lógica de consulta dentro da Action é idêntica à do OnGetAsync() do Razor Pages (a mesma consulta com Include e ThenInclude).
O Controller preenche o ViewModel e passa-o para a View: return View(viewModel).
Análise Comparativa:

Esta é a prova mais forte de que a complexidade da lógica de consulta de dados não depende da arquitetura de UI.
Ambas as arquiteturas identificaram a necessidade de um ViewModel para servir uma UI complexa.
Ambas usaram a mesma consulta EF Core de alta performance (Eager Loading) para preencher esse ViewModel.
Mais uma vez, a única diferença foi o "encanamento" de como o ViewModel preenchido chegou ao ficheiro .cshtml.
9. Atualização de Dados Relacionados
Se a leitura de dados relacionados (Secção 8) demonstrou a eficiência do EF Core, a atualização desses dados demonstra como cada arquitetura lida com operações complexas e transacionais. Esta é a funcionalidade mais complexa do tutorial.

9.1. Cenário 1: Atualizar Relação 1-N (Curso e Departamento)
Objetivo: Na página de edição de um Course (Curso), permitir a alteração do seu Department (Departamento) através de uma lista dropdown.

Implementação em Razor Pages:

O PageModel (Pages/Courses/Edit.cshtml.cs) herda de uma classe base (DepartmentNamePageModel) que tem a lógica para criar um SelectList dos departamentos.
OnGetAsync(): Carrega o curso e chama a lógica para popular o SelectList, que é armazenado numa propriedade (ex: ViewData["DepartmentID"] ou uma propriedade tipada).
OnPostAsync(): Usa TryUpdateModelAsync para aplicar de forma segura as alterações ao Course que foi carregado, incluindo a DepartmentID selecionada no dropdown.
Implementação em MVC:

O Controller (CoursesController.cs) é responsável por esta lógica.
Edit() [GET]: Carrega o curso e chama um método auxiliar (ou fá-lo diretamente) para criar o SelectList de departamentos, que é passado para a View através do ViewData["DepartmentID"].
Edit() [POST]: Usa TryUpdateModelAsync (ou [Bind]) para aplicar as alterações.
Análise: A lógica é idêntica. Ambos precisam de carregar dados "extra" (a lista de departamentos) para a View. Ambos usam a mesma estratégia de POST (carregar, tentar atualizar, salvar). A única diferença é onde a lógica para popular o dropdown reside (classe base do PageModel vs. método privado ou Action no Controller).

9.2. Cenário 2: Atualizar Relações 1-1 e N-N (Instrutor)
Objetivo: Na página de edição de um Instructor (Instrutor), permitir:

A edição do OfficeAssignment (Lotação) (relação 1-1).
A seleção dos Courses (Cursos) que ele leciona (relação N-N) através de checkboxes.
Análise da Implementação (Quase Idêntica):
Esta funcionalidade é tão complexa que ambas as arquiteturas adotam exatamente a mesma abordagem técnica, provando que, para lógica de negócio complexa, a arquitetura de UI torna-se apenas um "invólucro".

ViewModel (AssignedCourseData):

Ambas as arquiteturas reutilizam o mesmo ViewModel, AssignedCourseData, para representar a lista de checkboxes (contendo CourseID, Title e o booleano Assigned).
Lógica GET (Popular a Página):

Razor Pages: O OnGetAsync() no PageModel (Pages/Instructors/Edit.cshtml.cs) carrega o Instructor (com Include dos seus OfficeAssignment e Courses) e chama um método auxiliar (PopulateAssignedCourseData) para preencher a lista de checkboxes. O ViewModel é armazenado como uma propriedade no PageModel.
MVC: A Action Edit() [GET] no Controller (InstructorsController.cs) faz exatamente o mesmo: carrega o Instructor (com Include), chama o mesmo método auxiliar (PopulateAssignedCourseData) e passa a lista de checkboxes para a View (ex: via ViewData["Courses"]).
Renderização na View (.cshtml):

As Views são quase idênticas. Ambas usam os Tag Helpers label asp-for, input asp-for e span asp-validation-for para os campos simples (como LastName).
Ambas contêm um ciclo foreach para renderizar a lista de checkboxes do ViewModel (AssignedCourseData).
(Nota: A observação do tutorial sobre Ctrl+Z para corrigir a formatação do Razor é um bug de editor de versões mais antigas do Visual Studio, mas destaca a sintaxe complexa de renderização que é partilhada por ambos.)
Lógica POST (Processar a Atualização):

Razor Pages: O OnPostAsync(int? id, string[] selectedCourses) recebe os dados. Ele primeiro carrega a entidade Instructor original (com .Include(i => i.Courses)). Em seguida, usa TryUpdateModelAsync para aplicar as alterações simples (como LastName e OfficeAssignment.Location). Finalmente, chama um método auxiliar (UpdateInstructorCourses) que recebe o array selectedCourses, compara-o com os cursos atuais do instrutor e adiciona ou remove manualmente as entidades da coleção instructor.Courses.
MVC: A Action Edit(int? id, string[] selectedCourses) [POST] faz exatamente a mesma coisa. Carrega, usa TryUpdateModelAsync, e chama o mesmo método auxiliar (UpdateInstructorCourses) para manipular a coleção N-N.
9.3. Análise de Conceitos-Chave da Atualização
TryUpdateModelAsync (ou TryUpdateModel):

Este método é a estrela desta secção. Como vimos no CRUD (4.4), ele é a forma mais segura de aplicar valores de um formulário a uma entidade já rastreada pelo DbContext (a que carregámos com FindAsync ou FirstOrDefaultAsync).
Ele previne overposting (excesso de postagem) e aplica de forma inteligente as alterações, marcando apenas as propriedades modificadas.
Transações (Implícitas):

A operação POST da edição do Instrutor altera múltiplas tabelas: Instructor (dados pessoais), OfficeAssignment (lotação) e a tabela de junção CourseInstructor (cursos).
O Entity Framework Core é inteligente o suficiente para saber disto. Quando await _context.SaveChangesAsync() é chamado, o EF Core automaticamente envolve todas estas operações numa única transação de base de dados.
Se a atualização do nome do Instructor funcionar, mas a adição de um Course falhar, a base de dados reverte (rollback) tudo. A atualização do nome não é guardada. Isto garante a integridade dos dados sem que precisemos de escrever código de transação manualmente.
ActionName:

Este atributo (mencionado nos conceitos) é usado em MVC quando o nome do método C# não corresponde ao nome da Action que o roteamento espera.
É mais comum na funcionalidade Delete, onde temos uma Action Delete(int id) [GET] e outra DeleteConfirmed(int id) [POST]. Para que a segunda Action responda ao POST de /Delete/5, usamos [HttpPost, ActionName("Delete")].
Em Razor Pages, isto não é necessário, pois os handlers já são nomeados por convenção (OnGet, OnPost).
10. Tratamento de Conflitos de Simultaneidade
Conflitos de simultaneidade ocorrem quando um utilizador exibe os dados de uma entidade para editá-los e, em seguida, outro utilizador atualiza os dados dessa mesma entidade antes que o primeiro utilizador salve as suas alterações.

10.1. Conceitos: Simultaneidade Otimista vs. Pessimista
Simultaneidade Pessimista (Bloqueio): Envolve bloquear os dados no momento em que são lidos, impedindo que outros utilizadores os alterem. O EF Core não oferece suporte interno para isto, e não é uma abordagem prática para aplicações web.
Simultaneidade Otimista (Deteção): Permite que qualquer pessoa leia os dados. No momento de salvar (SaveChanges()), o sistema deteta se os dados foram alterados por outra pessoa desde que foram lidos. Esta é a abordagem padrão para a web.
Estratégia "O Cliente Vence" (O último vence): As alterações do utilizador atual sobrescrevem quaisquer alterações anteriores. Este é o comportamento padrão do EF Core sem tratamento de simultaneidade e pode levar à perda de dados.
Estratégia "O Armazenamento Vence" (Store Wins): As alterações do utilizador atual são descartadas se a base de dados tiver sido alterada. O utilizador é notificado com uma mensagem de erro. Esta é a estratégia implementada por ambos os tutoriais.
10.2. A Solução (Comum a Ambos): O Token de Acompanhamento
Ambos os tutoriais implementam a simultaneidade otimista usando uma coluna de acompanhamento na base de dados.

Implementação no Modelo:

Como reutilizámos a pasta Models do projeto Razor Pages, a entidade Department (e outras) possui a propriedade:

[Timestamp]
public byte[] ConcurrencyToken { get; set; }
O atributo [Timestamp] (apesar do nome, que vem de versões antigas do SQL Server) mapeia esta propriedade para uma coluna rowversion no SQL Server. Esta coluna é atualizada automaticamente pela base de dados sempre que a linha sofre um UPDATE.

Isto também pode ser configurado via API Fluente no DbContext com .Property(p => p.ConcurrencyToken).IsRowVersion().

Fluxo de Deteção (Idêntico):

GET: A página/view de Edição carrega o Department e inclui o valor do ConcurrencyToken num campo oculto (<input type="hidden" asp-for="ConcurrencyToken" />).
POST: O formulário é enviado com os novos valores (ex: Budget) e o valor original do ConcurrencyToken (de quando a página foi carregada).
SaveChanges(): O EF Core gera um comando UPDATE que inclui o token na cláusula WHERE:
UPDATE Department SET Budget = @p0, ... WHERE DepartmentID = @p1 AND ConcurrencyToken = @p2
Conflito: Se outro utilizador (Alice) salvou alterações depois de o utilizador atual (Júlio) ter carregado a página, o ConcurrencyToken na base de dados será diferente do valor em @p2. O UPDATE falha em encontrar uma linha (0 linhas afetadas).
Exceção: O EF Core vê que 0 linhas foram afetadas (quando esperava 1) e lança uma DbUpdateConcurrencyException.
10.3. Implementação e Análise Comparativa (Tratando a Exceção)
Ambas as arquiteturas implementam a mesma lógica "Store Wins" dentro de um bloco try...catch.

Implementação em Razor Pages (Pages/Departments/Edit.cshtml.cs):

O OnPostAsync() envolve a lógica de salvamento num try...catch.
No catch (DbUpdateConcurrencyException ex):
Obtém a entrada com falha: var entry = ex.Entries.Single();
Obtém os valores atuais da base de dados: var databaseValues = entry.GetDatabaseValues();
Recarrega o ConcurrencyToken do PageModel com o novo valor da base de dados: entry.Property("ConcurrencyToken").OriginalValue = databaseValues["ConcurrencyToken"];
Ponto-chave: Usa ModelState.Remove("Department.ConcurrencyToken") para forçar o Tag Helper (asp-for) a usar o novo valor da propriedade do modelo, em vez de usar o valor antigo que veio no POST e está no ModelState.
Re-exibe a página com a mensagem de erro: return Page();.
Implementação em MVC (Controllers/DepartmentsController.cs):

A Action Edit(int id, byte[] concurrencyToken) [POST] envolve a lógica num try...catch.
No catch (DbUpdateConcurrencyException ex):
Faz o mesmo: obtém entry, databaseValues, etc.
Obtém o Department com os valores da base de dados.
Define o novo valor do token no modelo que será enviado de volta para a View: departmentFromDb.ConcurrencyToken = (byte[])databaseValues["ConcurrencyToken"];
Ponto-chave: Não precisa de ModelState.Remove. Ao passar o departmentFromDb atualizado para return View(departmentFromDb), o Tag Helper (asp-for) lê o valor diretamente deste novo objeto de modelo.
Re-exibe a View com a mensagem de erro: return View(departmentFromDb);.
Análise: A lógica de negócio é idêntica. A única diferença técnica é como cada arquitetura limpa o valor antigo do ConcurrencyToken do ModelState para evitar um loop de erros: Razor Pages manipula o ModelState diretamente (ModelState.Remove), enquanto o MVC passa um objeto de modelo completamente novo para a View.

10.4. O Desafio Prático: Model Binding (Análise Específica do Projeto)
Durante a implementação do MVC, surgiu um problema prático crucial que não existe no Razor Pages.

O Problema: O nosso modelo partilhado usa a propriedade ConcurrencyToken. A View MVC (Edit.cshtml) gera, portanto, um campo oculto com name="ConcurrencyToken". No entanto, o tutorial original do MVC sugere uma assinatura de Action [POST] como:
public async Task<IActionResult> Edit(int id, [Bind(...)] Department department, byte[] rowVersion)
A Falha: O Model Binder do MVC falhou. Ele via o valor ConcurrencyToken vindo do formulário, mas não encontrava nenhum parâmetro de Action chamado concurrencyToken. O parâmetro rowVersion permanecia null, e o [Bind] no department não incluía o token.
A Solução: A assinatura da Action [POST] do MVC teve que ser alterada para corresponder exatamente aos dados que chegam do formulário. A forma mais limpa (usada no tutorial de MVC) é adicionar o token como um parâmetro de método com o nome correto:
public async Task<IActionResult> Edit(int id, byte[] concurrencyToken)
(E, claro, usar concurrencyToken dentro do try...catch)
Conclusão Comparativa: Isto expõe uma diferença fundamental. O [BindProperty] do Razor Pages lida com o binding de todas as propriedades do modelo (incluindo o token) de forma mais automática. O MVC, ao usar parâmetros de Action e o atributo [Bind], exige uma correspondência manual mais explícita entre os nomes dos campos do formulário (definidos pelo asp-for no modelo) e os nomes dos parâmetros no método do Controller.
11. Padrões de Herança no Modelo de Dados
Embora esta secção seja exclusiva do tutorial de MVC, a implementação da herança no modelo de dados foi aplicada a ambos os projetos (MVC e Razor Pages) para manter a consistência da camada de dados partilhada.

11.1. Conceitos: Estratégias de Herança do EF Core
O EF Core suporta várias estratégias para mapear uma hierarquia de classes C# (ex: Person como classe base, Student e Instructor como classes derivadas) para uma base de dados SQL:

TPH (Tabela por Hierarquia): É a estratégia padrão e a demonstrada neste tutorial. Uma única tabela (ex: People) armazena os dados de todas as classes da hierarquia. Uma coluna especial, chamada "Discriminador", é usada para identificar a que classe (Student ou Instructor) cada linha pertence.
TPT (Tabela por Tipo): Cada classe (base e derivadas) tem a sua própria tabela. A tabela Students teria apenas colunas de Student e uma FK para a tabela People, que teria os campos comuns. (Suportado no EF Core 5+).
TPC (Tabela por Classe Concreta): Cada classe concreta (ex: Student e Instructor) tem a sua própria tabela completa, com todas as suas propriedades, incluindo as herdadas. A classe base (Person) não tem uma tabela.
11.2. O Desafio: Corrigindo uma Falha no Tutorial
Durante a implementação, foi identificada uma falha crítica no tutorial oficial do MVC, que mistura perigosamente as estratégias TPT e TPH:

A Dessincronização do Tutorial:

Configuração do DbContext: O tutorial instrui a manter mapeamentos de tabelas separados (modelBuilder.Entity<Student>().ToTable("Estudantes");, ...ToTable("Instrutores")). Isto configura o DbContext para esperar uma estratégia TPT (Tabela por Tipo).
Script de Migração: Em seguida, o tutorial manda substituir a migração automática por um script SQL manual que renomeia Instrutores para Pessoas, move os dados de Estudantes para Pessoas, adiciona uma coluna Discriminator e apaga a tabela Estudantes. Isto implementa manualmente uma estratégia TPH (Tabela por Hierarquia).
O Risco: Esta dessincronização quebraria a aplicação. O DbContext (pensando em TPT) tentaria fazer consultas a uma tabela Estudantes que o script de migração (forçando TPH) teria apagado.

11.3. A Solução (Aplicada em Ambos os Projetos)
Para implementar corretamente a herança TPH num banco de dados com dados existentes, a seguinte correção em duas etapas foi aplicada (de forma idêntica) em ambos os projetos:

Correção no DbContext (SchoolContext.cs):

Para alinhar o DbContext com a estratégia TPH, os mapeamentos de tabelas das classes filhas foram removidos do OnModelCreating.
Removido: modelBuilder.Entity<Student>().ToTable("Estudantes");
Removido: modelBuilder.Entity<Instructor>().ToTable("Instrutores");
Mantido: modelBuilder.Entity<Person>().ToTable("Pessoas"); (ou Person, dependendo da convenção).
Isto força o EF Core a entender que Student e Instructor são parte da tabela Pessoas.
Manutenção da Migração Manual (Migrations/..._Inheritance.cs):

Mesmo com o DbContext corrigido, a migração gerada automaticamente (com Add-Migration) apenas tentaria apagar a tabela Estudantes, causando perda total de dados.
Portanto, foi essencial manter o script de migração manual do tutorial. Este script atua como o "camião de mudanças", executando migrationBuilder.Sql(...) para mover os dados de Estudantes para a nova tabela Pessoas e corrigir as chaves estrangeiras (Enrollment) antes de, com segurança, apagar a tabela Estudantes original.
11.4. Análise Comparativa
Esta é a prova final da tese central deste comparativo: a camada de dados é agnóstica à UI.

Implementação Idêntica: A implementação deste padrão de herança complexo foi 100% idêntica em ambos os projetos.
Separação de Responsabilidades: Toda a lógica residiu exclusivamente na camada de dados (Models/, Data/SchoolContext.cs, Migrations/).
Transparência para a UI: A UI (seja o StudentsController do MVC ou o PageModel Students/Index do Razor Pages) continua a fazer a mesma consulta: _context.Students.ToListAsync(). Ela não precisa de saber que, "debaixo do capô", o EF Core está agora a consultar a tabela Pessoas e a filtrar por WHERE Discriminator = 'Student'.
12. Tópicos Avançados (Exclusivos do Tutorial MVC)
O tutorial de MVC conclui com uma secção de "Tópicos Avançados" que não tem um equivalente direto no tutorial de Razor Pages. Isto sugere que a Microsoft direciona o MVC para cenários que podem exigir uma "fuga" do alto nível de abstração do EF Core para otimizações de baixo nível.

No entanto, como esta análise demonstra, estas são funcionalidades do Entity Framework Core, não do MVC. Elas podem ser usadas de forma 100% idêntica num PageModel de Razor Pages.

12.1. Execução de Consultas SQL Brutas (Raw SQL)
O EF Core permite executar consultas SQL diretamente contra a base de dados, o que é útil para consultas complexas que o LINQ não consegue expressar facilmente.

O Desafio: Erros Históricos no Tutorial

Durante a implementação, foram descobertas várias falhas na documentação do tutorial, que mistura métodos obsoletos com modernos.
Erro em FromSql: O tutorial sugere _context.Departments.FromSql(query, id). Isto falha, pois (como a análise do código-fonte do EF Core provou) a sobrecarga de FromSql no EF Core 8 espera um argumento FormattableString (interpolação), enquanto a sobrecarga FromSqlRaw espera (string sql, params object[] parameters). O tutorial misturou o nome de um com os argumentos do outro.
Erro em ExecuteSqlCommandAsync: O tutorial sugere_context.Database.ExecuteSqlCommandAsync(...) para atualizações em massa. Este método está obsoleto desde o EF Core 3.0.
A Solução e Análise (Comum a Ambos):
A forma correta (e segura contra Injeção de SQL) de executar SQL bruto é através de consultas parametrizadas, que existem em dois sabores:

Método "Raw" (Placeholders):

Para Consultas (SELECT): FromSqlRaw("... WHERE ID = {0}", id)
Para Comandos (UPDATE): ExecuteSqlRawAsync("UPDATE ... SET Credits = Credits * {0}", multiplier)
Análise: Esta é a correção direta para o código do tutorial. Envia a consulta e os parâmetros separadamente para a base de dados, garantindo a segurança.
Método "Interpolado" (Preferido):

Para Consultas (SELECT): FromSql($"... WHERE ID = {id}")
Para Comandos (UPDATE): ExecuteSqlAsync($"UPDATE ... SET Credits = Credits * {multiplier}")
Análise: Esta é a forma moderna. O C# converte a string interpolada ($"") num FormattableString, que o EF Core usa para criar uma consulta parametrizada segura. FromSql é agora um alias para FromSqlInterpolated.
Conclusão Comparativa: A capacidade de executar SQL bruto é uma funcionalidade do DbContext, totalmente disponível e idêntica em ambas as arquiteturas.

12.2. O Risco do SQL Bruto: Contornar a Validação
Uma descoberta crucial ao implementar os comandos de atualização em massa (como ExecuteSqlRawAsync) é que eles contornam completamente as regras de negócio e validação definidas no modelo C#.

O Problema: O modelo Course tem o atributo [Range(0, 5)] para a propriedade Credits. No entanto, ao executar o comando ExecuteSqlRawAsync("UPDATE Cursos SET Credits = Credits * {0}", 3), os cursos com 3 créditos foram atualizados para 9.
Análise (Comum a Ambos):
Os atributos de validação ([Range], [Required], etc.) são verificados apenas quando o EF Core está a usar o seu Change Tracker (Detetor de Alterações), tipicamente durante um await _context.SaveChangesAsync().
Comandos SQL brutos (como ExecuteSql...) são enviados diretamente para a base de dados, "contornando" o DbContext. A base de dados não tem conhecimento das regras de atributos C#.
Conclusão: Esta é uma ferramenta poderosa que, se usada incorretamente, pode facilmente corromper a base de dados com "dados sujos" que violam as próprias regras da aplicação. A responsabilidade da validação é transferida para o programador.
12.3. LINQ Dinâmico (EF.Property)
O Problema: A lógica de ordenação na página Index de Estudantes (Secção 5.1) usa um grande e verboso bloco switch para traduzir o parâmetro sortOrder (uma string) numa expressão LINQ (ex: s => s.LastName). Adicionar uma nova coluna para ordenação exige a alteração deste switch.

A Solução (Comum a Ambos):

O tutorial de MVC introduz o método EF.Property<object>(entity, stringPropertyName).

Isto permite reescrever todo o bloco switch de forma dinâmica e extensível:

// Ex: sortOrder = "LastName"
students = students.OrderBy(e => EF.Property<object>(e, sortOrder));
Análise: Esta é uma otimização de código fantástica. A lógica de ordenação no Controller (MVC) ou PageModel (Razor Pages) torna-se universal. Para adicionar ordenação a uma nova coluna, basta modificar a View (.cshtml) para passar o nome correto da propriedade na string sortOrder, sem necessidade de alterar o código C# do backend.

Conclusão Comparativa: Embora apenas o tutorial de MVC o mostre, esta técnica EF.Property é uma funcionalidade do EF Core e pode (e deve) ser aplicada de forma idêntica ao PageModel Students/Index.cshtml.cs no projeto Razor Pages, para obter a mesma simplificação de código.

12.4. Outros Conceitos (Comuns a Ambos)
Performance (AutoDetectChangesEnabled): O tutorial menciona que, para operações em batch (lote) de muitas entidades, desativar temporariamente o detetor de alterações (_context.ChangeTracker.AutoDetectChangesEnabled = false;) pode dar um grande ganho de performance, evitando que o EF Core verifique por alterações desnecessariamente.

Engenharia Reversa (scaffold-dbcontext): É mencionado o fluxo de trabalho "Database First" (onde as classes C# são geradas a partir de uma base de dados existente).

Análise Final: Todos os "Tópicos Avançados" são, na verdade, tópicos do Entity Framework Core. A sua inclusão no tutorial de MVC e omissão no de Razor Pages reforça a perceção de que o MVC é a arquitetura "preferida" para cenários que exigem personalização de baixo nível, embora, tecnicamente, ambas as arquiteturas tenham acesso às mesmas ferramentas.

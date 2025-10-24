using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IfsAcademicSystem.RazorPages.Migrations
{
    /// <inheritdoc />
    public partial class Inheritance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // --- 1. Remover TODAS as Chaves Estrangeiras que apontam para as tabelas antigas ---

            // Chaves que apontam para "Instrutores"
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Instrutores_InstructorsID",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_Instrutores_InstructorID",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Escritorios_Instrutores_InstructorID",
                table: "Escritorios");

            // Chave que aponta para "Estudantes"
            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Estudantes_StudentID",
                table: "Matriculas");

            // Remover o índice antigo de "Matriculas"
            migrationBuilder.DropIndex(name: "IX_Matriculas_StudentID", table: "Matriculas");


            // --- 2. Renomear "Instrutores" para "Pessoas" e adicionar as colunas de "Estudantes" ---

            migrationBuilder.RenameTable(
                name: "Instrutores",
                newName: "Pessoas");

            // Adiciona a coluna para a data de matrícula (que só estudantes têm)
            migrationBuilder.AddColumn<DateTime>(name: "EnrollmentDate", table: "Pessoas", nullable: true);

            // Adiciona a coluna "Discriminator" que o TPH usa para saber quem é quem
            migrationBuilder.AddColumn<string>(name: "Discriminator", table: "Pessoas", nullable: false, maxLength: 128, defaultValue: "Instructor");

            // Torna a "HireDate" (Data de Contratação) nula, pois estudantes não a possuem
            migrationBuilder.AlterColumn<DateTime>(name: "HireDate", table: "Pessoas", nullable: true);

            // Coluna temporária para guardar os IDs antigos dos estudantes
            migrationBuilder.AddColumn<int>(name: "OldId", table: "Pessoas", nullable: true);


            // --- 3. A MÁGICA: Mover os dados de "Estudantes" para "Pessoas" usando SQL ---

            // Copia os dados de "Estudantes" para "Pessoas", definindo o Discriminator como 'Student'
            migrationBuilder.Sql("INSERT INTO dbo.Pessoas (LastName, FirstName, HireDate, EnrollmentDate, Discriminator, OldId) SELECT LastName, FirstName, null AS HireDate, EnrollmentDate, 'Student' AS Discriminator, ID AS OldId FROM dbo.Estudantes");

            // Corrige a tabela "Matriculas" para apontar para os NOVOS IDs dos estudantes na tabela "Pessoas"
            migrationBuilder.Sql("UPDATE dbo.Matriculas SET StudentID = (SELECT ID FROM dbo.Pessoas WHERE OldId = Matriculas.StudentID AND Discriminator = 'Student')");


            // --- 4. Limpeza e Recriação de Vínculos ---

            // Remove a coluna temporária
            migrationBuilder.DropColumn(name: "OldId", table: "Pessoas");

            // AGORA SIM: Apaga a tabela "Estudantes" antiga, que já está vazia
            migrationBuilder.DropTable(
                name: "Estudantes");

            // Recria o índice para a tabela "Matriculas"
            migrationBuilder.CreateIndex(
                 name: "IX_Matriculas_StudentID",
                 table: "Matriculas",
                 column: "StudentID");

            // Recria TODAS as Chaves Estrangeiras, agora apontando para a nova tabela "Pessoas"
            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Pessoas_InstructorsID",
                table: "CourseInstructor",
                column: "InstructorsID",
                principalTable: "Pessoas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_Pessoas_InstructorID",
                table: "Departamentos",
                column: "InstructorID",
                principalTable: "Pessoas",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Escritorios_Pessoas_InstructorID",
                table: "Escritorios",
                column: "InstructorID",
                principalTable: "Pessoas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Pessoas_StudentID",
                table: "Matriculas",
                column: "StudentID",
                principalTable: "Pessoas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // O método "Down" reverte a migração. O tutorial ignora isso, mas ele é 
            // perigoso porque mover dados de volta (de TPH para TPT) é complexo
            // e pode causar perda de dados ou falhas de chave primária.

            // 1. Remover todas as FKs que apontam para "Pessoas"
            migrationBuilder.DropForeignKey(
                name: "FK_CourseInstructor_Pessoas_InstructorsID",
                table: "CourseInstructor");

            migrationBuilder.DropForeignKey(
                name: "FK_Departamentos_Pessoas_InstructorID",
                table: "Departamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Escritorios_Pessoas_InstructorID",
                table: "Escritorios");

            migrationBuilder.DropForeignKey(
                name: "FK_Matriculas_Pessoas_StudentID",
                table: "Matriculas");

            // 2. Recriar a tabela "Estudantes" que foi apagada
            migrationBuilder.CreateTable(
                name: "Estudantes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnrollmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estudantes", x => x.ID);
                });

            // 3. Mover os dados de "Estudantes" DE VOLTA para a tabela "Estudantes"
            // ATENÇÃO: Isso pode falhar se os IDs tiverem mudado!
            // Para produção, isso exigiria "SET IDENTITY_INSERT dbo.Estudantes ON"
            migrationBuilder.Sql("INSERT INTO dbo.Estudantes (FirstName, LastName, EnrollmentDate) SELECT FirstName, LastName, EnrollmentDate FROM dbo.Pessoas WHERE Discriminator = 'Student'");

            // 4. Excluir os estudantes da tabela "Pessoas"
            migrationBuilder.Sql("DELETE FROM dbo.Pessoas WHERE Discriminator = 'Student'");

            // 5. Renomear "Pessoas" de volta para "Instrutores"
            migrationBuilder.RenameTable(
                name: "Pessoas",
                newName: "Instrutores");

            // 6. Remover as colunas TPH da tabela "Instrutores"
            migrationBuilder.DropColumn(name: "Discriminator", table: "Instrutores");
            migrationBuilder.DropColumn(name: "EnrollmentDate", table: "Instrutores");
            // Não removemos "OldId", pois ele já foi removido no "Up"

            // 7. Restaurar a coluna "HireDate" para "não nula"
            migrationBuilder.AlterColumn<DateTime>(
                name: "HireDate",
                table: "Instrutores",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            // 8. Recriar as FKs originais
            migrationBuilder.AddForeignKey(
                name: "FK_CourseInstructor_Instrutores_InstructorsID",
                table: "CourseInstructor",
                column: "InstructorsID",
                principalTable: "Instrutores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Departamentos_Instrutores_InstructorID",
                table: "Departamentos",
                column: "InstructorID",
                principalTable: "Instrutores",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Escritorios_Instrutores_InstructorID",
                table: "Escritorios",
                column: "InstructorID",
                principalTable: "Instrutores",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matriculas_Estudantes_StudentID",
                table: "Matriculas",
                column: "StudentID",
                principalTable: "Estudantes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
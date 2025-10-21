using IfsAcademicSystem.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IfsAcademicSystem.Mvc.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            if (context.Students.Any())
            {
                return;   // O banco de dados já foi inicializado
            }

            // --- INSTRUCTORES ---
            var instructorArquimedes = new Instructor { FirstMidName = "Arquimedes", LastName = "Medeiros", HireDate = DateTime.Parse("2010-08-20") };
            var instructorFlaygner = new Instructor { FirstMidName = "Flaygner", LastName = "Rebouças", HireDate = DateTime.Parse("2012-05-15") };
            var abercrombie = new Instructor { FirstMidName = "Kim", LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") };
            var fakhouri = new Instructor { FirstMidName = "Fadi", LastName = "Fakhouri", HireDate = DateTime.Parse("2002-07-06") };
            var harui = new Instructor { FirstMidName = "Roger", LastName = "Harui", HireDate = DateTime.Parse("1998-07-01") };
            var kapoor = new Instructor { FirstMidName = "Candace", LastName = "Kapoor", HireDate = DateTime.Parse("2001-01-15") };
            var zheng = new Instructor { FirstMidName = "Roger", LastName = "Zheng", HireDate = DateTime.Parse("2004-02-12") };

            context.Instructors.AddRange(instructorArquimedes, instructorFlaygner, abercrombie, fakhouri, harui, kapoor, zheng);
            context.SaveChanges();

            // --- DEPARTAMENTOS ---
            var bsiDepartment = new Department { Name = "Sistemas de Informação", Budget = 500000, StartDate = DateTime.Parse("2015-09-01"), Administrator = instructorArquimedes };
            var english = new Department { Name = "English", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Administrator = abercrombie };
            var mathematics = new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), Administrator = fakhouri };
            var engineering = new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), Administrator = harui };

            context.Departments.AddRange(bsiDepartment, english, mathematics, engineering);
            context.SaveChanges();

            // --- ESCRITÓRIOS ---
            var officeAssignments = new OfficeAssignment[]
            {
                 new OfficeAssignment { InstructorID = instructorArquimedes.ID, Location = "Sala dos Professores 1" },
                 new OfficeAssignment { InstructorID = instructorFlaygner.ID, Location = "Sala dos Professores 2" },
                 new OfficeAssignment { InstructorID = fakhouri.ID, Location = "Smith 17" },
                 new OfficeAssignment { InstructorID = harui.ID, Location = "Gowan 27" },
                 new OfficeAssignment { InstructorID = kapoor.ID, Location = "Thompson 304" }
            };
            context.OfficeAssignments.AddRange(officeAssignments);
            context.SaveChanges();

            // --- CURSOS ---
            var progWebII = new Course { CourseID = 4027, Title = "Programação Web II", Credits = 5, DepartmentID = bsiDepartment.DepartmentID };
            var sad = new Course { CourseID = 4028, Title = "Sistemas de Apoio a Decisão", Credits = 4, DepartmentID = bsiDepartment.DepartmentID };
            var govTI = new Course { CourseID = 4024, Title = "Governança em TI", Credits = 4, DepartmentID = bsiDepartment.DepartmentID };
            var chemistry = new Course { CourseID = 1050, Title = "Chemistry", Credits = 3, DepartmentID = engineering.DepartmentID };
            var microeconomics = new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = bsiDepartment.DepartmentID };
            var calculus = new Course { CourseID = 1045, Title = "Calculus", Credits = 4, DepartmentID = mathematics.DepartmentID };

            context.Courses.AddRange(progWebII, sad, govTI, chemistry, microeconomics, calculus);
            context.SaveChanges();

            // --- ASSOCIAÇÃO CURSO-INSTRUTOR ---
            progWebII.Instructors = new List<Instructor> { instructorArquimedes };
            sad.Instructors = new List<Instructor> { instructorArquimedes };
            govTI.Instructors = new List<Instructor> { instructorFlaygner };
            chemistry.Instructors = new List<Instructor> { kapoor, harui };
            microeconomics.Instructors = new List<Instructor> { zheng };
            calculus.Instructors = new List<Instructor> { fakhouri };
            context.SaveChanges();

            // --- ESTUDANTES ---
            var students = new Student[]
            {
                new Student { FirstMidName = "ABEL ELIZIARIO", LastName = "DE MELO NETO", EnrollmentDate = DateTime.Parse("2022-01-18") },
                new Student { FirstMidName = "ANSELMO BARBOSA", LastName = "DOS SANTOS", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "CARLOS DANIEL", LastName = "SANTOS SOUZA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "FLAVIA KARYNE", LastName = "SANTOS SENA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "GUILHERME OLIVEIRA", LastName = "DO NASCIMENTO", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "GUSTAVO DOS ANJOS", LastName = "FERREIRA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "GUSTAVO MURILO", LastName = "SANTOS BARBOSA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "IURY PATRICK", LastName = "GOIS SANTOS", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "JEFERSON DE SOUZA", LastName = "ANDRADE", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "JOSE GUSTAVO", LastName = "CORREIA NASCIMENTO", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "LUCAS VASCONCELOS", LastName = "EVANGELISTA", EnrollmentDate = DateTime.Parse("2021-01-20") },
                new Student { FirstMidName = "MARIANO NASCIMENTO", LastName = "SANTOS", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "MARIZA DE JESUS", LastName = "OLIVEIRA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "MARTA GABRIELLE", LastName = "DOS REIS MOURA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "MATHEUS LEMOS", LastName = "DO NASCIMENTO", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "MOACYR MAXIMO", LastName = "PEREIRA NETO", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "VIVIANE DE GOES", LastName = "DA SILVA", EnrollmentDate = DateTime.Parse("2023-01-15") },
                new Student { FirstMidName = "WEMERSON GABRIEL", LastName = "OLIVEIRA SANTOS", EnrollmentDate = DateTime.Parse("2022-01-18") },
                new Student { FirstMidName = "WILLIAM BATISTA", LastName = "COSTA", EnrollmentDate = DateTime.Parse("2022-01-18") },
                new Student { FirstMidName = "Carson", LastName = "Alexander", EnrollmentDate = DateTime.Parse("2016-09-01") },
                new Student { FirstMidName = "Meredith", LastName = "Alonso", EnrollmentDate = DateTime.Parse("2018-09-01") },
                new Student { FirstMidName = "Arturo", LastName = "Anand", EnrollmentDate = DateTime.Parse("2019-09-01") }
            };
            context.Students.AddRange(students);
            context.SaveChanges();

            // --- MATRÍCULAS ---
            var enrollments = new Enrollment[]
            {
                new Enrollment { StudentID = students.Single(s => s.LastName == "CORREIA NASCIMENTO").ID, CourseID = progWebII.CourseID, Grade = Grade.A },
                new Enrollment { StudentID = students.Single(s => s.LastName == "ANDRADE").ID, CourseID = progWebII.CourseID, Grade = Grade.A },
                new Enrollment { StudentID = students.Single(s => s.FirstMidName == "MARIANO NASCIMENTO").ID, CourseID = progWebII.CourseID, Grade = Grade.A },
                new Enrollment { StudentID = students.Single(s => s.LastName == "CORREIA NASCIMENTO").ID, CourseID = sad.CourseID, Grade = Grade.B },
                new Enrollment { StudentID = students.Single(s => s.LastName == "ANDRADE").ID, CourseID = govTI.CourseID },
                new Enrollment { StudentID = students.Single(s => s.FirstMidName == "MARIANO NASCIMENTO").ID, CourseID = sad.CourseID, Grade = Grade.C },
                new Enrollment { StudentID = students.Single(s => s.LastName == "Alexander").ID, CourseID = chemistry.CourseID, Grade = Grade.A },
                new Enrollment { StudentID = students.Single(s => s.LastName == "Alexander").ID, CourseID = microeconomics.CourseID, Grade = Grade.C },
                new Enrollment { StudentID = students.Single(s => s.LastName == "Alonso").ID, CourseID = calculus.CourseID, Grade = Grade.B },
            };

            context.Enrollments.AddRange(enrollments);
            context.SaveChanges();
        }
    }
}
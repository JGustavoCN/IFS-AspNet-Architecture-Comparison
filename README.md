# Análise Comparativa: ASP.NET Core MVC vs. Razor Pages no IFS

[cite_start]Este repositório é um projeto de estudo desenvolvido para a disciplina de Programação WEB II do Instituto Federal de Sergipe (IFS). O objetivo é implementar um sistema acadêmico de exemplo utilizando duas arquiteturas distintas da plataforma ASP.NET Core: **MVC (Model-View-Controller)** e **Razor Pages**.

O foco principal é a análise prática das diferenças estruturais, produtividade e casos de uso de cada abordagem, servindo como um item de portfólio e material de estudo.

---

## 🚀 Tecnologias Utilizadas

* .NET 8 (ou a versão utilizada)
* ASP.NET Core (MVC e Razor Pages)
* Entity Framework Core
* SQL Server LocalDB (ou SQLite)
* Bootstrap 5

---

## 📂 Estrutura do Repositório

* `/src/ContosoUniversity.RazorPages`: Implementação completa usando a arquitetura Razor Pages.
* `/src/ContosoUniversity.Mvc`: Implementação completa usando o padrão Model-View-Controller.
* [cite_start]`ANALYSIS.md`: Um relatório técnico detalhado que cumpre o requisito do descritivo da avaliação, comparando as duas abordagens.

---

## 📊 Análise Comparativa

Para uma análise aprofundada das diferenças, vantagens e desvantagens de cada arquitetura, com exemplos de código, **[leia o relatório completo clicando aqui](./ANALYSIS.md)**.

---

## 🛠️ Como Executar os Projetos

1. Clone este repositório: `git clone <URL_DO_SEU_REPOSITORIO>`
2. Navegue até a pasta de um dos projetos. Exemplo: `cd src/ContosoUniversity.RazorPages`
3. Execute as migrações do Entity Framework Core: `dotnet ef database update`
4. Execute o projeto: `dotnet run`
5. Abra o navegador no endereço indicado no terminal (ex: `https://localhost:7234`).

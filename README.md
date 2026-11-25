# Agenda de Consultas

Aplicação web em **ASP.NET Core MVC** para gerenciamento de médicos e consultas, desenvolvida como trabalho prático da disciplina de Desenvolvimento Web.

## Tema

**Agenda de consultas** — relacionamento 1:N entre `Médico` e `Consulta`.

## Integrantes

- Afonso
- Bernardo

## Descrição

O sistema permite cadastrar médicos e agendar consultas vinculadas a um médico. Os dados ficam armazenados **em memória** (listas estáticas), sem banco de dados — ao reiniciar a aplicação, os dados são perdidos.

### Funcionalidades

- CRUD completo de **Médicos** e **Consultas**
- Relacionamento 1:N (`Médico` → `Consultas`)
- Dropdown de médico no cadastro/edição de consulta
- Validações com Data Annotations (mensagens em português)
- Mensagens de feedback com `TempData`
- Busca/filtro nas listagens
- Ordenação ao clicar nos títulos das colunas
- Página inicial com resumo (totais e próximas consultas)

## Pré-requisitos

- [.NET SDK 8 ou superior](https://dotnet.microsoft.com/download) (projeto gerado com .NET 10)

## Como executar

```bash
dotnet restore
dotnet run
```

Abra no navegador o endereço indicado no terminal (geralmente `https://localhost:7xxx` ou `http://localhost:5xxx`).

## Estrutura principal

- `Models/` — `Medico`, `Consulta`, `StatusConsulta`
- `Data/RepositorioMemoria.cs` — armazenamento em memória
- `Controllers/` — `HomeController`, `MedicosController`, `ConsultasController`
- `Views/` — telas tipadas com Tag Helpers e Bootstrap

## Prints das telas

*(adicionar prints das principais telas aqui antes da entrega)*

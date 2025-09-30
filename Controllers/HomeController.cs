using System.Diagnostics;
using MatriculasEAD.Data;
using MatriculasEAD.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatriculasEAD.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Método para criar dados de exemplo (apenas para desenvolvimento)
        public async Task<IActionResult> CriarDadosExemplo()
        {
            try
            {
                // Verificar se já existem dados
                if (_context.Alunos.Any())
                {
                    TempData["Warning"] = "Dados de exemplo já existem no sistema.";
                    return RedirectToAction("Index");
                }

                // Criar alunos
                var alunos = new List<Aluno>
                {
                    new Aluno { Nome = "João Silva", Email = "joao@exemplo.com", Telefone = "(11) 99999-1111" },
                    new Aluno { Nome = "Maria Santos", Email = "maria@exemplo.com", Telefone = "(11) 99999-2222" },
                    new Aluno { Nome = "Pedro Oliveira", Email = "pedro@exemplo.com", Telefone = "(11) 99999-3333" },
                    new Aluno { Nome = "Ana Costa", Email = "ana@exemplo.com", Telefone = "(11) 99999-4444" }
                };

                await _context.Alunos.AddRangeAsync(alunos);
                await _context.SaveChangesAsync();

                // Criar cursos
                var cursos = new List<Curso>
                {
                    new Curso 
                    { 
                        Titulo = "Programação C#", 
                        Descricao = "Curso completo de C# e .NET Core", 
                        PrecoBase = 299.90m, 
                        CargaHoraria = 40
                    },
                    new Curso 
                    { 
                        Titulo = "ASP.NET Core MVC", 
                        Descricao = "Desenvolvimento web com ASP.NET Core MVC", 
                        PrecoBase = 399.90m, 
                        CargaHoraria = 60
                    },
                    new Curso 
                    { 
                        Titulo = "Entity Framework Core", 
                        Descricao = "ORM para .NET - Mapeamento objeto-relacional", 
                        PrecoBase = 199.90m, 
                        CargaHoraria = 30
                    },
                    new Curso 
                    { 
                        Titulo = "Blazor WebAssembly", 
                        Descricao = "Criando SPAs com Blazor", 
                        PrecoBase = 349.90m, 
                        CargaHoraria = 45
                    }
                };

                await _context.Cursos.AddRangeAsync(cursos);
                await _context.SaveChangesAsync();

                // Criar matrículas
                var matriculas = new List<Matricula>
                {
                    new Matricula 
                    { 
                        AlunoId = alunos[0].Id, 
                        CursoId = cursos[0].Id, 
                        Data = DateTime.Today.AddDays(-30), 
                        PrecoPago = 299.90m, 
                        Status = StatusMatricula.Ativa, 
                        Progresso = 75 
                    },
                    new Matricula 
                    { 
                        AlunoId = alunos[1].Id, 
                        CursoId = cursos[1].Id, 
                        Data = DateTime.Today.AddDays(-15), 
                        PrecoPago = 399.90m, 
                        Status = StatusMatricula.Concluida, 
                        Progresso = 100,
                        NotaFinal = 9.5m
                    },
                    new Matricula 
                    { 
                        AlunoId = alunos[2].Id, 
                        CursoId = cursos[2].Id, 
                        Data = DateTime.Today.AddDays(-5), 
                        PrecoPago = 199.90m, 
                        Status = StatusMatricula.Ativa, 
                        Progresso = 25 
                    },
                    new Matricula 
                    { 
                        AlunoId = alunos[0].Id, 
                        CursoId = cursos[1].Id, 
                        Data = DateTime.Today.AddDays(-20), 
                        PrecoPago = 399.90m, 
                        Status = StatusMatricula.Ativa, 
                        Progresso = 50 
                    },
                    new Matricula 
                    { 
                        AlunoId = alunos[3].Id, 
                        CursoId = cursos[3].Id, 
                        Data = DateTime.Today.AddDays(-2), 
                        PrecoPago = 349.90m, 
                        Status = StatusMatricula.Ativa, 
                        Progresso = 10 
                    }
                };

                await _context.Matriculas.AddRangeAsync(matriculas);
                await _context.SaveChangesAsync();

                TempData["Success"] = $"Dados de exemplo criados com sucesso! {alunos.Count} alunos, {cursos.Count} cursos e {matriculas.Count} matrículas foram adicionados.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Erro ao criar dados de exemplo: {ex.Message}. Detalhes: {ex.InnerException?.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}

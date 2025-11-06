using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MatriculasEAD.Data;
using MatriculasEAD.Models;

namespace MatriculasEAD.Controllers
{
    public class MatriculasController : Controller
    {
        private readonly AppDbContext _context;

        public MatriculasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Matriculas
        public async Task<IActionResult> Index(string searchString, StatusMatricula? status)
        {
            try
            {
                // Verificar se o contexto está funcionando
                var totalMatriculas = await _context.Matriculas.CountAsync();
                ViewBag.TotalMatriculas = totalMatriculas;

                // Buscar matrículas com includes
                var query = _context.Matriculas
                    .Include(m => m.Aluno)
                    .Include(m => m.Curso)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchString))
                {
                    query = query.Where(m => 
                        m.Aluno.Nome.Contains(searchString) || 
                        m.Curso.Titulo.Contains(searchString));
                }

                if (status.HasValue)
                {
                    query = query.Where(m => m.Status == status.Value);
                }

                var matriculas = await query
                    .OrderBy(m => m.Data)
                    .ThenBy(m => m.Aluno.Nome)
                    .ToListAsync();

                ViewBag.SearchString = searchString;
                ViewBag.StatusFilter = status;
                ViewBag.StatusList = new SelectList(
                    Enum.GetValues(typeof(StatusMatricula)).Cast<StatusMatricula>()
                        .Select(s => new { Value = s, Text = s.ToString() }), 
                    "Value", "Text", status);

                return View(matriculas);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Erro ao carregar matrículas: {ex.Message}";
                return View(new List<Matricula>());
            }
        }

        // GET: Matriculas/Details/alunoId/cursoId
        public async Task<IActionResult> Details(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
                
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // GET: Matriculas/Create
        public async Task<IActionResult> Create(int? alunoId, int? cursoId)
        {
            var alunos = await _context.Alunos.OrderBy(a => a.Nome).ToListAsync();
            var cursos = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync();

            ViewData["AlunoId"] = new SelectList(alunos, "Id", "Nome", alunoId);
            ViewData["CursoId"] = new SelectList(cursos, "Id", "Titulo", cursoId);

            var matricula = new Matricula
            {
                Data = DateTime.Today, // Usar apenas a data, sem horário
                Status = StatusMatricula.Ativa,
                Progresso = 0 // Começar com progresso 0
            };

            if (alunoId.HasValue)
                matricula.AlunoId = alunoId.Value;
            if (cursoId.HasValue)
            {
                matricula.CursoId = cursoId.Value;
                var curso = await _context.Cursos.FindAsync(cursoId.Value);
                if (curso != null)
                    matricula.PrecoPago = curso.PrecoBase;
            }

            return View(matricula);
        }

        // POST: Matriculas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AlunoId,CursoId,Data,PrecoPago,Status,Progresso,NotaFinal")] Matricula matricula)
        {
            // Remover validação automática do modelo para os campos de navegação
            ModelState.Remove("Aluno");
            ModelState.Remove("Curso");

            // Log para debug - INÍCIO
            Console.WriteLine("=======================================================");
            Console.WriteLine($"[DEBUG] POST Create chamado!");
            Console.WriteLine($"[DEBUG] AlunoId: {matricula.AlunoId}");
            Console.WriteLine($"[DEBUG] CursoId: {matricula.CursoId}");
            Console.WriteLine($"[DEBUG] Data: {matricula.Data}");
            Console.WriteLine($"[DEBUG] PrecoPago: {matricula.PrecoPago}");
            Console.WriteLine($"[DEBUG] Status: {matricula.Status}");
            Console.WriteLine($"[DEBUG] Progresso: {matricula.Progresso}");
            Console.WriteLine($"[DEBUG] NotaFinal: {matricula.NotaFinal}");
            Console.WriteLine($"[DEBUG] NotaFinal: {matricula.NotaFinal}");
            Console.WriteLine("=======================================================");
            
            // Verificar se aluno e curso foram selecionados
            Console.WriteLine($"[DEBUG] Verificando se AlunoId e CursoId são válidos...");
            if (matricula.AlunoId == 0)
            {
                Console.WriteLine($"[ERROR] AlunoId é 0 - inválido!");
                ModelState.AddModelError("AlunoId", "Por favor, selecione um aluno.");
            }
            else
            {
                Console.WriteLine($"[DEBUG] AlunoId válido: {matricula.AlunoId}");
            }
            
            if (matricula.CursoId == 0)
            {
                Console.WriteLine($"[ERROR] CursoId é 0 - inválido!");
                ModelState.AddModelError("CursoId", "Por favor, selecione um curso.");
            }
            else
            {
                Console.WriteLine($"[DEBUG] CursoId válido: {matricula.CursoId}");
            }
            
            // Verificar se já existe uma matrícula para este aluno e curso
            if (matricula.AlunoId > 0 && matricula.CursoId > 0)
            {
                Console.WriteLine($"[DEBUG] Verificando se já existe matrícula para Aluno {matricula.AlunoId} e Curso {matricula.CursoId}...");
                var existeMatricula = await _context.Matriculas
                    .AnyAsync(m => m.AlunoId == matricula.AlunoId && m.CursoId == matricula.CursoId);

                if (existeMatricula)
                {
                    Console.WriteLine($"[ERROR] Matrícula duplicada encontrada!");
                    ModelState.AddModelError("", "Este aluno já está matriculado neste curso.");
                }
                else
                {
                    Console.WriteLine($"[DEBUG] Nenhuma matrícula duplicada encontrada. OK!");
                }
            }

            // Validar NotaFinal apenas se foi fornecida
            if (matricula.NotaFinal.HasValue && matricula.NotaFinal.Value > 0)
            {
                if (matricula.Progresso < 100)
                {
                    ModelState.AddModelError("NotaFinal", "A nota final só pode ser definida quando o progresso for 100%.");
                }
            }

            // Validar Status Concluída
            if (matricula.Status == StatusMatricula.Concluida && matricula.Progresso != 100)
            {
                ModelState.AddModelError("Status", "Para marcar como 'Concluída', o progresso deve ser 100%.");
            }

            // Se nenhum status foi selecionado, usar Ativa como padrão
            if (!Enum.IsDefined(typeof(StatusMatricula), matricula.Status))
            {
                matricula.Status = StatusMatricula.Ativa;
            }

            Console.WriteLine($"[DEBUG] Verificando ModelState.IsValid...");
            Console.WriteLine($"[DEBUG] ModelState.IsValid = {ModelState.IsValid}");
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Garantir que a data seja válida
                    if (matricula.Data == default(DateTime))
                    {
                        matricula.Data = DateTime.Today;
                        Console.WriteLine($"[DEBUG] Data era default, definida como: {matricula.Data}");
                    }

                    Console.WriteLine($"[DEBUG] ModelState válido! Preparando para salvar...");
                    Console.WriteLine($"[DEBUG] Adicionando matrícula ao contexto...");
                    
                    _context.Add(matricula);
                    
                    Console.WriteLine($"[DEBUG] Chamando SaveChangesAsync...");
                    var result = await _context.SaveChangesAsync();
                    
                    Console.WriteLine($"[SUCCESS] ✓ Matrícula salva com sucesso! Linhas afetadas: {result}");
                    Console.WriteLine("=======================================================");
                    
                    TempData["Success"] = "Matrícula criada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    var errorMessage = $"Erro ao salvar a matrícula: {ex.InnerException?.Message ?? ex.Message}";
                    Console.WriteLine($"[ERROR] ✗ {errorMessage}");
                    Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                    Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"[ERROR] Inner Exception: {ex.InnerException.GetType().Name}");
                        Console.WriteLine($"[ERROR] Inner Message: {ex.InnerException.Message}");
                    }
                    Console.WriteLine("=======================================================");
                    ModelState.AddModelError("", errorMessage);
                }
                catch (Exception ex)
                {
                    var errorMessage = $"Erro inesperado: {ex.Message}";
                    Console.WriteLine($"[ERROR] ✗ {errorMessage}");
                    Console.WriteLine($"[ERROR] Exception Type: {ex.GetType().Name}");
                    Console.WriteLine($"[ERROR] Stack: {ex.StackTrace}");
                    Console.WriteLine("=======================================================");
                    ModelState.AddModelError("", errorMessage);
                }
            }
            else
            {
                Console.WriteLine("[ERROR] ✗ ModelState INVÁLIDO!");
                Console.WriteLine("[DEBUG] Erros encontrados:");
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state != null && state.Errors.Count > 0)
                    {
                        foreach (var error in state.Errors)
                        {
                            Console.WriteLine($"[ERROR]   - Campo '{key}': {error.ErrorMessage}");
                            if (error.Exception != null)
                            {
                                Console.WriteLine($"[ERROR]     Exception: {error.Exception.Message}");
                            }
                        }
                    }
                }
                Console.WriteLine("=======================================================");
            }

            var alunos = await _context.Alunos.OrderBy(a => a.Nome).ToListAsync();
            var cursos = await _context.Cursos.OrderBy(c => c.Titulo).ToListAsync();
            ViewData["AlunoId"] = new SelectList(alunos, "Id", "Nome", matricula.AlunoId);
            ViewData["CursoId"] = new SelectList(cursos, "Id", "Titulo", matricula.CursoId);
            return View(matricula);
        }

        // GET: Matriculas/Edit/alunoId/cursoId
        public async Task<IActionResult> Edit(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
                
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matriculas/Edit/alunoId/cursoId
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int alunoId, int cursoId, [Bind("AlunoId,CursoId,Data,PrecoPago,Status,Progresso,NotaFinal")] Matricula matricula)
        {
            if (alunoId != matricula.AlunoId || cursoId != matricula.CursoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matricula);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Matrícula atualizada com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatriculaExists(matricula.AlunoId, matricula.CursoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Erro ao atualizar a matrícula. Tente novamente.");
                }
            }

            matricula.Aluno = await _context.Alunos.FindAsync(alunoId) ?? new Aluno();
            matricula.Curso = await _context.Cursos.FindAsync(cursoId) ?? new Curso();
            return View(matricula);
        }

        // GET: Matriculas/Delete/alunoId/cursoId
        public async Task<IActionResult> Delete(int? alunoId, int? cursoId)
        {
            if (alunoId == null || cursoId == null)
            {
                return NotFound();
            }

            var matricula = await _context.Matriculas
                .Include(m => m.Aluno)
                .Include(m => m.Curso)
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
                
            if (matricula == null)
            {
                return NotFound();
            }

            return View(matricula);
        }

        // POST: Matriculas/Delete/alunoId/cursoId
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int alunoId, int cursoId)
        {
            var matricula = await _context.Matriculas
                .FirstOrDefaultAsync(m => m.AlunoId == alunoId && m.CursoId == cursoId);
                
            if (matricula != null)
            {
                try
                {
                    _context.Matriculas.Remove(matricula);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Matrícula excluída com sucesso!";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Erro ao excluir a matrícula. Tente novamente.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MatriculaExists(int alunoId, int cursoId)
        {
            return _context.Matriculas.Any(e => e.AlunoId == alunoId && e.CursoId == cursoId);
        }

        // Método de diagnóstico para verificar conexão com o banco
        public async Task<IActionResult> Diagnostico()
        {
            try
            {
                var canConnect = await _context.Database.CanConnectAsync();
                var totalAlunos = await _context.Alunos.CountAsync();
                var totalCursos = await _context.Cursos.CountAsync();
                var totalMatriculas = await _context.Matriculas.CountAsync();

                return Json(new
                {
                    ConexaoOK = canConnect,
                    TotalAlunos = totalAlunos,
                    TotalCursos = totalCursos,
                    TotalMatriculas = totalMatriculas,
                    Erro = (string?)null
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ConexaoOK = false,
                    TotalAlunos = 0,
                    TotalCursos = 0,
                    TotalMatriculas = 0,
                    Erro = ex.Message + (ex.InnerException != null ? $" | Inner: {ex.InnerException.Message}" : "")
                });
            }
        }
    }
}

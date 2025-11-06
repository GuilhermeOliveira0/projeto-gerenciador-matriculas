using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatriculasEAD.Data;
using MatriculasEAD.Models;

namespace MatriculasEAD.Controllers
{
    public class AlunoesController : Controller
    {
        private readonly AppDbContext _context;

        public AlunoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Alunoes
        public async Task<IActionResult> Index(string searchString)
        {
            var alunos = _context.Alunos.Include(a => a.Matriculas).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                alunos = alunos.Where(a => a.Nome.Contains(searchString) || 
                                          a.Email.Contains(searchString));
            }

            ViewBag.SearchString = searchString;
            return View(await alunos.OrderBy(a => a.Nome).ToListAsync());
        }

        // GET: Alunoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.Matriculas)
                    .ThenInclude(m => m.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // GET: Alunoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Alunoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone")] Aluno aluno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(aluno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Aluno criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("IX_Alunos_Email") == true)
                    {
                        ModelState.AddModelError("Email", "Este email já está sendo usado por outro aluno.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao salvar o aluno. Tente novamente.");
                    }
                }
            }
            return View(aluno);
        }

        // GET: Alunoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }
            return View(aluno);
        }

        // POST: Alunoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone")] Aluno aluno)
        {
            if (id != aluno.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aluno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Aluno atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlunoExists(aluno.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException ex)
                {
                    if (ex.InnerException?.Message.Contains("IX_Alunos_Email") == true)
                    {
                        ModelState.AddModelError("Email", "Este email já está sendo usado por outro aluno.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Erro ao atualizar o aluno. Tente novamente.");
                    }
                }
            }
            return View(aluno);
        }

        // GET: Alunoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var aluno = await _context.Alunos
                .Include(a => a.Matriculas)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (aluno == null)
            {
                return NotFound();
            }

            return View(aluno);
        }

        // POST: Alunoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var aluno = await _context.Alunos
                .Include(a => a.Matriculas)
                .FirstOrDefaultAsync(a => a.Id == id);
                
            if (aluno != null)
            {
                if (aluno.Matriculas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir este aluno pois ele possui matrículas.";
                    return RedirectToAction(nameof(Index));
                }
                
                try
                {
                    _context.Alunos.Remove(aluno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Aluno excluído com sucesso!";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Erro ao excluir o aluno. Verifique se não há matrículas associadas.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AlunoExists(int id)
        {
            return _context.Alunos.Any(e => e.Id == id);
        }
    }
}

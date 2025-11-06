using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MatriculasEAD.Data;
using MatriculasEAD.Models;

namespace MatriculasEAD.Controllers
{
    public class CursoesController : Controller
    {
        private readonly AppDbContext _context;

        public CursoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cursoes
        public async Task<IActionResult> Index(string searchString)
        {
            var cursos = _context.Cursos.Include(c => c.Matriculas).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                cursos = cursos.Where(c => c.Titulo.Contains(searchString) || 
                                          (c.Descricao != null && c.Descricao.Contains(searchString)));
            }

            ViewBag.SearchString = searchString;
            return View(await cursos.OrderBy(c => c.Titulo).ToListAsync());
        }

        // GET: Cursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.Matriculas)
                    .ThenInclude(m => m.Aluno)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cursoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Descricao,PrecoBase,CargaHoraria")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(curso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Curso criado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Erro ao salvar o curso. Tente novamente.");
                }
            }
            return View(curso);
        }

        // GET: Cursoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        // POST: Cursoes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Descricao,PrecoBase,CargaHoraria")] Curso curso)
        {
            if (id != curso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(curso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Curso atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CursoExists(curso.Id))
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
                    ModelState.AddModelError("", "Erro ao atualizar o curso. Tente novamente.");
                }
            }
            return View(curso);
        }

        // GET: Cursoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Cursos
                .Include(c => c.Matriculas)
                .FirstOrDefaultAsync(m => m.Id == id);
                
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // POST: Cursoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var curso = await _context.Cursos
                .Include(c => c.Matriculas)
                .FirstOrDefaultAsync(c => c.Id == id);
                
            if (curso != null)
            {
                if (curso.Matriculas.Any())
                {
                    TempData["ErrorMessage"] = "Não é possível excluir este curso pois há alunos matriculados.";
                    return RedirectToAction(nameof(Index));
                }
                
                try
                {
                    _context.Cursos.Remove(curso);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Curso excluído com sucesso!";
                }
                catch (DbUpdateException)
                {
                    TempData["ErrorMessage"] = "Erro ao excluir o curso. Verifique se não há matrículas associadas.";
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool CursoExists(int id)
        {
            return _context.Cursos.Any(e => e.Id == id);
        }
    }
}

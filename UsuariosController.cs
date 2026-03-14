using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace pruebaTecnica.Controllers;

public class UsuariosController : Controller
{
	private readonly ApplicationDbContext _context;

	public UsuariosController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: /Usuarios
	public async Task<IActionResult> Index()
	{
		var usuarios = await _context.Usuarios.ToListAsync();
		return View(usuarios);
	}

	// GET: /Usuarios/Create
	public IActionResult Create()
	{
		return View();
	}

	// POST: /Usuarios/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(Usuario usuario)
	{
		if (!ModelState.IsValid)
		{
			return View(usuario);
		}

		_context.Add(usuario);
		await _context.SaveChangesAsync();
		return RedirectToAction(nameof(Index));
	}
	// GET: /Usuarios/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var usuario = await _context.Usuarios.FindAsync(id);
		if (usuario is null)
		{
			return NotFound();
		}

		return View(usuario);
	}

	// POST: /Usuarios/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, Usuario usuario)
	{
		if (id != usuario.Id)
		{
			return NotFound();
		}

		if (!ModelState.IsValid)
		{
			return View(usuario);
		}

		try
		{
			_context.Update(usuario);
			await _context.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!UsuarioExiste(usuario.Id))
			{
				return NotFound();
			}

			throw;
		}

		return RedirectToAction(nameof(Index));
	}

	// GET: /Usuarios/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id is null)
		{
			return NotFound();
		}

		var usuario = await _context.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
		if (usuario is null)
		{
			return NotFound();
		}

		return View(usuario);
	}

	// POST: /Usuarios/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var usuario = await _context.Usuarios.FindAsync(id);
		if (usuario is not null)
		{
			_context.Usuarios.Remove(usuario);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}

	private bool UsuarioExiste(int id)
	{
		return _context.Usuarios.Any(x => x.Id == id);
	}
    
}

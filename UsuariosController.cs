using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace pruebaTecnica.Controllers;

public class UsuariosController : Controller
{
	// DbContext inyectado para acceder a la tabla de usuarios con EF Core.
	private readonly ApplicationDbContext _context;

	public UsuariosController(ApplicationDbContext context)
	{
		_context = context;
	}

	// GET: /Usuarios
	public async Task<IActionResult> Index()
	{
		// Obtiene todos los usuarios registrados para mostrarlos en la vista.
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
		// Si hay errores de validacion, se regresa al formulario con los datos capturados.
		if (!ModelState.IsValid)
		{
			return View(usuario);
		}

		// Inserta el nuevo usuario y persiste cambios en base de datos.
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
		// Proteccion contra manipulacion del id en la ruta/formulario.
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
			// Si otro proceso elimino el registro, se responde 404.
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
			// Solo elimina si el usuario aun existe en la base de datos.
			_context.Usuarios.Remove(usuario);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}

	private bool UsuarioExiste(int id)
	{
		// Helper para validaciones de concurrencia en la actualizacion.
		return _context.Usuarios.Any(x => x.Id == id);
	}
    
}

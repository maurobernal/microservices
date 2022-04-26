using api.Entities;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace api.Controllers;

[ApiController]
[Route("[controller]")]
public class PostController : Controller
{
    private IAppDBContext _context;
    public PostController(IAppDBContext context) => _context = context;

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var res = await _context.Post.Where(w => w.Habilitado == true).ToListAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }


    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(Guid Id)
    {
        var res = await _context.Post.Where(w => w.ID == Id).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }


    [HttpPost]
    public async Task<IActionResult> Post(Post post)
{
        var cat = _context.Categorias.Where(w => w.ID == post.CategoriaId).AsNoTracking().FirstOrDefault();
        if (cat == null) return NotFound($"La categoria no existe:{post.CategoriaId}");
        post.Categoria = null;
        _context.Post.Add(post);
     
        await _context.SaveChangesAsync();
        return Ok(post);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Put(Post category, Guid Id)
    {
        if (Id != category.ID) throw new Exception();

        var res = await _context.Post.Where(w => w.ID == Id).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        res.Titulo = category.Titulo;
        res.Contenido = category.Contenido;
        res.CategoriaId = category.CategoriaId;
        res.Habilitado = category.Habilitado;
        await _context.SaveChangesAsync();

        return Ok(res);



    }


    [HttpDelete("Id")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var res = await _context.Post.Where(w => w.ID == Id).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        _context.Post.Remove(res);
        await _context.SaveChangesAsync();
        return Ok(Id);

    }
}

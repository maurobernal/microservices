using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using api.Interfaces;
namespace api.Controllers;


[ApiController]
[Route("[controller]")]
public class CategoriasController : Controller
{
    private IAppDBContext _context;
    public CategoriasController(IAppDBContext context)=>   _context = context;
    

    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var res = await _context.Categorias.Where(w => w.Habilitado == true).ToListAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }



    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(Guid Id) {
        var res = await _context.Categorias.Where(w => w.ID == Id).AsNoTracking().FirstOrDefaultAsync();
        if (res == null) return NotFound();
        return Ok(res);
    }


    [HttpPost]
    public async Task<IActionResult> Post(Categorias category)
    {
       
        _context.Categorias.Add(category);
        await _context.SaveChangesAsync();
        return Ok(category);
    }

    [HttpPut("{Id}")]
    public async Task<IActionResult> Put(Categorias category, Guid Id) 
        {
        if (Id != category.ID) throw new Exception();

        var res = await _context.Categorias.Where(w => w.ID == Id).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        res.Descripcion = category.Descripcion;
        res.Habilitado = category.Habilitado;
        await _context.SaveChangesAsync();

        return Ok(res);



    }


    [HttpDelete("Id")]
    public async Task<IActionResult> Delete(Guid Id)
    {
        var res = await _context.Categorias.Where(w => w.ID == Id).FirstOrDefaultAsync();
        if (res == null) return NotFound();
        _context.Categorias.Remove(res);
        await _context.SaveChangesAsync();
        return Ok(Id);

    }


}

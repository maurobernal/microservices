using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using api.Entities;
using api.Interfaces;
using api.Models;
using AutoMapper;
using Mapster;

namespace api.Controllers;


[ApiController]
[Route("[controller]")]
public class CategoriasController : Controller
{
    private IAppDBContext _context;
    private IMapper _mapper;

    public CategoriasController(IMapper mapper, IAppDBContext context)
    {
        _mapper = mapper;
        _context = context;
    }
    
    




    [HttpGet()]
    public async Task<IActionResult> GetAll()
    {
        var res = await _context.Categorias.Where(w => w.Habilitado == true).ToListAsync();
        if (res == null) return NotFound();

        //_mapper.Map<Ca<(res)
        //List<CategoriaModels> C = new();
        
        return Ok(res);
    }








    [HttpGet("{Id}")]
    public async Task<IActionResult> Get(Guid Id) {
        var res = await _context.Categorias.Where(w => w.ID == Id).AsNoTracking().FirstOrDefaultAsync();

        
        if (res == null) return NotFound();

        CategoriaModels C = _mapper.Map<CategoriaModels>(res);

      
        
        return Ok(C);
    }


    [HttpGet("{Id}/mapster")]
    public async Task<IActionResult> GetMapster(Guid Id)
    {
        var res = await _context.Categorias.Where(w => w.ID == Id).AsNoTracking().FirstOrDefaultAsync();
        if (res == null) return NotFound();
        
        
        TypeAdapterConfig<Categorias, CategoriaModels>
        .NewConfig()
        .Map(d => d.campoDescripcion, o => o.Descripcion)
        .Map(d => d.campoID, o => o.ID);



        CategoriaModels C = res.Adapt<CategoriaModels>();


        return Ok(C);
    }













    [HttpPost]
    public async Task<IActionResult> Post(CategoriaModels categorymodel)
    {

        Categorias category = _mapper.Map<Categorias>(categorymodel);

        category.Descripcion = categorymodel.campoDescripcion;
        category.FechaModificacion = DateTime.Now;
        category.FechaCreacion = DateTime.Now;
        category.Habilitado = true;

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

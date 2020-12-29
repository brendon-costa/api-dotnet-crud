using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

// https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context)
    {
        var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        return Ok(category);
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post(
        [FromBody]Category category,
        [FromServices]DataContext context
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch
        {
            return BadRequest(new {message = "Não foi possível criar a categoria"});
        }
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>>  Put(
        int id, 
        [FromBody]Category category, 
        [FromServices]DataContext context
    )
    {
        if(category.Id != id)
            return NotFound(new {message = "Categoria não encontrada"});

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        try {
            context.Entry<Category>(category).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(category);
        }
        catch(DbUpdateConcurrencyException)
        {
            return BadRequest(new {message = "Este regritos já foi atualizado"});
        }
        catch(Exception)
        {
            return BadRequest(new {message = "Não foi possível atualizar a categoria"});
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Delete(
        int id,
        [FromServices]DataContext context
    )
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if(category == null)
            return NotFound(new {message = "Categoria não encontrada"});
        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new {message = "Categoria removida com sucesso"});
        }
        catch (Exception) {
            return BadRequest(new {message = "Não foi possível remover a categoria"});
        }
    }
}
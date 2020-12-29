using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get()
    {
        return new List<Category>();
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        return new Category();
    }

    [HttpPost]
    [Route("")]
    public async Task<ActionResult<Category>> Post([FromBody]Category category)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(category);
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>>  Put(int id, [FromBody]Category category)
    {
        if(category.Id != id)
            return NotFound(new {message = "Categoria não encontrada"});

        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        return Ok(category);
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> Delete()
    {
        return Ok();
    }
}
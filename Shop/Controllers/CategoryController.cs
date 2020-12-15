using Microsoft.AspNetCore.Mvc;
using Shop.Models;

// https://localhost:5001/categories
[Route("categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    public string Get()
    {
        return "Get";
    }

    [HttpGet]
    [Route("{id:int}")]
    public string GetById(int id)
    {
        return "GetById " + id;
    }

    [HttpPost]
    [Route("")]
    public Category Post([FromBody]Category category)
    {
        return category;
    }

    [HttpPut]
    [Route("{id:int}")]
    public Category Put(int id, [FromBody]Category category)
    {
        if(category.Id == id)
            return category;
        
        return null;
    }

    [HttpDelete]
    [Route("{id:int}")]
    public string Delete()
    {
        return "Delete";
    }
}
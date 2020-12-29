using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;


namespace Shop.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get([FromServices]DataContext context)
        {
            var products = await context
                .Products
                .AsNoTracking()
                .ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id, [FromServices]DataContext context)
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return Ok(product);
        }

        [HttpGet]
        [Route("categories/{id:int}")]
        public async Task<ActionResult<Product>> GetByCategory(int id, [FromServices]DataContext context)
        {
            var product = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x => x.Category.Id == id)
                .ToListAsync();
            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromBody]Product product,
            [FromServices]DataContext context
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                return Ok(product);
            }
            catch
            {
                return BadRequest(new {message = "Não foi possível criar um produto"});
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>>  Put(
            int id, 
            [FromBody]Product produtc, 
            [FromServices]DataContext context
        )
        {
            if(produtc.Id != id)
                return NotFound(new {message = "Produto não encontrada"});

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            try {
                context.Entry<Product>(produtc).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(produtc);
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
        public async Task<ActionResult<Product>> Delete(
            int id,
            [FromServices]DataContext context
        )
        {
            var produtc = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if(produtc == null)
                return NotFound(new {message = "Produto não encontrada"});
            try
            {
                context.Products.Remove(produtc);
                await context.SaveChangesAsync();
                return Ok(new {message = "Produto removida com sucesso"});
            }
            catch (Exception) {
                return BadRequest(new {message = "Não foi possível remover o produto"});
            }
        }


    }
}
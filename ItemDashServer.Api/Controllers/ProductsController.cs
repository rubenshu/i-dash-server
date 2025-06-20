using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Application.Products;
using ItemDashServer.Application.Products.Queries;
using ItemDashServer.Application.Products.Commands;
using ItemDashServer.Application.Products.QueryHandlers;
using ItemDashServer.Application.Products.CommandHandlers;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/products")]
[Authorize]
public class ProductsController(ILogger<ProductsController> logger) : ControllerBase
{
    private readonly ILogger<ProductsController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll(
        [FromServices] IGetProductsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new GetProductsQuery(), cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return NotFound();
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching products");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(
        int id,
        [FromServices] IGetProductByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new GetProductByIdQuery(id), cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return NotFound();
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching product with id {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create(
        [FromBody] CreateProductCommand command,
        [FromServices] ICreateProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await handler.ExecuteAsync(command, cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return BadRequest(result.Error ?? "Product creation failed.");
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating product");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        [FromServices] IUpdateProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != command.Id)
            return BadRequest("ID in URL does not match ID in body.");

        try
        {
            var result = await handler.ExecuteAsync(command, cancellationToken);
            if (!result.IsSuccess || !result.Value)
                return NotFound();
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product with id {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new DeleteProductCommand(id), cancellationToken);
            if (!result.IsSuccess || !result.Value)
                return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting product with id {ProductId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
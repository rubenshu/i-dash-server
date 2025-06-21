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
public class ProductsController() : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetProductsQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetProductsQuery();
        return Ok(await handler.ExecuteAsync(query, cancellationToken));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetProductByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetProductByIdQuery(id);
        return Ok(await handler.ExecuteAsync(query, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateProductCommand command,
        [FromServices] ICreateProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(command, cancellationToken));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateProductCommand command,
        [FromServices] IUpdateProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        return Ok(await handler.HandleAsync(command, cancellationToken));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteProductCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteProductCommand(id);
        return Ok(await handler.HandleAsync(command, cancellationToken));
    }
}
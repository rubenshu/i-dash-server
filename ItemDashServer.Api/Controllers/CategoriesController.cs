using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Commands;
using ItemDashServer.Application.Common;
using ItemDashServer.Domain.Entities;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Authorize]
public class CategoriesController() : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetCategoriesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(new GetCategoriesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetCategoryByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.ExecuteAsync(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] Product product,
        [FromServices] ICreateCategoryCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var cmd = new CreateCategoryCommand(product.Id, product.Name, product.Description, product.Price);
        var result = await handler.HandleAsync(cmd, cancellationToken);
        return Ok(result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryCommand command,
        [FromServices] IUpdateCategoryCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteCategoryCommandHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(new DeleteCategoryCommand(id), cancellationToken);
        return Ok(result);
    }
}
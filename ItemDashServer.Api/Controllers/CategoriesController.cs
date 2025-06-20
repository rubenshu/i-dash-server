using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.QueryHandlers;
using ItemDashServer.Application.Categories.CommandHandlers;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Authorize]
public class CategoriesController(ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly ILogger<CategoriesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll(
        [FromServices] IGetCategoriesQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new GetCategoriesQuery(), cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return NotFound();
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(
        int id,
        [FromServices] IGetCategoryByIdQueryHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new GetCategoryByIdQuery(id), cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return NotFound();
            return Ok(result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category with id {CategoryId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create(
        [FromBody] CreateCategoryCommand command,
        [FromServices] ICreateCategoryCommandHandler handler,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result = await handler.ExecuteAsync(command, cancellationToken);
            if (!result.IsSuccess || result.Value == null)
                return BadRequest(result.Error ?? "Category creation failed.");
            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryCommand command,
        [FromServices] IUpdateCategoryCommandHandler handler,
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
            _logger.LogError(ex, "Error updating category with id {CategoryId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteCategoryCommandHandler handler,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await handler.ExecuteAsync(new DeleteCategoryCommand(id), cancellationToken);
            if (!result.IsSuccess || !result.Value)
                return NotFound();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category with id {CategoryId}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
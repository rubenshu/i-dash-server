using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ItemDashServer.Application.Categories;
using ItemDashServer.Application.Categories.Queries;
using ItemDashServer.Application.Categories.Commands;

namespace ItemDashServer.Api.Controllers;

[ApiController]
[Route("api/v1/categories")]
[Authorize]
public class CategoriesController(IMediator mediator, ILogger<CategoriesController> logger) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<CategoriesController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAll()
    {
        try
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching categories");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoryDto>> GetById(int id)
    {
        try
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category == null)
                return NotFound();
            return Ok(category);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching category with id {CategoryId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CategoryDto>> Create([FromBody] CreateCategoryCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var created = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != command.Id)
            return BadRequest("ID in URL does not match ID in body.");

        try
        {
            var updated = await _mediator.Send(command);
            if (updated is bool result && !result)
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
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _mediator.Send(new DeleteCategoryCommand(id));
            if (!result)
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
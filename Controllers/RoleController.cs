using LibraCore.Backend.DTOs;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
  private readonly ILogger<RoleController> _logger;
  private readonly IRoleService _roleService;

  public RoleController(ILogger<RoleController> logger, IRoleService roleService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<RoleModel>>> GetAll()
  {
    var roles = await _roleService.GetAllAsync();

    if (!roles.Any())
    {
      _logger.LogInformation("No roles found.");
      return NotFound("No roles available.");
    }

    return Ok(roles);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<RoleModel>> GetById(int id)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    {
      var role = await _roleService.GetAsync(id);
      if (role == null)
      {
        _logger.LogInformation("No role found.");
        return NotFound("Role not found.");
      }
      return Ok(role);
    }
  }

  [HttpPost]
  public async Task<ActionResult<RoleModel>> Create(CreateRoleRequest createRoleRequest)
  {
    if (string.IsNullOrWhiteSpace(createRoleRequest.Name))
    {
      _logger.LogWarning("Invalid create request: Name is null or empty.");
      return BadRequest("Role name is required.");
    }

    {
      var newRole = new RoleModel(name: createRoleRequest.Name);
      var createdRole = await _roleService.CreateAsync(newRole);

      _logger.LogInformation("Role created successfully with Name: {Name}", createdRole.Name);

      // 201 Created status. This response indicates that the resource was successfully created. 
      // It includes the URI of the newly created resource (using the GetById action) in the Location header 
      // along with the created role object in the response body.
      return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
    }
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<RoleModel>> Update(int id, UpdateRoleRequest updateRoleRequest)
  {
    if (updateRoleRequest == null)
    {
      _logger.LogWarning("Invalid update request: {RoleId}", id);
      return BadRequest("Invalid update request.");
    }

    if (string.IsNullOrWhiteSpace(updateRoleRequest.Name))
    {
      _logger.LogWarning("Role name is missing for update: {RoleId}", id);
      return BadRequest("Role name cannot be empty.");
    }

    var roleWithUpdates = new RoleModel { Id = id, Name = updateRoleRequest.Name };
    var updatedRole = await _roleService.UpdateAsync(id, roleWithUpdates);

    if (updatedRole == null)
    {
      _logger.LogWarning("Role not found for update: {RoleId}", id);
      return NotFound($"Role with ID {id} not found.");
    }

    return Ok(updatedRole);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteRole(int id)
  {
    var result = await _roleService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("Role with id {Id} not found.", id);
      return NotFound($"Role with id {id} not found.");
    }

    return NoContent(); // 204 No Content (successfully deleted)
  }
}

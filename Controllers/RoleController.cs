using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
using LibraCore.Backend.DTOs.Role;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

/// <summary>
/// Provides endpoints for managing roles in the system.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class RoleController : ControllerBase
{
  private readonly ILogger<RoleController> _logger;

  private readonly IValidator<CreateRoleRequest> _createRoleRequestValidator;

  private readonly IValidator<UpdateRoleRequest> _updateRoleRequestValidator;

  private readonly IRoleService _roleService;

  public RoleController(ILogger<RoleController> logger, IValidator<CreateRoleRequest> createRoleRequestValidator, IValidator<UpdateRoleRequest> updateRoleRequestValidator, IRoleService roleService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _createRoleRequestValidator = createRoleRequestValidator ?? throw new ArgumentNullException(nameof(createRoleRequestValidator));
    _updateRoleRequestValidator = updateRoleRequestValidator ?? throw new ArgumentNullException(nameof(updateRoleRequestValidator));
    _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
  }

  /// <summary>
  /// Retrieves a role by its unique identifier.
  /// </summary>
  /// <param name="id">The unique ID of the role to retrieve.</param>
  /// <returns>The role with the specified ID, or a 404 status if not found.</returns>
  /// <remarks>Required permission: <c>role:read</c></remarks>
  [HttpGet("{id}")]
  [Authorize("role:read")]
  public async Task<ActionResult<RoleModel>> GetById(int id)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    var role = await _roleService.GetAsync(id);
    if (role == null)
    {
      _logger.LogInformation("No role found.");
      return NotFound(new MessageResponse("Role not found."));
    }
    return Ok(role);
  }

  /// <summary>
  /// Retrieves all roles in the system.
  /// </summary>
  /// <returns>A list of all roles.</returns>
  /// <remarks>Required permission: <c>role:read</c></remarks>
  [HttpGet]
  [Authorize("role:read")]
  public async Task<ActionResult<IEnumerable<RoleModel>>> GetAll()
  {
    var roles = await _roleService.GetAllAsync();

    if (!roles.Any())
    {
      _logger.LogInformation("No roles found.");
      return NotFound(new MessageResponse("No roles available."));
    }

    return Ok(roles);
  }

  /// <summary>
  /// Creates a new role in the system.
  /// </summary>
  /// <param name="createRoleRequest">The request containing details for the new role.</param>
  /// <returns>The details of the newly created role.</returns>
  /// <remarks>Required permission: <c>role:write</c></remarks>
  [HttpPost]
  [Authorize("role:write")]
  public async Task<ActionResult<RoleModel>> Create(CreateRoleRequest createRoleRequest)
  {
    ValidationResult validationResult = await _createRoleRequestValidator.ValidateAsync(createRoleRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid create request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    var newRole = new RoleModel
    {
      Name = createRoleRequest.Name
    };
    var createdRole = await _roleService.CreateAsync(newRole);

    _logger.LogInformation("Role created successfully with Name: {Name}", createdRole.Name);

    // 201 Created status. This response indicates that the resource was successfully created. 
    // It includes the URI of the newly created resource (using the GetById action) in the Location header 
    // along with the created role object in the response body.
    return CreatedAtAction(nameof(GetById), new { id = createdRole.Id }, createdRole);
  }

  /// <summary>
  /// Updates an existing role by its ID.
  /// </summary>
  /// <param name="id">The ID of the role to update.</param>
  /// <param name="updateRoleRequest">The request containing updated role details.</param>
  /// <returns>The updated role details, or an error if validation fails.</returns>
  /// <remarks>Required permission: <c>role:write</c></remarks>
  [HttpPut("{id}")]
  [Authorize("role:write")]
  public async Task<ActionResult<RoleModel>> Update(int id, UpdateRoleRequest updateRoleRequest)
  {
    ValidationResult validationResult = await _updateRoleRequestValidator.ValidateAsync(updateRoleRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid update request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    if (id != updateRoleRequest.Id)
    {
      _logger.LogWarning("'Id' in the request body does not match the 'Id' in the path: {BodyId} vs {PathId}", updateRoleRequest.Id, id);
      return BadRequest(new RequestValidationFailureResponse("Id", "The 'Id' in the request body must match the 'Id' in the path."));
    }

    var roleWithUpdates = new RoleModel
    {
      Id = id,
      Name = updateRoleRequest.Name
    };
    var updatedRole = await _roleService.UpdateAsync(id, roleWithUpdates);

    if (updatedRole == null)
    {
      _logger.LogWarning("Role not found for update: {RoleId}", id);
      return NotFound(new MessageResponse($"Role with ID {id} not found."));
    }

    return Ok(updatedRole);
  }

  /// <summary>
  /// Deletes a role by its ID.
  /// </summary>
  /// <param name="id">The unique ID of the role to delete.</param>
  /// <returns>A 204 response if the role is successfully deleted.</returns>
  /// <remarks>Required permission: <c>role:delete</c></remarks>
  [HttpDelete("{id}")]
  [Authorize("role:delete")]
  public async Task<ActionResult> DeleteRole(int id)
  {
    var result = await _roleService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("Role with id {Id} not found.", id);
      return NotFound(new MessageResponse($"Role with id {id} not found."));
    }

    return NoContent(); // 204 No Content (successfully deleted)
  }
}

using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
using LibraCore.Backend.DTOs.UserStatus;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

/// <summary>
/// Provides endpoints for managing User Statuses in the system.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class UserStatusController : ControllerBase
{
  private readonly ILogger<UserStatusController> _logger;

  private readonly IValidator<CreateUserStatusRequest> _createUserStatusRequestValidator;

  private readonly IValidator<UpdateUserStatusRequest> _updateUserStatusRequestValidator;

  private readonly IUserStatusService _userStatusService;

  public UserStatusController(ILogger<UserStatusController> logger, IValidator<CreateUserStatusRequest> createUserStatusRequestValidator, IValidator<UpdateUserStatusRequest> updateUserStatusRequestValidator, IUserStatusService userStatusService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _createUserStatusRequestValidator = createUserStatusRequestValidator ?? throw new ArgumentNullException(nameof(createUserStatusRequestValidator));
    _updateUserStatusRequestValidator = updateUserStatusRequestValidator ?? throw new ArgumentNullException(nameof(updateUserStatusRequestValidator));
    _userStatusService = userStatusService ?? throw new ArgumentNullException(nameof(userStatusService));
  }

  /// <summary>
  /// Retrieves a user status by its ID.
  /// </summary>
  /// <param name="id">The ID of the user status to retrieve.</param>
  /// <returns>The user status details if found.</returns>
  /// <remarks>Required permission: <c>user_status:read</c></remarks>
  [HttpGet("{id}")]
  [Authorize("user_status:read")]
  public async Task<ActionResult<UserStatusModel>> GetById(int id)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    var userStatus = await _userStatusService.GetAsync(id);
    if (userStatus == null)
    {
      _logger.LogInformation("No user status found.");
      return NotFound(new MessageResponse("User status not found."));
    }
    return Ok(userStatus);
  }

  /// <summary>
  /// Retrieves all user statuses.
  /// </summary>
  /// <returns>A list of all user statuses.</returns>
  /// <remarks>Required permission: <c>user_status:read</c></remarks>
  [HttpGet]
  [Authorize("user_status:read")]
  public async Task<ActionResult<IEnumerable<UserStatusModel>>> GetAllAsync()
  {
    var userStatuses = await _userStatusService.GetAllAsync();

    if (!userStatuses.Any())
    {
      _logger.LogInformation("No user statuses found.");
      return NotFound(new MessageResponse("No user statuses available."));
    }

    return Ok(userStatuses);
  }

  /// <summary>
  /// Creates a new user status.
  /// </summary>
  /// <param name="createUserStatusRequest">The details of the user status to create.</param>
  /// <returns>The newly created user status.</returns>
  /// <remarks>Required permission: <c>user_status:write</c></remarks>
  [HttpPost]
  [Authorize("user_status:write")]
  public async Task<ActionResult<UserStatusModel>> Create(CreateUserStatusRequest createUserStatusRequest)
  {
    ValidationResult validationResult = await _createUserStatusRequestValidator.ValidateAsync(createUserStatusRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid create request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    var newUserStatus = new UserStatusModel
    {
      Name = createUserStatusRequest.Name
    };
    var createdUserStatus = await _userStatusService.CreateAsync(newUserStatus);

    _logger.LogInformation("User status created successfully with Name: {Name}", createdUserStatus.Name);

    return CreatedAtAction(nameof(GetById), new { id = createdUserStatus.Id }, createdUserStatus);
  }

  /// <summary>
  /// Updates an existing user status.
  /// </summary>
  /// <param name="id">The ID of the user status to update.</param>
  /// <param name="updateUserStatusRequest">The updated user status details.</param>
  /// <returns>The updated user status details.</returns>
  /// <remarks>Required permission: <c>user_status:write</c></remarks>
  [HttpPut("{id}")]
  [Authorize("user_status:write")]
  public async Task<ActionResult<UserStatusModel>> Update(int id, UpdateUserStatusRequest updateUserStatusRequest)
  {
    ValidationResult validationResult = await _updateUserStatusRequestValidator.ValidateAsync(updateUserStatusRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid update request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    if (id != updateUserStatusRequest.Id)
    {
      _logger.LogWarning("'Id' in the request body does not match the 'Id' in the path: {BodyId} vs {PathId}", updateUserStatusRequest.Id, id);
      return BadRequest(new RequestValidationFailureResponse("Id", "The 'Id' in the request body must match the 'Id' in the path."));
    }

    var userStatusWithUpdates = new UserStatusModel
    {
      Id = id,
      Name = updateUserStatusRequest.Name
    };
    var updatedUserStatus = await _userStatusService.UpdateAsync(id, userStatusWithUpdates);

    if (updatedUserStatus == null)
    {
      _logger.LogWarning("User status not found for update: {UserStatusId}", id);
      return NotFound(new MessageResponse($"User status with ID {id} not found."));
    }

    return Ok(updatedUserStatus);
  }

  /// <summary>
  /// Deletes a user status by its ID.
  /// </summary>
  /// <param name="id">The ID of the user status to delete.</param>
  /// <returns>No content if deletion is successful.</returns>
  /// <remarks>Required permission: <c>user_status:delete</c></remarks>
  [HttpDelete("{id}")]
  [Authorize("user_status:delete")]
  public async Task<ActionResult> Delete(int id)
  {
    var result = await _userStatusService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("User status with id {Id} not found.", id);
      return NotFound(new MessageResponse($"User status with id {id} not found."));
    }

    return NoContent();
  }
}
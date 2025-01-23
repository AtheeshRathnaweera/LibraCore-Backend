
using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
using LibraCore.Backend.DTOs.UserActiveStatus;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class UserActiveStatusController : ControllerBase
{
  private readonly ILogger<UserActiveStatusController> _logger;

  private readonly IValidator<CreateUserActiveStatusRequest> _createUserActiveStatusRequestValidator;

  private readonly IValidator<UpdateUserActiveStatusRequest> _updateUserActiveStatusRequestValidator;

  private readonly IUserActiveStatusService _userActiveStatusService;

  public UserActiveStatusController(ILogger<UserActiveStatusController> logger, IValidator<CreateUserActiveStatusRequest> createUserActiveStatusRequestValidator, IValidator<UpdateUserActiveStatusRequest> updateUserActiveStatusRequestValidator, IUserActiveStatusService userActiveStatusService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _createUserActiveStatusRequestValidator = createUserActiveStatusRequestValidator ?? throw new ArgumentNullException(nameof(createUserActiveStatusRequestValidator));
    _updateUserActiveStatusRequestValidator = updateUserActiveStatusRequestValidator ?? throw new ArgumentNullException(nameof(updateUserActiveStatusRequestValidator));
    _userActiveStatusService = userActiveStatusService ?? throw new ArgumentNullException(nameof(userActiveStatusService));
  }

  [HttpGet("{id}")]
  [Authorize("user_active_status:read")]
  public async Task<ActionResult<UserActiveStatusModel>> GetById(int id, [FromQuery] string? expand = null)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    var userActiveStatus = await _userActiveStatusService.GetAsync(id, expand);
    if (userActiveStatus == null)
    {
      _logger.LogInformation("No user active status found.");
      return NotFound(new MessageResponse("User active status not found."));
    }
    return Ok(userActiveStatus);
  }

  [HttpGet]
  [Authorize("user_active_status:read")]
  public async Task<ActionResult<IEnumerable<UserActiveStatusModel>>> GetAll([FromQuery] string? expand = null)
  {
    var userActiveStatuses = await _userActiveStatusService.GetAllAsync(expand);

    if (!userActiveStatuses.Any())
    {
      _logger.LogInformation("No user active statuses found.");
      return NotFound(new MessageResponse("No user active statuses available."));
    }

    return Ok(userActiveStatuses);
  }

  [HttpPost]
  [Authorize("user_active_status:write")]
  public async Task<ActionResult<UserActiveStatusModel>> Create(CreateUserActiveStatusRequest createUserActiveStatusRequest)
  {
    ValidationResult validationResult = await _createUserActiveStatusRequestValidator.ValidateAsync(createUserActiveStatusRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid create request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    var newUserActiveStatus = new UserActiveStatusModel
    {
      UserId = createUserActiveStatusRequest.UserId,
      UserStatusId = createUserActiveStatusRequest.UserStatusId
    };
    var createdUserActiveStatus = await _userActiveStatusService.CreateAsync(newUserActiveStatus);

    _logger.LogInformation("User Active Status created successfully for the User Id: {Name}", createdUserActiveStatus.UserId);

    return CreatedAtAction(nameof(GetById), new { id = createdUserActiveStatus.Id }, createdUserActiveStatus);
  }

  [HttpPut("{id}")]
  [Authorize("user_active_status:write")]
  public async Task<ActionResult<RoleModel>> Update(int id, UpdateUserActiveStatusRequest updateUserActiveStatusRequest)
  {
    ValidationResult validationResult = await _updateUserActiveStatusRequestValidator.ValidateAsync(updateUserActiveStatusRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid update request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    if (id != updateUserActiveStatusRequest.Id)
    {
      _logger.LogWarning("'Id' in the request body does not match the 'Id' in the path: {BodyId} vs {PathId}", updateUserActiveStatusRequest.Id, id);
      return BadRequest(new RequestValidationFailureResponse("Id", "The 'Id' in the request body must match the 'Id' in the path."));
    }

    var userActiveStatusWithUpdates = new UserActiveStatusModel
    {
      Id = id,
      UserId = updateUserActiveStatusRequest.UserId,
      UserStatusId = updateUserActiveStatusRequest.UserStatusId
    };
    var updatedUserActiveStatus = await _userActiveStatusService.UpdateAsync(id, userActiveStatusWithUpdates);

    if (updatedUserActiveStatus == null)
    {
      _logger.LogWarning("User active status not found for update: {UserActiveStatusId}", id);
      return NotFound(new MessageResponse($"User active status with ID {id} not found."));
    }

    return Ok(updatedUserActiveStatus);
  }

  [HttpDelete("{id}")]
  [Authorize("user_active_status:delete")]
  public async Task<ActionResult> DeleteRole(int id)
  {
    var result = await _userActiveStatusService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("User active status with id {Id} not found.", id);
      return NotFound(new MessageResponse($"User active status with id {id} not found."));
    }

    return NoContent();
  }
}
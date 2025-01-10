using LibraCore.Backend.DTOs;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Models;

[ApiController]
[Route("api/[controller]")]
public class UserStatusController : ControllerBase
{
  private readonly ILogger<UserStatusController> _logger;
  private readonly IUserStatusService _userStatusService;

  public UserStatusController(ILogger<UserStatusController> logger, IUserStatusService userStatusService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _userStatusService = userStatusService ?? throw new ArgumentNullException(nameof(userStatusService));
  }

  [HttpGet("{id}")]
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
      return NotFound("Role user status found.");
    }
    return Ok(userStatus);
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<UserStatusModel>>> GetAllAsync()
  {
    var userStatuses = await _userStatusService.GetAllAsync();

    if (!userStatuses.Any())
    {
      _logger.LogInformation("No user statuses found.");
      return NotFound("No user statuses available.");
    }

    return Ok(userStatuses);
  }

  [HttpPost]
  public async Task<ActionResult<UserStatusModel>> Create(CreateUserStatusRequest createUserStatusRequest)
  {
    if (string.IsNullOrWhiteSpace(createUserStatusRequest.Name))
    {
      _logger.LogWarning("Invalid create request: Name is null or empty.");
      return BadRequest("Role name is required.");
    }

    var newUserStatus = new UserStatusModel(name: createUserStatusRequest.Name);
    var createdUserStatus = await _userStatusService.CreateAsync(newUserStatus);

    _logger.LogInformation("User Status created successfully with Name: {Name}", createdUserStatus.Name);

    return CreatedAtAction(nameof(GetById), new { id = createdUserStatus.Id }, createdUserStatus);
  }

  [HttpPut("{id}")]
  public async Task<ActionResult<UserStatusModel>> Update(int id, UpdateUserStatusRequest updateUserStatusRequest)
  {
    if (updateUserStatusRequest == null)
    {
      _logger.LogWarning("Invalid update request: {UserStatusId}", id);
      return BadRequest("Invalid update request.");
    }

    if (string.IsNullOrWhiteSpace(updateUserStatusRequest.Name))
    {
      _logger.LogWarning("User Status name is missing for update: {UserStatusId}", id);
      return BadRequest("User Status name cannot be empty.");
    }

    var userStatusWithUpdates = new UserStatusModel { Id = id, Name = updateUserStatusRequest.Name };
    var updatedUserStatus = await _userStatusService.UpdateAsync(id, userStatusWithUpdates);

    if (updatedUserStatus == null)
    {
      _logger.LogWarning("User Status not found for update: {UserStatusId}", id);
      return NotFound($"User Status with ID {id} not found.");
    }

    return Ok(updatedUserStatus);
  }

  [HttpDelete("{id}")]
  public async Task<ActionResult> DeleteRole(int id)
  {
    var result = await _userStatusService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("User Status with id {Id} not found.", id);
      return NotFound($"User Status with id {id} not found.");
    }

    return NoContent();
  }
}
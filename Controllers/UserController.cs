using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
using LibraCore.Backend.DTOs.User;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

/// <summary>
/// Provides endpoints for managing Users in the system.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class UserController : ControllerBase
{
  private readonly ILogger<UserController> _logger;

  private readonly IValidator<CreateUserRequest> _createUserRequestValidator;

  private readonly IValidator<UpdateUserRequest> _updateUserRequestValidator;
  
  private readonly IUserService _userService;

  public UserController(
    ILogger<UserController> logger,
    IValidator<CreateUserRequest> createUserRequestValidator,
    IValidator<UpdateUserRequest> updateUserRequestValidator,
    IUserService userService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _createUserRequestValidator = createUserRequestValidator ?? throw new ArgumentNullException(nameof(createUserRequestValidator));
    _updateUserRequestValidator = updateUserRequestValidator ?? throw new ArgumentNullException(nameof(updateUserRequestValidator));
    _userService = userService ?? throw new ArgumentNullException(nameof(userService));
  }

  /// <summary>
  /// Retrieves a user by their ID.
  /// </summary>
  /// <param name="id">The ID of the user to retrieve.</param>
  /// <returns>The user details if found.</returns>
  /// <remarks>Required permission: <c>user:read</c></remarks>
  [HttpGet("{id}")]
  [Authorize("user:read")]
  public async Task<ActionResult<UserModel>> GetById(int id)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    var user = await _userService.GetAsync(id);
    if (user == null)
    {
      _logger.LogInformation("No user found.");
      return NotFound(new MessageResponse("User not found."));
    }
    return Ok(user);
  }

  /// <summary>
  /// Retrieves all users.
  /// </summary>
  /// <returns>A list of all users.</returns>
  /// <remarks>Required permission: <c>user:read</c></remarks>
  [HttpGet]
  [Authorize("user:read")]
  public async Task<ActionResult<IEnumerable<UserModel>>> GetAll()
  {
    var users = await _userService.GetAllAsync();

    if (!users.Any())
    {
      _logger.LogInformation("No users found.");
      return NotFound(new MessageResponse("No users available."));
    }

    return Ok(users);
  }

  /// <summary>
  /// Creates a new user.
  /// </summary>
  /// <param name="createUserRequest">The details of the user to create.</param>
  /// <returns>The newly created user.</returns>
  /// <remarks>Required permission: <c>user:write</c></remarks>
  [HttpPost]
  [Authorize("user:write")]
  public async Task<ActionResult<UserModel>> Create(CreateUserRequest createUserRequest)
  {
    ValidationResult validationResult = await _createUserRequestValidator.ValidateAsync(createUserRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid create request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    var newUser = new UserModel
    {
      FirstName = createUserRequest.FirstName,
      LastName = createUserRequest.LastName,
      AddressOne = createUserRequest.AddressOne,
      AddressTwo = createUserRequest.AddressTwo,
      City = createUserRequest.City,
      District = createUserRequest.District,
      Email = createUserRequest.Email,
      PhoneNumber = createUserRequest.PhoneNumber,
      NIC = createUserRequest.NIC,
      DateOfBirth = createUserRequest.DateOfBirth
    };
    var createdUser = await _userService.CreateAsync(newUser);

    _logger.LogInformation("User created successfully with Name: {Name}", createdUser.FirstName);

    return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
  }

  /// <summary>
  /// Updates an existing user.
  /// </summary>
  /// <param name="id">The ID of the user to update.</param>
  /// <param name="updateUserRequest">The updated user details.</param>
  /// <returns>The updated user details.</returns>
  /// <remarks>Required permission: <c>user:write</c></remarks>
  [HttpPut("{id}")]
  [Authorize("user:write")]
  public async Task<ActionResult<UserModel>> Update(int id, UpdateUserRequest updateUserRequest){
    ValidationResult validationResult = await _updateUserRequestValidator.ValidateAsync(updateUserRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid update request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    if (id != updateUserRequest.Id)
    {
      _logger.LogWarning("'Id' in the request body does not match the 'Id' in the path: {BodyId} vs {PathId}", updateUserRequest.Id, id);
      return BadRequest(new RequestValidationFailureResponse("Id", "The 'Id' in the request body must match the 'Id' in the path."));
    }

    var userWithUpdates = new UserModel
    {
      FirstName = updateUserRequest.FirstName,
      LastName = updateUserRequest.LastName,
      AddressOne = updateUserRequest.AddressOne,
      AddressTwo = updateUserRequest.AddressTwo,
      City = updateUserRequest.City,
      District = updateUserRequest.District,
      Email = updateUserRequest.Email,
      PhoneNumber = updateUserRequest.PhoneNumber,
      NIC = updateUserRequest.NIC,
      DateOfBirth = updateUserRequest.DateOfBirth
    };
    var updatedUser = await _userService.UpdateAsync(id, userWithUpdates);

    if (updatedUser == null)
    {
      _logger.LogWarning("User not found for update: {UserId}", id);
      return NotFound(new MessageResponse($"User with ID {id} not found."));
    }

    return Ok(updatedUser);
  }

  /// <summary>
  /// Deletes a user by their ID.
  /// </summary>
  /// <param name="id">The ID of the user to delete.</param>
  /// <returns>No content if deletion is successful.</returns>
  /// <remarks>Required permission: <c>user:delete</c></remarks>
  [HttpDelete("{id}")]
  [Authorize("user:delete")]
  public async Task<ActionResult> Delete(int id)
  {
    var result = await _userService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("User with id {Id} not found.", id);
      return NotFound(new MessageResponse($"User with id {id} not found."));
    }

    return NoContent();
  }
}
using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
using LibraCore.Backend.DTOs.User;
using LibraCore.Backend.Models;
using LibraCore.Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraCore.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
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

  [HttpDelete("{id}")]
  [Authorize("user:delete")]
  public async Task<ActionResult> DeleteRole(int id)
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
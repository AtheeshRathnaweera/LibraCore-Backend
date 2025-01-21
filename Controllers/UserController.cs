using FluentValidation;
using FluentValidation.Results;
using LibraCore.Backend.DTOs;
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
  private readonly ILogger<RoleController> _logger;
  private readonly IValidator<CreateUserRequest> _createUserRequestValidator;
  private readonly IValidator<UpdateUserRequest> _updateUserRequestValidator;
  private readonly IUserService _userService;

  public UserController(ILogger<RoleController> logger, IValidator<CreateUserRequest> createUserRequestValidator, IValidator<UpdateUserRequest> updateUserRequestValidator, IUserService userService)
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

    var newUser = new UserModel(
      firstName: createUserRequest.FirstName,
      lastName: createUserRequest.LastName
    );
    var createdUser = await _userService.CreateAsync(newUser);

    _logger.LogInformation("User created successfully with Name: {Name}", createdUser.FirstName);

    return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
  }
}
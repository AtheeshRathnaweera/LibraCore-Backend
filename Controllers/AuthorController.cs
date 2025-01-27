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
/// Provides endpoints for managing Authors in the system.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize]
public class AuthorController : ControllerBase
{
  private readonly ILogger<AuthorController> _logger;

  private readonly IValidator<CreateAuthorRequest> _createAuthorRequestValidator;

  private readonly IValidator<UpdateAuthorRequest> _updateAuthorRequestValidator;

  private readonly IAuthorService _authorService;

  public AuthorController(ILogger<AuthorController> logger, IValidator<CreateAuthorRequest> createAuthorRequestValidator, IValidator<UpdateAuthorRequest> updateAuthorRequestValidator, IAuthorService authorService)
  {
    _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    _createAuthorRequestValidator = createAuthorRequestValidator ?? throw new ArgumentNullException(nameof(createAuthorRequestValidator));
    _updateAuthorRequestValidator = updateAuthorRequestValidator ?? throw new ArgumentNullException(nameof(updateAuthorRequestValidator));
    _authorService = authorService ?? throw new ArgumentNullException(nameof(authorService));
  }

  /// <summary>
  /// Retrieves a author by their ID.
  /// </summary>
  /// <param name="id">The ID of the author to retrieve.</param>
  /// <returns>The author details if found.</returns>
  /// <remarks>Required permission: <c>author:read</c></remarks>
  [HttpGet("{id}")]
  [Authorize("author:read")]
  public async Task<ActionResult<AuthorModel>> GetById(int id)
  {
    if (id <= 0)
    {
      _logger.LogWarning("Invalid ID: {Id}", id);
      return BadRequest("Invalid ID provided.");
    }

    var author = await _authorService.GetAsync(id);
    if (author == null)
    {
      _logger.LogInformation("No author found.");
      return NotFound(new MessageResponse("Author not found."));
    }
    return Ok(author);
  }

  /// <summary>
  /// Retrieves all authors.
  /// </summary>
  /// <returns>A list of all authors.</returns>
  /// <remarks>Required permission: <c>author:read</c></remarks>
  [HttpGet]
  [Authorize("author:read")]
  public async Task<ActionResult<IEnumerable<AuthorModel>>> GetAll()
  {
    var authors = await _authorService.GetAllAsync();

    if (!authors.Any())
    {
      _logger.LogInformation("No authors found.");
      return NotFound(new MessageResponse("No authors available."));
    }

    return Ok(authors);
  }

  /// <summary>
  /// Creates a new author.
  /// </summary>
  /// <param name="createAuthorRequest">The details of the author to create.</param>
  /// <returns>The newly created author.</returns>
  /// <remarks>Required permission: <c>author:write</c></remarks>
  [HttpPost]
  [Authorize("author:write")]
  public async Task<ActionResult<AuthorModel>> Create(CreateAuthorRequest createAuthorRequest)
  {
    ValidationResult validationResult = await _createAuthorRequestValidator.ValidateAsync(createAuthorRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid create request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    var authorRole = new AuthorModel
    {
      FirstName = createAuthorRequest.FirstName,
      LastName = createAuthorRequest.LastName,
      DateOfBirth = createAuthorRequest.DateOfBirth,
      DateOfDeath = createAuthorRequest.DateOfDeath,
      Nationality = createAuthorRequest.Nationality,
      ImageUrl = createAuthorRequest.ImageUrl
    };
    var createdAuthor = await _authorService.CreateAsync(authorRole);

    _logger.LogInformation("Author created successfully with FirstName: {FirstName}", createdAuthor.FirstName);

    return CreatedAtAction(nameof(GetById), new { id = createdAuthor.Id }, createdAuthor);
  }

  /// <summary>
  /// Updates an existing author.
  /// </summary>
  /// <param name="id">The ID of the author to update.</param>
  /// <param name="updateAuthorRequest">The updated author details.</param>
  /// <returns>The updated author details.</returns>
  /// <remarks>Required permission: <c>author:write</c></remarks>
  [HttpPut("{id}")]
  [Authorize("author:write")]
  public async Task<ActionResult<AuthorModel>> Update(int id, UpdateAuthorRequest updateAuthorRequest)
  {
    ValidationResult validationResult = await _updateAuthorRequestValidator.ValidateAsync(updateAuthorRequest);

    if (!validationResult.IsValid)
    {
      _logger.LogWarning("Invalid update request: {errors}", validationResult.Errors);
      return BadRequest(new RequestValidationFailureResponse(validationResult.ToDictionary()));
    }

    if (id != updateAuthorRequest.Id)
    {
      _logger.LogWarning("'Id' in the request body does not match the 'Id' in the path: {BodyId} vs {PathId}", updateAuthorRequest.Id, id);
      return BadRequest(new RequestValidationFailureResponse("Id", "The 'Id' in the request body must match the 'Id' in the path."));
    }

    var authorWithUpdates = new AuthorModel
    {
      Id = id,
      FirstName = updateAuthorRequest.FirstName,
      LastName = updateAuthorRequest.LastName,
      DateOfBirth = updateAuthorRequest.DateOfBirth,
      DateOfDeath = updateAuthorRequest.DateOfDeath,
      Nationality = updateAuthorRequest.Nationality,
      ImageUrl = updateAuthorRequest.ImageUrl
    };
    var updatedAuthor = await _authorService.UpdateAsync(id, authorWithUpdates);

    if (updatedAuthor == null)
    {
      _logger.LogWarning("Author not found for update: {AuthorId}", id);
      return NotFound(new MessageResponse($"Author with ID {id} not found."));
    }

    return Ok(updatedAuthor);
  }

  /// <summary>
  /// Deletes an author by their ID.
  /// </summary>
  /// <param name="id">The ID of the author to delete.</param>
  /// <returns>No content if deletion is successful.</returns>
  /// <remarks>Required permission: <c>author:delete</c></remarks>
  [HttpDelete("{id}")]
  [Authorize("author:delete")]
  public async Task<ActionResult> Delete(int id)
  {
    var result = await _authorService.DeleteAsync(id);

    if (!result)
    {
      _logger.LogWarning("Author with id {Id} not found.", id);
      return NotFound(new MessageResponse($"Author with id {id} not found."));
    }

    return NoContent();
  }
}
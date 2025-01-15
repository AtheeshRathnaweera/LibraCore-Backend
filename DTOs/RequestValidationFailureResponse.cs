namespace LibraCore.Backend.DTOs;

public class RequestValidationFailureResponse
{
  public IDictionary<string, string[]> Errors { get; set; }

  public RequestValidationFailureResponse(string propertyName, string error)
  {
    Errors = new Dictionary<string, string[]> { { propertyName, new[] { error } } };
  }

  public RequestValidationFailureResponse(IDictionary<string, string[]> errors)
  {
    Errors = errors;
  }
}
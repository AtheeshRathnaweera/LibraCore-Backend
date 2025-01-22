namespace LibraCore.Backend.DTOs;

public class AuthorizationErrorResponse {
  public string? Message { get; set; }
  
  public List<string>? Reasons { get; set; }
}
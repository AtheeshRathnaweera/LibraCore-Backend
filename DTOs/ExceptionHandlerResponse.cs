namespace LibraCore.Backend.DTOs;

public class ExceptionHandlerResponse
{
  public required string Message { get; set; }
  public DateTime Timestamp { get; set; }
  public ExceptionDebugInfo? Debug { get; set; }
}
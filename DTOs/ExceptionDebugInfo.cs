namespace LibraCore.Backend.DTOs;

public class ExceptionDebugInfo
{
  public required string ExceptionType { get; set; }
  
  public required string StackTrace { get; set; }
}
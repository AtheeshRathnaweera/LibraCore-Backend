namespace LibraCore.Backend.DTOs;

public class MessageResponse
{
  public string? Message { get; set; }

  public MessageResponse(string message)
  {
    Message = message;
  }
}
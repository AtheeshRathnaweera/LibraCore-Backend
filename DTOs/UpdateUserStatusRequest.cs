namespace LibraCore.Backend.DTOs;

public class UpdateUserStatusRequest
{
  public required int Id { get; set; }
  public required string Name { get; set; }
}
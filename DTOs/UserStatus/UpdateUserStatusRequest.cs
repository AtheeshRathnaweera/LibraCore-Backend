namespace LibraCore.Backend.DTOs.UserStatus;

public class UpdateUserStatusRequest
{
  public required int Id { get; set; }
  public required string Name { get; set; }
}
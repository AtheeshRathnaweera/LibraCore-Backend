namespace LibraCore.Backend.DTOs.UserActiveStatus;

public class CreateUserActiveStatusRequest
{
  public int UserId { get; set; }

  public int UserStatusId { get; set; }
}
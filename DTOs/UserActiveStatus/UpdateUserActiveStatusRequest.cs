namespace LibraCore.Backend.DTOs.UserActiveStatus;

public class UpdateUserActiveStatusRequest
{
  public int Id { get; set; }
  
  public int UserId { get; set; }
  
  public int UserStatusId { get; set; }
}
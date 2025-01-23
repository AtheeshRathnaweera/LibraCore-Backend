namespace LibraCore.Backend.Models;

public class UserActiveStatusModel
{
  public int Id { get; set; }

  public int UserId { get; set; }

  public int UserStatusId { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  // Optional: Include related data if needed
  public UserModel? User { get; set; }

  public UserStatusModel? UserStatus { get; set; }

  public override string ToString()
  {
    return $"UserActiveStatusModel: Id={Id}, UserId={UserId}, UserStatusId={UserStatusId}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}";
  }
}
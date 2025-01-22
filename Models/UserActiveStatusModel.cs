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

  public UserActiveStatusModel()
  {
  }

  public UserActiveStatusModel(int userId, int userStatusId)
  {
    UserId = userId;
    UserStatusId = userStatusId;
  }

  public override string ToString()
  {
    return $"Id: {Id}, UserId: {UserId}, UserStatusId: {UserStatusId}, CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}";
  }
}
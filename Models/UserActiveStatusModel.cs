using System.Text.Json.Serialization;

namespace LibraCore.Backend.Models;

public class UserActiveStatusModel
{
  public int Id { get; set; }

  public int UserId { get; set; }

  public int UserStatusId { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public UserModel? User { get; set; }

  [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
  public UserStatusModel? UserStatus { get; set; }

  public override string ToString()
  {
    return $"UserActiveStatusModel: Id={Id}, UserId={UserId}, UserStatusId={UserStatusId}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}";
  }
}
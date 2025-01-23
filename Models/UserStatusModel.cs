namespace LibraCore.Backend.Models;

public class UserStatusModel
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public override string ToString()
  {
    return $"UserStatusModel: Id={Id}, Name={Name}, CreatedAt={CreatedAt}, UpdatedAt={UpdatedAt}";
  }
}
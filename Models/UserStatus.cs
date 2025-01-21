namespace LibraCore.Backend.Models;

public class UserStatus
{
  public required int Id { get; set; }

  public required string Name { get; set; }

  public required DateTime CreatedAt { get; set; }
  
  public DateTime UpdatedAt { get; set; }
}
namespace LibraCore.Backend.Models;

public class UserStatusModel
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public UserStatusModel()
  {
  }

  public UserStatusModel(string name)
  {
    Name = name;
  }

  public UserStatusModel(int id, string name)
  {
    Id = id;
    Name = name;
  }
}
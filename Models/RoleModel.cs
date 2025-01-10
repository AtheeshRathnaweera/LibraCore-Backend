namespace LibraCore.Backend.Models;

public class RoleModel
{
  public int Id { get; set; }

  public string? Name { get; set; }

  public DateTime CreatedAt { get; set; }

  public DateTime? UpdatedAt { get; set; }

  public RoleModel()
  {

  }

  public RoleModel(string name)
  {
    Name = name;
    CreatedAt = DateTime.UtcNow;
    UpdatedAt = CreatedAt;
  }
}
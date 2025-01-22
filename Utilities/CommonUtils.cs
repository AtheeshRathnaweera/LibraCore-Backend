namespace LibraCore.Backend.Utilities;

public static class CommonUtils
{
  public static bool BeAValidDate(DateTime date)
  {
    return !date.Equals(default(DateTime));
  }

  public static bool PropertyExists(dynamic obj, string propertyName)
  {
    return obj != null && obj?.GetType().GetProperty(propertyName) != null;
  }
}
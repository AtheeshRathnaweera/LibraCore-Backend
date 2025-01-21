namespace LibraCore.Backend.Utilities;

public static class CommonUtils
{
  public static bool BeAValidDate(DateTime date)
  {
    return !date.Equals(default(DateTime));
  }
}
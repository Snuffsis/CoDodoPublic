namespace CoDodoWeb.Models;

public class ApiSettings
{
  public const string SettingsKey = "CoDodoAPI";
  public string Username { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string BaseUrl { get; set; } = string.Empty;
}
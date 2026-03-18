using CoDodoWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CoDodoWeb.Controllers;

public class CoDodoController : Controller
{
  private static readonly JsonSerializerOptions ApiJsonOptions = new(JsonSerializerDefaults.Web)
  {
    Converters =
    {
      new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: true),
    },
  };

  private readonly ILogger<CoDodoController> _logger;
  private readonly ApiSettings _apiSettings;
  private readonly IHttpClientFactory _httpClientFactory;

  public CoDodoController(
    ILogger<CoDodoController> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<ApiSettings> apiSettings)
  {
    _logger = logger;
    _httpClientFactory = httpClientFactory;
    _apiSettings = apiSettings.Value;
  }
  
  public async Task<IActionResult> Index(CancellationToken cancellationToken)
  {
    _logger.LogInformation("CoDodo index page requested");
    CoDodoIndexViewModel model = await LoadIndexViewModelAsync(cancellationToken);
    return View(model);
  }

  [HttpGet]
  public async Task<IActionResult> Opportunity(Guid id, CancellationToken cancellationToken)
  {
    OpportunityDetailsViewModel model = await LoadOpportunityDetailsViewModelAsync(id, cancellationToken);
    return View(model);
  }

  [HttpGet]
  public async Task<IActionResult> Processes(ProcessStatus? status, string? personName, CancellationToken cancellationToken)
  {
    ProcessOverviewViewModel model = await LoadProcessOverviewViewModelAsync(status, personName, cancellationToken);
    return View(model);
  }

  [HttpGet]
  public async Task<IActionResult> GetProcesses(CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Get, "processes");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    List<ProcessModel>? processes = await response.Content.ReadFromJsonAsync<List<ProcessModel>>(ApiJsonOptions, cancellationToken);
    return Ok(processes ?? []);
  }

  [HttpGet]
  public async Task<IActionResult> GetProcess(Guid id, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Get, $"processes/{id}");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    ProcessModel? process = await response.Content.ReadFromJsonAsync<ProcessModel>(ApiJsonOptions, cancellationToken);
    return process is null ? StatusCode(StatusCodes.Status502BadGateway) : Ok(process);
  }

  [HttpPost]
  public async Task<IActionResult> CreateProcess([FromBody] CreateProcessRequest requestBody, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Post, "processes", requestBody);
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    Guid? processId = await response.Content.ReadFromJsonAsync<Guid>(ApiJsonOptions, cancellationToken);
    return processId is null ? StatusCode(StatusCodes.Status502BadGateway) : Ok(processId.Value);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteProcess(Guid id, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Delete, $"process/{id}");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    return response.IsSuccessStatusCode
      ? NoContent()
      : await CreateErrorResultAsync(response, cancellationToken);
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> UpdateProcessStatus(UpdateProcessStatusRequest requestBody, CancellationToken cancellationToken)
  {
    string path = BuildUpdateProcessPath(requestBody.Name, requestBody.UriForAssignment, requestBody.Status);
    using var request = CreateApiRequest(HttpMethod.Put, path);
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
      TempData["ErrorMessage"] = string.IsNullOrWhiteSpace(responseBody)
        ? "Failed to update process status."
        : responseBody;

      return RedirectToAction(nameof(Opportunity), new { id = requestBody.OpportunityId });
    }

    TempData["SuccessMessage"] = $"Status for '{requestBody.Name}' updated to {requestBody.Status}.";
    return RedirectToAction(nameof(Opportunity), new { id = requestBody.OpportunityId });
  }

  [HttpGet]
  public async Task<IActionResult> GetOpportunities(CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Get, "opportunities");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    List<OpportunityModel>? opportunities = await response.Content.ReadFromJsonAsync<List<OpportunityModel>>(ApiJsonOptions, cancellationToken);
    return Ok(opportunities ?? []);
  }

  [HttpGet]
  public async Task<IActionResult> GetOpportunity(Guid id, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Get, $"opportunities/{id}");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    OpportunityModel? opportunity = await response.Content.ReadFromJsonAsync<OpportunityModel>(ApiJsonOptions, cancellationToken);
    return opportunity is null ? StatusCode(StatusCodes.Status502BadGateway) : Ok(opportunity);
  }

  [HttpPost]
  public async Task<IActionResult> CreateOpportunity([FromBody] CreateOpportunityRequest requestBody, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Post, "opportunities", requestBody);
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      return await CreateErrorResultAsync(response, cancellationToken);
    }

    Guid? opportunityId = await response.Content.ReadFromJsonAsync<Guid>(ApiJsonOptions, cancellationToken);
    return opportunityId is null ? StatusCode(StatusCodes.Status502BadGateway) : Ok(opportunityId.Value);
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteOpportunity(Guid id, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Delete, $"opportunity/{id}");
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    return response.IsSuccessStatusCode
      ? NoContent()
      : await CreateErrorResultAsync(response, cancellationToken);
  }

  private async Task<HttpResponseMessage> SendApiRequestAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    HttpClient client = _httpClientFactory.CreateClient();
    return await client.SendAsync(request, cancellationToken);
  }

  private HttpRequestMessage CreateApiRequest(HttpMethod method, string path, object? body = null)
  {
    var request = new HttpRequestMessage(method, BuildApiUri(path));
    request.Headers.Authorization = CreateBasicAuthHeader();

    if (body is not null)
    {
      request.Content = JsonContent.Create(body, options: ApiJsonOptions);
    }

    return request;
  }

  private Uri BuildApiUri(string path)
  {
    var baseUri = new Uri(_apiSettings.BaseUrl.TrimEnd('/') + "/");
    return new Uri(baseUri, path.TrimStart('/'));
  }

  private AuthenticationHeaderValue CreateBasicAuthHeader()
  {
    string rawCredentials = $"{_apiSettings.Username}:{_apiSettings.Password}";
    string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(rawCredentials));
    return new AuthenticationHeaderValue("Basic", encodedCredentials);
  }

  private static async Task<IActionResult> CreateErrorResultAsync(HttpResponseMessage response, CancellationToken cancellationToken)
  {
    string body = await response.Content.ReadAsStringAsync(cancellationToken);

    if (string.IsNullOrWhiteSpace(body))
    {
      return new StatusCodeResult((int)response.StatusCode);
    }

    return new ContentResult
    {
      StatusCode = (int)response.StatusCode,
      Content = body,
      ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/problem+json",
    };
  }

  private async Task<CoDodoIndexViewModel> LoadIndexViewModelAsync(CancellationToken cancellationToken)
  {
    (IReadOnlyList<ProcessModel> processes, string? processError) =
      await TryGetListFromApiAsync<ProcessModel>("processes", cancellationToken);
    (IReadOnlyList<OpportunityModel> opportunities, string? opportunityError) =
      await TryGetListFromApiAsync<OpportunityModel>("opportunities", cancellationToken);

    var errors = new List<string>();
    if (!string.IsNullOrWhiteSpace(processError))
    {
      errors.Add(processError);
    }

    if (!string.IsNullOrWhiteSpace(opportunityError))
    {
      errors.Add(opportunityError);
    }

    var processCountsByOpportunity = processes
      .Where(p => p.Opportunity is not null)
      .GroupBy(p => p.Opportunity!.Id)
      .ToDictionary(group => group.Key, group => group.Count());

    return new CoDodoIndexViewModel
    {
      Opportunities = opportunities,
      ProcessCountsByOpportunity = processCountsByOpportunity,
      ErrorMessage = errors.Count > 0 ? string.Join(" | ", errors) : null,
    };
  }

  private async Task<OpportunityDetailsViewModel> LoadOpportunityDetailsViewModelAsync(Guid opportunityId, CancellationToken cancellationToken)
  {
    (IReadOnlyList<ProcessModel> processes, string? processError) =
      await TryGetListFromApiAsync<ProcessModel>("processes", cancellationToken);
    (IReadOnlyList<OpportunityModel> opportunities, string? opportunityError) =
      await TryGetListFromApiAsync<OpportunityModel>("opportunities", cancellationToken);

    OpportunityModel? opportunity = opportunities.FirstOrDefault(o => o.Id == opportunityId);
    IReadOnlyList<ProcessModel> filteredProcesses = processes
      .Where(p => p.Opportunity?.Id == opportunityId)
      .ToList();

    var errors = new List<string>();
    if (!string.IsNullOrWhiteSpace(processError))
    {
      errors.Add(processError);
    }

    if (!string.IsNullOrWhiteSpace(opportunityError))
    {
      errors.Add(opportunityError);
    }

    if (opportunity is null)
    {
      errors.Add("Opportunity was not found.");
    }

    return new OpportunityDetailsViewModel
    {
      Opportunity = opportunity,
      Processes = filteredProcesses,
      ErrorMessage = TempData["ErrorMessage"] as string ?? (errors.Count > 0 ? string.Join(" | ", errors) : null),
      SuccessMessage = TempData["SuccessMessage"] as string,
    };
  }

  private async Task<ProcessOverviewViewModel> LoadProcessOverviewViewModelAsync(
    ProcessStatus? status,
    string? personName,
    CancellationToken cancellationToken)
  {
    (IReadOnlyList<ProcessModel> processes, string? processError) =
      await TryGetListFromApiAsync<ProcessModel>("processes", cancellationToken);

    string normalizedPersonName = personName?.Trim() ?? string.Empty;

    IEnumerable<ProcessModel> filtered = processes;
    if (status.HasValue)
    {
      filtered = filtered.Where(process => process.Status == status.Value);
    }

    if (!string.IsNullOrWhiteSpace(normalizedPersonName))
    {
      filtered = filtered.Where(process =>
        process.Opportunity?.NameOfSalesLead.Contains(normalizedPersonName, StringComparison.OrdinalIgnoreCase) == true);
    }

    return new ProcessOverviewViewModel
    {
      Processes = filtered
        .OrderBy(process => process.Status)
        .ThenBy(process => process.Name, StringComparer.OrdinalIgnoreCase)
        .ToList(),
      StatusFilter = status,
      PersonNameFilter = normalizedPersonName,
      ErrorMessage = processError,
    };
  }

  private async Task<(IReadOnlyList<T> Items, string? Error)> TryGetListFromApiAsync<T>(string path, CancellationToken cancellationToken)
  {
    using var request = CreateApiRequest(HttpMethod.Get, path);
    using HttpResponseMessage response = await SendApiRequestAsync(request, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
      string details = string.IsNullOrWhiteSpace(responseBody)
        ? $"HTTP {(int)response.StatusCode}"
        : responseBody;

      _logger.LogWarning("Failed loading {Path} for index page. StatusCode: {StatusCode}. Body: {Body}", path, (int)response.StatusCode, responseBody);

      return ([], $"Failed loading `{path}` from API: {details}");
    }
    
    List<T>? items = await response.Content.ReadFromJsonAsync<List<T>>(ApiJsonOptions, cancellationToken);
    return (items ?? [], null);
  }

  private static string BuildUpdateProcessPath(string name, string uriForAssignment, ProcessStatus status)
  {
    QueryString query = QueryString.Create(new[]
    {
      new KeyValuePair<string, string?>("name", name),
      new KeyValuePair<string, string?>("uriForAssignment", uriForAssignment),
      new KeyValuePair<string, string?>("status", status.ToString()),
    });

    return $"processes{query}";
  }
}
using SharedKernel;

namespace CoDodoApi.Infrastructure;

internal sealed class DateTimeProvider : IDateTimeProvider
{
  public DateTime UtcNow => DateTime.UtcNow;
}
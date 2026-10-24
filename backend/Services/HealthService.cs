namespace backend.Services;
public class HealthService : IHealthService
{
    public string GetStatus() => "Healthy";
}
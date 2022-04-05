namespace ToyRobot.Common.Services;

public class ToyRobotServices
{
    public static ToyRobotServices Instance { get; private set; } = new();
    IServiceProvider? serviceProvider;
    private ToyRobotServices() { }
    public void SetServiceProvider(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public T? GetService<T>()
    {
        return (T?)this.serviceProvider?.GetService(typeof(T));
    }
}

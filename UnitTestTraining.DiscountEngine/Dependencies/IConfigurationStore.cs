namespace UnitTestTraining.DiscountEngine.Dependencies
{
    public interface IConfigurationStore
    {
        T Get<T>(string key);
        T GetOrDefault<T>(string key, T defaultValue = default(T));
    }
}
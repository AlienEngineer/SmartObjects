namespace SmartObjects
{
    public interface IObjectFactory<out T>
    {
        T GetInstance();
    }
}
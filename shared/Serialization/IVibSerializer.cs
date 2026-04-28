namespace shared.Serialization
{
    public interface IVibSerializer<T>
    {
        string Serialize(T obj);
        T Deserialize(string data);
    }
}

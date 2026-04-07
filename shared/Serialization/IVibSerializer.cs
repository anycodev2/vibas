namespace shared.Serialization
{
    public interface IVibSerializer
    {
        string Serialize(object obj);
        T Deserialize<T>(string data);
    }
}

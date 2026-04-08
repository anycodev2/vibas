using shared.Serialization;

namespace shared.Services
{
    public abstract class VibFileService<T>
    {
        protected readonly IVibSerializer<T> Serializer;

        public VibFileService(IVibSerializer<T> serializer)
        {
            Serializer = serializer;
        }

        public abstract T Open(string filePath);
        public abstract void Save(T data);
        public abstract void Close(T data);
    }
}

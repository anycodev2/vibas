using shared.Serialization;
using System;

namespace shared.Services
{
    public abstract class VibFileService<T>
    {
        protected readonly IVibSerializer Serializer;

        public VibFileService(IVibSerializer serializer)
        {
            Serializer = serializer;
        }

        public abstract T Open(string filePath);
        public abstract void Save(T data);
        public abstract void Close(T data);
    }
}

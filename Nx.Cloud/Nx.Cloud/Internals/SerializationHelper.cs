using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Nx.Cloud.Internals
{
    internal static class SerializationHelper<T>
        where T : class, new()
    {
        /// <summary>
        /// Serialize in binary mode instance of T to stream.
        /// </summary>
        /// <param name="entity">Entity to serialize.</param>
        /// <returns>Binary stream with serialized instance of T.</returns>
        public static Stream SerializeToStream(T entity)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, entity);
                return stream;
            }
        }

        /// <summary>
        /// Serialize in binary mode instance of T to byte array.
        /// </summary>
        /// <param name="entity">Entity to serialize.</param>
        /// <returns>Serialized instance of T as byte array</returns>
        public static byte[] SerializeToByteArray(T entity)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, entity);
                byte[] buffer = new byte[stream.Length];
                stream.Seek(0, 0);
                stream.Read(buffer, 0, (int)stream.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Deserialize in binary mode stream to instance of T.
        /// </summary>
        /// <param name="stream">Binary stream to deserialize.</param>
        /// <returns>Instance of T.</returns>
        public static T Deserialize(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(stream);
        }

        /// <summary>
        /// Deserialize in binary mode byte array to instance of T.
        /// </summary>
        /// <param name="stream">Byte array to deserialize.</param>
        /// <returns>Instance of T.</returns>
        public static T Deserialize(byte[] buffer)
        {
            using (Stream stream = new MemoryStream(buffer))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}

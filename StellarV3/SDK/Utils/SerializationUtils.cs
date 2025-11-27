namespace StellarV3.SDK.Utils
{
    internal static class SerializationUtils
    {
        public static byte[] ToByteArray(object obj)
        {
            byte[] array;
            if (obj == null)
            {
                array = null;
            }
            else
            {
                #pragma warning disable SYSLIB0011
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                #pragma warning restore SYSLIB0011
                MemoryStream memoryStream = new MemoryStream();
                binaryFormatter.Serialize(memoryStream, obj);
                array = memoryStream.ToArray();
            }
            return array;
        }

        public static T IL2CPPFromByteArray<T>(byte[] data)
        {
            T t;
            if (data == null)
            {
                t = default;
            }
            else
            {
                Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter binaryFormatter = new Il2CppSystem.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                Il2CppSystem.IO.MemoryStream memoryStream = new Il2CppSystem.IO.MemoryStream(data);
                object obj = binaryFormatter.Deserialize(memoryStream);
                t = (T)obj;
            }
            return t;
        }

        public static T FromManagedToIL2CPP<T>(object obj)
        {
            return IL2CPPFromByteArray<T>(ToByteArray(obj));
        }
    }
}

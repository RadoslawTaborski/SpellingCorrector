using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.IO;

namespace SimilarWordsFinder.Serializers
{
    public class ProtocolBuffersSerializer : ISerializer
    {
        #region Public
        public byte[] Serialize<T>(T record) where T : class
        {
            if (null == record) return null;

            try
            {
                using (var stream = new MemoryStream())
                {
                    Serializer.Serialize(stream, record);
                    return stream.ToArray();
                }
            }
            catch
            {
                // Log error
                throw;
            }
        }

        public T Deserialize<T>(byte[] data) where T : class
        {
            if (null == data) return null;

            try
            {
                using (var stream = new MemoryStream(data))
                {
                    return Serializer.Deserialize<T>(stream);
                }
            }
            catch
            {
                // Log error
                throw;
            }
        }
        #endregion
    }
}

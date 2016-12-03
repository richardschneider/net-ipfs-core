using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Ipfs
{
    static class ProtobufHelper
    {
        static MethodInfo writeRawBytes = typeof(CodedOutputStream)
            .GetMethod("WriteRawBytes",
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new Type[] { typeof(byte[]) },
                null);
        static MethodInfo readRawBytes = typeof(CodedInputStream)
             .GetMethod("ReadRawBytes",
                 BindingFlags.NonPublic | BindingFlags.Instance,
                 null,
                 new Type[] { typeof(int) },
                 null);

        public static void WriteSomeBytes(this CodedOutputStream stream, byte[] bytes)
        {
            writeRawBytes.Invoke(stream, new object[] { bytes });
        }

        public static byte[] ReadSomeBytes(this CodedInputStream stream, int length)
        {
            return (byte[])readRawBytes.Invoke(stream, new object[] { length });
        }
    }
}

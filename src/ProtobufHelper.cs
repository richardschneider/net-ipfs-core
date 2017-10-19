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
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(m =>
                m.Name == "WriteRawBytes" && m.GetParameters().Count() == 1
            );
        static MethodInfo readRawBytes = typeof(CodedInputStream)
            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
            .Single(m =>
                m.Name == "ReadRawBytes"
            );

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

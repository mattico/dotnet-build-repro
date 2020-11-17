using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Serde.Tests {
    static class TraitHelpers {
        public static void serialize_tuple2_i64_u64((long, ulong) value, ISerializer serializer) {
            serializer.serialize_i64(value.Item1);
            serializer.serialize_u64(value.Item2);
        }

        public static (long, ulong) deserialize_tuple2_i64_u64(IDeserializer deserializer) {
            return (
                deserializer.deserialize_i64(),
                deserializer.deserialize_u64()
            );
        }

        public static void serialize_vector_u32(List<uint> value, ISerializer serializer) {
            serializer.serialize_len(value.Count);
            foreach (var item in value) {
                serializer.serialize_u32(item);
            }
        }

        public static List<uint> deserialize_vector_u32(IDeserializer deserializer) {
            long length = deserializer.deserialize_len();
            var obj = new List<uint>((int)length);
            for (long i = 0; i < length; i++) {
                obj.Add(deserializer.deserialize_u32());
            }
            return obj;
        }

    }


} // end of namespace Serde.Tests

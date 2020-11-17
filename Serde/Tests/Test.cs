using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Serde.Tests {

    public sealed class Test: IEquatable<Test> {
        public List<uint> a;
        public (long, ulong) b;
        public Choice c;

        public Test(List<uint> _a, (long, ulong) _b, Choice _c) {
            if (_a == null) throw new ArgumentNullException(nameof(_a));
            a = _a;
            b = _b;
            if (_c == null) throw new ArgumentNullException(nameof(_c));
            c = _c;
        }

        public void Serialize(ISerializer serializer) {
            serializer.increase_container_depth();
            TraitHelpers.serialize_vector_u32(a, serializer);
            TraitHelpers.serialize_tuple2_i64_u64(b, serializer);
            c.Serialize(serializer);
            serializer.decrease_container_depth();
        }

        public int LcsSerialize(byte[] outputBuffer) => LcsSerialize(new ArraySegment<byte>(outputBuffer));

        public int LcsSerialize(ArraySegment<byte> outputBuffer) {
            ISerializer serializer = new Lcs.LcsSerializer(outputBuffer);
            Serialize(serializer);
            return serializer.get_buffer_offset();
        }

        public byte[] LcsSerialize()  {
            ISerializer serializer = new Lcs.LcsSerializer();
            Serialize(serializer);
            return serializer.get_bytes();
        }

        public static Test Deserialize(IDeserializer deserializer) {
            deserializer.increase_container_depth();
            Test obj = new Test(
            	TraitHelpers.deserialize_vector_u32(deserializer),
            	TraitHelpers.deserialize_tuple2_i64_u64(deserializer),
            	Choice.Deserialize(deserializer));
            deserializer.decrease_container_depth();
            return obj;
        }

        public static Test LcsDeserialize(byte[] input) => LcsDeserialize(new ArraySegment<byte>(input));

        public static Test LcsDeserialize(ArraySegment<byte> input) {
            if (input == null) {
                 throw new DeserializationException("Cannot deserialize null array");
            }
            IDeserializer deserializer = new Lcs.LcsDeserializer(input);
            Test value = Deserialize(deserializer);
            if (deserializer.get_buffer_offset() < input.Count) {
                 throw new DeserializationException("Some input bytes were not read");
            }
            return value;
        }
        public override bool Equals(object obj) => obj is Test other && Equals(other);

        public static bool operator ==(Test left, Test right) => Equals(left, right);

        public static bool operator !=(Test left, Test right) => !Equals(left, right);

        public bool Equals(Test other) {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (!Enumerable.SequenceEqual(a, other.a)) return false;
            if (!b.Equals(other.b)) return false;
            if (!c.Equals(other.c)) return false;
            return true;
        }

        public override int GetHashCode() {
            unchecked {
                int value = 7;
                value = 31 * value + a.GetHashCode();
                value = 31 * value + b.GetHashCode();
                value = 31 * value + c.GetHashCode();
                return value;
            }
        }
    }

} // end of namespace Serde.Tests

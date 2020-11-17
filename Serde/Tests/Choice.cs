using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Numerics;

namespace Serde.Tests {

    public abstract class Choice: IEquatable<Choice> {

        public abstract void Serialize(ISerializer serializer);

        public static Choice Deserialize(IDeserializer deserializer) {
            int index = deserializer.deserialize_variant_index();
            switch (index) {
                case 0: return A.Load(deserializer);
                case 1: return B.Load(deserializer);
                case 2: return C.Load(deserializer);
                default: throw new DeserializationException("Unknown variant index for Choice: " + index);
            }
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

        public static Choice LcsDeserialize(byte[] input) => LcsDeserialize(new ArraySegment<byte>(input));

        public static Choice LcsDeserialize(ArraySegment<byte> input) {
            if (input == null) {
                 throw new DeserializationException("Cannot deserialize null array");
            }
            IDeserializer deserializer = new Lcs.LcsDeserializer(input);
            Choice value = Deserialize(deserializer);
            if (deserializer.get_buffer_offset() < input.Count) {
                 throw new DeserializationException("Some input bytes were not read");
            }
            return value;
        }
        public override int GetHashCode() {
            switch (this) {
            case A x: return x.GetHashCode();
            case B x: return x.GetHashCode();
            case C x: return x.GetHashCode();
            default: throw new InvalidOperationException("Unknown variant type");
            }
        }
        public override bool Equals(object obj) => obj is Choice other && Equals(other);

        public bool Equals(Choice other) {
            if (other == null) return true;
            if (ReferenceEquals(this, other)) return true;
            if (GetType() != other.GetType()) return false;
            switch (this) {
            case A x: return x.Equals((A)other);
            case B x: return x.Equals((B)other);
            case C x: return x.Equals((C)other);
            default: throw new InvalidOperationException("Unknown variant type");
            }
        }


        public sealed class A: Choice, IEquatable<A> {
            public A() {
            }

            public override void Serialize(ISerializer serializer) {
                serializer.increase_container_depth();
                serializer.serialize_variant_index(0);
                serializer.decrease_container_depth();
            }

            internal static A Load(IDeserializer deserializer) {
                deserializer.increase_container_depth();
                A obj = new A(
                	);
                deserializer.decrease_container_depth();
                return obj;
            }
            public override bool Equals(object obj) => obj is A other && Equals(other);

            public static bool operator ==(A left, A right) => Equals(left, right);

            public static bool operator !=(A left, A right) => !Equals(left, right);

            public bool Equals(A other) {
                if (other == null) return false;
                if (ReferenceEquals(this, other)) return true;
                return true;
            }

            public override int GetHashCode() {
                unchecked {
                    int value = 7;
                    return value;
                }
            }
        }

        public sealed class B: Choice, IEquatable<B> {
            public ulong Value;

            public B(ulong _Value) {
                Value = _Value;
            }

            public override void Serialize(ISerializer serializer) {
                serializer.increase_container_depth();
                serializer.serialize_variant_index(1);
                serializer.serialize_u64(Value);
                serializer.decrease_container_depth();
            }

            internal static B Load(IDeserializer deserializer) {
                deserializer.increase_container_depth();
                B obj = new B(
                	deserializer.deserialize_u64());
                deserializer.decrease_container_depth();
                return obj;
            }
            public override bool Equals(object obj) => obj is B other && Equals(other);

            public static bool operator ==(B left, B right) => Equals(left, right);

            public static bool operator !=(B left, B right) => !Equals(left, right);

            public bool Equals(B other) {
                if (other == null) return false;
                if (ReferenceEquals(this, other)) return true;
                if (!Value.Equals(other.Value)) return false;
                return true;
            }

            public override int GetHashCode() {
                unchecked {
                    int value = 7;
                    value = 31 * value + Value.GetHashCode();
                    return value;
                }
            }
        }

        public sealed class C: Choice, IEquatable<C> {
            public byte x;

            public C(byte _x) {
                x = _x;
            }

            public override void Serialize(ISerializer serializer) {
                serializer.increase_container_depth();
                serializer.serialize_variant_index(2);
                serializer.serialize_u8(x);
                serializer.decrease_container_depth();
            }

            internal static C Load(IDeserializer deserializer) {
                deserializer.increase_container_depth();
                C obj = new C(
                	deserializer.deserialize_u8());
                deserializer.decrease_container_depth();
                return obj;
            }
            public override bool Equals(object obj) => obj is C other && Equals(other);

            public static bool operator ==(C left, C right) => Equals(left, right);

            public static bool operator !=(C left, C right) => !Equals(left, right);

            public bool Equals(C other) {
                if (other == null) return false;
                if (ReferenceEquals(this, other)) return true;
                if (!x.Equals(other.x)) return false;
                return true;
            }

            public override int GetHashCode() {
                unchecked {
                    int value = 7;
                    value = 31 * value + x.GetHashCode();
                    return value;
                }
            }
        }
    }


} // end of namespace Serde.Tests

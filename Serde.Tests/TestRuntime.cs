
using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace Serde.Tests {
    public class TestRuntime {
        [Test]
        public void TestRoundTrip() {
            byte[] input = new byte[] {2, 4, 0, 0, 0, 6, 0, 0, 0, 253, 255, 255, 255, 255, 255, 255, 255, 5, 0, 0, 0, 0, 0, 0, 0, 2, 7};

            Test test = Test.LcsDeserialize(input);

            List<uint> a = new List<uint>(new uint[] { 4, 6 });
            var b = ((long)-3, (ulong)5);
            Choice c = new Choice.C((byte) 7);
            Test test2 = new Test(a, b, c);

            Assert.AreEqual(test, test2);

            byte[] output = test2.LcsSerialize();

            CollectionAssert.AreEqual(input, output);

            byte[] input2 = new byte[] {2, 4, 0, 0, 0, 6, 0, 0, 0, 253, 255, 255, 255, 255, 255, 255, 255, 5, 0, 0, 0, 0, 0, 0, 0, 2, 7, 1};
            Assert.Throws<DeserializationException>(() => Test.LcsDeserialize(input2));
        }
    }
}


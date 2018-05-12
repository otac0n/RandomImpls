// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls
{
    using System;
    using System.Linq;

    /// <summary>
    /// Reimplements the <see cref="Random"/> class, allowing derrived classes to produce random numbers from a simple sequence of bytes.
    /// </summary>
    public abstract class BytesRandom : Random
    {
        private const int DoubleSize = sizeof(double);
        private static readonly int DoubleExponentByteA;
        private static readonly int DoubleExponentByteB;

        static BytesRandom()
        {
            var bytes = BitConverter.GetBytes(1.0D);
            DoubleExponentByteA = Enumerable.Range(0, DoubleSize).Where(i => bytes[i] == 0x3F).Single();
            DoubleExponentByteB = Enumerable.Range(0, DoubleSize).Where(i => bytes[i] == 0xF0).Single();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BytesRandom"/> class.
        /// </summary>
        public BytesRandom()
        {
        }

        /// <inheritdoc />
        public override int Next() => this.Next(0, int.MaxValue);

        /// <inheritdoc />
        public override int Next(int maxValue) => this.Next(0, maxValue);

        /// <inheritdoc />
        public override int Next(int minValue, int maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException(nameof(maxValue));
            }
            else if (maxValue == minValue)
            {
                return minValue;
            }

            var diff = (ulong)maxValue - (ulong)minValue;
            if (diff <= byte.MaxValue)
            {
                var chop = (byte)(byte.MaxValue - (byte.MaxValue + 1) % diff);

                byte rand;
                do
                {
                    rand = this.NextByte();
                }
                while (rand > chop);

                return rand % (byte)diff + minValue;
            }
            else if (diff <= ushort.MaxValue)
            {
                var chop = (ushort)(ushort.MaxValue - (ushort.MaxValue + 1) % diff);

                ushort rand;
                do
                {
                    rand = this.NextUInt16();
                }
                while (rand > chop);

                return rand % (ushort)diff + minValue;
            }
            else
            {
                var chop = (uint)(uint.MaxValue - (uint.MaxValue + 1UL) % diff);

                uint rand;
                do
                {
                    rand = this.NextUInt32();
                }
                while (rand > chop);

                return (int)(rand % (uint)diff) + minValue;
            }
        }

        /// <inheritdoc />
        public abstract override void NextBytes(byte[] buffer);

        /// <inheritdoc />
        protected override double Sample()
        {
            // Read the double's data from the array.
            var doubleData = new byte[DoubleSize];
            this.NextBytes(doubleData);

            // Constrain the double to be in the range [1, 2).
            doubleData[DoubleExponentByteA] = 0x3F;
            doubleData[DoubleExponentByteB] = (byte)((doubleData[DoubleExponentByteB] & 0x0F) | 0xF0);

            // Convert the bits!
            var value = BitConverter.ToDouble(doubleData, 0);

            // Subtract one, to get the range [0, 1).
            return value - 1;
        }

        private byte NextByte()
        {
            var data = new byte[1];
            this.NextBytes(data);
            return data[0];
        }

        private ushort NextUInt16()
        {
            var data = new byte[2];
            this.NextBytes(data);
            return BitConverter.ToUInt16(data, 0);
        }

        private uint NextUInt32()
        {
            var data = new byte[4];
            this.NextBytes(data);
            return BitConverter.ToUInt32(data, 0);
        }
    }
}

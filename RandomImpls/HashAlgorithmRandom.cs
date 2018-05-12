// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Wraps an instance of <see cref="HashAlgorithm"/> and implements the <see cref="Random"/> class.
    /// </summary>
    /// <remarks>
    /// This class produces a stream of random bits based on a similar approach to the Counter (CTR) mode of a block cipher.
    /// The <c>seed</c> value is first hashed to produce an initialization vector.  Then, for each block of bytes requested,
    /// the IV is XORed with successive counter values of the same length as the IV.
    /// </remarks>
    public class HashAlgorithmRandom : BytesRandom
    {
        private readonly byte[] counter;
        private readonly HashAlgorithm hashAlgorithm;
        private readonly byte[] key;
        private byte[] currentBlock;
        private int currentOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="HashAlgorithmRandom"/> class.
        /// </summary>
        /// <param name="hashAlgorithm">The <see cref="HashAlgorithm"/> to use in generating random bytes.</param>
        /// <param name="seed">The seed bytes to use with the hash algorithm.</param>
        /// <exception cref="ArgumentNullException"><paramref name="hashAlgorithm"/> is null.</exception>
        public HashAlgorithmRandom(HashAlgorithm hashAlgorithm, byte[] seed = null)
        {
            this.hashAlgorithm = hashAlgorithm ?? throw new ArgumentNullException(nameof(hashAlgorithm));
            this.key = this.hashAlgorithm.ComputeHash(seed ?? new byte[0]);
            this.counter = new byte[this.key.Length];

            this.currentBlock = this.key;
            this.currentOffset = 0;
        }

        /// <inheritdoc />
        public override void NextBytes(byte[] buffer)
        {
            var offset = 0;
            while (offset < buffer.Length)
            {
                this.EnsureAvailable();

                var read = Math.Min(buffer.Length - offset, this.currentBlock.Length - this.currentOffset);
                Array.Copy(this.currentBlock, this.currentOffset, buffer, offset, read);
                this.currentOffset += read;
                offset += read;
            }
        }

        private void EnsureAvailable()
        {
            if (this.currentOffset >= this.currentBlock.Length)
            {
                var inc = true;
                for (var i = 0; i < this.counter.Length; i++)
                {
                    if (inc)
                    {
                        inc = this.counter[i]++ == 0;
                    }

                    this.currentBlock[i] = (byte)(this.key[i] ^ this.counter[i]);
                }

                this.currentBlock = this.hashAlgorithm.ComputeHash(this.currentBlock);
                this.currentOffset = 0;
            }
        }
    }
}

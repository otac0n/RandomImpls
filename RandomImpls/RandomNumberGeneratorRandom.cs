// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls
{
    using System;
    using System.Security.Cryptography;

    /// <summary>
    /// Wraps an instance of <see cref="RandomNumberGenerator"/> and implements the <see cref="Random"/> class.
    /// </summary>
    public class RandomNumberGeneratorRandom : BytesRandom
    {
        private readonly RandomNumberGenerator randomNumberGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomNumberGeneratorRandom"/> class, wrapping a given <see cref="RandomNumberGenerator"/> instance.
        /// </summary>
        /// <param name="randomNumberGenerator">The instance of <see cref="RandomNumberGenerator"/> to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="randomNumberGenerator"/> is null.</exception>
        public RandomNumberGeneratorRandom(RandomNumberGenerator randomNumberGenerator)
        {
            this.randomNumberGenerator = randomNumberGenerator ?? throw new ArgumentNullException(nameof(randomNumberGenerator));
        }

        public override void NextBytes(byte[] buffer)
        {
            this.randomNumberGenerator.GetBytes(buffer);
        }
    }
}

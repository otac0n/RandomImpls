// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls
{
    using System;

    /// <summary>
    /// Provides extension methods for instance of the <see cref="Random"/> class.
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random floating point number with the specified normal distribution.
        /// </summary>
        /// <param name="instance">An underlying <see cref="Random"/> instance to use for generation of values.</param>
        /// <param name="mean">The mean value of the distribution.</param>
        /// <param name="variance">The variance of the distribution.</param>
        /// <returns>A double precision floating point number from the specified normal distribution.</returns>
        public static double NextDoubleNormal(this Random instance, double mean, double variance)
        {
            var value = instance.NextDoubleNormal();
            return value * variance + mean;
        }

        /// <summary>
        /// Returns a random floating point number with a standard normal distribution.
        /// </summary>
        /// <param name="instance">An underlying <see cref="Random"/> instance to use for generation of values.</param>
        /// <returns>A double precision floating point number from the standard normal distribution.</returns>
        public static double NextDoubleNormal(this Random instance)
        {
            double u1, u2;
            do
            {
                u1 = instance.NextDouble();
                u2 = instance.NextDouble();
            }
            while (u1 <= double.Epsilon);

            return Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
        }
    }
}

// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides the interface of the <see cref="Random"/> class statically, using a thread-static backing instance.
    /// </summary>
    public static class StaticRandom
    {
        [ThreadStatic]
        private static Random instance;

        /// <summary>
        /// Gets a thread-static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <remarks>
        /// This instance should not be shared accross threads.  Instead, access this property statically.
        /// </remarks>
        public static Random Instance
        {
            get { return instance ?? (instance = new Random(Thread.CurrentThread.ManagedThreadId * 37 + Environment.TickCount)); }
        }

        /// <summary>
        /// Invokes the <see cref="Random.Next()"/> on a static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <returns>Returns the random integer returned from the static instance.</returns>
        public static int Next() => Instance.Next();

        /// <summary>
        /// Invokes the <see cref="Random.Next(int)"/> on a static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="maxValue">The maximum value to pass to the static instance.</param>
        /// <returns>Returns the random integer returned from the static instance.</returns>
        public static int Next(int maxValue) => Instance.Next(maxValue);

        /// <summary>
        /// Invokes the <see cref="Random.Next(int, int)"/> on a static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="minValue">The minimum value to pass to the static instance.</param>
        /// <param name="maxValue">The maximum value to pass to the static instance.</param>
        /// <returns>Returns the random integer returned from the static instance.</returns>
        public static int Next(int minValue, int maxValue) => Instance.Next(minValue, maxValue);

        /// <summary>
        /// Invokes the <see cref="Random.NextBytes(byte[])"/> on a static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <param name="buffer">The buffer to pass to the static instance.</param>
        public static void NextBytes(byte[] buffer) => Instance.NextBytes(buffer);

        /// <summary>
        /// Invokes the <see cref="Random.NextDouble()"/> on a static instance of the <see cref="Random"/> class.
        /// </summary>
        /// <returns>Returns the random floating-point number returned from the static instance.</returns>
        public static double NextDouble() => Instance.NextDouble();
    }
}

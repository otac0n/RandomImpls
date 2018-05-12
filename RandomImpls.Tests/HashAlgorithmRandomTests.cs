// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls.Tests
{
    using System.Security.Cryptography;
    using Xunit;

    public class HashAlgorithmRandomTests
    {
        [Fact]
        public void NextDouble_Always_ReturnsValuesWithinRange()
        {
            var rand = new HashAlgorithmRandom(new SHA256CryptoServiceProvider());
            var samples = 1000000;
            var failures = 0;
            for (var i = 0; i < samples; i++)
            {
                var value = rand.NextDouble();
                if (value >= 1 || value < 0)
                {
                    failures++;
                }
            }

            Assert.Equal(0, failures);
        }

        [Fact]
        public void Next_WithNoArguments_ReturnsPositiveValues()
        {
            var rand = new HashAlgorithmRandom(new SHA256CryptoServiceProvider());
            var samples = 10000;
            var failures = 0;
            for (var i = 0; i < samples; i++)
            {
                var value = rand.Next();
                if (value < 0)
                {
                    failures++;
                }
            }

            Assert.Equal(0, failures);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(100, 200)]
        [InlineData(-100, 100)]
        [InlineData(-200, 0)]
        [InlineData(byte.MinValue / 2, byte.MaxValue / 2)]
        [InlineData(byte.MinValue, byte.MaxValue)]
        [InlineData(short.MinValue / 2, short.MaxValue / 2)]
        [InlineData(short.MinValue, short.MaxValue)]
        [InlineData(int.MinValue / 2, int.MaxValue / 2)]
        [InlineData(int.MinValue, int.MaxValue)]
        public void Next_WithMinAndMaxValues_ReturnsValuesWithinTheRange(int minValue, int maxValue)
        {
            var rand = new HashAlgorithmRandom(new SHA256CryptoServiceProvider());
            var samples = 10000;
            var failures = 0;
            for (var i = 0; i < samples; i++)
            {
                var value = rand.Next(minValue, maxValue);
                if (value < minValue || value > maxValue || (value == maxValue && maxValue > minValue))
                {
                    failures++;
                }
            }

            Assert.Equal(0, failures);
        }
    }
}

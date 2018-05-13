// Copyright © John Gietzen. All Rights Reserved. This source is subject to the MIT license. Please see license.md for more information.

namespace RandomImpls.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Xunit;

    public class StaticRandomTests
    {
        [Fact]
        public void Next_SimultaneouslyOnSeveralThreads_YieldsUniqueSequences()
        {
            const int Samples = 32;
            var result = new List<string>();

            const int ThreadCount = 10;
            using (var semaphore = new Semaphore(0, ThreadCount))
            {
                var waiting = 0;

                void ThreadBody()
                {
                    var list = new List<int>(Samples);
                    Interlocked.Increment(ref waiting);
                    semaphore.WaitOne();
                    do
                    {
                        list.Add(StaticRandom.Next());
                    }
                    while (list.Count < Samples);

                    lock (result)
                    {
                        result.Add(string.Join(",", list));
                    }
                }

                var threads = Enumerable
                    .Range(0, ThreadCount)
                    .Select(i => new Thread(ThreadBody))
                    .ToArray();

                foreach (var t in threads)
                {
                    t.Start();
                }

                while (waiting < ThreadCount)
                {
                    Thread.Sleep(0);
                }

                semaphore.Release(ThreadCount);

                foreach (var t in threads)
                {
                    t.Join();
                }
            }

            var set = new HashSet<string>(result);
            Assert.Equal(ThreadCount, set.Count);
        }
    }
}

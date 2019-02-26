using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace WeightedRoundRobin
{
    class Program
    {
        static void Main(string[] args)
        {
            var dic = new ConcurrentDictionary<string, int>();
            var wr = new WeightedRoundRobin();
            Parallel.For(1, 1000001, (n) =>
            {
                var s = wr.GetServer();
                var key = $"Server:{s.IP},Weight:{s.Weight}";
                dic.AddOrUpdate(key, 1, (k, v) => v + 1);
            });

            foreach (var kvp in dic)
            {
                Console.WriteLine($"{kvp.Key} Processed {kvp.Value} Request");
            }

            Console.ReadKey();
        }
    }
}

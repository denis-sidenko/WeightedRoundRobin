using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace WeightedRoundRobin
{
    /// <summary>
    /// Round robin algorithm 
    /// </summary>
    public class WeightedRoundRobin
    {
        private readonly int _greatestCommonDivisor = 0;
        private readonly int _maxWeight = 0;
        private readonly int _nodesCount = 0;
        private int _currentDispatchWeight = 0;
        private int _lastChosenNode = -1;

        private SpinLock _sLock = new SpinLock(true);

        private readonly List<Server> _nodes = new List<Server>()
        {
            new Server()
            {
                IP = "192.168.0.100",
                Weight = 3
            },
            new Server()
            {
                IP = "192.168.0.101",
                Weight = 2
            },
            new Server()
            {
                IP = "192.168.0.102",
                Weight = 6
            },
            new Server()
            {
                IP = "192.168.0.103",
                Weight = 4
            },
            new Server()
            {
                IP = "192.168.0.104",
                Weight = 1
            },
        }.OrderBy(a => a.Weight).ToList();

        public WeightedRoundRobin()
        {
            _greatestCommonDivisor = GetGcd(_nodes);
            _maxWeight = GetMaxWeight(_nodes);
            _nodesCount = _nodes.Count;
        }

        /// <summary>
        /// An implementation of Weighted Round Robin
        /// </summary>
        /// <returns></returns>
        public Server GetServer()
        {
            var isLocked = false;
            _sLock.Enter(ref isLocked);

            do
            {
                _lastChosenNode = (_lastChosenNode + 1) % _nodesCount;
                if (_lastChosenNode == 0)
                {
                    _currentDispatchWeight = _currentDispatchWeight - _greatestCommonDivisor;
                    if (_currentDispatchWeight <= 0)
                    {
                        _currentDispatchWeight = _maxWeight;
                    }
                }
            } while (_nodes[_lastChosenNode].Weight < _currentDispatchWeight);

            if (isLocked)
            {
                _sLock.Exit(true);
            }

            return _nodes[_lastChosenNode];
        }

        /// <summary>
        /// Get Greatest Common Divisor
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        private int GetGcd(List<Server> servers)
        {
            return 1;
        }

        /// <summary>
        /// Get max weight
        /// </summary>
        /// <param name="servers"></param>
        /// <returns></returns>
        private int GetMaxWeight(List<Server> servers)
        {
            var max = 0;
            foreach (var s in servers)
            {
                if (s.Weight > max)
                    max = s.Weight;
            }
            return max;
        }
    }
}
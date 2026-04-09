using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreedyAlgs
{
    public class UnionFind
    {
        private readonly Dictionary<string, string> _parent = new Dictionary<string, string>();
        private readonly Dictionary<string, int> _rank = new Dictionary<string, int>();

        public void Add(string city)
        {
            _parent[city] = city;
            _rank[city] = 0;
        }

        public string Find(string city)
        {
            if (_parent[city] != city)
                _parent[city] = Find(_parent[city]);
            return _parent[city];
        }

        public bool Union(string a, string b)
        {
            string rootA = Find(a);
            string rootB = Find(b);

            if (rootA == rootB)
                return false;

            if (_rank[rootA] < _rank[rootB])
                (rootA, rootB) = (rootB, rootA);

            _parent[rootB] = rootA;

            if (_rank[rootA] == _rank[rootB])
                _rank[rootA]++;

            return true;
        }
    }

    public class DijkstraData
    {
        public string PreviousCity { get; set; } = string.Empty;
        public int Distance { get; set; } = 0;
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var cities = new List<string>
            {
                "Москва", "Тверь", "Новгород", "Рязань", "Ярославль", "Кострома", "Псков", "Питер"
            };

            var roads = new List<Edge>()
            {
                new Edge(){ From="Москва", To="Тверь", Weight=170},
                new Edge(){ From="Москва", To="Рязань", Weight=200},
                new Edge(){ From="Москва", To="Ярославль", Weight=260},
                new Edge(){ From="Тверь", To="Питер", Weight=480},
                new Edge(){ From="Тверь", To="Новгород", Weight=320},
                new Edge(){ From="Питер", To="Новгород", Weight=180},
                new Edge(){ From="Питер", To="Псков", Weight=280},
                new Edge(){ From="Новгород", To="Псков", Weight=210},
                new Edge(){ From="Ярославль", To="Кострома", Weight=90},
            };

            var adjacency = new Dictionary<string, List<Edge>>();
            foreach (var city in cities)
            {
                adjacency[city] = new List<Edge>();
            }

            foreach (var road in roads)
            {
                adjacency[road.From].Add(road);
                adjacency[road.To].Add(new Edge() { From=road.To, To=road.From, Weight=road.Weight });
            }

            string start = "Москва";

            var track = new Dictionary<string, DijkstraData>();

            var opened = new Dictionary<string, DijkstraData>();
            opened[start] = new DijkstraData() { PreviousCity = null, Distance = 0 };

            while (opened.Count > 0)
            {
                var currentCity = GetCityWithMinDistance(opened);
                Console.WriteLine($"Обрабатываем город {currentCity}. ({opened[currentCity].Distance} от {start})");

                track[currentCity] = opened[currentCity];
                opened.Remove(currentCity);

                foreach (var road in adjacency[currentCity])
                {
                    string neighbour = road.To;

                    if (track.ContainsKey(neighbour))
                        continue;

                    int newDistance = track[currentCity].Distance + road.Weight;

                    if (!opened.ContainsKey(neighbour) || newDistance < opened[neighbour].Distance)
                    {
                        opened[neighbour] = new DijkstraData()
                        {
                            PreviousCity = currentCity,
                            Distance = newDistance,
                        };
                        Console.WriteLine($"Обновляем {neighbour}: {newDistance} км через {currentCity}");
                    }
                }
            }

            foreach (var city in cities)
            {
                Console.WriteLine($"Кротчайшее расстояние от {start} до {city}: {track[city].Distance} км");
            }
        }

        static string GetCityWithMinDistance(Dictionary<string, DijkstraData> opened)
        {
            string bestCity = null;
            int bestDistance = int.MaxValue;

            foreach (var pair in opened)
            {
                if (pair.Value.Distance < bestDistance)
                {
                    bestDistance = pair.Value.Distance;
                    bestCity = pair.Key;
                }
            }

            return bestCity;
        }

        static List<Edge> FindMST(List<Edge> edges, List<string> cities)
        {
            var uf = new UnionFind();
            var mst = new List<Edge>();

            foreach (var city in cities)
                uf.Add(city);

            edges.Sort();

            foreach (var edge in edges)
            {
                if (uf.Union(edge.From, edge.To))
                {
                    mst.Add(edge);
                    Console.WriteLine($"Добавляем: {edge.From} - {edge.To}, {edge.Weight} км");

                    if (mst.Count == cities.Count - 1)
                        break;
                }
                else
                {
                    Console.WriteLine($"Пропускаем: {edge.From} - {edge.To}");
                }
            }

            return mst;
        }
    }
}

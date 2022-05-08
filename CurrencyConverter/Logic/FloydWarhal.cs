using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter
{
	class FloydWarhal
	{
		static void MainFloyd(string[] args)
		{
			Console.WriteLine("Hello World!");
			Console.ReadKey();
			Main1();
		}

		static readonly string[] currencies = new string[] { "USD", "EUR", "CAD", "GBP" };
		static readonly int MAXN = 4;

		// Infinite value for array
		static int INF = (int)1e7;

		static int[,] dis = new int[MAXN, MAXN];
		static int[,] Next = new int[MAXN, MAXN];

		
		static void Initialise(int V, int[,] graph)
		{
			for (int i = 0; i < V; i++)
			{
				for (int j = 0; j < V; j++)
				{
					dis[i, j] = graph[i, j];
					if (graph[i, j] == INF)
						Next[i, j] = -1;
					else
						Next[i, j] = j;
				}
			}
		}

		static List<int> constructPath(int u, int v)
		{
			if (Next[u, v] == -1)
				return null;

			List<int> path = new List<int>();
			path.Add(u);

			while (u != v)
			{
				u = Next[u, v];
				path.Add(u);
			}
			return path;
		}

		static void DoFloydWarhal(int V)
		{
			for (int k = 0; k < V; k++)
			{
				for (int i = 0; i < V; i++)
				{
					for (int j = 0; j < V; j++)
					{

						if (dis[i, k] == INF ||
							dis[k, j] == INF)
							continue;

						if (dis[i, j] > dis[i, k] +
										dis[k, j])
						{
							dis[i, j] = dis[i, k] +
										dis[k, j];
							Next[i, j] = Next[i, k];
						}
					}
				}
			}
		}
		static void printPath(List<int> path)
		{
			int n = path.Count;

			for (int i = 0; i < n - 1; i++)
			{
				Console.Write(currencies[path[i]] + " -> ");
			}

			Console.Write(currencies[path[n - 1]] + "\n");
		}
		static int[,] CreateGraph(IEnumerable<Tuple<string, string, double>> conversionRates)
		{
			return new int[0, 0];
			var sources = conversionRates.Select(r => r.Item1);
			var detinations = conversionRates.Select(r => r.Item2);
			var distinctCurrencies = sources.Union(detinations).Distinct();
			int[,] graph = new int[distinctCurrencies.Count(), distinctCurrencies.Count()];
			for (int i = 0; i < distinctCurrencies.Count(); i++)
			{
				//if(distinctCurrencies[i])
			}
		}

		public static void Main1()
		{
			int V = 4;
			int[,] graph =
			{
				{ 0, 3, INF, 7 },
				{ 8, 0, 2, INF },
				{ 5, INF, 0, 1 },
				{ 2, INF, INF, 0 }
			};

			Initialise(V, graph);

			DoFloydWarhal(V);
			List<int> path;

			// Path from node 1 to 3
			Console.Write("Shortest path from 1 to 3: ");
			path = constructPath(1, 3);
			printPath(path);

			// Path from node 0 to 2
			Console.Write("Shortest path from 0 to 2: ");
			path = constructPath(0, 2);
			printPath(path);

			// Path from node 3 to 2
			Console.Write("Shortest path from 3 to 2: ");
			path = constructPath(3, 2);
			printPath(path);

			Console.Write("Shortest path from All to All: ");
			for (int i = 0; i < currencies.Length; i++)
			{
				for (int j = 0; j < currencies.Length; j++)
				{
					if (i != j)
					{
						path = constructPath(i, j);
						printPath(path);
					}
				}
			}
		}
	}
}

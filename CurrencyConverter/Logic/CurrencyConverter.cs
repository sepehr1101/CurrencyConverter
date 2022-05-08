using CurrencyConverter.Entity;
using CurrencyConverter.Exceptions;
using Dijkstra.NET.Graph;
using Dijkstra.NET.ShortestPath;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyConverter.Logic
{
    public sealed class Converter : ICurrencyConverter
    {
		private static readonly Lazy<Converter> _instance = new Lazy<Converter>(() => new Converter());
		public static Converter _converter => _instance.Value;

		public Converter()
		{ 

		}

		private Graph<string, double> _graph;
		private IEnumerable<Tuple<string, string, double>> _conversionRates;
		private List<string> _currencies;
		private List<ConvertionResult> _convertionResultsCashe;
        
		public void ClearConfiguration()
        {
			_graph = null;
			_conversionRates = null;
			_convertionResultsCashe = null;
        }
        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
            var graphAndCurrencies = GetGraphAndCurrencies(conversionRates);
            _graph = graphAndCurrencies.Item1;
            _currencies = graphAndCurrencies.Item2;
            _conversionRates = conversionRates;
        }
        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            CheckConfig();
            var isReversed = false;
            var rateFromCache = GetRateFromCacheIfPossible(fromCurrency, toCurrency);
            if (rateFromCache.HasValue)
            {
                return amount * rateFromCache.Value;
            }
            uint[] path = GetTheShortestPath(fromCurrency, toCurrency, ref isReversed);
            double convertRate = GetConvertRate(isReversed, path);
            UpdateResultCashe(fromCurrency, toCurrency, convertRate);
            return convertRate * amount;
        }

        private uint[] GetTheShortestPath(string fromCurrency, string toCurrency, ref bool isReversed)
        {
            var result = _graph.Dijkstra((uint)_currencies.FindIndex(c => c == fromCurrency) + 1, (uint)_currencies.FindIndex(c => c == toCurrency) + 1);
            uint[] path = result.GetPath().ToArray();
            if (path != null && path.Length > 0)
            {
                return path;
            }
            isReversed = true;
            result = _graph.Dijkstra((uint)_currencies.FindIndex(c => c == toCurrency) + 1, (uint)_currencies.FindIndex(c => c == fromCurrency) + 1);
            path = result.GetPath().ToArray();
            if (path == null || !path.Any())
            {
                throw new NoPathException(fromCurrency, toCurrency);
            }
            return path;
        }
        private double GetConvertRate(bool isReversed, uint[] path)
        {
            double convertRate = 1;
            for (int i = 0; i < path.Length - 1; i++)
            {
                var currencyStartIndex = (int)path[i] - 1;
                var currencyEndIndex = (int)path[i + 1] - 1;
                var tuple = _conversionRates.Where(c =>
                     c.Item1 == _currencies.ElementAt(currencyStartIndex) &&
                     c.Item2 == _currencies.ElementAt(currencyEndIndex)).First();
                convertRate = isReversed ? convertRate / tuple.Item3 : convertRate * tuple.Item3;
                Console.Write(_currencies.ElementAt((int)path[i] - 1) + "-");
            }

            return convertRate;
        }       
		private (Graph<string, double>, List<string>) GetGraphAndCurrencies(IEnumerable<Tuple<string, string, double>> conversionRates)
        {
			var graph = new Graph<string, double>();
			var currencies = new List<string>();
			for (int i = 0; i < conversionRates.Count(); i++)
			{
				if (!currencies.Contains(conversionRates.ElementAt(i).Item1))
				{
					currencies.Add(conversionRates.ElementAt(i).Item1);
					graph.AddNode(currencies[i]);
				}
				if (!currencies.Contains(conversionRates.ElementAt(i).Item2))
				{
					currencies.Add(conversionRates.ElementAt(i).Item2);
					graph.AddNode(currencies[i]);
				}
				var currencySourceIndex = (uint)currencies.FindIndex(c => c == conversionRates.ElementAt(i).Item1);
				var currencyDestinationIndex = (uint)currencies.FindIndex(c => c == conversionRates.ElementAt(i).Item2);
				graph.Connect(currencySourceIndex + 1, currencyDestinationIndex + 1, i + 2, i);
			}
			return (graph, currencies);
		}
		private void UpdateResultCashe(string @from,string @to, double rate)
        {
            if (_convertionResultsCashe == null)
            {
				_convertionResultsCashe = new List<ConvertionResult>();
            }
			if (!_convertionResultsCashe.Any(c => c.Source == from && c.Destination == to))
			{
				_convertionResultsCashe.Add(new ConvertionResult()
				{
					Destination = to,
					Rate = rate,
					Source = from
				});
			}
        }
		private double? GetRateFromCacheIfPossible(string @from, string to)
		{
            if (_convertionResultsCashe == null)
            {
				return null;
            }
			var fromCache = _convertionResultsCashe.FirstOrDefault(c => c.Source == from && c.Destination == to);
            if (fromCache != null)
            {
				return fromCache.Rate;
            }
			return null;
		}
        private void CheckConfig()
        {
            if(_conversionRates==null || !_conversionRates.Any())
            {
                throw new NoConfigException(nameof(_conversionRates));
            }
        }
    }
}

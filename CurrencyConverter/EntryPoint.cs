using CurrencyConverter.Logic;
using System;
using System.Collections.Generic;

namespace CurrencyConverter
{
    public class EntryPoint
    {
		private readonly ICurrencyConverter _converter;
        public EntryPoint(ICurrencyConverter currencyConverter)
        {
			_converter = currencyConverter;
        }
        public void Run(string[] args)
        {
            List<Tuple<string, string, double>> conversionRates = GetTuples();
            _converter.UpdateConfiguration(conversionRates);
            Console.WriteLine("Please Enter Your Source And Destination Currencies And The Desired Amount...");
            while (true)
            {                
                string @from= Console.ReadLine();
                string to = Console.ReadLine();
                double amount = Convert.ToDouble(Console.ReadLine());
                var convertedAmount= _converter.Convert(from, to, amount);
                Console.WriteLine($"{Environment.NewLine} {amount} in {from} would be {convertedAmount} in {to}");
                Console.WriteLine($"---------------------------------------------------------------------{Environment.NewLine}");
                //Check unit tests for more
            }
        }

        private List<Tuple<string, string, double>> GetTuples()
        {
            return new List<Tuple<string, string, double>>()
            {
                new Tuple<string, string, double>("USD","CAD",1.2),
                new Tuple<string, string, double>("USD","RLS",1.3),
                new Tuple<string, string, double>("CAD","X",1.4),
                new Tuple<string, string, double>("RLS","X",1.5),
                new Tuple<string, string, double>("RLS","Q",1.6),
                new Tuple<string, string, double>("X","W",1.7)
            };
        }
    }
}

using System;
using Xunit;
using CurrencyConverter.Logic;
using System.Collections.Generic;

namespace CurrencyConverter.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test_Main_Usage_1()
        {
            ICurrencyConverter converter = new Converter();
            var conversionRates = GetTuples();
            converter.UpdateConfiguration(conversionRates);
            var result = converter.Convert("USD", "W", 1000);

            //USD -> CAD -> X -> W 
            // 1000*1.2*1.4*1.7 = 2856
            Assert.Equal(2856, result);
        }

        [Fact]
        public void Test_Main_Usage_2()
        {
            ICurrencyConverter converter = new Converter();
            var conversionRates = GetTuples();
            converter.UpdateConfiguration(conversionRates);
            var result = converter.Convert("CAD", "W", 1000);

            // CAD -> X -> W 
            // 1000*1.4*1.7 = 2380
            Assert.Equal(2380, result);
        }

        [Fact]
        public void Test_Reverse()
        {
            ICurrencyConverter converter = new Converter();
            var conversionRates = GetTuples();
            converter.UpdateConfiguration(conversionRates);
            var result = converter.Convert("X", "USD", 1000);

            // X -> CAD -> USD 
            // 1000/1.2/1.4 = 595.2380952380953
            Assert.Equal(595.24, result,2); 
        }

        [Fact]
        public void Test_Same_Currency()
        {
            ICurrencyConverter converter = new Converter();
            var conversionRates = GetTuples();
            converter.UpdateConfiguration(conversionRates);
            var result = converter.Convert("USD", "USD", 1000);

            // USD -> USD 
            // 1000
            Assert.Equal(1000, result);
        }

        [Fact]
        public void Test_Same_Currency_Unreachable()
        {
            ICurrencyConverter converter = new Converter();
            var conversionRates = GetTuples();
            converter.UpdateConfiguration(conversionRates);
            var result = converter.Convert("W", "W", 1000);

            // W -> W 
            // 1000
            Assert.Equal(1000, result);
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

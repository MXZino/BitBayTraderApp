using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BitBayTraderApp.Shared.Models.DTO.PublicRest
{
    public class CurrentStatus
    {
        public string Status { get; set; }
        public Ticker Ticker { get; set; }
    }

    public class First
    {
        /// <summary>
        /// Kod waluty z wybranego rynku.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Minimalna wartość waluty jaką można wystawić.
        /// </summary>
        public double MinOffer { get; set; }

        /// <summary>
        /// Ilość miejsc po przecinku obsługiwana przez daną walutę.
        /// </summary>
        public int Scale { get; set; }
    }

    public class Second
    {
        /// <summary>
        /// Kod waluty z wybranego rynku.
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Minimalna wartość waluty jaką można wystawić.
        /// </summary>
        public double MinOffer { get; set; }

        /// <summary>
        /// Ilość miejsc po przecinku obsługiwana przez daną walutę.
        /// </summary>
        public int Scale { get; set; }
    }

    public class Market
    {
        /// <summary>
        /// Kod rynku dla którego zostało wykonane zapytanie.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tablica informacji pierwszej waluty z pary
        /// </summary>
        public First First { get; set; }

        /// <summary>
        /// Tablica informacji drugiej waluty z pary
        /// </summary>
        public Second Second { get; set; }
    }

    public class Ticker
    {
        public Market Market { get; set; }

        /// <summary>
        /// Czas aktualizacji danych.
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// Aktualnie najlepszy kurs dla ofert kupna.
        /// </summary>
        public double HighestBid { get; set; }

        /// <summary>
        /// Aktualnie najlepszy kurs dla ofert sprzedaży.
        /// </summary>
        public double LowestAsk { get; set; }

        /// <summary>
        /// Kurs ostatniej transakcji.
        /// </summary>
        public double Rate { get; set; }

        /// <summary>
        /// Kurs przedostatniej transakcji.
        /// </summary>
        public double PreviousRate { get; set; }
    }
}

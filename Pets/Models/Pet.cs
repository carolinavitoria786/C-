using System;

namespace PetsApi.Models
{
    public class Animal
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public int idade { get; set; }
        public string? cor { get; set; }
        public string? tipo { get; set; }
        public decimal peso_kg { get; set; }
        public bool vacinado { get; set; }

    }
}
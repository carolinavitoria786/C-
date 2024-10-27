using System;

namespace PizzaStoreApi.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Tamanho { get; set; }
        public decimal Preco { get; set; }

    }
}
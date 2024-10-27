using System;

namespace BibliotecaApi.Models

{
    public class Biblioteca
    {
        public int id { get; set; }
        public string? nome { get; set; }
        public string? inicio_funcionamento { get; set; }
        public string? fim_funcionamento { get; set; }
        public string? inauguracao { get; set; }
        public string? contato { get; set; }
    }

}
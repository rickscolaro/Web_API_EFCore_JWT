using System.ComponentModel.DataAnnotations;

namespace APICatalogo.ViewModel {
    public class CreateProdutoViewModel {


        [Required]
        [MaxLength(80)]
        public string Nome { get; set; }

        [Required]
        [MaxLength(300)]
        public string Descricao { get; set; }

        [Required]
        public decimal Preco { get; set; }

        [Required]
        [MaxLength(500)]
        public string ImagemUrl { get; set; }

        public float Estoque { get; set; }

        public DateTime DataCadastro { get; set; }

        public int CategoriaId { get; set; } // Chave estrangeira
    }
}
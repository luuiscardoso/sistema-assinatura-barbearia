using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Domain.DTO
{
    public record SearchFilterDTO
    {
        /// <summary>
        /// CPF do cliente contendo exatamente 11 dígitos numéricos.
        /// </summary>
        /// <example>12345678901</example>
        [StringLength(11)]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF vazio ou com tamanho inválido.")]
        public string? Cpf { get; init; }

        /// <summary>
        /// Nome do cliente ou parte do nome.
        /// </summary>
        /// <example>João</example>
        [StringLength(80)]
        public string? Nome { get; init; }

        /// <summary>
        /// E-mail do cliente.
        /// </summary>
        /// <example>cliente@email.com</example>
        [EmailAddress(ErrorMessage = "Insira um e-mail válido.")]
        public string? Email { get; init; }

        /// <summary>
        /// Data inicial para o filtro de criação da assinatura.
        /// </summary>
        /// <example>2024-01-01</example>
        public DateTime? DataInicio { get; init; }

        /// <summary>
        /// Data final para o filtro de criação da assinatura.
        /// </summary>
        /// <example>2024-12-31</example>
        public DateTime? DataFinal { get; init; }

        /// <summary>
        /// Número da página para paginação dos resultados.
        /// </summary>
        /// <example>1</example>
        public required int PageNumber { get; init; }
    }

}

namespace APIAssinaturaBarbearia.DTO
{
    public class TokenResponseDTO
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiracao { get; set; }
    }
}

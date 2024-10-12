﻿using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.DTO
{
    public class TokenDTO
    {
        [Required(ErrorMessage = "Token obrigatório.")]
        public string TokenPrincipal { get; set; }

        [Required(ErrorMessage = "Refresh Token obrigatório.")]
        public string RefreshToken { get; set; }
    }
}

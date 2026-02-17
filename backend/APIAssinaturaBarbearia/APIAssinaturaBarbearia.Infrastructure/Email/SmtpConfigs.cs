using System.ComponentModel.DataAnnotations;

namespace APIAssinaturaBarbearia.Infrastructure.Email
{
    public class SmtpConfigs
    {

        [Required(AllowEmptyStrings = false)]
        public required string Remetente { get; set; }

        [Required(AllowEmptyStrings = false)]
        public required string Senha { get; set; }
    }
}

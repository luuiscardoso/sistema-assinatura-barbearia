using APIAssinaturaBarbearia.Application.Interfaces;
using APIAssinaturaBarbearia.Controllers;
using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Infrastructure.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using TestesAPI.IntegrationTests.CustomFactoryConfig;

namespace TestesAPI.IntegrationTests
{
    public class AssinaturaControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>, IAsyncLifetime
    {
        private readonly HttpClient _httpClient;
        private readonly BdContext _context;
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly IConfiguration _configuration;

        public AssinaturaControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _httpClient = factory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:5001/");
            var scope = factory.Services.CreateScope();

            IServiceProvider serviceProvider = scope.ServiceProvider;
            _context = serviceProvider.GetRequiredService<BdContext>();
            _tokenService = serviceProvider.GetRequiredService<ITokenService>();

            _configuration = serviceProvider.GetRequiredService<IConfiguration>();
            _userService = serviceProvider.GetRequiredService<IUserService>();
            _adminService = serviceProvider.GetRequiredService<IAdminService>();
        }

        public async Task<OkObjectResult> RealizarLoginAuxiliar()
        {
            LoginDTO loginDTO = new LoginDTO()
            {
                Email = "teste@gmail",
                Senha = "@mM12345678"
            };
            var authController = new AuthController(_userService, _adminService);
            OkObjectResult? result = (await authController.Login(loginDTO)) as OkObjectResult;

            return result;
        }

        [Fact]
        public async Task ObterTodasAssinaturas_RetornaStatusCode200()
        {
            //Arrange
            TokenResponseDTO? tokenObject = (await RealizarLoginAuxiliar()).Value as TokenResponseDTO;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObject.Token);

            //Act
            HttpResponseMessage response = await _httpClient.GetAsync("subscriptions?numeroPagina=1");

            //Assert 
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task ObterAssinaturaPorId_InformandoIdAssinaturaExistente_RetornaStatusCode200()
        {
            //Arrange
            TokenResponseDTO? tokenObject = (await RealizarLoginAuxiliar()).Value as TokenResponseDTO;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObject.Token);

            Assinatura assinatura = (_context.Assinaturas.ToList())[0];
            var id = assinatura.AssinaturaId;

            //Act
            HttpResponseMessage response = await _httpClient.GetAsync($"subscriptions/{id}");
            var content = await response.Content.ReadAsStringAsync();
            Assinatura? assinaturaRetornada = JsonConvert.DeserializeObject<Assinatura>(content);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(assinaturaRetornada);
            Assert.Equal(assinatura.AssinaturaId, assinaturaRetornada.AssinaturaId);
        }

        [Fact]
        public async Task CriarAssinatura_InformandoDadosClienteValidos_RetornaStatusCode201()
        {
            //Arrange 
            TokenResponseDTO? tokenObject = (await RealizarLoginAuxiliar()).Value as TokenResponseDTO;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObject.Token);

            ClienteCadastroDTO clienteDTO = new ClienteCadastroDTO() { Cpf = "23457062324", Nome = "Teste" };

            StringContent content = new StringContent(JsonConvert.SerializeObject(clienteDTO), Encoding.UTF8, "application/json");

            //Act
            HttpResponseMessage response = await _httpClient.PostAsync("subscriptions", content);
            var assinaturaCriada = _context.Assinaturas.Include(a => a.Cliente).Single(a => a.Cliente.Cpf == clienteDTO.Cpf);
            var clienteCriado = assinaturaCriada.Cliente;


            //Arrange
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(assinaturaCriada.AssinaturaId, clienteCriado.AssinaturaId);
        }

        [Fact]
        public async Task AtualizaTodaAssinatura_InformandoDadosValidos_RetornaStatusCodeNoContent()
        {
            //Arrange 
            TokenResponseDTO? tokenObject = (await RealizarLoginAuxiliar()).Value as TokenResponseDTO;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObject.Token);

            var assinatura = (_context.Assinaturas.Include(a => a.Cliente).ToList())[0];
            var id = assinatura.AssinaturaId;

            var patchDoc = new JsonPatchDocument<AssinaturaUpdateDTO>();
            patchDoc.Replace(x => x.Cpf, "12345543219");
            patchDoc.Replace(x => x.Nome, "Novo nome");
            patchDoc.Replace(x => x.Fim, DateTime.Now.AddMonths(2));
            patchDoc.Replace(x => x.Status, true);

            var serializedContent = JsonConvert.SerializeObject(patchDoc);
            StringContent content = new StringContent(serializedContent, Encoding.UTF8, "application/json-patch+json");

            //Act
            var response = await _httpClient.PatchAsync($"subscriptions/{id}", content);
            var assinaturaAtualizada = _context.Assinaturas.AsNoTracking()
                                                           .Include(a => a.Cliente)
                                                           .Single(a => a.AssinaturaId == assinatura.AssinaturaId);

            bool atualizou = assinatura.Cliente.Cpf != assinaturaAtualizada.Cliente.Cpf 
                             && assinatura.Cliente.Nome != assinaturaAtualizada.Cliente.Nome
                             && assinatura.Fim != assinaturaAtualizada.Fim
                             && assinatura.Status != assinaturaAtualizada.Status;

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(atualizou);
        }

        [Fact]
        public async Task ExcluiAssinatura_InformandoAssinaturaExistente_RetornaStatusCode201()
        {
            //Arrange
            TokenResponseDTO? tokenObject = (await RealizarLoginAuxiliar()).Value as TokenResponseDTO;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenObject.Token);

            var assinaturaExcluir = (_context.Assinaturas.ToList())[0];
            var id = assinaturaExcluir.AssinaturaId;

            //Act
            var response = await _httpClient.DeleteAsync($"subscriptions/{id}");
            bool excluiu = (_context.Assinaturas.AsNoTracking().FirstOrDefault(a => a.AssinaturaId == id)) is null;

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.True(excluiu);
        }

        public async Task InitializeAsync()
        {
            List<Assinatura> assinaturas = new List<Assinatura>()
            {
                new Assinatura(DateTime.Now, DateTime.Now.AddMonths(1), false),
                new Assinatura(DateTime.Now, DateTime.Now.AddMonths(1), true)
            };

            List<Cliente> clientes = new List<Cliente>()
            {
                new Cliente(){Nome = "Teste1", Cpf = "12345678900", Assinatura = assinaturas[0]},
                new Cliente(){Nome = "Teste2", Cpf = "32145478922", Assinatura = assinaturas[1]}
            };
            //await _adminService.CriarPerfilAsync("Admin");
            await _adminService.CriarUsuarioAsync(new UsuarioCadastroDTO { Cpf = "12345678901", Email = "teste@gmail", Nome = "teste", Senha = "@mM12345678" });
            //await _adminService.AssociarPerfilUsuarioAsync("teste@gmail", "Admin");
            await _context.Assinaturas.AddRangeAsync(assinaturas);
            await _context.Clientes.AddRangeAsync(clientes);
            await _context.SaveChangesAsync();
        }

        public async Task DisposeAsync()
        {
            await _context.Database.EnsureDeletedAsync();
            await _context.DisposeAsync();
        }
    }
}


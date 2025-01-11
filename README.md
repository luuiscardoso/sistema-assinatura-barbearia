#  API RESTFull de controle de clientes e planos de assinatura de cortes de cabelo para um barbeiro autonomo.  

Projeto de desenvolvimento que surgiu a partir de uma conversa entre mim e meu melhor amigo, que também é barbeiro e autônomo. Durante a conversa falamos sobre a sua dificuldade em implementar um modelo de assinatura de cortes devido a problemas relacionados ao gerenciamento dessas assinaturas, clientes, faturas, datas etc. Pensando nisso, tirei a ideia do papel a partir da identificação dos requisitos, escolha da arquitetura e design patterns.   

## Principais tecnologias

- **C#**
- **.NET 8**
- **SQL Server**
- **Entity Framework**
- **xUnit**
- **ASP.NET Identity**
- **Swagger**

## Arquitetura e design patterns
- **Clean Architecture** 
- **Repository** 
- **Unity of work** 

## Serviços Implementadas  

- Gerenciamento de clientes e assinaturas.  
- Controle de datas de vencimento, renovação e pagamento.  
- Sistema seguro de autenticação e autorização com tokens JWT e Identity.  
- Tratamento de exceções com filtros e middlewares personalizados.  
- Documentação detalhada e interativa com Swagger.
- Mecanismo de reset e recuperação de senha com integração com e-mail.
- Testes de unidade e integração.

## 🚀 Começando
### Pré-requisitos

Para rodar o projeto localmente, tenha instalado o .NET SDK e o SQL Server com uma instancia rodando em sua máquina. Em seguida, crie um banco de dados para receber o esquema de dados do projeto posteriormente. Após isso, clone o repositório, navegue até o diretório que contem a Program.cs (APIAssinaturaBarbearia) e crie os arquivos appsettings.json, appsettings.Development.json e appsettings.Test.json, substituindo os valores de exemplo pelos seus. Configurações feitas, navegue até o diretório contendo a sln e execute o comando dotnet restore para baixar todas as dependencias do projeto. Finalmente, execute o comando dotnet ef database update para construir o esquema no seu banco de dados e o comando dotnet run para rodar o projeto com toda documentação do swagger. O projeto deverá estar acessível no seu navegador, geralmente em http://localhost:5000, https://localhost:5001 ou outra URL de lauchSettings.json.

```bash
git clone https://github.com/luuiscardoso/sistema-assinatura-barbearia.git
```

```bash
1º cd APIAssinaturaBarbearia
2º dotnet restore
3º dotnet ef database update (certifique de ter criado os arquivos appsettings antes)
4º dotnet run
```

```
appsettings.json:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=seuserver;Database=seubanco;Integrated Security=SSPI;TrustServerCertificate=True"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information",
      "Microsoft.AspNetCore.Routing": "Debug"
    }
  },
  "JWT": {
    "ChaveSecreta": "suachave",
    "ValidadeTokenMinutos": 2,
    "ValidadeRefreshTokenMinutos": 30
  },
  "RateLimiter": {
    "LimiteRequisicoes": 1,
    "Janela": 3,
    "LimiteFila": 0
  },
  "Email": {
    "Remetente": "seuemail",
    "Nome": "seunome",
    "Senha": "sua senha de aplicativo (gmail) ou senha normal",
    "Server": "smptserver",
    "Port": porta do seu smtp server,
    "Security": "security do seu smtp server"
  },
  "AllowedHosts": "*"
}

appsettings.Development.json:

{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}

appsettings.Test.json:

{
  "ConnectionStrings": {
    "TestConnection": "Server=seuserver;Database=seubancodetestes;Integrated Security=SSPI;TrustServerCertificate=True"
  },
  "Kestrel": {
    "Endpoints": {
      "Https": {
        "Url": "https://localhost:5001"
      }
    }
  },
  "RateLimiter": {
    "LimiteRequisicoes": 50,
    "Janela": 5,
    "LimiteFila":  0
  }
}
```

## Próximos Passos 

1. Desenvolver o **front-end** com **React** (em andamento).   
2. Implementar implantação na **nuvem** mediante estudo de uso do **Docker** e **Azure Kubernetes Service (AKS)**.  
3. Configurar pipelines de CI/CD para automação de deploys com GitHub Actions.  


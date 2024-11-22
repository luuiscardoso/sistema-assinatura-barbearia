#  MVP - Controle de clientes e planos de assinatura para corte de cabelo para um barbeiro autonomo.  

Nos ultimos meses, venho trabalhando em um projeto em desenvolvimento que surgiu a partir de uma conversa entre mim e meu melhor amigo, que também é barbeiro e autônomo. Durante a conversa falamos sobre a sua dificuldade em implementar um modelo de assinatura de cortes devido a problemas relacionados ao gerenciamento dessas assinaturas, clientes, faturas, datas etc. Pensando nisso, tirei a ideia do papel a partir da identificação dos requisitos, escolha da arquitetura e design patterns.   

## Tecnologias 

- **Back-end:** 
  - **ASP.NET Core:** Framework principal para construção da API com C#.  
  - **Autenticação e Autorização:** JWT e ASP.NET Identity para login do barbeiro.  
  - **Banco de Dados:** SQL Server, gerenciado com o ORM Entity Framework Core.
  - **Testes:** xUnit para os testes de unidade e integração.  

- **Arquitetura e design patterns:**
  - **Clean Architecture:** Para transferência de dados entre camadas, promovendo encapsulamento.  
  - **Padrão Repository:** Tratamento centralizado de erros, oferecendo respostas padronizadas e uma experiência aprimorada ao cliente.  
  - **Unity of work:** Abordagem modular para facilitar a manutenção e expansão de funcionalidades.  

## Funcionalidades Implementadas  

- Gerenciamento de clientes e assinaturas.  
- Controle de datas de vencimento e renovação.  
- Sistema seguro de autenticação e autorização com tokens JWT e Identity.  
- Tratamento de exceções com filtros e middlewares personalizados.  
- Documentação detalhada e interativa com Swagger.  

## Próximos Passos e melhorias futuras a serem desenvolvidas

1. Concluir a migração para **Clean Architecture**.  
2. Expandir os **testes unitários** e iniciar os **testes de integração**.  
3. Desenvolver o **front-end** com **React** (em andamento).  
4. Adicionar novas funcionalidades e suporte a regras de negócio avançadas.  
5. Implementar implantação na **nuvem** mediante estudo de uso do **Docker** e **Azure Kubernetes Service (AKS)**.  
6. Configurar pipelines de CI/CD para automação de deploys com GitHub Actions.  


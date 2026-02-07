using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace APIAssinaturaBarbearia.Infrastructure.Extensions;

public static class SearchFilterExtension
{
    public static IQueryable<Assinatura> Search(this IQueryable<Assinatura> query, SearchFilterDTO searchFilterDTO)
    {
        if (!string.IsNullOrWhiteSpace(searchFilterDTO.Nome))
            query = query.Where(p =>
                EF.Functions.Like(p.Cliente!.Nome, $"%{searchFilterDTO.Nome}%"));

        if (!string.IsNullOrWhiteSpace(searchFilterDTO.Cpf))
            query = query.Where(p => p.Cliente!.Cpf == searchFilterDTO.Cpf);

        if (searchFilterDTO.DataInicio is not null)
            query = query.Where(p => p.Inicio >= searchFilterDTO.DataInicio);

        if (searchFilterDTO.DataFinal is not null)
            query = query.Where(p => p.Fim <= searchFilterDTO.DataFinal);

        return query;
    }
}

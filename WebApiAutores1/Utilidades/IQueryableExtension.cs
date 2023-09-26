using WebApiAutores1.DTOs;

namespace WebApiAutores1.Utilidades
{
    public static class IQueryableExtension
    {
        public static IQueryable<T> Pagina<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO) 
        {
            return queryable
                .Skip((paginacionDTO.Pagina - 1) * paginacionDTO.RecordsPorPagina)
                .Take(paginacionDTO.RecordsPorPagina);
        }
    }
}

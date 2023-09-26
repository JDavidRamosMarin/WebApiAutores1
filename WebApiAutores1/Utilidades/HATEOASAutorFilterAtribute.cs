using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApiAutores1.DTOs;
using WebApiAutores1.Servicios;

namespace WebApiAutores1.Utilidades
{
    public class HATEOASAutorFilterAtribute: HATEOASFiltroAtribute
    {
        private readonly GeneradorEnlaces generadorEnlaces;

        public HATEOASAutorFilterAtribute(GeneradorEnlaces generadorEnlaces)
        {
            this.generadorEnlaces = generadorEnlaces;
        }

        public override async Task OnResultExecutionAsync(ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            var debeIncluir = DebeIncluirHATEOAS(context);

            if (!debeIncluir)
            {
                await next();
                return;
            }

            var resultado = context.Result as ObjectResult;
            //var modelo = resultado.Value as AutorDTO ?? throw new
            //    ArgumentNullException("Se esperaba una instancia de AutorDTO");
            var autorDTO = resultado.Value as AutorDTO;
            if (autorDTO == null)
            {
                var autoresDTO = resultado.Value as List<AutorDTO> ?? 
                    throw new ArgumentException("Se esperaba una instancia de AutoresDTo o List<AutoresDTO>");

                autoresDTO.ForEach(async autor => await generadorEnlaces.GenerarEnlaces(autor));
                resultado.Value = autoresDTO;
            }
            else
            {
                await generadorEnlaces.GenerarEnlaces(autorDTO);
            }
            await next();

        }
    }
}

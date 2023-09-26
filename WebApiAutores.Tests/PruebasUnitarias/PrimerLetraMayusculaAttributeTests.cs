using System.ComponentModel.DataAnnotations;
using WebApiAutores1.Validaciones;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class PrimerLetraMayusculaAttributeTests
    {
        [TestMethod]
        public void PrimeraLetraMayuscula_DevuelveError()
        {
            // Prueba
            var primeraLetraMayuscula = new PrimerLetraMayusculaAttribute();
            var valor = "felipe";
            var valContext = new ValidationContext(new { Nombre = valor });

            // Ejecucion 
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            // Verificacion
            Assert.AreEqual("La primera letra debe ser mayuscula", resultado?.ErrorMessage);
        }

        [TestMethod]
        public void ValorNulo_DevuelveError()
        {
            // Prueba
            var primeraLetraMayuscula = new PrimerLetraMayusculaAttribute();
            string ?valor = null;
            var valContext = new ValidationContext(new { Nombre = valor });

            // Ejecucion 
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            // Verificacion
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void ValorConPrimeraLetraMayuscula_DevuelveError()
        {
            // Prueba
            var primeraLetraMayuscula = new PrimerLetraMayusculaAttribute();
            var valor = "Felipe";
            var valContext = new ValidationContext(new { Nombre = valor });

            // Ejecucion 
            var resultado = primeraLetraMayuscula.GetValidationResult(valor, valContext);

            // Verificacion
            Assert.IsNull(resultado);
        }
    }
}
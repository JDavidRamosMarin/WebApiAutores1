﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiAutores.Tests.Mocks;
using WebApiAutores1.Controllers.V1;

namespace WebApiAutores.Tests.PruebasUnitarias
{
    [TestClass]
    public class RootControllerTests
    {
        [TestMethod]
        public async Task SiUsuarioEsAdmin_Obtener4Links()
        {
            // Preparacion
            var authorizationService = new AuthorizationSserviceSuccesedMock();
            authorizationService.resultado = AuthorizationResult.Success();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();

            // Ejecución
            var resultado = await rootController.Get();

            // Verificación
            Assert.AreEqual(4, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtener4Links()
        {
            // Preparacion
            var authorizationService = new AuthorizationSserviceSuccesedMock();
            authorizationService.resultado = AuthorizationResult.Failed();
            var rootController = new RootController(authorizationService);
            rootController.Url = new URLHelperMock();

            // Ejecución
            var resultado = await rootController.Get();

            // Verificación
            Assert.AreEqual(2, resultado.Value.Count());
        }

        [TestMethod]
        public async Task SiUsuarioNoEsAdmin_Obtener4Links_UsandoMoq()
        {
            // Preparacion
            var mockAuthorizationService = new Mock<IAuthorizationService>();
            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<IEnumerable<IAuthorizationRequirement>>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            mockAuthorizationService.Setup(x => x.AuthorizeAsync(
                It.IsAny<ClaimsPrincipal>(),
                It.IsAny<object>(),
                It.IsAny<string>()
                )).Returns(Task.FromResult(AuthorizationResult.Failed()));

            var mockURLHelper = new Mock<IUrlHelper>();
            mockURLHelper.Setup(x =>
            x.Link(It.IsAny<string>(),
            It.IsAny<object>())).
            Returns(string.Empty);

            var rootController = new RootController(mockAuthorizationService.Object);
            rootController.Url = mockURLHelper.Object;

            // Ejecución
            var resultado = await rootController.Get();

            // Verificación
            Assert.AreEqual(2, resultado.Value.Count());
        }
    }
}

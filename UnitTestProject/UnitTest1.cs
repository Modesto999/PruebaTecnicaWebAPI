using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using Xunit;
using System.Linq;
using System.Net;
using PruebaTecnicaWebAPI.Models;
using MongoDB.Driver;
using PruebaTecnicaWebAPI.Data;
using PruebaTecnicaWebAPI.Controllers;

namespace UnitTestProject
{
    public class ProveedorControllerTest
    {
        [Fact]
        public async Task Get_DeberiaRetornarTodosLosProveedores()
        {
            // Arrange
            // Crear una lista de proveedores de ejemplo
           
            var proveedoresDeEjemplo = new List<Proveedor>
           
        {
            new Proveedor { Id = "1", 
                NIT = "12332123", 
                RazonSocial = "Empresa1", 
                Direccion = "Carrera 23 calle 1", 
                Ciudad="Cali", Departamento="Valle", 
                Correo="correo1@gmail.com", 
                Activo=false,
                FechaCreacion= System.DateTime.Now,
                NombreContacto="Diego Vargas", 
                CorreoContacto="diego@gmail.com" },
            new Proveedor { Id = "2",
                NIT = "12332123",
                RazonSocial = "Empresa2",
                Direccion = "Carrera 23 calle 1",
                Ciudad="Cali", Departamento="Valle",
                Correo="correo1@gmail.com",
                Activo=false,
                FechaCreacion= System.DateTime.Now,
                NombreContacto="Juan Vargas",
                CorreoContacto="diego@gmail.com" }     
        };

            // Crear un falso (fake) para IMongoCollection<Proveedor>
            var fakeCollection = A.Fake<IMongoCollection<Proveedor>>();

            // Configurar el comportamiento del falso para que simule el comportamiento de MongoDB
            // 1.  A.Fake<IAsyncCursor<Proveedor>>(): Crea un falso para el cursor asíncrono que se utiliza para iterar sobre los resultados de la consulta.
            // 2.  A.Collection(proveedoresDeEjemplo).ToAsyncCursor(): Convierte la lista de proveedores de ejemplo en un cursor asíncrono que FakeItEasy puede usar.
            // 3.  A.CallTo(() => fakeCollection.FindAsync(A<FilterDefinition<Proveedor>>._, A<FindOptions<Proveedor, Proveedor>>._, A<CancellationToken>._))
            //     .Returns(Task.FromResult(fakeCursor)): Configura el falso para que cuando se llame al método FindAsync en el falso de la colección, retorne un Task que contiene el falso del cursor.  A<FilterDefinition<Proveedor>>._  y  A<CancellationToken>._  son comodines que coinciden con cualquier filtro y token de cancelación.
            var fakeCursor = A.Fake<IAsyncCursor<Proveedor>>();
           // A.CollectionOfFake<Proveedor>(count).AsEnumerable();
            A.CallTo(() => fakeCollection.FindAsync(A<FilterDefinition<Proveedor>>._, A<FindOptions<Proveedor, Proveedor>>._, A<CancellationToken>._))
                .Returns(Task.FromResult(fakeCursor));

            // Configurar el comportamiento del falso del cursor para simular la iteración.
            A.CallTo(() => fakeCursor.Current).Returns(proveedoresDeEjemplo);
            object value = A.CallTo(() => fakeCursor.MoveNextAsync(A<CancellationToken>._)).Returns(Task.FromResult(true)).Then.Returns(Task.FromResult(false));

            // Crear un falso para MongoDBService
            var fakeMongoDBService = A.Fake<MongoDBService>();
            A.CallTo(() => fakeMongoDBService.Database?.GetCollection<Proveedor>("proveedor"))
                .Returns(fakeCollection); // Configurar para que devuelva la colección falsa

            // Crear una instancia del controlador con el falso servicio de MongoDB
            var controller = new ProveedorController(fakeMongoDBService);

            // Act
            // Llamar al método Get del controlador
            var result = await controller.Get();

            // Assert
            // Verificar que el resultado sea el esperado
            Assert.NotNull(result); // Verificar que el resultado no sea nulo
            Assert.Equal(proveedoresDeEjemplo.Count(), result.Count()); //Verificar que la cantidad de proveedores sea la misma
            Assert.Equal(proveedoresDeEjemplo[0].Id, result.ElementAt(0).Id); //Verificar que los ids sean iguales
            Assert.Equal(proveedoresDeEjemplo[0].RazonSocial, result.ElementAt(0).RazonSocial); //Verificar que los nombres sean iguales
            Assert.Equal(proveedoresDeEjemplo[1].Id, result.ElementAt(1).Id);
            Assert.Equal(proveedoresDeEjemplo[1].RazonSocial, result.ElementAt(1).RazonSocial);

            // Verificar que el método FindAsync se haya llamado al menos una vez
            A.CallTo(() => fakeCollection.FindAsync(A<FilterDefinition<Proveedor>>._, A<FindOptions<Proveedor, Proveedor>>._, A<CancellationToken>._))
                .MustHaveHappenedOnceOrMore();
        }
    }
}
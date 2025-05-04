using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PruebaTecnicaWebAPI.Data;
using MongoDB.Driver;
using PruebaTecnicaWebAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace PruebaTecnicaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class ProveedorController : ControllerBase
    {
        private readonly IMongoCollection<Proveedor> _proveedores;
        public ProveedorController(MongoDBService mongoDBService ) {

            _proveedores = mongoDBService.Database?.GetCollection<Proveedor>("proveedor");

        }

        [HttpGet]
        public async Task<IEnumerable<Proveedor>> Get()
        {
            return await _proveedores.Find(FilterDefinition<Proveedor>.Empty).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedor?>> GetById(string id)
        {
            var filter = Builders<Proveedor>.Filter.Eq(x => x.Id, id);
            var proveedor = _proveedores.Find(filter).FirstOrDefault();
            return proveedor is not null ? Ok(proveedor) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Proveedor proveedor)
        {
            await _proveedores.InsertOneAsync(proveedor);
            return CreatedAtAction(nameof(GetById), new {id= proveedor.Id}, proveedor);
        }

        [HttpPut]
        public async Task<ActionResult> Update(Proveedor proveedor)
        {
            var filter = Builders<Proveedor>.Filter.Eq(x => x.Id, proveedor.Id);
            await _proveedores.ReplaceOneAsync(filter,proveedor);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(string id)
        {
            var filter = Builders<Proveedor>.Filter.Eq(x => x.Id, id);
            await _proveedores.DeleteOneAsync(filter);
            return Ok();
        }

    }
}

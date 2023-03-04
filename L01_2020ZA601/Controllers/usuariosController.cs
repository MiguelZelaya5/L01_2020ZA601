using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using L01_2020ZA601.Models;
using Microsoft.EntityFrameworkCore;

namespace L01_2020ZA601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        private readonly blogDbContext _blogDbContexto;
        public usuariosController(blogDbContext blogDbContexto) 
        {
            _blogDbContexto= blogDbContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<usuarios> listadoEquipo = (from e in _blogDbContexto.usuarios
                                           select e).ToList();

            if (listadoEquipo.Count == 0)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            usuarios? equipo = (from e in _blogDbContexto.usuarios
                               where e.usuarioId == id
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        /// <summary>
        /// Busca por nombre y apellido
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{nombre}/{apellido}")]
        public IActionResult FindNombre(string nombre, string apellido)
        {
            usuarios? equipo = (from e in _blogDbContexto.usuarios
                               where e.nombre.Contains(nombre) && e.apellido.Contains(apellido)
                               select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        /// <summary>
        /// Buscar por rol
        /// </summary>
        /// <param name="nombre"></param>
        /// <param name="apellido"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{rol}")]
        public IActionResult FindRol(int rol)
        {
            usuarios? equipo = (from e in _blogDbContexto.usuarios
                                where e.rolId == rol
                                select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardadEquipo([FromBody] usuarios usuario)
        {
            try
            {
                _blogDbContexto.usuarios.Add(usuario);
                _blogDbContexto.SaveChanges();
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] usuarios equipoModificar)
        {
            ///Para actualizar un registro,se pbtiene el regitro original de la base de datos
            ///al cual alteraremos algunas propiedades
            usuarios? equipoActual = (from e in _blogDbContexto.usuarios
                                     where e.usuarioId == id
                                     select e).FirstOrDefault();
            ///Verifivamos si existe en base el id
            if (equipoActual == null)
            {
                return NotFound();
            }
            ///Si se encuentran registros, se alteran los campos modificables.
            equipoActual.nombre = equipoModificar.nombre;
            equipoActual.usuarioId = equipoModificar.usuarioId;
            equipoActual.rolId = equipoModificar.rolId;
            equipoActual.nombreUsuario = equipoModificar.nombreUsuario;
            equipoActual.clave = equipoModificar.clave;
            equipoActual.apellido = equipoModificar.apellido;

            ///se Marca el registro como modificado en el contexto
            ///y se envia la modificacion a la base
            _blogDbContexto.Entry(equipoActual).State = EntityState.Modified;
            _blogDbContexto.SaveChanges();

            return Ok(equipoModificar);

        }
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarEquipo(int id)
        {
            usuarios? equipo = (from e in _blogDbContexto.usuarios
                               where e.usuarioId == id
                               select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            _blogDbContexto.usuarios.Attach(equipo);
            _blogDbContexto.usuarios.Remove(equipo);
            _blogDbContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}

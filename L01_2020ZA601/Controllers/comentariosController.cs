using L01_2020ZA601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020ZA601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class comentariosController : ControllerBase
    {
        private readonly blogDbContext _blogDbContexto;
        public comentariosController(blogDbContext blogDbContexto)
        {
            _blogDbContexto = blogDbContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<comentarios> listadoEquipo = (from e in _blogDbContexto.comentarios
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
            comentarios? equipo = (from e in _blogDbContexto.comentarios
                                      where e.cometarioId == id
                                      select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        /// <summary>
        /// Mostrar la lista de comentarios en base al id del usuario
        /// </summary>
        /// <param name="publicacion"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Find/{idusuario}")]
        public IActionResult FindNombre(int idusuario)
        {
            List<comentarios> listadoEquipo = (from e in _blogDbContexto.comentarios
                                               where e.usuarioId== idusuario
                                               select e).ToList();
            
            if (listadoEquipo == null)
            {
                return NotFound();
            }
            return Ok(listadoEquipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardadEquipo([FromBody] comentarios comentario)
        {
            try
            {
                _blogDbContexto.comentarios.Add(comentario);
                _blogDbContexto.SaveChanges();
                return Ok(comentario);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] comentarios equipoModificar)
        {
            ///Para actualizar un registro,se pbtiene el regitro original de la base de datos
            ///al cual alteraremos algunas propiedades
            comentarios? equipoActual = (from e in _blogDbContexto.comentarios
                                            where e.cometarioId == id
                                            select e).FirstOrDefault();
            ///Verifivamos si existe en base el id
            if (equipoActual == null)
            {
                return NotFound();
            }
            ///Si se encuentran registros, se alteran los campos modificables.
            equipoActual.cometarioId = equipoModificar.cometarioId;
            equipoActual.publicacionId = equipoModificar.publicacionId;
            equipoActual.usuarioId = equipoModificar.usuarioId;
            equipoActual.comentario = equipoModificar.comentario;


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
            comentarios? equipo = (from e in _blogDbContexto.comentarios
                                      where e.cometarioId == id
                                      select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            _blogDbContexto.comentarios.Attach(equipo);
            _blogDbContexto.comentarios.Remove(equipo);
            _blogDbContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}

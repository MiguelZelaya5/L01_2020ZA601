using L01_2020ZA601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace L01_2020ZA601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class calificacionesController : ControllerBase
    {
        private readonly blogDbContext _blogDbContexto;
        public calificacionesController(blogDbContext blogDbContexto)
        {
            _blogDbContexto = blogDbContexto;
        }
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<calificaciones> listadoEquipo = (from e in _blogDbContexto.calificaciones
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
            calificaciones? equipo = (from e in _blogDbContexto.calificaciones
                                where e.calificacionId == id
                                select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }
        [HttpGet]
        [Route("Find/{publicacion}")]
        public IActionResult FindNombre(int publicacion)
        {
            calificaciones? equipo = (from e in _blogDbContexto.calificaciones
                                where e.publicacionId== publicacion
                                      select e).FirstOrDefault();
            if (equipo == null)
            {
                return NotFound();
            }
            return Ok(equipo);
        }

        [HttpPost]
        [Route("Add")]
        public IActionResult GuardadEquipo([FromBody] calificaciones calificacione)
        {
            try
            {
                _blogDbContexto.calificaciones.Add(calificacione);
                _blogDbContexto.SaveChanges();
                return Ok(calificacione);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarEquipo(int id, [FromBody] calificaciones equipoModificar)
        {
            ///Para actualizar un registro,se pbtiene el regitro original de la base de datos
            ///al cual alteraremos algunas propiedades
            calificaciones? equipoActual = (from e in _blogDbContexto.calificaciones
                                      where e.calificacionId == id
                                      select e).FirstOrDefault();
            ///Verifivamos si existe en base el id
            if (equipoActual == null)
            {
                return NotFound();
            }
            ///Si se encuentran registros, se alteran los campos modificables.
            equipoActual.calificacionId= equipoModificar.calificacionId;
            equipoActual.publicacionId = equipoModificar.publicacionId;
            equipoActual.usuarioId = equipoModificar.usuarioId;
            equipoActual.calificacionId = equipoModificar.calificacionId;
            

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
            calificaciones? equipo = (from e in _blogDbContexto.calificaciones
                                where e.calificacionId == id
                                select e).FirstOrDefault();

            if (equipo == null)
            {
                return NotFound();
            }
            _blogDbContexto.calificaciones.Attach(equipo);
            _blogDbContexto.calificaciones.Remove(equipo);
            _blogDbContexto.SaveChanges();
            return Ok(equipo);
        }
    }
}

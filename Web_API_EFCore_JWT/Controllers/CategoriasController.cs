using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo_Fundamentos.Services;
using APICatalogo_Repositorio.DTOs;
using APICatalogo_Repositorio.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo___Copia.Controllers {

    [Authorize(AuthenticationSchemes = "Bearer")] // Protegendo a Api categorias 
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriasController : Controller {

        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration; // Uso para fazer um GET diferente (autor)
        private readonly ILogger _logger;

       
        public CategoriasController(IUnitOfWork uof, IConfiguration config,
            ILogger<CategoriasController> logger, IMapper mapper) {

            _uof = uof;
            _configuration = config; // Para poder ler os item de configuração definidos no (appsettings.json)
            _logger = logger;
            _mapper = mapper;
        }




        // api/categorias
        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> Get() {

            //_logger.LogInformation("===================GET api/categorias=====================");// Da uma informação no console

            try {

                var categorias = _uof.CategoriaRepository.Get().ToList();
                var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
                return categoriasDto;

            } catch (System.Exception) {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar buscar as categorias no banco");
            }
        }

        // api/categorias/1
        [HttpGet("{id}", Name = "ObterCategoria")]
        public ActionResult<Categoria> Get(int id) {

            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            _logger.LogInformation($"===================GET api/categorias/{id}=======================");

            try {

                if (categoria == null) {
                    return NotFound($"A categoria com Id={id} não foi encontrada");
                }
                return categoria;

            } catch (System.Exception) {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar obter a categoria no banco");
            }
        }

        // api/categorias
        [HttpPost]
        public ActionResult Post([FromBody] Categoria categoria) {

            try {

                if (!ModelState.IsValid) {
                    return BadRequest(ModelState);
                }

                _uof.CategoriaRepository.Add(categoria);
                _uof.Commit(); // .SaveChanges() do UnitOfWork

                return new CreatedAtRouteResult("ObterCategoria",
                    new { id = categoria.CategoriaId }, categoria);

            } catch (System.Exception) {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Erro ao tentar criar uma categoria no banco");
            }
        }

        // api/categorias/1
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Categoria categoria) {

            try {


                if (id != categoria.CategoriaId) {
                    return BadRequest($"Erro ao tentar alterar a categoria com Id={id}.");
                }

                _uof.CategoriaRepository.Update(categoria);
                _uof.Commit();
                return Ok(categoria);

            } catch (System.Exception) {

                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar alterar uma categoria no banco");
            }




        }

        //  api/categorias/1
        [HttpDelete("{id}")]
        public ActionResult<Categoria> Delete(int id) {

            var categoria = _uof.CategoriaRepository.GetById(c => c.CategoriaId == id);

            try {

                if (categoria == null) {
                    return NotFound($"A categoria com Id={id} não foi encontrada");
                }

                _uof.CategoriaRepository.Delete(categoria);
                _uof.Commit(); // .SaveChanges() do UnitOfWork
                return Ok($"A categoria com Id = {id} foi deletado");

            } catch (System.Exception) {


                return StatusCode(StatusCodes.Status500InternalServerError,
                   "Erro ao tentar excluir a categoria no banco");
            }

        }



        //************************Outros Metodos de requisição***************************

        // Buscar categorias com todos os produtos do banco de dados Usando o Include
        // api/categorias/produtos
        [HttpGet("produtos")]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategoriasProdutos() {

            _logger.LogInformation("===================GET api/categorias/produtos==================");

            //return Get().Include(x => x.Produtos);

            var categorias = _uof.CategoriaRepository.GetCategoriasProdutos().ToList();
            var categoriasDto = _mapper.Map<List<CategoriaDTO>>(categorias);
            return categoriasDto;
        }



        //  api/categorias/saudação/nome
        // https://localhost:7097/api/categorias/saudacao/Ricardo
        [HttpGet("saudacao/{nome}")]
        public ActionResult<string> GetSaudacao([FromServices]
            IMeuServico meuServico, string nome) {

            return meuServico.Saudacao(nome);
        }

        //api/categorias/autor
        [HttpGet("autor")]
        public string GetAutor() {

            var autor = _configuration["autor"];
            return $"Autor: {autor}";
        }


    }
}
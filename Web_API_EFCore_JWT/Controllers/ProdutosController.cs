using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo_Fundamentos.Filters;
using APICatalogo_Repositorio.DTOs;
using APICatalogo_Repositorio.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ApiCatalogo.Controllers {

    // Obrigatório para Api funcionar
    [Route("api/[Controller]")]
    [ApiController]
    public class ProdutosController : Controller {


        private readonly IUnitOfWork _uof;
        private readonly IMapper _mapper;

        // Construtor para usar nas requisições(Get, Post...)
        // Injetando instancia do AppDbContext(IUnitOfWork)
        // Injetando instancia do Mapper para as DTOs
        public ProdutosController(IUnitOfWork uof, IMapper mapper) {

            _uof = uof;
            _mapper = mapper;
        }



        // api/produtos
        [HttpGet]
        //[ServiceFilter(typeof(ApiLogginFilter))] // Informado serviço do filtro criado 
        public ActionResult<IEnumerable<ProdutoDTO>> Get() {

            var produtos = _uof.ProdutoRepository.Get().ToList();
            // Mapear para DTO
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }

        // api/produtos/1
        // {id:int:min(1)} é uma restrição(inteiro com valor minimo 1) . O método não gera request
        [HttpGet("{id:int:min(1)}", Name = "ObterProduto")]
        public ActionResult<ProdutoDTO> GetId(int id) {

            //AsNoTracking desabilita o gerenciamento do estado das entidades
            //so deve ser usado em consultas sem alteração
            //var produto = _context.Produtos.AsNoTracking()
            //    .FirstOrDefault(p => p.ProdutoId == id);
            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            if (produto == null) {
                return NotFound();
            }
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return produtoDto;
        }

        // api/produtos
        [HttpPost]
        public IActionResult Post([FromBody] ProdutoDTO produtoDto) {

            //a validação do ModelState é feito automaticamente
            //quando aplicamos o atributo [ApiController]
            // if (!ModelState.IsValid) {
            //     return BadRequest(ModelState);
            // }


            // Primeiro trabalha com Produto não ProdutoDTO
            var produto = _mapper.Map<Produto>(produtoDto);
            _uof.ProdutoRepository.Add(produto);
            _uof.Commit(); // .SaveChanges() do UnitOfWork


            // Depois mapeia para ProdutoDTO
            var produtoDTO = _mapper.Map<ProdutoDTO>(produto);


            // E exibe como ProdutoDTO
            return new CreatedAtRouteResult("ObterProduto",
                new { id = produto.ProdutoId }, produtoDTO);
        }

        // api/produtos/1
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ProdutoDTO produtoDto) {

            //a validação do ModelState é feito automaticamente
            //quando aplicamos o atributo [ApiController]
            // if (!ModelState.IsValid) {
            //     return BadRequest(ModelState);
            // }


            if (id != produtoDto.ProdutoId) {
                return BadRequest();
            }

            var produto = _mapper.Map<Produto>(produtoDto);

            _uof.ProdutoRepository.Update(produto);
            _uof.Commit(); // .SaveChanges() do UnitOfWork
            return Ok(produtoDto);

        }

        // api/produtos/1
        [HttpDelete("{id}")]
        public ActionResult<ProdutoDTO> Delete(int id) {

            var produto = _uof.ProdutoRepository.GetById(p => p.ProdutoId == id);

            // Você Deleta Produto não Produto DTO
            if (produto == null) {
                return NotFound();
            }

            _uof.ProdutoRepository.Delete(produto);
            _uof.Commit();// .SaveChanges() do UnitOfWork

            var produtoDto = _mapper.Map<ProdutoDTO>(produto);

            return Ok($"O produto com Id = {id} foi deletado");
        }

        //************************Outros Metodos de requisição***************************

        [HttpGet("menorpreco")] // Do menor preço ao maior
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorPreco() {

            var produtos = _uof.ProdutoRepository.GetProdutosPorPreco().ToList();
            // Mapear para DTO
            var produtosDto = _mapper.Map<List<ProdutoDTO>>(produtos);
            return produtosDto;
        }


    }
}

using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo_Repositorio.DTOs.Mappings {

    public class MappingProfile : Profile {

        // Mapeando as classes de Produto e Categoria para ProdutoDTO e CategoriaDTO (Mostrando só o necessário para o usuário)
        public MappingProfile() {

            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
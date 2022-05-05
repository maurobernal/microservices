using AutoMapper;
using api.Entities;
using api.Models;
namespace api.Profiles;

public class ProfilesAutomapper:Profile
{
    public ProfilesAutomapper()
    {
        //CreateMap<origen,destino>()
        CreateMap<Categorias, CategoriaModels>()
            .ForMember(d => d.campoDescripcion, o => o.MapFrom(m => "Descr:" + m.Descripcion))
            .ForMember(d => d.campoID, o => o.MapFrom(m => m.ID));
        //reverseMap

        CreateMap<CategoriaModels, Categorias>()
             .ForMember(d => d.Descripcion, o => o.MapFrom(m => m.campoDescripcion));
              
            
    }

}

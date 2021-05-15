using AutoMapper;
using WebApi.Entities;
using WebApi.Services.PersonService.Dto;
using WebApi.Services.TaskService.Dto;
using WebApi.Services.UserService.Dto;


namespace WebApi.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<User, UserDto>()
                .ReverseMap();

            CreateMap<CreateTaskDto, Tasks>();

            CreateMap<Tasks, TaskDto>()
                .ForMember( dest => dest.ProjectName, src => src.MapFrom( x => x.Project.Name))
                .ReverseMap();

            //automapper for tasks
            CreateMap<Persons,PersonDto>()
                .ReverseMap();


        }
    }
}
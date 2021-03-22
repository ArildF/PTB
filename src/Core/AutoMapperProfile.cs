using AutoMapper;
using Rogue.Ptb.Core.Export;

namespace Rogue.Ptb.Core
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Task, TaskDto>();
            CreateMap<TaskDto, Task>();
        }
        
    }
}
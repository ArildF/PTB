using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Rogue.Ptb.Core.Export
{
	public class DtoMapper : IDtoMapper
	{
		private readonly IMapper _mapper;

		public DtoMapper(IMapper mapper)
		{
			_mapper = mapper;
		}


		public IEnumerable<TaskDto> MapTasksToDtos(IEnumerable<Task> tasks)
		{
			var first = tasks.First();
			var firstDto = _mapper.Map<TaskDto>(first);
			return tasks.Select(t => _mapper.Map<TaskDto>(t));
		}

		public IEnumerable<Task> MapDtosToTasks(IEnumerable<TaskDto> taskDtos)
		{
			return taskDtos.Select(dto => _mapper.Map<Task>(dto));
		}


	}
}

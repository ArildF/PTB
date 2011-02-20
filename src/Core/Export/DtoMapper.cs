using System.Collections.Generic;
using System.Linq;
using Glue;

namespace Rogue.Ptb.Core.Export
{
	public class DtoMapper : IDtoMapper
	{
		private readonly Mapping<Task, TaskDto> _taskMapping;

		public DtoMapper()
		{
			_taskMapping = new Mapping<Task, TaskDto>();
			_taskMapping.Relate(t => t.Id, dto => dto.Id);
			_taskMapping.Relate(t => t.State, dto => dto.State);
			_taskMapping.Relate(t => t.Title, t => t.Title);
		}


		public IEnumerable<TaskDto> MapTasksToDtos(IEnumerable<Task> tasks)
		{
			return tasks.Select(t => _taskMapping.Map(t));
		}

		public IEnumerable<Task> MapDtosToTasks(IEnumerable<TaskDto> taskDtos)
		{
			return taskDtos.Select(dto => _taskMapping.Map(dto));
		}


	}
}

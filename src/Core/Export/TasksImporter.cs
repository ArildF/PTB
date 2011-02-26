using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Rogue.Ptb.Core.Export
{
	public class TasksImporter : ITasksImporter
	{
		private readonly IRepositoryProvider _provider;
		private readonly IDtoMapper _mapper;

		public TasksImporter(IRepositoryProvider provider, IDtoMapper mapper)
		{
			_provider = provider;
			_mapper = mapper;
		}

		public void ImportAll(string path)
		{
			using (var stream = File.OpenRead(path))
			{
				var serializer = new XmlSerializer(typeof(TaskDto[]));

				using (var repository = _provider.Open<Task>())
				{
					var dtos = (TaskDto[])serializer.Deserialize(stream);

					foreach (var taskDto in dtos)
					{
						taskDto.FixUpMissingData();
					}

					var tasks = _mapper.MapDtosToTasks(dtos).ToArray();

					repository.MergeAll(tasks);
				}

			}
		}
	}
}

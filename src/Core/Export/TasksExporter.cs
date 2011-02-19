using System.IO;
using System.Xml.Serialization;
using System.Linq;

namespace Rogue.Ptb.Core.Export
{
	public class TasksExporter : ITasksExporter
	{
		private readonly IDtoMapper _mapper;
		private readonly IRepositoryProvider _provider;

		public TasksExporter(IDtoMapper mapper, IRepositoryProvider provider)
		{
			_mapper = mapper;
			_provider = provider;
		}

		public void ExportAll(string path)
		{
			using (var stream = File.OpenWrite(path))
			{
				var serializer = new XmlSerializer(typeof (TaskDto[]));

				using (var repository = _provider.Open<Task>())
				{
					var tasks = repository.FindAll();
					var dtos = _mapper.MapTasksToDtos(tasks).ToArray();

					serializer.Serialize(stream, dtos);
				}

			}
		}
	}
}

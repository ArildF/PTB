using System;
using System.IO;
using System.Linq;
using System.Net;
using Rogue.Ptb.Infrastructure;

namespace Rogue.Ptb.Core
{
	public class PtbNoteAttachmentWebRequestFactory : IWebRequestCreate, IStartable
	{
		private readonly IRepositoryProvider _provider;

		public PtbNoteAttachmentWebRequestFactory(IRepositoryProvider provider)
		{
			_provider = provider;
		}

		public void Start()
		{
			WebRequest.RegisterPrefix("ptbnoteattachment:", this);
		}

		public WebRequest Create(Uri uri)
		{
			return new PtbNoteAttachmentWebRequest(_provider.Open<Attachment>(), uri);
		}

		public class PtbNoteAttachmentWebRequest : WebRequest
		{
			private readonly IRepository<Attachment> _repository;
			private readonly Uri _uri;

			public PtbNoteAttachmentWebRequest(IRepository<Attachment> repository, Uri uri)
			{
				_repository = repository;
				_uri = uri;
			}

			public override WebResponse GetResponse()
			{
				return new PtbNoteAttachmentWebResponse(_repository, _uri);
			}

		}

		public class PtbNoteAttachmentWebResponse : WebResponse
		{
			private readonly IRepository<Attachment> _repository;
			private readonly Uri _uri;

			public PtbNoteAttachmentWebResponse(IRepository<Attachment> repository, Uri uri)
			{
				_repository = repository;
				_uri = uri;
			}

			public override Stream GetResponseStream()
			{
				if (!Guid.TryParse(_uri.Host, out var guid))
				{
					return Stream.Null;
				}

				var attachment = _repository.FindAll().SingleOrDefault(a => a.Id == guid);
				if (attachment == null)
				{
					return Stream.Null;
				}

				return new MemoryStream(attachment.Content);
			}
		}
	}
}
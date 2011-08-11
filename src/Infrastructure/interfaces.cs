using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rogue.Ptb.Infrastructure
{

	public interface IEventAggregator
	{
		IObservable<T> Listen<T>();
		void Publish<T>(T message = default(T));
		void AddSource<T>(IObservable<T> observable);
	}
}

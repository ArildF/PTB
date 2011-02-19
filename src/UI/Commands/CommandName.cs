using System;
using StructureMap.Pipeline;

namespace Rogue.Ptb.UI.Commands
{
	public class CommandName
	{
		private readonly string _name;

		private CommandName(string name)
		{
			_name = name;
		}

		public string Name
		{
			get {
				return _name;
			}
		}

		public static CommandName Create(string name)
		{
			return new CommandName(name);
		}

		public static CommandName Create<TCommand>()
		{
			return new CommandName(typeof(TCommand).Name);
		}

		public bool Equals(CommandName other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return Equals(other._name, _name);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (CommandName)) return false;
			return Equals((CommandName) obj);
		}

		public override int GetHashCode()
		{
			return (_name != null ? _name.GetHashCode() : 0);
		}

		public static bool operator ==(CommandName left, CommandName right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(CommandName left, CommandName right)
		{
			return !Equals(left, right);
		}
	}
}
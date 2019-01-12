/* Copyright (c) 2019 ExT (V.Sigalkin) */

#if EXTOSC

using System;

namespace extEditorOSC
{
	[AttributeUsage(AttributeTargets.Class)]
	public class OSCEditorComponentAttribute : Attribute
	{
		#region Public Vars

		public string Name
		{
			get { return _name; }
		}

		public string Group
		{
			get { return _group; }
		}

		#endregion

		#region Private Vars

		private readonly string _name;

		private readonly string _group = "Other";

		#endregion

		#region Public Methods

		public OSCEditorComponentAttribute(string name)
		{
			_name = name;
		}

		public OSCEditorComponentAttribute(string group, string name)
		{
			_group = group;
			_name = name;
		}

		#endregion
	}
}

#endif
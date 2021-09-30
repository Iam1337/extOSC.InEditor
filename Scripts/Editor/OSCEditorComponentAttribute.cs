/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;

namespace extOSC.InEditor
{
	[AttributeUsage(AttributeTargets.Class)]
	public class OSCEditorComponentAttribute : Attribute
	{
		#region Public Vars

		public readonly string Name;

		public readonly string Group;

		#endregion

		#region Public Methods

		public OSCEditorComponentAttribute(string name)
		{
			Name = name;
			Group = "Other";
		}

		public OSCEditorComponentAttribute(string group, string name)
		{
			Group = group;
			Name = name;
		}

		#endregion
	}
}
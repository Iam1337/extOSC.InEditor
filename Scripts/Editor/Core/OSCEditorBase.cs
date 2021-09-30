/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using UnityEditor;

using System;

namespace extOSC.InEditor
{
	public abstract class OSCEditorBase : IDisposable
	{
		#region Public Vars

		public abstract bool IsStarted  { get; }

		#endregion

		#region Public Methods

		protected OSCEditorBase()
		{
			EditorApplication.update += Update;
		}

		~OSCEditorBase()
		{
			Dispose();
		}

		public abstract void Connect();

		public abstract void Close();

		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}

		#endregion

		#region Private Methods

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				EditorApplication.update -= Update;
			}

			Close();
		}

		protected virtual void Update()
		{ }

		#endregion
	}
}
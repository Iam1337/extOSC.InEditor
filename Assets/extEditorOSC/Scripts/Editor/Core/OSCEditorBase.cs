/* Copyright (c) 2018 ExT (V.Sigalkin) */

#if EXTOSC

using UnityEditor;

using System;

namespace extEditorOSC.Core
{
	public abstract class OSCEditorBase : IDisposable
	{
		#region Public Vars

		public abstract bool IsAvaible { get; }

		#endregion

		#region Public Methods

		public OSCEditorBase()
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

#endif
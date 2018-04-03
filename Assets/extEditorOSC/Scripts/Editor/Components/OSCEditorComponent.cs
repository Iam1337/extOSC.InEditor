using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace extEditorOSC.Components
{
	public abstract class OSCEditorComponent
	{
		#region Public Vars

		public int Index
		{
			get { return _index; }
		}

		public bool Active
		{
			get { return _active; }
		}

		#endregion

		#region Private Vars

		private int _index;

		private bool _active;

		#endregion

		#region Public Methods

		public void SetActive(bool active)
		{
			if (_active == active)
				return;

			if (active)
			{
				OnEnable();
			}
			else
			{
				OnDisable();
			}

			_active = active;
		}

		#endregion

		#region Protected Methods

		protected abstract void OnEnable();

		protected abstract void OnChangeIndex(int index);

		protected abstract void OnDisable();

		#endregion
	}
}

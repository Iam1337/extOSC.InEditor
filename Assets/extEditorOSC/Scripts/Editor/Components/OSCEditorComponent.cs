/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEditor;

using System.Linq;

namespace extEditorOSC.Components
{
	public abstract class OSCEditorComponent
	{
		#region Public Vars

		public bool Active
		{
			get { return _active; }
			set
			{
				if (_active == value) return;
				
				SetActive(value);
			}
		}

		public string Name
		{
			get
			{
				if (string.IsNullOrEmpty(_name))
					LoadAttributeValues();

				return _name;
			}
		}

		public string Group
		{
			get
			{
				if (string.IsNullOrEmpty(_group))
					LoadAttributeValues();

				return _group;
			}
		}

		#endregion

		#region Private Vars

		private string _name;

		private string _group;

		private bool _active;

		private EditorApplication.CallbackFunction _updateCallback;

		#endregion

		#region Public Methods

		public void SetActive(bool active)
		{
			if (_active == active)
				return;

			if (_updateCallback == null)
				_updateCallback = Update;
  
			if (active)
			{
				OnEnable();

				var invocationList = EditorApplication.update.GetInvocationList();
				if (!invocationList.Contains(_updateCallback))
				{
					EditorApplication.update += _updateCallback;
				}
			}
			else
			{
				OnDisable();

				var invocationList = EditorApplication.update.GetInvocationList();
				if (invocationList.Contains(_updateCallback))
				{
					EditorApplication.update -= Update;
				}
			}

			_active = active;
		}

		#endregion

		#region Protected Methods

		protected virtual void OnEnable()
		{ }

		protected virtual void Update()
		{ }

		protected virtual void OnDisable()
		{ }

		#endregion

		#region Private Methods

		private void LoadAttributeValues()
		{
			var attributes = GetType().GetCustomAttributes(typeof(OSCEditorComponentAttribute), true);
			if (attributes.Length < 0)
			{
				_name = GetType().Name;
				_group = GetType().Namespace;

				return;
			}

			var attribute = (OSCEditorComponentAttribute)attributes[0];

			_name = attribute.Name;
			_group = attribute.Name;
		}

		#endregion
	}
}

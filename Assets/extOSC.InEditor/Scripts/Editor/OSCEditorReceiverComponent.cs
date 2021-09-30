/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

using System;
using System.Collections.Generic;

using extOSC.Core;

namespace extOSC.InEditor
{
	[Serializable]
	public abstract class OSCEditorReceiverComponent : OSCEditorComponent
	{
		#region Public Vars

		public OSCEditorReceiver Receiver
		{
			get => _receiver;
			set
			{
				if (_receiver == value)
					return;

				var rebind = _binds != null;

				Unbind();

				_receiver = value;

				if (rebind)
					Bind();
			}
		}

		#endregion

		#region Private Vars

		private OSCEditorReceiver _receiver;

		private List<IOSCBind> _binds;

		#endregion

		#region Public Methods

		public void Bind()
		{
			if (_binds != null) Unbind();

			_binds = new List<IOSCBind>();

			PopulateBinds(_binds);

			if (_receiver != null & _binds != null)
			{
				foreach (var bind in _binds)
				{
					_receiver.Bind(bind);
				}
			}
		}

		public void Unbind()
		{
			if (_receiver != null && _binds != null)
			{
				foreach (var bind in _binds)
				{
					_receiver.Unbind(bind);
				}
			}

			_binds = null;
		}

		#endregion

		#region Protected Methods

		protected override void OnEnable()
		{
			Bind();
		}

		protected override void OnDisable()
		{
			Unbind();
		}

		protected abstract void PopulateBinds(List<IOSCBind> binds);

		#endregion
	}
}
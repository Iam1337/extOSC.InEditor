/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;
using System.Collections.Generic;

using extOSC.Core;

namespace extEditorOSC.Components
{
	[Serializable]
	public abstract class OSCEditorReceiverComponent : OSCEditorComponent
	{
		#region Public Vars

		public OSCEditorReceiver Receiver
		{
			get { return _recevier; }
			set
			{
				if (_recevier == value)
					return;

				var rebind = _binds != null;

				Unbind();

				_recevier = value;

				if (rebind)
					Bind();
			}
		}

		#endregion

		#region Private Vars

		private OSCEditorReceiver _recevier;

		private OSCEditorReceiver _bindedReceiver;

		private List<IOSCBind> _binds;

		#endregion

		#region Public Methods

		public void Bind()
		{
			if (_binds != null) Unbind();

			_binds = new List<IOSCBind>();

			PopulateBinds(_binds);

			if (_recevier != null & _binds != null)
			{
				foreach (var bind in _binds)
				{
					_recevier.Bind(bind);
				}
			}
		}

		public void Unbind()
		{
			if (_recevier != null && _binds != null)
			{
				foreach (var bind in _binds)
				{
					_recevier.Unbind(bind);
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
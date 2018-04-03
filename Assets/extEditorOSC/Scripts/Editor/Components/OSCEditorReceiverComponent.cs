/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;

namespace extEditorOSC.Components
{
	[Serializable]
	public abstract class OSCEditorReceiverComponent : OSCEditorComponent
	{
		#region Public Vars

		#endregion

		#region Public Methods

		public OSCEditorReceiverComponent()
		{ }

		public abstract void InitBinds(OSCEditorReceiver receiver);

		#endregion

		#region Protected Methods

		protected override void OnEnable()
		{
			throw new NotImplementedException();
		}

		protected override void OnChangeIndex(int index)
		{
			throw new NotImplementedException();
		}

		protected override void OnDisable()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
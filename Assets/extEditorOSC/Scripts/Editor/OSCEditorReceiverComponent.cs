/* Copyright (c) 2018 ExT (V.Sigalkin) */

using System;

namespace extEditorOSC
{
	[Serializable]
	public abstract class OSCEditorReceiverComponent
	{
		#region Public Vars

		public int ReceiverId;

		#endregion

		#region Public Methods

		public OSCEditorReceiverComponent()
		{ }

		public abstract void InitBinds(OSCEditorReceiver receiver);

		#endregion
	}
}
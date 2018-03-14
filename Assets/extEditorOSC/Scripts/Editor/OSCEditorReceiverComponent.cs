/* Copyright (c) 2018 ExT (V.Sigalkin) */

namespace extEditorOSC
{
	public abstract class OSCEditorReceiverComponent
	{
		#region Public Methods

		public OSCEditorReceiverComponent()
		{ }

		public abstract void InitBinds(OSCEditorReceiver receiver);

		#endregion
	}
}
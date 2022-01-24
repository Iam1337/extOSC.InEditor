/* Copyright (c) 2021 dr. ext (Vladimir Sigalkin) */

namespace extOSC.InEditor.Examples
{
	[OSCEditorComponent("Examples", "Example Transmitter Component")]
	public class OSCEditorTransmitterComponentExample : OSCEditorTransmitterComponent
	{
		#region Protected Methods

		protected override void Update()
		{
			var message = new OSCMessage("/editor/example");
			message.AddValue(OSCValue.String("Editor message!"));

			Send(message);
		}

		#endregion
	}
}
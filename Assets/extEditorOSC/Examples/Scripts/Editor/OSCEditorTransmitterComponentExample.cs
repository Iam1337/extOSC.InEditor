/* Copyright (c) 2018 ExT (V.Sigalkin) */

using extOSC;

using extEditorOSC;
using extEditorOSC.Components;

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
/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using System.Collections.Generic;

using extOSC;
using extOSC.Core;

using extEditorOSC;
using extEditorOSC.Components;

[OSCEditorComponent("Examples", "Example Receiver Component")]
public class OSCEditorReceiverComponentExample : OSCEditorReceiverComponent
{
	#region Protected Methods

	protected override void PopulateBinds(List<IOSCBind> binds)
	{
		binds.Add(new OSCBind("/editor/example", MessageReceive));
	}

	#endregion

	#region Private Methods

	private void MessageReceive(OSCMessage message)
	{
		Debug.LogFormat("Received message: {0}", message);
	}

	// Custom update method.
	private void Update()
	{
		var message = new OSCMessage("/editor/example");
		message.AddValue(OSCValue.String("Editor message!"));

		//OSCEditorManager.Transmitter.Send(message);
	}

	#endregion
}
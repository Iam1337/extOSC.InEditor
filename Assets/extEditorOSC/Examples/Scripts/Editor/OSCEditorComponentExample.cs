/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

using UnityEditor;

using extOSC;

using extEditorOSC;

public class OSCEditorComponentExample : OSCEditorReceiverComponent
{
	#region Public Methods

	public override void InitBinds(OSCEditorReceiver receiver)
	{
		// Binding address
		receiver.Bind("/editor/example", MessageReceive);

		// Register Update method in Editor Update loop.
		EditorApplication.update += Update;
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

		OSCEditorManager.Transmitter.Send(message);
	}

	#endregion
}
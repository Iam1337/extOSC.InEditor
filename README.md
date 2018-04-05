# extEditorOSC
**This project require: [extOSC](http://u3d.as/ADA) (>= 1.8.2 version) asset.**

This asset allows you to use all features of extOSC directly into the Unity Editor.
To create your own OSC Editor Component, you only need to create a subclass from *"OSCEditorReceiverComponent"* or the *"OSCEditorTransmitterComponent"*, and implement all the functions what you need.

Example:

**Example of custom OSC Editor Receiver Component**: *OSCEditorReceiverComponentExample.cs*
```C#
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

	#endregion
}
```

**Example of custom OSC Editor Transmitter Component**: *OSCEditorTransmitterComponentExample.cs*
```C#
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
```

## Screenshots
<img src="https://i.imgur.com/6IJlD95.png" width="400"> <img src="https://i.imgur.com/dFH3Vp7.png" width="400">

## TODO
- Documentation
- Better README.md

## Installation
Download project and copy 'Assets/extEditorOSC' folder to project Assets folder.

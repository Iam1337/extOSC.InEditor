# extEditorOSC - Open Sound Control Protocol for Unity Editor

Created by V. Sigalkin (ext)

### What Is extEditorOSC?
This asset allows you to use all features of extOSC directly into the Unity Editor.
To create your own OSC Editor Component, you only need to create a subclass from *"OSCEditorReceiverComponent"* or the *"OSCEditorTransmitterComponent"*, and implement all the functions what you need.

**This project require: [extOSC](https://github.com/Iam1337/extOSC) asset.**

### Release Notes:

You can read release notes in [versions.txt](Assets/extEditorOSC/versions.txt) file.

### Examples:

**Custom OSC Editor Receiver Component**<br>
This example is implemented in a file: [OSCEditorReceiverComponentExample.cs](Assets/extEditorOSC/Examples/Scripts/Editor/OSCEditorReceiverComponentExample.cs)
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

**Custom OSC Editor Transmitter Component**<br>
This example is implemented in a file: [OSCEditorTransmitterComponentExample.cs](Assets/extEditorOSC/Examples/Scripts/Editor/OSCEditorTransmitterComponentExample.cs)
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

### Installation:

1) Copy [extOSC](https://github.com/Iam1337/extOSC) in your project.
2) Copy the [Assets/extEditorOSC](Assets/extEditorOSC) folder into your Assets directory within your Unity project.

### Screenshots
<img src="https://i.imgur.com/6IJlD95.png" width="400"> <img src="https://i.imgur.com/dFH3Vp7.png" width="400">

### Author Contacts:
\> [telegram.me/iam1337](http://telegram.me/iam1337) <br>
\> [ext@iron-wall.org](mailto:ext@iron-wall.org)

## License
This project is under the MIT License.

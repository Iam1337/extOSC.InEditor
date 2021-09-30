# extOSC.InEditor - Open Sound Control Protocol for Unity Editor

Created by [iam1337](https://github.com/iam1337)

![](https://img.shields.io/badge/unity-2021.1%20or%20later-green.svg)
[![⚙ Build and Release](https://github.com/Iam1337/extOSC.InEditor/actions/workflows/ci.yml/badge.svg)](https://github.com/Iam1337/extOSC.InEditor/actions/workflows/ci.yml)
[![](https://img.shields.io/github/license/iam1337/extOSC.InEditor.svg)](https://github.com/Iam1337/extOSC.InEditor/blob/master/LICENSE)

### Table of Contents
- [Introduction](#introduction)
- [Installation](#installation)
- [Examples](#examples)
- - [Custom OSC Editor Receiver Component](#custom-osc-editor-receiver-component)
- - [Custom OSC Editor Transmitter Component](#custom-osc-editor-transmitter-component)
- [Author Contacts](#author-contacts)

## Introduction
This asset allows you to use all features of extOSC directly into the Unity Editor.
To create your own OSC Editor Component, you only need to create a subclass from *"OSCEditorReceiverComponent"* or the *"OSCEditorTransmitterComponent"*, and implement all the functions what you need.

## Installation:
**Old school**

Just copy the [Assets/extOSC.InEditor](Assets/extOSC.InEditor) folder into your Assets directory within your Unity project, or [download latest extOSC.InEditor.unitypackage](https://github.com/iam1337/extOSC.InEditor/releases).

**Package Manager**

Project supports Unity Package Manager. To install the project as a Git package do the following:

1. In Unity, open **Window > Package Manager**.
2. Press the **+** button, choose **"Add package from git URL..."**
3. Enter "https://github.com/iam1337/extOSC.InEditor.git#upm" and press Add.


## Examples:
### Custom OSC Editor Receiver Component
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

### Custom OSC Editor Transmitter Component
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

## Screenshots
<img src="https://i.imgur.com/6IJlD95.png" width="400"> <img src="https://i.imgur.com/dFH3Vp7.png" width="400">

## Author Contacts:
\> [telegram.me/iam1337](http://telegram.me/iam1337) <br>
\> [ext@iron-wall.org](mailto:ext@iron-wall.org)

## License
This project is under the MIT License.

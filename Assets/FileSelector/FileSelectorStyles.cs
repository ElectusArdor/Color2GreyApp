using UnityEngine;
using System.Collections;

public class FileSelectorStyles : MonoBehaviour {
	
	public GUIStyle windowStyle;
	public GUIStyle buttonStyle;
	public GUIStyle titleStyle;
	public GUIStyle labelStyle;
	public GUIStyle textFieldStyle;

    private void OnGUI()
    {
		FileSelector.windowStyle.fontSize = (int)24;
		FileSelector.buttonStyle.fontSize = (int)20;
		FileSelector.titleStyle.fontSize = (int)24;
		FileSelector.labelStyle.fontSize = (int)24;
		FileSelector.textFieldStyle.fontSize = (int)20;
	}
}

/* Copyright (c) 2019 ExT (V.Sigalkin) */

#if !EXTOSC
using UnityEditor;
using UnityEngine;

namespace oscEditorOSC.Windows
{
    [InitializeOnLoad]
    public class OSCWindowWarning : EditorWindow
    {
        #region Static Public Methods

        [MenuItem("Tools/extEditorOSC/Editor Manager (Warning!)", false, 0)]
        [MenuItem("Tools/extEditorOSC/Editor Components (Warning!)", false, 1)]
        public static void ShowWindow()
        {
            var instance = EditorWindow.GetWindow<OSCWindowWarning>(true, "extEditorOSC", true);
            instance.maxSize = instance.minSize = new Vector2(300, 93);
            instance.ShowUtility();
        }

        #endregion

        #region Unity Methods

        protected void OnGUI()
        {
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.VerticalScope("box"))
                {
                    GUILayout.Label("Warning! extOSC not found.", EditorStyles.boldLabel);
                    GUILayout.Space(5f);
                    GUILayout.Label("extEditorOSC require extOSC asset.");
                }

                GUI.color = Color.yellow;
                var button = GUILayout.Button("Download extOSC", GUILayout.Height(30f));
                if (button)
                {
                    Application.OpenURL("https://github.com/Iam1337/extOSC");
                }

                GUI.color = Color.white;
            }
        }

        #endregion
    }
}

#endif
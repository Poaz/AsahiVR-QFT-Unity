using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Rokoko.Smartsuit.Networking
{
    [CustomEditor(typeof(CommandSender), true)]
    public class CommandSenderEditor : SmartsuitAbstractEditor
    {
        bool toggleCommands = false;

        public override void OnInspectorGUI()
        {
            EditorGUI.indentLevel++;
            notesToggle = EditorGUILayout.Foldout(notesToggle, "Notes");
            EditorGUI.indentLevel--;
            if (notesToggle)
            {

                //EditorStyles.label.wordWrap = true;
                GUI.skin.label.wordWrap = true;
                GUILayout.Label("This component provides the interface to send commands to the Smartsuits. There must be one component of this type active in the scene.");
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            }
            DrawDefaultInspector();
            CommandSender commandSender = target as CommandSender;
            var allCommandSenders = FindObjectsOfType<CommandSender>();
            if (allCommandSenders.Length > 1)
            {
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                GUILayout.Label("<color=#FD515A>(!) You have two CommandSender components active in the scene.</color>", warningStyle);
            }
            var receiver = GameObject.FindObjectOfType<SmartsuitReceiver>();
            if (!receiver)
            {
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                GUILayout.Label("<color=#FD515A>(!) You also need a SmartsuitReceiver component in the scene.</color>", warningStyle);
                if (GUILayout.Button("Add SmartsuitReceiver"))
                {
                    Undo.AddComponent<SmartsuitReceiver>(commandSender.gameObject);
                }
            }

            if (Application.isPlaying)
            {

                toggleCommands = EditorGUILayout.Foldout(toggleCommands, "Commands");
                if (toggleCommands)
                {
                    GUIStyle titleColor = new GUIStyle();
                    titleColor.normal.textColor = Color.white;

                    //GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    GUILayout.BeginVertical(livePanelStyle);
                    EditorGUILayout.LabelField("Pending", titleColor);
                    foreach (var request in commandSender.PendingSuitRequests)
                    {
                        EditorGUILayout.LabelField(request.ToString());
                        //GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    }
                    GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(5) });
                    EditorGUILayout.LabelField("Finished", titleColor);
                    foreach (var request in commandSender.OldSuitRequests)
                    {
                        EditorGUILayout.LabelField(request.ToString() + " - " + (request.Succeeded() ? "OK" : "FAILED"));
                        //GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
                    }
                    GUILayout.EndVertical();
                }
            }
        }
    }
}

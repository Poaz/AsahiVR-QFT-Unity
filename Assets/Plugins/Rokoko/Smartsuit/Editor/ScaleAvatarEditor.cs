using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// @cond nodoc
namespace Rokoko.Smartsuit
{

    [CustomEditor(typeof(ScaleAvatar))]
    public class ScaleAvatarEditor : SmartsuitAbstractEditor
    {
        bool toggleNotes = false;
        protected new void OnEnable()
        {
            base.OnEnable();
            //bodydim = serializedObject.FindProperty("bodydim");
            livePanel = MakeTex(1, 1, Color.black);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            ScaleAvatar avatar = target as ScaleAvatar;
            EditorGUI.indentLevel++;
            toggleNotes = EditorGUILayout.Foldout(toggleNotes, "Notes");
            EditorGUI.indentLevel--;

            if (toggleNotes)
            {
                GUI.skin.label.wordWrap = true;
                GUILayout.Label("Scale avatar will scale your avatar joints based on suits body dimensions.");
                GUILayout.Box("", new GUILayoutOption[] { GUILayout.ExpandWidth(true), GUILayout.Height(1) });
            }
            var actor = avatar.GetComponent<SmartsuitActor>();
            bool hasAnimator = avatar.GetComponent<Animator>();
            if (!actor && !hasAnimator)
            {
                GUILayout.Label("ScaleAvatar requires a SmartsuitActor or Animator to function");
            }
            EditorGUI.indentLevel++;
            if (Application.isPlaying)
            {
                if (actor)
                {
                    GUI.enabled = false;
                    if (actor.HasProfile)
                    {
                        
                        GUILayout.BeginVertical(livePanelStyle);
                        EditorGUILayout.TextField("Profile name", avatar.AvatarBody.ProfileName);
                        EditorGUILayout.FloatField("Head", avatar.AvatarBody._head);
                        EditorGUILayout.FloatField("Neck", avatar.AvatarBody._neck);
                        EditorGUILayout.FloatField("Middle back", avatar.AvatarBody._middle_back);
                        EditorGUILayout.FloatField("Shoulder blade", avatar.AvatarBody._shoulder_blade);
                        EditorGUILayout.FloatField("Upper arm", avatar.AvatarBody._upper_arm);
                        EditorGUILayout.FloatField("Forearm", avatar.AvatarBody._forearm);
                        EditorGUILayout.FloatField("Hand", avatar.AvatarBody._hand);
                        EditorGUILayout.FloatField("Low back", avatar.AvatarBody._low_back);
                        EditorGUILayout.FloatField("Hip", avatar.AvatarBody._hip);
                        EditorGUILayout.FloatField("Hip width", avatar.AvatarBody._hip_width);
                        EditorGUILayout.FloatField("Thigh", avatar.AvatarBody._thigh);
                        EditorGUILayout.FloatField("Leg", avatar.AvatarBody._leg);
                        EditorGUILayout.FloatField("Foot length", avatar.AvatarBody._foot_length);
                        EditorGUILayout.FloatField("Foot height", avatar.AvatarBody._foot_height);
                        EditorGUILayout.FloatField("Foot width", avatar.AvatarBody._foot_width);
                        EditorGUILayout.FloatField("Foot heel offset", avatar.AvatarBody._foot_heel_offset);
                        
                        GUILayout.EndVertical();
                    } else
                    {
                        EditorGUILayout.LabelField("Waiting body dimensions from Smartsuit.");
                    }
                    GUI.enabled = true;

                }
            }
            //EditorGUILayout.PropertyField(bodydim, true);
            EditorGUI.indentLevel--;
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
            serializedObject.ApplyModifiedProperties();

        }
    }
}
/// @endcond
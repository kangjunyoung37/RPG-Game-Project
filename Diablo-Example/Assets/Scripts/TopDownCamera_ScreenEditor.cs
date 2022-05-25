using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace kang.Cameras
{
    [CustomEditor(typeof(TopDownCamera))]
    public class TopDownCamera_ScreenEditor : Editor
    {
        
        #region Variables
        private TopDownCamera targetCamera;
        #endregion Variables

        public override void OnInspectorGUI()
        {
            targetCamera = (TopDownCamera)target;
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            if (!targetCamera || !targetCamera.target)
            {
                return;
            }
            Transform cameraTarget = targetCamera.target;
            Vector3 targetPosition = cameraTarget.position;
            targetPosition.y += targetCamera.lookAtHeight;

            Handles.color = new Color(1f, 0f, 0f, 0.15f);
            Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distanse);

            Handles.color = new Color(0f, 1f, 0f, 0.75f);
            Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distanse);

            Handles.color = new Color(1f, 0f, 0f, 0.5f);
            targetCamera.distanse = Handles.ScaleSlider(targetCamera.distanse, targetPosition, -cameraTarget.forward,Quaternion.identity,targetCamera.distanse,0.1f);
            targetCamera.distanse = Mathf.Clamp(targetCamera.distanse, 2f, float.MaxValue);

            Handles.color = new Color(0f, 0f, 1f, 0.5f);
            targetCamera.height = Handles.ScaleSlider(targetCamera.height, targetPosition, Vector3.up, Quaternion.identity, targetCamera.height, 0.1f);
            targetCamera.height = Mathf.Clamp(targetCamera.height,2f,float.MaxValue);

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.UpperCenter;

            Handles.Label(targetPosition + (-cameraTarget.forward * targetCamera.distanse), "Distance", labelStyle);

            labelStyle.alignment = TextAnchor.MiddleRight;
            Handles.Label(targetPosition + (Vector3.up * targetCamera.height), "height", labelStyle);

            targetCamera.HandleCamera();


        }
    }
}


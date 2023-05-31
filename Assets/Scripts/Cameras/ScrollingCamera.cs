using Cinemachine;
using UnityEngine;

namespace Cameras
{
    public class ScrollingCamera : CinemachineExtension
    {
        public bool scrollEnabled;
        public float scrollSpeed;
        public float lastX;
        
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            Vector3 pos = state.RawPosition;
            // Disallow the camera going to the left:
            if (pos.x < lastX) pos.x = lastX;
            lastX = pos.x;
            
            if (scrollEnabled) pos.x += scrollSpeed * deltaTime;
            
            
            state.RawPosition = pos;
        }
    }
}
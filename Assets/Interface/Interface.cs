using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AspectGgj2023.Interface 
{

    public class Interface : MonoBehaviour
    {
        # region Internal references
        [SerializeField]
        private Camera cameraObject;
        # endregion

        /// <summary>
        /// Distance to the border of the screen under which the camera moves.
        /// </summary>
        private int cameraMovementTriggerDistance = 20;

        /// <summary>
        /// Speed of the camera.
        /// </summary>
        private float cameraSpeed = 10;

        /// <summary>
        /// Minimum position of the bounding box of the camera on the XY.
        /// </summary>
        private Vector2 minimumCameraOffset = new Vector2(-10, -6);
        
        /// <summary>
        /// Maximum position of the bounding box of the camera on the XY.
        /// </summary>
        private Vector2 maximumCameraOffset = new Vector2(10, 6);


        private void Start() 
        {
            Debug.Assert(cameraObject != null);
        }

        private void Update() {
            CameraControl();
        }

        private void CameraControl()
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 cameraMovement = new Vector3();

            if (Screen.height - mousePosition.y < cameraMovementTriggerDistance)
            {
                cameraMovement.y = cameraSpeed;
            }
            else if (mousePosition.y < cameraMovementTriggerDistance)
            {
                cameraMovement.y = -cameraSpeed;
            }

            if (Screen.width - mousePosition.x < cameraMovementTriggerDistance)
            {
                cameraMovement.x = cameraSpeed;
            }
            else if (mousePosition.x < cameraMovementTriggerDistance)
            {
                cameraMovement.x = -cameraSpeed;
            }

            Vector3 newCameraPosition = cameraObject.transform.position + cameraMovement * Time.deltaTime;
            cameraObject.transform.position = new Vector3(
                Mathf.Clamp(newCameraPosition.x, minimumCameraOffset.x, maximumCameraOffset.x),
                Mathf.Clamp(newCameraPosition.y, minimumCameraOffset.y, maximumCameraOffset.y),
                newCameraPosition.z
            );
        }
    }

}
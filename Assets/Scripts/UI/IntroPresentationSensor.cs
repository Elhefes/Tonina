using UnityEngine;

public class IntroPresentationSensor : MonoBehaviour
{
    public MouseLook mouseLook;
    public GameObject minimapPresentation;
    public GameObject zoomPresentation;
    public GameObject viewTogglePresentation;

    private Vector3 staticRay;
    private bool minimapping;
    private bool minZoomed;
    private bool maxZoomed;

    private void Update()
    {
        if (minimapPresentation.activeInHierarchy && !mouseLook.cameraOnPlayer)
        {
            // Cast a ray downwards from the camera's position
            Ray ray = new Ray(mouseLook.transform.position, mouseLook.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseLook.layerMask))
            {
                if (!minimapping) staticRay = hit.point;
                minimapping = true;

                if (Vector3.Distance(hit.point, staticRay) > 12.5f)
                {
                    minimapPresentation.SetActive(false);
                    zoomPresentation.SetActive(true);
                    minimapping = false;
                }
            }
        }
        
        else if (zoomPresentation.activeSelf)
        {
            if (mouseLook.distanceFromObject > 19f) minZoomed = true;
            else if (mouseLook.distanceFromObject < 11f) maxZoomed = true;

            if (minZoomed && maxZoomed)
            {
                zoomPresentation.SetActive(false);
                viewTogglePresentation.SetActive(true);
                minZoomed = false;
                maxZoomed = false;
            }
        }
    }
}

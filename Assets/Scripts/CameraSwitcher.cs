using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera battleFieldCamera;
    public Camera farmCamera;
    void Start()
    {
        battleFieldCamera.enabled = true;
        farmCamera.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCameras();
        }
    }

    private void SwitchCameras()
    {
        battleFieldCamera.enabled = !battleFieldCamera.enabled;
        farmCamera.enabled = !farmCamera.enabled;
    }
}

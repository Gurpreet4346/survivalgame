using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableNonMenuCameras : MonoBehaviour
{
    private Camera mainMenuCamera;

    void Start()
    {
        // Find the Main Menu Camera at the start
        mainMenuCamera = Camera.main;

        if (mainMenuCamera == null || !mainMenuCamera.CompareTag("MainCamera"))
        {
            Debug.LogError("Main Menu Camera not found or not tagged as MainCamera!");
        }

        // Disable any non-main menu cameras present at the start
        DisableExtraCameras();
    }

    void Update()
    {
        // Continuously check for new cameras during runtime
        DisableExtraCameras();
    }

    private void DisableExtraCameras()
    {
        Camera[] cameras = Object.FindObjectsByType<Camera>(FindObjectsSortMode.None);

        foreach (Camera cam in cameras)
        {
            // Skip the Main Menu Camera
            if (cam == mainMenuCamera || cam.CompareTag("MainCamera"))
            {
                continue;
            }

            // Disable any other cameras (e.g., runtime-spawned prefab cameras)
            if (cam.gameObject.activeSelf)
            {
                Debug.Log($"Disabling runtime camera: {cam.name}");
                cam.gameObject.SetActive(false);
            }
        }
    }
}

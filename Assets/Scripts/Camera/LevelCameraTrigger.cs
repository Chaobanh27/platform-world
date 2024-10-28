using UnityEngine;

public class LevelCameraTrigger : MonoBehaviour
{
    private LevelCameraController levelCameraController;

    private void Awake()
    {
        levelCameraController = GetComponentInParent<LevelCameraController>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();
        if(playerController != null)
        {
            levelCameraController.EnableCamera(true);
            levelCameraController.SetNewTarget(playerController.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController playerController = collision.GetComponent<PlayerController>();

        if (playerController != null)
        {
            levelCameraController.EnableCamera(false);
        }
    }
}

using Unity.Cinemachine;
using UnityEngine;

public class LevelCameraController : MonoBehaviour
{
    private CinemachineCamera cinemachine;

    private void Awake()
    {
        cinemachine = GetComponentInChildren<CinemachineCamera>(true);
        EnableCamera(false);
    }
    public void EnableCamera(bool isEnable)
    {
        cinemachine.gameObject.SetActive(isEnable);
    }
    public void SetNewTarget(Transform newTarget)
    {
        cinemachine.Follow = newTarget;
    }
}

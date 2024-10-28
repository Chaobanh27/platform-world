using Unity.Cinemachine;
using UnityEngine;

public class CameraManagerController : MonoBehaviour
{
    public static CameraManagerController instance;

    [Header("Camera Shake")]
    [SerializeField] private Vector2 shakeVelocity;
    private CinemachineImpulseSource impulseSource;

    private void Awake()
    {
        instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void CameraShake()
    {
        impulseSource.DefaultVelocity = new Vector2(shakeVelocity.x, shakeVelocity.y);
        impulseSource.GenerateImpulse();
    }
}

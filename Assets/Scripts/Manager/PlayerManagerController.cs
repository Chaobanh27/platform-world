using System;
using System.Collections;
using UnityEngine;

public class PlayerManagerController : MonoBehaviour
{
    public static PlayerManagerController instance;
    public static event Action onPlayerRespawn;


    [Header("Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay;
    public PlayerController playerController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(respawnPoint == null)
        {
            respawnPoint = FindFirstObjectByType<StartPointController>().transform;
        }

        if(playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
        }
    }

    public void RespawnPlayerController()
    {
        StartCoroutine(RespawnPlayerCoroutine());

    }

    private IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        GameObject newPlayer = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        playerController = newPlayer.GetComponent<PlayerController>();
        onPlayerRespawn?.Invoke();
    }

    public void UpdateRespawnPosition(Transform newRespawnPoint)
    {
        respawnPoint = newRespawnPoint;
    }
}

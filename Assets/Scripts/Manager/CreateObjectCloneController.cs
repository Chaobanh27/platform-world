using System.Collections;
using UnityEngine;

public class CreateObjectCloneController : MonoBehaviour
{
    public static CreateObjectCloneController instance;

    public GameObject arrowPrefab;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreateObjectClone(GameObject prefab, Transform target, float delay)
    {
        StartCoroutine(CreateObjectCloneCoroutine(prefab, target, delay));
    }

    private IEnumerator CreateObjectCloneCoroutine(GameObject prefab, Transform target, float delay)
    {
        Vector3 newPosition = target.position;

        yield return new WaitForSeconds(delay);

        GameObject newObject = Instantiate(prefab, newPosition, Quaternion.identity);
    }
}

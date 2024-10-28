using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateFirstSelectedUIController : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}

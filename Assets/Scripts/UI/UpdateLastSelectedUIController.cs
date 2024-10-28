using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpdateLastSelectedUIController : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private MainMenuUIController mainMenuUIController;

    private void Awake()
    {
        mainMenuUIController = GetComponentInParent<MainMenuUIController>();
    }
    public void OnSelect(BaseEventData eventData)
    {
        mainMenuUIController.UpdateLastSelected(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}

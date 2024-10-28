using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FireTrapController : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private FireTrapBtnController fireTrapBtnController;
    private Animator fireTrapAnimator;
    private CapsuleCollider2D fireTrapCollider;
    private bool isActive;

    private void Awake()
    {
        fireTrapAnimator = GetComponent<Animator>();
        fireTrapCollider = GetComponent<CapsuleCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        if(fireTrapBtnController == null)
        {
            Debug.Log("You don't have fire button on " + gameObject.name + "!");
        }
        SetFire(true);
    }

    public void SwitchOffFireTrap()
    {
        if (isActive == false) return;

        StartCoroutine(FireTrapCoroutine());
    }
    private IEnumerator FireTrapCoroutine()
    {
        SetFire(false);
        yield return new WaitForSeconds(duration);
        SetFire(true);
    }

    private void SetFire(bool active)
    {
        fireTrapCollider.enabled = active;
        isActive = active;
        fireTrapAnimator.SetBool("isActive", isActive);
    }
}

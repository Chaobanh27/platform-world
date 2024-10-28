using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrapBtnController : MonoBehaviour
{
    private Animator fireTrapBtnAnimator;
    private FireTrapController fireTrapController;
    [SerializeField] private float coolDown;
    private bool isBtnActive;

    private void Awake()
    {
        fireTrapBtnAnimator = GetComponent<Animator>();
        fireTrapController = GetComponentInParent<FireTrapController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if(playerController != null)
        {
            StartCoroutine(FireTrapBtnCoroutine());
        }
    }

    private IEnumerator FireTrapBtnCoroutine()
    {
        isBtnActive = true;
        fireTrapBtnAnimator.SetBool("isBtnActive", isBtnActive);
        fireTrapController.SwitchOffFireTrap();
        yield return new WaitForSeconds(coolDown);
        isBtnActive = false;
        fireTrapBtnAnimator.SetBool("isBtnActive", isBtnActive);
    }
}

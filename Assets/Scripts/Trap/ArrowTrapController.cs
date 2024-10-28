using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrapController : TrampolineController
{
    [Header("Info")]
    [SerializeField] private float coolDown;
    [SerializeField] private bool rotationRight;
    [SerializeField] private float rotationSpeed = 120;
    private int arrowDirection;
    void Update()
    {
        RotationController();
    }

    private void RotationController()
    {
        arrowDirection = rotationRight ? 1 : -1;
        transform.Rotate(0,0, (rotationSpeed * arrowDirection) * Time.deltaTime);
    }

    private void DestroyGameObject()
    {
        GameObject arrowPrefab = CreateObjectCloneController.instance.arrowPrefab;
        CreateObjectCloneController.instance.CreateObjectClone(arrowPrefab, transform, coolDown);

        Destroy(gameObject);
    }
}

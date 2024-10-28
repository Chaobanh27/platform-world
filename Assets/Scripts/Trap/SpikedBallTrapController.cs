using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikedBallTrapController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D spikedBallRigidbody;
    [SerializeField] private float pushForce;
    // Start is called before the first frame update
    void Start()
    {
     Vector2 pushVector = new Vector2(pushForce, 0);
        spikedBallRigidbody.AddForce(pushVector, ForceMode2D.Impulse);
    }
}

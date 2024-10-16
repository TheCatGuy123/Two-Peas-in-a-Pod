using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingSpike : MonoBehaviour
{
    [SerializeField] private string breakableLayer;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        //if hits something breakable, destroy it!
        if(LayerMask.LayerToName(other.gameObject.layer) == breakableLayer)
        {
            Destroy(other.gameObject);
        }
    }
}

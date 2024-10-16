using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    
    // Update is called once per frame
    void Update()
    {
        //always move move "right" (changes depending on rotation)
        GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;
    }
}

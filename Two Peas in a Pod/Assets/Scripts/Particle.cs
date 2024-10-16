using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    private Vector2 Direction;
    // Start is called before the first frame update
    void Start()
    {
        // random direction
        Direction = new Vector2(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        // move in direction
        transform.Translate(Direction);
    }
}

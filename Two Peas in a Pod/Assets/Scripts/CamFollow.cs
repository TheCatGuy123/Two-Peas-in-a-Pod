using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform player1;
    [SerializeField] private Transform focus;
    [SerializeField] private float smoothTime;
    private Vector3 velocity = Vector3.zero;

    public void FixedUpdate()
    {
        // change camera size based on player distance
        float distance = Mathf.Abs(Vector2.Distance((Vector2)player.position, (Vector2)player1.position));
        if (distance > 16)
        {
            GetComponent<Camera>().orthographicSize = distance/1.5f;
        }
        else
        {
            GetComponent<Camera>().orthographicSize = 8;
        }
        focus.position = (player.position + player1.position)/2;
        // Define a target position above and behind the target transform
        Vector3 focusPoint = focus.TransformPoint(new Vector3(0, 0, -10));

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, focusPoint, ref velocity, smoothTime);
    }
}

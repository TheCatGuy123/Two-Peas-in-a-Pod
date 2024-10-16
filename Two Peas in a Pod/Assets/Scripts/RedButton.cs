using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedButton : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    [SerializeField] private int type, timeMoving;
    [SerializeField] private Sprite buttonImg, normalButtonImg;
    [SerializeField] private GameObject particle;
    [SerializeField] private Vector3 movement, movePosition, spawnPlace;
    public bool isSpringed;
    private GameObject pressingObject;
    public bool pressed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.tag == "Player" || other.tag == "CanPress") && !pressed)
        {
            pressingObject = other.gameObject;
            gameObject.GetComponent<SpriteRenderer>().sprite = buttonImg;
            pressed = true;
            if (type == 0)
            {
                foreach(GameObject i in objects)
                {
                    Destroy(i);
                }
            }
            else if (type == 1)
            {
                foreach(GameObject i in objects)
                {
                    Destroy(i);
                    for(int x = 0; x<15; x++)
                    {
                        GameObject currParticle = Instantiate(particle, i.transform.position + new Vector3(Random.Range(-0.4f, 0.4f), Random.Range(-0.4f, 0.4f), 0), i.transform.rotation);
                        Destroy(currParticle, 0.5f);
                        currParticle.GetComponent<Rigidbody2D>().velocity = new Vector3(Random.Range(-3, 3), Random.Range(4, 5));
                    }
                }
            }
            else if (type == 2)
            {
                foreach(GameObject i in objects)
                {
                    StartCoroutine(MoveTo(i, false));
                }
            }
            else if (type == 3)
            {
                foreach(GameObject i in objects)
                {
                    i.transform.position = movePosition;
                    if (i.GetComponent<Rigidbody2D>())
                    {
                        i.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                    }
                }
            }
            else if (type == 4)
            {
                foreach(GameObject i in objects)
                {
                    Instantiate(i, spawnPlace, i.transform.rotation);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == pressingObject && isSpringed)
        {
            pressed = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = normalButtonImg;
            if (type == 2)
            {
                foreach(GameObject i in objects)
                {
                    StartCoroutine(MoveTo(i, true));
                }
            }
        }
    }

    IEnumerator MoveTo(GameObject thisObject, bool movingBack)
    {
        for(int x = 0; x<timeMoving; x++)
        {
            if (movingBack)
            {
                thisObject.transform.Translate(-(movement.x/timeMoving), -(movement.y/timeMoving), 0);
            }
            else
            {
                thisObject.transform.Translate(movement.x/timeMoving, movement.y/timeMoving, 0);
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}

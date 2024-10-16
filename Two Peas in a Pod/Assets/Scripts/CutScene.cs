using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Sprite[] pictures;
    private int currPicture;
    [SerializeField] string nextScene;

    // Update is called once per frame
    void Update()
    {
        // on click, go to the next picture
        if(Input.GetMouseButtonDown(0))
        {
            if(currPicture < pictures.Length - 1)
            {
                currPicture += 1;
                GetComponent<SpriteRenderer>().sprite = pictures[currPicture];
            }
            else
            {
                // if already on last picture, move scenes
                SceneManager.LoadScene(nextScene);
            }
        }   
    }
}

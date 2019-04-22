using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySpriteAtEnd : MonoBehaviour
{
    //made to destroy a sprite when animation ends
    public void destroySprite()
    {
        Destroy(gameObject);
    }
}

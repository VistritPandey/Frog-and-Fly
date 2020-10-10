using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lilypad : MonoBehaviour
{
    // when an object enters the lilypad's collider
    // check if it's the frog and if so
    // change the layer to default
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Frog"))
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    // when an object enters the lilypad's collider
    // check if it's the frog and if so
    // change the layer to default
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Frog"))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}

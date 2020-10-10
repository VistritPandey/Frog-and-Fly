using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lilypad : MonoBehaviour
{
  
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Frog"))
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Frog"))
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}

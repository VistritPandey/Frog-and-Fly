using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// Code found at http://findnerd.com/list/view/Random-movement-of-a-sprite-in-2D/22107/
// Additional code and comments added

public class FlyMovement : MonoBehaviour
{
    [Header("Fly Variables")]
    [Tooltip("How fast the fly flies")]
    public float moveSpeed ;
    [Tooltip("Direction the fly is heading in")]
    public Vector3 currentDirection;
    [Tooltip("How fast the fly turns")]
    public float turnSpeed;
    
    private float _targetAngle;
    private Vector3 _currentPos;
    private bool _play = true;
    private Vector3 _direction;
    
    void Start()
    {
        currentDirection = Vector3.up;
        InvokeRepeating ("Start1", 0f, 5f);
    }
    void Start1(){
        _play = true;
        _direction = new Vector3(Random.Range(-3.0f,3.0f),Random.Range(-4.0f,4.0f),0); //random position in x and y
    }
    void Update()
    {
        _currentPos = transform.position;//current position of gameObject
        if(_play)
        { //calculating direction
            currentDirection = _direction - _currentPos;  
    
            currentDirection.z = 0;
            currentDirection.Normalize ();
            _play = false;
        }    
        Vector3 target = currentDirection * moveSpeed + _currentPos;  //calculating target position
        transform.position = Vector3.Lerp (_currentPos, target, Time.deltaTime);//movement from current position to target position
        _targetAngle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg - 90; //angle of rotation of gameobject
        transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (0, 0, _targetAngle), turnSpeed * Time.deltaTime); //rotation from current _direction to target _direction
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        
        if (col.gameObject.CompareTag("Border"))
        {
            CancelInvoke ();//stop call to start1 method
            _direction = new Vector3 (Random.Range (-3.0f, 3.0f), Random.Range (-4.0f, 4.0f), 0); //again provide random position in x and y
            _play = true;            
        }

    }

    void OnCollisionExit2D(Collision2D col)
    {
        InvokeRepeating ("Start1", 2f, 5f);
    }
}


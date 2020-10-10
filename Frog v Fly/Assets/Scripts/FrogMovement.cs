using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

enum Direction {
    Up = 0, 
    Down = 1,
    Left = 2, 
    Right = 3}

public class FrogMovement : MonoBehaviour
{
    [Header("Frog Variables")]
    [Tooltip("Flag telling the frog to move randomly or chase the fly")]
    public bool moveRandomly = false;
    [Tooltip("Time between each frog hop")]
    public float timeBetweenHops = 1.5f;
    [Tooltip("Flag to show the frog's raycast")]
    public bool showRaycast = false;
    
    [ReadOnly]
    [Header("Simulation Manager")]
    public SimulationManager simulationManager;

    private Vector3 _previousPad; // the position of the previous lily pad the frog is on
    private Vector3 _currentPad; // the position of the lily pad the frog is currently on

    private Animator _anim; // the frogs animator
    private Rigidbody2D _rb; // the frogs rigidbody (for physics and collisions)
    private Collider2D _tongueCollider; // the tongue collider on the frog

    private Direction _lastMovement; // the last direction the frog moved in

    private float _timer = 0; // timer current value
    private bool _timerEnded; // keeps track of if timer between movements has completed

    private float _distanceFromFly = 0; // frog's current distance from fly

    // Start is called before the first frame update
    void Start()
    {
        // get the frog's animator and rigidbody
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _tongueCollider = GetComponentInChildren<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (simulationManager.flyActive)
        {
            _distanceFromFly = Vector3.Distance(transform.position, simulationManager.fly.transform.position);
            simulationManager.UpdateDistance(_distanceFromFly);
        }
            
        bool wait = Wait();
        if (simulationManager.flyActive && wait)
        {

            Direction flyDir = GetFlyDirection();

            Move(moveRandomly ? RandomDirection() : flyDir);
        }

    }

    void Move(Direction dir)
    {
        
        // move the frog in the direction given by the method argument
        switch (dir)
        {
            case Direction.Up:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                break;
            case Direction.Down:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -180));
                break;
            case Direction.Left:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -270));
                break;
            case Direction.Right:
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
                break;
        }
        
        // generate a raycast of length 3 that looks down the frog's nose
        Vector3 forward = transform.TransformDirection(Vector2.up) * 3;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, forward);
        Debug.DrawRay(transform.position, forward, Color.white); //uncomment this to see the raycast in the scene

        
        // if the raycast hits a lilypad collider, move the frog to that lilypad
        if (hit.collider.gameObject.CompareTag("LilyPad"))
        {
            // play the frog's hop animation
            _anim.Play("frog_hop", 0, 0);
            
            transform.position = hit.collider.gameObject.transform.position;
            _previousPad = _currentPad;
            _currentPad = transform.position;

            if (_previousPad != _currentPad)
            {
                simulationManager.frogMovesValue += 1;
            }
        }
        
    }

    Direction RandomDirection()
    {
        Direction dir = (Direction) Random.Range(0, 4);

        if (dir == _lastMovement)
        {
            return RandomDirection();
        }

        _lastMovement = dir;
        return dir; 

    }
    
    Direction GetFlyDirection()
    {
        Vector3 directionVector = transform.position - simulationManager.fly.transform.position;

        // get absolute values of x and y to find see if the fly is farther horizontally or vertically
        float xDir = Mathf.Abs(directionVector.x);
        float yDir = Mathf.Abs(directionVector.y);

        if (xDir >= yDir)
        {
            if (directionVector.x < 0)
            {
                return Direction.Right;
            }
            
            return Direction.Left;
        }

        if (directionVector.y < 0)
        {
            return Direction.Up;
        }
        
        return Direction.Down;
    }

    private bool Wait()
    {
        _timer += Time.deltaTime;
 
        if (_timer >= timeBetweenHops)
        {
            _timer = 0;
            return true; //max reached - waited x - seconds
        }
 
        return false;
    }

    // if the fly enters the frog's collider, catch the fly
    private void OnTriggerEnter2D(Collider2D col)
    {
        // check if object colliding is the fly
        if (col.gameObject.CompareTag("Fly"))
        {
            // destroy the fly object
            Destroy(col.gameObject);
            // tell the sim manager that there's no fly currently active
            simulationManager.flyActive = false;
        }
    }
}

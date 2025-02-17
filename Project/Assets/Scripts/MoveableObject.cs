using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    public float objectRBMass = 0.0f; //this should be 0 unless the object is moveable, then it should be equal to the mass of the object's rigidbody
    Rigidbody2D objectRB;

    [HideInInspector] public float currentObjectRBMass; //this is the mass of the object when it is being moved by the player (equal to the player mass plus the object mass)

    private void Start()
    {
        objectRB = GetComponent<Rigidbody2D>();
        currentObjectRBMass = objectRBMass;
        objectRB.mass = currentObjectRBMass;
    }

    private void Update()
    {
        objectRB.mass = currentObjectRBMass;
    }
}

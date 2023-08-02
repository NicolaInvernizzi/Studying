using UnityEngine;

public class TestingWordLocal : MonoBehaviour
{
    [SerializeField]
    Vector3 position;
    [SerializeField]
    Transform test;
    [SerializeField]
    bool transformOn;
    [SerializeField]
    bool inverseTransformOn;

    // transform start position -2, 0, -2
    void Start()
    {
        position = test.position; // 0, 0, 0
    }

    void Update()
    {
        // based on this transform the position is in:
        // local space = relative to origin
        // world space = relative to transform position

        // TransformPoint = transform is the new origin (0, 0, 0)
        // Flow: what is the object position relative to the origin ? (from local) ->
        // place the object with the same coordinates relative to the transform (to world)

        // InverseTransformPoint = origin is the new transform (0, 0, 0)
        // Flow: what is the object position relative to the transform ? (from world) ->
        // place the object with the same coordinates relative to the origin (to local)

        if (transformOn)
        {
            position = transform.TransformPoint(position);
        }

        if (inverseTransformOn)
        {
            position = transform.InverseTransformPoint(position);
        }

        if (!transformOn && !inverseTransformOn)
        {
            position = test.position;
        }

        test.position = position;

        // Code analysis:

        // if transformOn = true & inverseTransformOn = false ->
        // object test will move diagonally (down-left), becuase:
        // frame1: position = 0, 0, 0 -> TransformPoint -> 0, 0, 0 (transform world/origin local) =
        // -2, 0, -2 (transform local/origin world)
        // frame2: position = -2, 0, -2 -> TransformPoint -> -2, 0, -2 (transform world/origin local) =
        // -4, 0, -4 (transform local/origin world)
        // so the object is moving...

        // if transformOn = true & inverseTransformOn = true ->
        // object test won't move, because: 
        // frame1: position = 0, 0, 0 -> TransformPoint -> 0, 0, 0 (transform world/origin local) =
        // -2, 0, -2 (transform local/origin world)
        //         position = -2, 0, -2 -> InverseTransformPoint -> 0, 0, 0 (transform local/origin world) =
        //         2, 0, 2 (transform world/origin local) 

        // if transformOn = false & inverseTransformOn = true ->
        // object test will move diagonally (up-right), becuase: 
        // frame1: position = 0, 0, 0 -> InverseTransformPoint -> 4, 0, 4 (transform world/origin local) =
        // 2, 0, 2 (transform local/origin world)
        // frame2: position = -2, 0, -2 -> InverseTransformPoint -> -2, 0, -2 (transform world/origin local) =
        // -4, 0, -4 (transform local/origin world)
        // so the object is moving...



        // Conclusions:

        // Object A and Object B in a world with origin O (0, 0, 0):
        // local = the position of an object relative to another object in that world.
        // Ex. A position relative to B
        // world = the position of an object relative to the origin of that world.
        // Ex. A position relative to O

    }
}

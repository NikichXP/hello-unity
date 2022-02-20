using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObjectScript : MonoBehaviour
{

    public Transform Transform;
    public GameObject Target;
    public float distance;
    public float intensity;

    void Start()
    {
    }

    void Update()
    {
        var position = Transform.position;
        var actualDistance = Target.transform.position - position + new Vector3(-distance, distance, 0);
        var transformVector = actualDistance * intensity;
        position += transformVector;
        Transform.position = position;
    }
}

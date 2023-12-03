using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reveal : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] new Light light;

    // Update is called once per frame
    void Update()
    {
        if( material && light)
        {
            material.SetVector("MyLightPosition", light.transform.position);
            material.SetVector("MyLightDirection", -light.transform.forward);
            material.SetFloat("MyLightAngle", light.spotAngle);
        }
    }
}

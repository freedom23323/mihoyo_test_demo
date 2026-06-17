using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCube : MonoBehaviour,IDestructible
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLaserHit(Vector3 hitPoint)
    {
        Debug.Log("OnLaserHit!");
        Destroy(gameObject);
    }
}

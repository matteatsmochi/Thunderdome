using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popups : MonoBehaviour
{
    public GameObject PlusOnePrefab;
    public GameObject MinusOnePrefab;

    public void SpawnPlusOne()
    {
        GameObject one = Instantiate(PlusOnePrefab, new Vector2(-400, -125), Quaternion.identity);
        one.transform.parent = gameObject.transform;
    }

   public void SpawnMinusOne()
    {
       GameObject one = Instantiate(MinusOnePrefab, new Vector2(-350, -125), Quaternion.identity);
       one.transform.parent = gameObject.transform;
    }

    
    
}

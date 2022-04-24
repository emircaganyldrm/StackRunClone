using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        //checking if object is a kinematic brick
        if (other.gameObject.layer == LayerMask.NameToLayer("Block") && other.transform.parent !=null)
        {
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false; 
            other.gameObject.transform.SetParent(null);
            StartCoroutine(BrickStacker.Instance.ArrangeHeightEnum());
            BrickStacker.Instance.totalBricks--;
            Destroy(other.gameObject, 2f);
        }
    }
    
}

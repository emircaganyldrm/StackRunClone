using System.Collections;
using UnityEngine;

public class BridgePlacer : MonoBehaviour
{
    public GameObject BridgePrefab;
    public int requiredBricks = 3;
    public float rayDistance = 2f;
    public Transform rayTransform;
    public float bridgeTimer=.5f;

    public delegate void InSufficientBricks();

    public static event InSufficientBricks OnInSufficientBricks;
    private void Start()
    {
        StartCoroutine(PlaceBridge());
    }
    
    private IEnumerator PlaceBridge()
    {
        while (true)
        {
            if (!IsGrounded() && BrickStacker.Instance.totalBricks >= requiredBricks)
            {
                BrickStacker.Instance.DeStack(requiredBricks);
                ObjectPool.Instance.GetFromPool(rayTransform.position + Vector3.down * .5f);
                yield return new WaitForSeconds(bridgeTimer);
            }
            else if (!IsGrounded() && BrickStacker.Instance.totalBricks < requiredBricks)
            {
                OnInSufficientBricks?.Invoke();
            }
            
            yield return new WaitForEndOfFrame();
        }
        
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(rayTransform.position, Vector3.down * rayDistance);
        return Physics.Raycast(rayTransform.position, Vector3.down, rayDistance);
    }

}

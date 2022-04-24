using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Speed")]
    public float zSpeed = .1f;
    public float xSpeed = 1;
    [Tooltip("Maximum x value that player can move")]
    public float xBound = 3;
    [Header("Gravity")]
    public float gravity;
    public bool gravityEnabled = true;
    private Vector2 firstPos, swipeDelta;
    private CharacterController cc;
    private bool isDragging;

    private Vector3 moveDirection;

    private void Start() 
    {
        cc = GetComponent<CharacterController>();
        cc.detectCollisions = false;
        BridgePlacer.OnInSufficientBricks += EnableGravity;
    }

    private void Update() 
    {
        #region Mouse Controls
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            firstPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            firstPos = Vector2.zero;
        }
        #endregion

        #region Touch Controls
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                isDragging = true;
                firstPos = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                firstPos = Vector2.zero;
            }
        }
        #endregion
        
        //Swipe controls
        swipeDelta = Vector2.zero;

        if (isDragging)
        {
            if (Input.touches.Length < 0)
            {
                swipeDelta = Input.touches[0].position - firstPos;
            }
            if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - firstPos;
            }
        }
        
    }

    private void FixedUpdate() 
    {
        moveDirection.z = zSpeed;
        if (gravityEnabled)
        {
            moveDirection.y = -gravity;
        }
        if (isDragging)
        {
            Debug.Log(swipeDelta);
            swipeDelta.x = Mathf.Clamp(swipeDelta.x, -50f, 50f);
            moveDirection.x = swipeDelta.x * xSpeed / 50 * Time.deltaTime;
            if (transform.position.x >= xBound && moveDirection.x > 0)
            {
                moveDirection.x = 0;
            }
            else if (transform.position.x <= -xBound && moveDirection.x < 0)
            {
                moveDirection.x = 0;
            }
        }
        else moveDirection.x = 0;
        
        cc.Move(moveDirection);
    }

    private void EnableGravity()
    {
        gravityEnabled = true;
        BridgePlacer.OnInSufficientBricks -= EnableGravity;
    }
}

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private MoveDirection direction = MoveDirection.Up;
    private enum MoveDirection { Up, Forward }

    private Vector3 origin;
    bool isReturning = false;

    private Vector3 lastPosition;
    public Vector3 PlatformVelocity { get; private set; }

    private void Start()
    {
        origin = transform.position;
        lastPosition = transform.position;
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        switch (direction) 
        {
            case MoveDirection.Up:
                movement = Vector3.up * moveSpeed * Time.deltaTime;      
                break;
            case MoveDirection.Forward:
                movement = Vector3.forward * moveSpeed * Time.deltaTime;
                break;


        }
        PlatformVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        Move(movement);
    }

    private void Move(Vector3 dir) 
    {
        float distance = Vector3.Distance(origin, transform.position);
        if (distance >= maxDistance)
        {
            isReturning = true;
        }
        else if (distance <= 0.05f)
        {
            isReturning = false;
        }

        if (isReturning)
            dir = -dir;

        transform.position += dir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 start = Application.isPlaying ? origin : transform.position;
        Vector3 end = start;
        

        switch (direction) 
        {
            case MoveDirection.Up:
                end += Vector3.up * maxDistance;
                break;
            case MoveDirection.Forward:
                end += transform.forward * maxDistance;
                break;
        }

        Gizmos.DrawWireCube(end, new Vector3(5f, 0.125f, 5f));
    }
}

using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 moveDirection = Vector3.right; // 移动方向，默认为水平方向
    public float moveDistance = 5f; // 移动范围
    public float moveSpeed = 2f; // 移动速度

    private Vector3 _startPosition;
    private bool _movingForward = true;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        MovePlatform();
    }

    void MovePlatform()
    {
        float displacement = Vector3.Distance(_startPosition, transform.position);
        
        if (_movingForward && displacement >= moveDistance)
        {
            _movingForward = false;
        }
        else if (!_movingForward && displacement <= 0.1f)
        {
            _movingForward = true;
        }
        
        if (_movingForward)
        {
            transform.position += moveDirection.normalized * moveSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= moveDirection.normalized * moveSpeed * Time.deltaTime;
        }
    }
}

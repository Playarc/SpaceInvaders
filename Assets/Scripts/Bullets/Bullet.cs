using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IBulletPoolObject
{
    [SerializeField]
    private Rigidbody2D _rigidBody = null;

    public float movementSpeed = 1f;

    private Transform _cachedTr = null;

    private float _yDeathValue = 0f;
    private Action _onBulletDeadHandler = null;

    void Awake()
    {
        _cachedTr = transform;
    }

    public void Init(Vector2 startPosition, float yDeathValue, Action onBulletDeadHandler)
    {
        _yDeathValue = yDeathValue;
        _onBulletDeadHandler = onBulletDeadHandler;

        _cachedTr.position = startPosition;
    }

    private void Move()
    {
        SetPosition(_cachedTr.position - (Vector3.down * movementSpeed * Time.fixedDeltaTime));
        CheckIfBulletIsOutOfScreen();
    }

    private void CheckIfBulletIsOutOfScreen()
    {
        if (_cachedTr.position.y >= _yDeathValue)
        {
            CallOnBulletDeadHandler();
        }
    }

    private void SetPosition(Vector2 pos)
    {
        _rigidBody.MovePosition(pos);
    }

    void FixedUpdate()
    {
        Move();
    }

    private void CallOnBulletDeadHandler()
    {
        if (_onBulletDeadHandler != null)
        {
            _onBulletDeadHandler();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        CallOnBulletDeadHandler();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

    public void Use()
    {
        gameObject.SetActive(true);
    }
}
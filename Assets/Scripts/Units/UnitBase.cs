using UnityEngine;
using System;

public abstract class UnitBase : MonoBehaviour
{
    public bool IsDead { get; private set; }

    public Transform CachedTransform { get { return _cachedTransform; } }
    protected Transform _cachedTransform = null;

    private float _endYPoint = 0f;
    private Action _onDeadHandler = null;
    private Action<UnitBase> _onUnitDeadRowHandler = null;
    private Action _onUnitPassBottomLineHandler = null;

    protected virtual void Awake()
    {
        _cachedTransform = transform;
    }

    public void Init(Transform parent, Vector3 startLocalPosition, float endYPoint, Action onUnitPassBottomLineHandler, Action onDeadHandler)
    {
        _cachedTransform.SetParent(parent, false);
        _cachedTransform.localPosition = startLocalPosition;

        _endYPoint = endYPoint;

        _onUnitPassBottomLineHandler = onUnitPassBottomLineHandler;
        _onDeadHandler = onDeadHandler;

        IsDead = false;
    }

    public void InitOnDeadHandler(Action<UnitBase> onUnitDeadRowHandler)
    {
        _onUnitDeadRowHandler = onUnitDeadRowHandler;
    }

    protected virtual void FixedUpdate()
    {
        if (!IsDead)
        {
            CheckForPassingBottomLine();
        }
    }

    private void CheckForPassingBottomLine()
    {
        if (_cachedTransform.position.y - _cachedTransform.localScale.y / 2f < _endYPoint)
        {
            IsDead = true;

            InvokeOnUnitPassedDeathLineEvent();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Dead();
    }

    public void Dead()
    {
        _onUnitPassBottomLineHandler = null;

        IsDead = true;

        InvokeOnDeadEvent();
    }

    private void InvokeOnDeadEvent()
    {
        if (_onDeadHandler != null)
        {
            _onDeadHandler();
            _onDeadHandler = null;
        }

        if (_onUnitDeadRowHandler != null)
        {
            _onUnitDeadRowHandler(this);
            _onUnitDeadRowHandler = null;
        }
    }

    private void InvokeOnUnitPassedDeathLineEvent()
    {
        if (_onUnitPassBottomLineHandler != null)
        {
            _onUnitPassBottomLineHandler();
            _onUnitPassBottomLineHandler = null;
        }
    }

    public abstract UnitType Type { get; }
}
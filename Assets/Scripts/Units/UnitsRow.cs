using UnityEngine;
using System.Collections.Generic;
using System;

public class UnitsRow : MonoBehaviour
{
    public delegate UnitBase CreateUnit(Transform parent);

    public float movementSpeed = 1f;
    public float downMovementSpeed = 1f;

    public bool HaveUnits { get { return _units.Count > 0; } }

    private List<UnitBase> _units = null;
    private Transform _cachedTr = null;

    private bool _right = true;

    private Transform _leftSideTr = null, _rightSideTr = null;
    private Transform _leftestUnit = null, _rightestUnit = null;

    private Action _onRowEmpty;

    void Awake()
    {
        _cachedTr = transform;
        _units = new List<UnitBase>();
    }

    public void Init(Transform leftSideTr, Transform rightSideTr, Action OnRowEmpty)
    {
        _leftSideTr = leftSideTr;
        _rightSideTr = rightSideTr;

        _onRowEmpty = OnRowEmpty;
    }

    public void Fill(CreateUnit createUnitHandler)
    {
        for (int i = 0; i < _cachedTr.childCount; i++)
        {
            if (createUnitHandler != null)
            {
                UnitBase unit = createUnitHandler(_cachedTr.GetChild(i));
                unit.InitOnDeadHandler(OnUnitDead);
                _units.Add(unit);
            }
        }

        UpdateSideUnits();
    }

    private void OnUnitDead(UnitBase unit)
    {
        _units.Remove(unit);
        UpdateSideUnits();

        if (!HaveUnits)
        {
            if (_onRowEmpty != null)
            {
                _onRowEmpty();
                _onRowEmpty = null;
            }
        }
    }

    void FixedUpdate()
    {
        if (HaveUnits)
        {
            Transform lastUnit = null;
            Vector3 target;

            if (_right)
            {
                lastUnit = _rightestUnit;
                target = _rightSideTr.position;
                target.x -= 0.5f; // each unit has container which size is 1f
            }
            else
            {
                lastUnit = _leftestUnit;
                target = _leftSideTr.position;
                target.x += 0.5f; // each unit has container which size is 1f
            }

            target.x += _cachedTr.position.x - lastUnit.position.x;
            target.y = _cachedTr.position.y;
            Move(target);
        }
    }

    private void Move(Vector3 target)
    {
        float step = movementSpeed * Time.fixedDeltaTime;
        _cachedTr.position = Vector3.MoveTowards(_cachedTr.position, target, step);

        if (Vector3.Distance(_cachedTr.position, target) < step)
        {
            _right = !_right;
        }
    }

    private void UpdateSideUnits()
    {
        if (HaveUnits)
        {
            _leftestUnit = GetLeftestUnit();
            _rightestUnit = GetRightestUnit();
        }
    }

    private Transform GetLeftestUnit()
    {
        UnitBase leftestUnit = _units[0];
        for (int i = 1; i < _units.Count; i++)
        {
            if (_units[i].CachedTransform.position.x < leftestUnit.CachedTransform.position.x)
            {
                leftestUnit = _units[i];
            }
        }

        return leftestUnit.CachedTransform;
    }

    private Transform GetRightestUnit()
    {
        UnitBase rightestUnit = _units[0];
        for (int i = 1; i < _units.Count; i++)
        {
            if (_units[i].CachedTransform.position.x > rightestUnit.CachedTransform.position.x)
            {
                rightestUnit = _units[i];
            }
        }

        return rightestUnit.CachedTransform;
    }
}
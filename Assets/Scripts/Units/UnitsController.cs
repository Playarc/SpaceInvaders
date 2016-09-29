using UnityEngine;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random;

public class UnitsController : MonoBehaviour
{
    [SerializeField]
    private Transform _rowTemplate = null;
    [SerializeField]
    private Transform _rowSpawnAnchor = null;

    public int rowsCount = 4;
    public float rowsYInterval = 1f;
    public float downMovementSpeed = 0.3f;

    public event Action<UnitBase> OnUnitPassedBottomLine;
    public event Action<UnitBase> OnUnitKilled;
    public event Action OnAllRowsEmpty;

    private List<UnitBase> _units = null;
    private List<UnitsRow> _rows = null;

    private Camera _camera = null;
    private float _halfOfGameScrWidth, _topGameScrPoint;
    private float _yDeathLine;

    private Transform _leftSideTr = null, _rightSideTr = null;

    private Transform _cachedTr = null;

    void Awake()
    {
        _cachedTr = transform;
    }

    public void Init(Camera gameCamera, Bounds gameScreenBounds, float yDeathLine, Transform leftSideTr, Transform rightSideTr)
    {
        _camera = gameCamera;

        _halfOfGameScrWidth = gameScreenBounds.size.x / 2f;
        _topGameScrPoint = gameScreenBounds.center.y + gameScreenBounds.extents.y;

        _yDeathLine = yDeathLine;

        _leftSideTr = leftSideTr;
        _rightSideTr = rightSideTr;

        _units = new List<UnitBase>();
        _rows = new List<UnitsRow>();
    }

    void FixedUpdate()
    {
        _cachedTr.Translate(Vector3.down * downMovementSpeed * Time.fixedDeltaTime);
    }

    public void CreateUnits()
    {
        Vector3 rowAnchorPos = _rowSpawnAnchor.position;

        for (int i = 0; i < rowsCount; i++)
        {
            rowAnchorPos.y = rowsYInterval * i;

            UnitsRow row = CreateRow(rowAnchorPos);
            row.Init(_leftSideTr, _rightSideTr, OnOneRowEmpty);
            row.Fill(CreateUnit);

            _rows.Add(row);
        }
    }

    private void OnOneRowEmpty()
    {
        for (int i = 0; i < _rows.Count; i++)
        {
            if (_rows[i].HaveUnits)
            {
                return;
            }
        }

        if (OnAllRowsEmpty != null)
        {
            OnAllRowsEmpty();
        }
    }

    private UnitsRow CreateRow(Vector3 pos)
    {
        Transform instRowTr = Instantiate(_rowTemplate);
        instRowTr.SetParent(_rowTemplate.parent, false);
        instRowTr.localPosition = pos;
        instRowTr.gameObject.SetActive(true);

        return instRowTr.GetComponent<UnitsRow>();
    }

    private UnitBase CreateUnit(Transform parent)
    {
        UnitType randomType = (UnitType)Random.Range((int)UnitType.begin + 1, (int)UnitType.end);
        UnitBase unit = UnitsFactory.Create(randomType);
        unit.Init(parent, Vector3.zero, _yDeathLine,
            () =>
            {
                if (OnUnitPassedBottomLine != null)
                {
                    OnUnitPassedBottomLine(unit);
                }
            },
            () =>
            {
                KillUnit(unit);
            });

        return unit;
    }

    private void KillUnit(UnitBase unit)
    {
        if (OnUnitKilled != null)
        {
            OnUnitKilled(unit);
        }

        Destroy(unit.gameObject);
    }
}

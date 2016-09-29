using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField]
    private Transform _bulletSpawnPlaceTr = null;

    public float movementSpeed = 1f;

    private InputController _inputCtrl = null;
    private BulletsPool _bulletsPool = null;
    private Transform _cachedTr = null;

    private Vector3 _leftSidePoint, _rightSidePoint;
    private float _yDeathValue = 0f;

    void Awake()
    {
        _cachedTr = transform;

        _inputCtrl = new InputController();
        _inputCtrl.OnSpacePressed += OnSpacePressed;
        _inputCtrl.OnArrowPressed += OnArrowPressed;

        _bulletsPool = new BulletsPool();
    }

    public void Init(Bounds screenBounds, Transform leftSideTr, Transform rightSideTr)
    {
        _leftSidePoint = new Vector3(leftSideTr.position.x + 0.5f, _cachedTr.position.y, _cachedTr.position.z);
        _rightSidePoint = new Vector3(rightSideTr.position.x - 0.5f, _cachedTr.position.y, _cachedTr.position.z);

        _yDeathValue = screenBounds.extents.y;
    }

    private void OnSpacePressed()
    {
        Fire();
    }

    private void OnArrowPressed(bool right)
    {
        Move(right);
    }

    private void Fire()
    {
        Bullet b = _bulletsPool.Get();
        b.Init(_bulletSpawnPlaceTr.position, _yDeathValue, () =>
        {
            _bulletsPool.Return(b);
        });
    }

    private void Move(bool right)
    {
        Vector3 target = right ? _rightSidePoint : _leftSidePoint;
        float step = movementSpeed * Time.deltaTime;
        _cachedTr.position = Vector3.MoveTowards(_cachedTr.position, target, step);
    }

    void Update()
    {
        _inputCtrl.Update();
    }
}
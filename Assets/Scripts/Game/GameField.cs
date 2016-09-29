using UnityEngine;

public class GameField : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _backgroundSpriteRenderer = null;
    [SerializeField]
    private Transform _deathLineTr = null;
    [SerializeField]
    private Transform _leftSideLineTr = null;
    [SerializeField]
    private Transform _rightSideLineTr = null;
    [SerializeField]
    private Transform _mainCharacterSpawnPointTr = null;

    public Transform MainCharacterSpawnPointTransform { get { return _mainCharacterSpawnPointTr; } }
    public Transform LeftSideLineTransform { get { return _leftSideLineTr; } }
    public Transform RightSideLineTransform { get { return _rightSideLineTr; } }
    public float DeathLineYPos { get { return _deathLineTr.position.y; } }

    public void Fit(Bounds bounds)
    {
        float spriteHeightInUnits = _backgroundSpriteRenderer.sprite.texture.height / _backgroundSpriteRenderer.sprite.pixelsPerUnit;
        float yFitScale = bounds.size.y / spriteHeightInUnits;

        Transform backgroundTr = _backgroundSpriteRenderer.transform;
        Vector3 localScale = backgroundTr.localScale;
        localScale.Scale(new Vector3(yFitScale, yFitScale, 1f));
        backgroundTr.localScale = localScale;
    }
}
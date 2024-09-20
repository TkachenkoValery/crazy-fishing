using UnityEngine;

[CreateAssetMenu(fileName = "FishConfig", menuName = "Config/FishConfig")]
public class FishConfig : ScriptableObject
{
    [SerializeField] private Sprite _Sprite;
    public Sprite Sprite { get => _Sprite; }

    [SerializeField] private float _Acceleration;
    public float Acceleration { get => _Acceleration; }

    [SerializeField] private Vector2 _StartAnchoredPosition;
    public Vector2 StartAnchoredPosition { get => _StartAnchoredPosition; }

    [SerializeField] private float _Height;
    public float Height { get => _Height; }

    [SerializeField] private Axis _MovementAxis;
    public Axis MovementAxis { get => _MovementAxis; }

    [SerializeField] private float _MinimalPositionAlongMovingAxis;
    public float MinimalPositionAlongMovingAxis { get => _MinimalPositionAlongMovingAxis; }

    [SerializeField] private float _MaximalPositionAlongMovingAxis;
    public float MaximalPositionAlongMovingAxis { get => _MaximalPositionAlongMovingAxis; }

    [SerializeField] int _Reward = 50;
    public int Reward { get => _Reward; }
}
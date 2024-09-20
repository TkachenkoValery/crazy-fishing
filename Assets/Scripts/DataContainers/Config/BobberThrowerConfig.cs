using UnityEngine;

[CreateAssetMenu(fileName = "BobberThrowerConfig", menuName = "Config/BobberThrowerConfig")]
public class BobberThrowerConfig : ScriptableObject
{
    [SerializeField] private float _ThrowingMinimalSpeed = 6;
    public float ThrowingMinimalSpeed { get => _ThrowingMinimalSpeed; }

    [SerializeField] private float _ThrowingMaximalSpeed = 15;
    public float ThrowingMaximalSpeed { get => _ThrowingMaximalSpeed; }

    [SerializeField] private float _ThrowingAcceleration = 0.12f;
    public float ThrowingAcceleration { get => _ThrowingAcceleration; }

    [SerializeField] private float _WaitingInWaterDuration = 2;
    public float WaitingInWaterDuration { get => _WaitingInWaterDuration; }

    [SerializeField] private int _CoinsDecrease = 100;
    public int CoinsDecrease { get => _CoinsDecrease; }

    public void ChangeDirection()
    {
        _ThrowingAcceleration *= -1;
    }
}
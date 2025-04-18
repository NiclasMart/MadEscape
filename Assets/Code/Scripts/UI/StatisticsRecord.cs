using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Statistics", menuName = "Scriptable Objects/StatisticsRecord")]
public class StatisticsRecord : ScriptableObject
{
    [HideInInspector] public UnityEvent<int> OnKillCountUpdate = new();
    public int TotalKills { get; private set; }
    public void AddKill()
    {
        TotalKills++;
        OnKillCountUpdate?.Invoke(TotalKills);
    }

    [HideInInspector] public UnityEvent<float> OnDealtDamageUpdate = new();
    public float TotalDealtDamage { get; private set; }
    public void AddDealtDamage(float damage)
    {
        TotalDealtDamage += damage;
        OnDealtDamageUpdate?.Invoke(TotalDealtDamage);
    }

    [HideInInspector] public UnityEvent<float> OnSufferedDamageUpdate = new();
    public float TotalSufferedDamage { get; private set; }
    public void AddSufferedDamage(float damage)
    {
        TotalSufferedDamage += damage;
        OnSufferedDamageUpdate?.Invoke(TotalSufferedDamage);
    }

    public void Reset()
    {
        TotalKills = 0;
        OnKillCountUpdate?.Invoke(TotalKills);

        TotalDealtDamage = 0;
        OnDealtDamageUpdate?.Invoke(TotalDealtDamage);

        TotalSufferedDamage = 0;
        OnSufferedDamageUpdate?.Invoke(TotalSufferedDamage);
    }
}

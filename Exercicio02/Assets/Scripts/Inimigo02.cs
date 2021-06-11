using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inimigo02 : MonoBehaviour
{
    private Inimigos comportamento;
    public UnityEvent AlertOthers;
    private List<Inimigo01> Stop;
    void Start()
    {
        comportamento = GetComponent<Inimigos>();
        AlertOthers = new UnityEvent();
        Stop = FindObjectsOfType<Inimigo01>().ToList();
        foreach (var enemy in Stop)
        {
            AlertOthers.AddListener(enemy.AwareOfPlayer);
        }
    }

    void Update()
    {
        if (comportamento.PlayerDetected)
        {
            AlarmAll();
        }
    }

    void AlarmAll()
    {
        AlertOthers.Invoke();
    }
}

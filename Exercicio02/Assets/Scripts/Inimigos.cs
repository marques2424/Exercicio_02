using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using Panda;
using UnityEditor.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Inimigos : MonoBehaviour
{
    public NavMeshAgent agent;
    private Transform pTransform;
    [Task]
    private bool playerDetected;
    [Task]
    private bool alerted;
    [Task]
    private bool globalDetection;
    public UnityEvent AlertOthers;
    private Vector3 startPos;
    private bool _canAlertOthers;
    public Vector3 ultimaPlayerPos;
    void Start()
    {
        pTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _canAlertOthers = false;
        startPos = transform.position;
        playerDetected = false;
        alerted = false;
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, (pTransform.position - transform.position).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, 100) && hit.transform.CompareTag("Player"))
        {
            globalDetection = false;
            playerDetected = true;
            alerted = true;
            ultimaPlayerPos = pTransform.position;
        }
        else
        {
            playerDetected = false;
        }
    }

    [Task]
    void Chase()
    {
        agent.SetDestination(ultimaPlayerPos);
        Task.current.Succeed();
    }

    [Task]
    void CheckAround()
    {
        if (playerDetected)
        {
            Task.current.Fail();
            return;
        }
        agent.SetDestination(transform.position + Random.rotation * Vector3.forward * 3);
        Task.current.Succeed();
    }

    [Task]
    void CheckLastPosition()
    {
        agent.SetDestination(ultimaPlayerPos);
        if (transform.position == ultimaPlayerPos)
        {
            Task.current.Succeed();
        }
    }

    [Task]
    void ForgetPlayer()
    {
        alerted = false;
        Task.current.Succeed();
    }

    [Task]
    void ReturnToOrigin()
    {
        agent.SetDestination(startPos);
        if (transform.position == startPos)
        {
            Task.current.Succeed();
        }
    }

   
    public bool PlayerDetected => playerDetected;


    public bool GlobalDetection
    {
        get => globalDetection;
        set => globalDetection = value;
    }

    public bool Alerted
    {
        get => alerted;
        set => alerted = value;
    }
}
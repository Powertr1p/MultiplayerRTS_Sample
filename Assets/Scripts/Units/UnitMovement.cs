using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class UnitMovement : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Camera _camera;
    
#region Server

    [Command]
    private void CmdMove(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return;
        _navMeshAgent.SetDestination(position);
    }
    
#endregion

#region Client

    public override void OnStartAuthority()
    {
        _camera = Camera.main;
    }

    [ClientCallback]
    private void Update()
    {
        if (!hasAuthority) return;
        if (!Mouse.current.rightButton.wasPressedThisFrame) return;

        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;
        
        CmdMove(hit.point);
    }

#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
using Meta.XR.MRUtilityKit;

public class RuntimeNavMeshBuilder : MonoBehaviour
{

    private NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = GetComponent<NavMeshSurface>();
        MRUK.Instance.RegisterSceneLoadedCallback(NavMeshBuilding);
    }

    public void NavMeshBuilding()
    {
        StartCoroutine(BuildNavMeshRoutine());
    }


    public IEnumerator BuildNavMeshRoutine()
    {
        yield return new WaitForEndOfFrame();
        navMeshSurface.BuildNavMesh();
    }
}

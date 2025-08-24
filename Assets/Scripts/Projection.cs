using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations = 50;
    [SerializeField] private GameObject Dock;

    private Scene _simulationScene;
    private PhysicsScene _physicsScene;

    [SerializeField] private GameObject ghostDogPrefab;
    private GameObject ghostDog;
    private Rigidbody ghostRb;

    private void Start()
    {
        _line.positionCount = _maxPhysicsFrameIterations;
        CreatePhysicsScene();
    }

    private void CreatePhysicsScene()
    {
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
        _physicsScene = _simulationScene.GetPhysicsScene();

        GameObject ghostObj = Instantiate(Dock, Dock.transform.position, Dock.transform.rotation);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);

        ghostDog = Instantiate(ghostDogPrefab, Vector3.zero, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostDog, _simulationScene);
        ghostRb = ghostDog.GetComponent<Rigidbody>();
        ghostDog.GetComponent<Renderer>().enabled = false;
    }
    public void SimulateTrajectory(Vector3 pos, Vector3 launchForce, Vector3 currentVelocity)
    {
        ghostDog.transform.position = pos;

        ghostRb.linearVelocity = currentVelocity;
        ghostRb.AddForce(launchForce, ForceMode.Impulse);

        for (int i = 0; i < _maxPhysicsFrameIterations; i++)
        {
            _physicsScene.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostDog.transform.position);
        }
    }

    public Tuple<Vector3, float> GetPoint(float percentageOfArcCompleted)
    {
        //Get all points above initial point y value;
        List<Vector3> myPoints = new List<Vector3>();
        Vector3[] linePoints = new Vector3[_line.positionCount];
        _line.GetPositions(linePoints);

        float initialY = linePoints[0].y;

        for(int i = 1; i < linePoints.Length; i++)
        {
            if (linePoints[i].y >= initialY)
            {
                myPoints.Add(linePoints[i]);
            }
        }

        //Get size of array and select point where percentageOfArc will be completed;

        int index = (int)((float)myPoints.Count * percentageOfArcCompleted);

        if (index >= myPoints.Count) index = myPoints.Count - 1;
        Vector3 point = myPoints[index];

        //Get amount of time until that point is reached;
        float time = Time.fixedDeltaTime * index + Time.fixedDeltaTime;

        return new Tuple<Vector3, float>(point, time);
    }
}
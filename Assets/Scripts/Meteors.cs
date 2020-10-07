using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Meteors : MonoBehaviour
{
    [HideInInspector]
    [Tooltip("Create a non-linear or looping path using nodes")]
    public bool nonLinearPath = false;

    [HideInInspector]
    [Tooltip("Amount of loops around the path the meteor shower makes")]
    public int amountOfLoops;
    [HideInInspector]
    [Tooltip("Nodes that define the path of the meteor shower")]
    public List<Vector3> pathPoints;
    [HideInInspector]
    [Tooltip("Does the path include the start point?")]
    public bool pathIncludesStart = true;
    [HideInInspector]
    [Tooltip("Does the path include the end point?")]
    public bool pathIncludesEnd = false;

    [Space(20)]

    [Tooltip("Meteor shower start point")]
    [HideInInspector]
    public Vector3 start;
    [Tooltip("Meteor shower end point")]
    [HideInInspector]
    public Vector3 end;

    [Space(10)]
    [HideInInspector]
    [Tooltip("Meteor shower speed")]
    public float speed;

    private Vector3 nextDestination;
    private int pathInd = 0;
    private int loopCount = 0;
    private Vector3 direction;
    private float gamma = 0.03f;
    private bool smoothCheck = false;

    public void Start()
    {
        nextDestination = new Vector3();
        this.transform.position = start;
        UpdateNextDestination();
    }

    public void Update()
    {
        // Move meteor towards their destination
        Vector3 pos = transform.position;
        float speedMult = speed * Time.deltaTime;
        transform.position = new Vector3(pos.x + direction.x * speedMult, pos.y + direction.y * speedMult, pos.z + direction.z * speedMult);
        pos = transform.position;
        // If the meteors have reached their destination, update their next destination
        if (pos.x > nextDestination.x - gamma && pos.x < nextDestination.x + gamma && pos.y > nextDestination.y - gamma && pos.y < nextDestination.y + gamma && pos.z > nextDestination.z - gamma && pos.z < nextDestination.z + gamma) 
        {
            UpdateNextDestination();
        }
    }

    public void UpdateNextDestination() 
    {
        if (nonLinearPath)
        {
            if (pathInd >= pathPoints.Count)
            {
                // Go to the end if pathIncludesEnd
                if (pathIncludesEnd && pathInd == pathPoints.Count)
                {
                    nextDestination = end;
                    pathInd++;
                }
                // Go to the start
                else if (pathIncludesStart)
                {
                    nextDestination = start;
                    pathInd = 0;
                    loopCount++;
                }
                // Go back to the first path point
                else
                {
                    nextDestination = pathPoints[0];
                    pathInd = 1;
                    loopCount++;
                }
                
                if (loopCount == amountOfLoops) 
                {
                    // Meteors have finished looping, send them to their end point
                    if (!pathIncludesEnd)
                    {
                        nextDestination = end;
                        nonLinearPath = false;
                    }
                    else 
                    {
                        MeteorReachedEnd();
                    }
                }
            }
            else
            {
                nextDestination = pathPoints[pathInd];
                pathInd++;
            }
        }
        else 
        {
            if (nextDestination == end)
            {
                MeteorReachedEnd();
            }
            else 
            {
                nextDestination = end;
            }
        }
        direction = (nextDestination - transform.position);
        direction.Normalize();
    }

    public void MeteorReachedEnd() 
    {
        // The meteors have reached their final desination, so destroy
        Destroy(this.gameObject);
    }

    // Use a planet's centre as an end point rather than a specific vector
    public void SetEndPlanet(Planet planet) 
    {
        end = planet.transform.position;
    }
    
    // The meteors collide with something
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Taxi") 
        {
            GameObject taxi = collision.gameObject;
            // Destroy the taxi
            // End the game
        }
    }
}




#if UNITY_EDITOR
[CustomEditor(typeof(Meteors))]
public class Meteors_Editor : Editor
{

    private SerializedProperty _pathPoints;

    private void OnEnable()
    {
        // do this only once here
        _pathPoints = serializedObject.FindProperty("pathPoints");
        _pathPoints.arraySize = 0;
    }
    public override void OnInspectorGUI()
    {
        Meteors script = (Meteors)target;

        //Add the default "this script" variable at the top of the component
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((Meteors)target), typeof(Meteors), false);
        GUI.enabled = true;

        // draw checkbox for the bool
        script.nonLinearPath = EditorGUILayout.Toggle("Non Linear Path", script.nonLinearPath);
        if (script.nonLinearPath) // if bool is true, show other fields
        {
            script.amountOfLoops = EditorGUILayout.IntField("Amount Of Loops", script.amountOfLoops);

            serializedObject.Update();
            _pathPoints.arraySize = EditorGUILayout.IntField("Path Points Size", _pathPoints.arraySize);
            for (int i = 0; i < _pathPoints.arraySize; i++)
            {
                var dialogue = _pathPoints.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue, new GUIContent("pathPoints " + i), true);
            }
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            script.pathIncludesStart = EditorGUILayout.Toggle("Path Includes Start", script.pathIncludesStart);
            script.pathIncludesEnd = EditorGUILayout.Toggle("Path Includes End", script.pathIncludesEnd);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
        // The variables that should always show
        script.start = EditorGUILayout.Vector3Field("Start", script.start);
        script.end = EditorGUILayout.Vector3Field("End", script.end);
        script.speed = EditorGUILayout.FloatField("Speed", script.speed);

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
#endif

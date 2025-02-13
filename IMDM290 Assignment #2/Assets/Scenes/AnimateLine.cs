/*
I attempted to make the animation resemble that of an actual beating heart by speeding up the animation,
shortening the range of the lines, and adding in a transitioning color closer to that of a heart's

I would have liked to change the oscillation to be more like a beating heart but I could not get the result I wanted,
and I have been sick all week, so I just kind of gave up on that
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateLine : MonoBehaviour
{
    LineRenderer[] lineRenderer;
    static int numSphere = 200; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPosition, endPosition;
    float lerpFraction; // Lerp point between 0~1
    float t;

    //switch between a light and dark pink (are hearts actually colored this way?)
    Color lerpColor = Color.red;
    Color lerpColor2 = Color.red;
    Color pink = new Color(0.99f, 0.56f, 0.67f);
    Color darkPink = new Color(0.66f, 0.25f, 0.39f);

    void Start()
    {
        // Assign proper types and sizes to the variables.
        lineRenderer = new LineRenderer[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere]; 
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random start positions
            //decreased starting positions 
            float r = 5f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        
            // Heart shape end position
            t = i * 2 * Mathf.PI / numSphere;
            endPosition[i] = new Vector3( 
                        5f*Mathf.Sqrt(2f) * Mathf.Sin(t) *  Mathf.Sin(t) *  Mathf.Sin(t),
                        5f* (- Mathf.Cos(t) * Mathf.Cos(t) * Mathf.Cos(t) - Mathf.Cos(t) * Mathf.Cos(t) + 2 *Mathf.Cos(t)) + 3f,
                        10f + Mathf.Sin(time));
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Position
            initPos[i] = startPosition[i];
            // Color
        
        }
        // Line
        for (int i = 1; i < numSphere; i++){
            //For creating line renderer object
            lineRenderer[i] = new GameObject("Line").AddComponent<LineRenderer>();
            lineRenderer[i].material = new Material(Shader.Find("Sprites/Default")); 
                            
            //For drawing line in the world space, provide the x,y,z values
            lineRenderer[i].SetPosition(0, initPos[i]);
            lineRenderer[i].SetPosition(1, initPos[i-1]); 
        }
    }

    void Update()
    {
        // Measure Time 
        time += Time.deltaTime;
        
        for (int i =1; i < numSphere; i++){
            //sped up oscillation
            lerpFraction = Mathf.Sin(time * 5) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            lineRenderer[i].SetPosition(0, Vector3.Lerp(startPosition[i-1], endPosition[i-1], lerpFraction)); 
            lineRenderer[i].SetPosition(1, Vector3.Lerp(startPosition[i], endPosition[i], lerpFraction)); 
            int randindex = (int)Random.Range(1,numSphere);
            
            // Color Update over time
            lerpColor = Color.Lerp(darkPink, pink, Mathf.PingPong(Time.time * 2, 1));
            lerpColor2 = Color.Lerp(darkPink, pink, Mathf.PingPong(Time.time * 2.1f, 1));
            lineRenderer[i].startColor = lerpColor;
            lineRenderer[i].endColor = lerpColor2;
        }
    }
}

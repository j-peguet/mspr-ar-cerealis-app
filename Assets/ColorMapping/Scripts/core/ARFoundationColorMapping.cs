using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARFoundationColorMapping : MonoBehaviour
{
    public ARTrackedImageManager imageManager;

    public GameObject arPrefabs;
    public GameObject[] listARPrefabs;

    public int realWidth;
    public int realHeight;

    private Dictionary<string, GameObject> dictPrebaps = new Dictionary<string, GameObject>();

    private GameObject arContents;
    private Dictionary<string, GameObject> listARContents = new Dictionary<string, GameObject>();

    private GameObject drawObj;
    private Dictionary<string, GameObject> listDrawObj = new Dictionary<string, GameObject>();

    private GameObject cube;
    private Dictionary<string, GameObject> listCube = new Dictionary<string, GameObject>();

    private bool isStart = false;
    private bool isEnd = false;

    public Text timeText;
    private float time;

    private void Update()
    {
        if (isStart)
        {
            time += Time.deltaTime;
        }

        if (isEnd)
        {
            timeText.text = (time * 1000).ToString() + "ms";

            time = 0.0f;
            isEnd = false;
        }
    }

    void Start()
    {
        imageManager.trackedImagesChanged += OnTrackedImagesChanged;

        foreach(GameObject prepab in listARPrefabs)
        {
            Debug.Log("Prepab Name" + prepab.name);
            dictPrebaps.Add(prepab.name, prepab);
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // ARTrackedImage trackedImage = null;

        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            // trackedImage = eventArgs.added[i];

            string imgName = trackedImage.referenceImage.name;
            Debug.Log("Image Name" + imgName);
          
            // arContents = Instantiate(arPrefabs, trackedImage.transform);
            // GameObject newPrefap = Instantiate(dictPrebaps[trackedImage.name], trackedImage.transform);
            GameObject newPrefap = Instantiate(dictPrebaps[trackedImage.referenceImage.name], trackedImage.transform);
            newPrefap.name = trackedImage.referenceImage.name;
            Debug.Log("Position" + trackedImage.transform.localPosition.x + " --- " + trackedImage.transform.localPosition.y);
            Debug.Log(trackedImage.transform.localPosition.x);
            Debug.Log(trackedImage.transform.localPosition.y);
            // cube = CreateCubeForARFoundationTarget(arContents.gameObject, trackedImage.size.x, trackedImage.size.y);
            // cube = CreateCubeForARFoundationTarget(newPrefap.gameObject, trackedImage.size.x, trackedImage.size.y);
            GameObject newCube  = CreateCubeForARFoundationTarget(newPrefap.gameObject, trackedImage.size.x, trackedImage.size.y);
                
            listARContents.Add(trackedImage.referenceImage.name, newPrefap);
            listCube.Add(trackedImage.referenceImage.name, newCube);

            // Coloring(listARContents[trackedImage.referenceImage.name]);
        }

        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            // trackedImage = eventArgs.updated[i];

            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                // arContents.SetActive(true);
                listARContents[trackedImage.referenceImage.name].SetActive(true);
                // Coloring(listARContents[trackedImage.referenceImage.name]);
            }
            else
            {
                // arContents.SetActive(false);
                // listARContents[trackedImage.referenceImage.name].SetActive(false);
            }
        }

        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            // arContents.SetActive(false);
            listARContents[trackedImage.referenceImage.name].SetActive(false);
        }
    }

    public void Play()
    {
        //isStart = true;

        // Coloring(listARContents["CubeObject"]);

        foreach(ARTrackedImage trackedImage in imageManager.trackables)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                Coloring(listARContents[trackedImage.referenceImage.name]);
            }
        }

        // float[] srcValue = AirarManager.Instance.CalculateMarkerImageVertex(cube);
        // float[] srcValue = AirarManager.Instance.CalculateMarkerImageVertex(listCube["sw-12"]);

        // Texture2D screenShotTex = ScreenShot.GetScreenShot(arContents);
        // Texture2D screenShotTex = ScreenShot.GetScreenShot(listARContents["sw-12"]);

        //AirarManager.Instance.ProcessColoredMapTexture(screenShotTex, srcValue, realWidth, realHeight, (resultTex) =>
        //{
            // drawObj = GameObject.FindGameObjectWithTag("coloring");
            // drawObj = listARContents["sw-12"];
            // drawObj.GetComponent<Renderer>().material.mainTexture = resultTex;
            

            //isStart = false;
            //isEnd = true;
        //});
    }

    private void Coloring(GameObject gameObject)
    {
        isStart = true;
        Debug.Log("gameObject" + gameObject);

        Debug.Log("ListCube[gameObject.name]" + listARContents[gameObject.name]);

        // float[] srcValue = AirarManager.Instance.CalculateMarkerImageVertex(cube);
        float[] srcValue = AirarManager.Instance.CalculateMarkerImageVertex(listCube[gameObject.name]);
        Debug.Log("SRC Value" + srcValue.Length);

        // Texture2D screenShotTex = ScreenShot.GetScreenShot(arContents);
        Texture2D screenShotTex = ScreenShot.GetScreenShot(listARContents[gameObject.name]);
        Debug.Log("screenShotTex" + screenShotTex.name);

        AirarManager.Instance.ProcessColoredMapTexture(screenShotTex, srcValue, realWidth, realHeight, (resultTex) =>
        {
            // drawObj = GameObject.FindGameObjectWithTag("coloring");
            // Debug.Log("Draw Object" + drawObj);
            // Debug.Log(drawObj);
            // drawObj = gameObject;
            listARContents[gameObject.name].GetComponent<Renderer>().material.mainTexture = resultTex;
            

            isStart = false;
            isEnd = true;
        });
    }

    /// <summary>
    /// Create a full size cube on the ARFoundation marker image
    /// </summary>
    /// <param name="targetWidth">marker image width</param>
    /// <param name="targetHeight">marker image height</param>
    public GameObject CreateCubeForARFoundationTarget(GameObject parentObj, float targetWidth, float targetHeight)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.GetComponent<Renderer>().material = AirarManager.Instance.transparentMat;
        //cube.GetComponent<Renderer>().material.color = Color.green;
        cube.transform.SetParent(parentObj.transform);
        cube.transform.localPosition = Vector3.zero;
        cube.transform.localRotation = Quaternion.Euler(Vector3.zero);
        cube.transform.localScale = new Vector3(targetWidth, 0.001f, targetHeight);

        return cube;
    }
}

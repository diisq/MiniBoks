using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // for Text

public class ShapeSpawner : MonoBehaviour
{
    public GameObject squarePrefab;
    public GameObject circlePrefab;
    public GameObject trianglePrefab;
    public GameObject conveyorPrefab; // conveyor belt prefab

    public string spawnedLayerName = "SpawnedLayer"; // optional: use layer instead of tag
    private int spawnedLayer;

    // track spawned objects
    private List<GameObject> spawnedObjects = new List<GameObject>();

    [Header("UI Feedback")]
    public Text feedbackText;           // assign a UI Text in Canvas
    public float fadeDuration = 1f;     // how long fade in/out takes
    public float stayDuration = 1f;     // how long text stays visible

    private Coroutine feedbackRoutine;

    void Start()
    {
        spawnedLayer = LayerMask.NameToLayer(spawnedLayerName);
        if (spawnedLayer == -1)
        {
            Debug.LogError("Layer " + spawnedLayerName + " does not exist! Add it in Project Settings > Tags and Layers.");
        }

        if (feedbackText != null)
            feedbackText.canvasRenderer.SetAlpha(0f); // start invisible
    }

    void Update()
    {
        // delete last spawned with Z
        if (Input.GetKeyDown(KeyCode.Z) && spawnedObjects.Count > 0)
        {
            GameObject last = spawnedObjects[spawnedObjects.Count - 1];
            spawnedObjects.RemoveAt(spawnedObjects.Count - 1);

            // strip (Clone) from the name
            string deletedName = last.name.Replace("(Clone)", "").Trim();

            Destroy(last);

            // show feedback
            if (feedbackText != null)
            {
                if (feedbackRoutine != null) StopCoroutine(feedbackRoutine);
                feedbackRoutine = StartCoroutine(ShowFeedback(deletedName + " got destroyed"));
            }
        }
    }

    public void SpawnSquare()
    {
        Spawn(squarePrefab);
    }

    public void SpawnCircle()
    {
        Spawn(circlePrefab);
    }

    public void SpawnTriangle()
    {
        Spawn(trianglePrefab);
    }

    public void SpawnConveyor()
    {
        Spawn(conveyorPrefab);
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        obj.layer = spawnedLayer; // assign to custom layer

        // track it for undo delete
        spawnedObjects.Add(obj);
    }

    public void ClearAll()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == spawnedLayer)
            {
                Destroy(obj);
            }
        }

        // also clear the list so Z doesn't reference destroyed objects
        spawnedObjects.Clear();
    }

    private System.Collections.IEnumerator ShowFeedback(string message)
    {
        feedbackText.text = message;

        // Fade In
        feedbackText.CrossFadeAlpha(1f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration + stayDuration);

        // Fade Out
        feedbackText.CrossFadeAlpha(0f, fadeDuration, false);
    }
}

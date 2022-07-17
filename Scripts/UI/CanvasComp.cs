using UnityEngine;

public class CanvasComp : MonoBehaviour
{
    public Canvas CanvasPrefab;
    void Start()
    {
        if (!CanvasPrefab) { return; }
        Instantiate(CanvasPrefab);
    }
}

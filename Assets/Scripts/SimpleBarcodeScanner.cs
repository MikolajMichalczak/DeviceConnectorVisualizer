using UnityEngine;
using Vuforia;

public class SimpleBarcodeScanner : MonoBehaviour
{
    [SerializeField]
    bool isScannerMocked;
    public GameObject model;
    private GameObject modelInstance;
    BarcodeBehaviour mBarcodeBehaviour;
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
    }

    void Update()
    {
        if ((mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null) || isScannerMocked)
        {
            if (modelInstance == null)
            {
                ModeSelector.Selector.panel.SetActive(true);
                modelInstance = Instantiate(model, new Vector3(0,0,0), Quaternion.identity);
            }
        }
    }

    public void MockScanner()
    {
        isScannerMocked = !isScannerMocked;
    }

}
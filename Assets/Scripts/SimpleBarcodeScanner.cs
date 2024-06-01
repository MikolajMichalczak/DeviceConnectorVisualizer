using System;
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
                String barCodeText = mBarcodeBehaviour.InstanceData.Text;
                if(barCodeText == "ABC-abc-1234")
                {
                    ModeSelector.Selector.panel.SetActive(true);
                    modelInstance = Instantiate(model, new Vector3(0,0,0), Quaternion.identity);
                } else if (barCodeText == "DEF-def-5678") {
                    //pokaz drugi model (ukryj wczesniejszy i przepnij panel na nowy?)
                }
            }
        }
    }

    public void MockScanner()
    {
        isScannerMocked = !isScannerMocked;
    }

}
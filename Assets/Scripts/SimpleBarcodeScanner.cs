using System;
using UnityEngine;
using Vuforia;
 
public class SimpleBarcodeScanner : MonoBehaviour
{
    [SerializeField]
    bool isScannerMocked;
    public GameObject model_1;
    public GameObject model_2;
    BarcodeBehaviour mBarcodeBehaviour;
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
    }
 
    void Update()
    {
        if ((mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null) || isScannerMocked)
        {
            ModeSelector.Selector.panel.SetActive(true);
            String barCodeText = mBarcodeBehaviour.InstanceData.Text;
            if(barCodeText == "ABC-abc-1234")
            {
                model_2.gameObject.SetActive(false);
                model_1.gameObject.SetActive(true);
            } else if (barCodeText == "DEF-def-5678") {
                model_1.gameObject.SetActive(false);
                model_2.gameObject.SetActive(true);
            }
        }
    }
 
    public void MockScanner()
    {
        isScannerMocked = !isScannerMocked;
    }
 
}
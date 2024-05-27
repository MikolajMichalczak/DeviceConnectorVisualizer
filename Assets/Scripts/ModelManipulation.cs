using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModelManipulation : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.3f;
    [SerializeField]
    private float zoomSpeed = 0.7f;
    [SerializeField]
    private string selectableTag = "selectable";
    [SerializeField]
    private Material highlightMaterial;
    [SerializeField]
    private Material defaultMaterial;
    [SerializeField]
    private List<GameObject> ports;
    [SerializeField]
    private GameObject textField;

    private float dist;
    private bool dragging = false;
    private Vector3 offset;
    Transform selection;

    void Update()
    {
        if (ModeSelector.Selector.mode != ModeSelector.Mode.view)
        {
            //Wyłączanie widoczności portów
            HighlightPorts(false);
            if (selection != null)
            {
                ResetViewedObj(selection.gameObject);
            }
        }
        switch (ModeSelector.Selector.mode)
        {
            case ModeSelector.Mode.view:
                HighlightPorts(true);
                ViewObj();
                break;
            case ModeSelector.Mode.rotate:
                RotateObj();
                break;
            case ModeSelector.Mode.move:
                MoveObj();
                break;
            case ModeSelector.Mode.pull:
                PullObj();
                break;
            case ModeSelector.Mode.push:
                PushObj();
                break;
        }
    }

    void ViewObj()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Transform currentSelection = hit.transform;
                    if (currentSelection.CompareTag(selectableTag))
                    {
                        if (selection != null && selection != currentSelection)
                        {
                            ResetViewedObj(selection.gameObject);
                        }
                        selection = currentSelection;
                        HandleSelectedObj(selection.gameObject);
                    }
                }
            }
        }
    }

    void HandleSelectedObj(GameObject obj)
    {
        SetMaterial(obj, highlightMaterial);
        if(!obj.TryGetComponent<Port>(out Port port))
        {
            Debug.LogWarning("Component not found -> port");
            return;
        }
        FindObjectsOfType<Canvas>()[0].transform.Find("wtyczka").gameObject.SetActive(true);
        //TODO: wyświetlanie wtyczki, zmienna port ma w sobie info w co kliknął uzytkownik, można do niej wsadzic model wtyczki do wyświeltenia idk
        
    }

    void ResetViewedObj(GameObject obj)
    {
        SetMaterial(obj, defaultMaterial);
        //TODO: usuwanie modelu/info o wyświetlanej wtyczce
    }

    void HighlightPorts(bool isActive)
    {
        foreach (var port in ports)
        {
            port.SetActive(isActive);
        }
    }

    void SetMaterial(GameObject obj, Material material)
    {
        if (obj.TryGetComponent<Renderer>(out var selectionRenderer))
        {
            selectionRenderer.material = material;
        }
    }

    void RotateObj()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.Rotate(touch.deltaPosition.y * sensitivity, touch.deltaPosition.x * sensitivity, 0f, Space.World);
            }
        }
    }

    void MoveObj()
    {
        Vector3 v3;
        if (Input.touchCount != 1)
        {
            dragging = false;
            return;
        }
        Touch touch = Input.touches[0];
        Vector3 pos = touch.position;
        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                dist = hit.transform.position.z - Camera.main.transform.position.z;
                v3 = new Vector3(pos.x, pos.y, dist);
                v3 = Camera.main.ScreenToWorldPoint(v3);
                offset = transform.position - v3;
                dragging = true;
            }
        }
        if (dragging && touch.phase == TouchPhase.Moved)
        {
            v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            v3 = Camera.main.ScreenToWorldPoint(v3);
            transform.position = v3 + offset;
        }
        if (dragging && (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled))
        {
            dragging = false;
        }
    }

    void PullObj()
    {
        const int pull = 1;
        MoveInOut(pull);
    }

    void PushObj()
    {
        const int push = -1;
        MoveInOut(push);
    }

    void MoveInOut(int direction)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 pos = Camera.main.ScreenToWorldPoint(touch.position);
            transform.position = Vector3.MoveTowards(transform.position, direction * pos, zoomSpeed * Time.deltaTime);
        }
    }
}


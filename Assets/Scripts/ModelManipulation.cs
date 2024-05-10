using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ModelManipulation : MonoBehaviour
{
    [SerializeField]
    private float sensitivity = 0.3f;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private float zoomSpeed = 0.7f;

    private float dist;
    private bool dragging = false;
    private Vector3 offset;

    void Update()
    {
        switch (ModeSelector.Selector.mode)
        {
            case ModeSelector.Mode.view:
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
        //TODO: wybór portu lub złącza
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


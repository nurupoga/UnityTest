using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class Teleport : MonoBehaviour
{
    [SerializeField]
    GameObject leftHand, rightHand;
    GameObject line;
    int positionCount = 10;
    float angle = 30;
    float groundPositionY = 0;
    float lazerStartDistance = 0.15f;
    float distance = 5;
    float minDistance = 1;
    float maxDistance = 10;
    float width = 0.1f;
    bool activeDraw = false;
    bool useLeftHand = false;
    Vector3 hitPoint;
    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        Line();
    }
    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        Controller();
    }
    void Controller()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LIndexTrigger))
        {
            activeDraw = true;
            line.SetActive(true);
            useLeftHand = true;
        }
        else if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            activeDraw = true;
            line.SetActive(true);
            useLeftHand = false;
        }
        if (OVRInput.GetUp(OVRInput.RawButton.LIndexTrigger))
        {
            activeDraw = false;
            line.SetActive(false);
            distance = 5;
            this.transform.position = new Vector3(hitPoint.x, this.transform.position.y, hitPoint.z);
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.RIndexTrigger))
        {
            activeDraw = false;
            line.SetActive(false);
            distance = 5;
            this.transform.position = new Vector3(hitPoint.x, this.transform.position.y, hitPoint.z);
        }
        if(activeDraw == true && useLeftHand == true)
        {
            Ray();
        }
        else if (activeDraw == true && useLeftHand == false)
        {
            RightRay();
        }
    }
    void ChangeDirection()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickLeft))
        {
            this.transform.Rotate(0, -angle, 0);
        }
        if (OVRInput.GetDown(OVRInput.RawButton.LThumbstickRight))
        {
            this.transform.Rotate(0, angle, 0);
        }
    }
    void Line()
    {
        line = new GameObject("Line");
        line.transform.parent = leftHand.transform;
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material.color = new Color32(127, 255, 212, 1);
        lineRenderer.receiveShadows = false;
        lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
        lineRenderer.loop = false;
        lineRenderer.positionCount = positionCount;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
    void Ray()
    {
        ChangeDistance();
        Vector3 rayStartPosition = leftHand.transform.position + leftHand.transform.forward * lazerStartDistance;
        Vector3 rayMiddlePosition = rayStartPosition + leftHand.transform.forward * (distance / 2);
        Vector3 rayFinishPosition = rayStartPosition + leftHand.transform.forward * distance;
        rayFinishPosition.y = groundPositionY;
        Vector3 tmp0 = rayStartPosition;
        for(int i = 0; i < positionCount; i++)
        {
            float plotPosition = (i / (float)(positionCount - 1));
            Vector3 tmp1  = Vector3.Lerp(rayStartPosition, rayMiddlePosition, plotPosition);
            Vector3 tmp2 = Vector3.Lerp(rayMiddlePosition, rayFinishPosition, plotPosition);
            Vector3 tmp3 = Vector3.Lerp(tmp1, tmp2, plotPosition);
            RaycastHit hit;
            if(Physics.Linecast(tmp0, tmp3, out hit))
            {
                hitPoint = hit.point;
                for(int j = i; j < positionCount; j++)
                {
                    lineRenderer.SetPosition(j, hitPoint);
                }
                break;
            }
            else
            {
                lineRenderer.SetPosition(i, tmp3);
                tmp0 = tmp3;
            }
        }
    }
    void RightRay()
    {
        ChangeDistance();
        Vector3 rayStartPosition = rightHand.transform.position + rightHand.transform.forward * lazerStartDistance;
        Vector3 rayMiddlePosition = rayStartPosition + rightHand.transform.forward * (distance / 2);
        Vector3 rayFinishPosition = rayStartPosition + rightHand.transform.forward * distance;
        rayFinishPosition.y = groundPositionY;
        Vector3 tmp0 = rayStartPosition;
        for (int i = 0; i < positionCount; i++)
        {
            float plotPosition = (i / (float)(positionCount - 1));
            Vector3 tmp1 = Vector3.Lerp(rayStartPosition, rayMiddlePosition, plotPosition);
            Vector3 tmp2 = Vector3.Lerp(rayMiddlePosition, rayFinishPosition, plotPosition);
            Vector3 tmp3 = Vector3.Lerp(tmp1, tmp2, plotPosition);
            RaycastHit hit;
            if (Physics.Linecast(tmp0, tmp3, out hit))
            {
                hitPoint = hit.point;
                for (int j = i; j < positionCount; j++)
                {
                    lineRenderer.SetPosition(j, hitPoint);
                }
                break;
            }
            else
            {
                lineRenderer.SetPosition(i, tmp3);
                tmp0 = tmp3;
            }
        }
    }
    void ChangeDistance()
    {
        Vector2 stickR = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
        distance = Mathf.Clamp(distance += stickR.y, minDistance, maxDistance);
    }
}
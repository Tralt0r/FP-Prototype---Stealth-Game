using UnityEngine;

public class MoveOverTime : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Settings")]
    public float travelTime = 3f;

    private float timer = 0f;
    private bool isMoving = false;

    public bool fromCurrentPosition = false;
    public bool fromVector = false;
    public bool useLocalSpace = false;

    public Vector3 VectorPointA;
    public Vector3 VectorRotationA;
    public Vector3 VectorPointB;
    public Vector3 VectorRotationB;

    void Start()
    {
        if (fromVector)
        {
            if (useLocalSpace)
            {
                transform.localPosition = VectorPointA;
                transform.localRotation = Quaternion.Euler(VectorRotationA);
            }
            else
            {
                transform.position = VectorPointA;
                transform.rotation = Quaternion.Euler(VectorRotationA);
            }
        }
        else
        {
            if (fromCurrentPosition)
            {
                pointA = this.gameObject.transform;
            }
            else
            {
                if (useLocalSpace)
                    transform.localPosition = pointA.localPosition;
                else
                    transform.position = pointA.position;
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;
            float t = timer / travelTime;

            if (fromVector)
            {
                Quaternion rotA = Quaternion.Euler(VectorRotationA);
                Quaternion rotB = Quaternion.Euler(VectorRotationB);

                if (useLocalSpace)
                {
                    transform.localPosition = Vector3.Lerp(VectorPointA, VectorPointB, t);
                    transform.localRotation = Quaternion.Lerp(rotA, rotB, t);
                }
                else
                {
                    transform.position = Vector3.Lerp(VectorPointA, VectorPointB, t);
                    transform.rotation = Quaternion.Lerp(rotA, rotB, t);
                }
            }
            else
            {
                if (useLocalSpace)
                    transform.localPosition = Vector3.Lerp(pointA.localPosition, pointB.localPosition, t);
                else
                    transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
            }

            if (t >= 1f)
            {
                if (fromVector)
                {
                    if (useLocalSpace)
                    {
                        transform.localPosition = VectorPointB;
                        transform.localRotation = Quaternion.Euler(VectorRotationB);
                    }
                    else
                    {
                        transform.position = VectorPointB;
                        transform.rotation = Quaternion.Euler(VectorRotationB);
                    }
                }
                else
                {
                    if (useLocalSpace)
                        transform.localPosition = pointB.localPosition;
                    else
                        transform.position = pointB.position;
                }

                isMoving = false;
            }
        }
    }

    public void StartMove()
    {
        if (!fromVector)
        {
            if (fromCurrentPosition)
            {
                pointA = this.gameObject.transform;
            }
            else
            {
                if (useLocalSpace)
                    transform.localPosition = pointA.localPosition;
                else
                    transform.position = pointA.position;
            }
        }

        timer = 0f;
        isMoving = true;
    }
}
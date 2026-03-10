using UnityEngine;

public class MultiMoveOverTime : MonoBehaviour
{
    [Header("Movement Points")]
    public Transform pointA;
    public Transform pointB;

    [Header("Settings")]
    public float travelTime = 3f;

    private float timer = 0f;
    private bool isMoving = false;
    public bool fromCurrentPosition = false;
    

    void Start()
    {
        if (fromCurrentPosition)
        {
            pointA = this.gameObject.transform;
        }
        else
        {
            transform.position = pointA.position;
        }

        if (pointA != null)
        {
            transform.position = pointA.position;
        }
    }

    void Update()
    {
        if (isMoving)
        {
            timer += Time.deltaTime;

            float t = timer / travelTime;
            transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

            if (t >= 1f)
            {
                transform.position = pointB.position;
                isMoving = false;
            }
        }
    }

    public void StartMove()
    {
        if (fromCurrentPosition)
        {
            pointA = this.gameObject.transform;
        }
        else
        {
            transform.position = pointA.position;
        }

        timer = 0f;
        isMoving = true;
    }
}
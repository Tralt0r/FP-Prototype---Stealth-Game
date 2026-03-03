using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public string targetTag = "entity";
    public bool triggerOnce = true;
    private bool hasBeenTriggered = false;

    public UnityEngine.Events.UnityEvent onTrigger;

    public Color triggerColorSolid = Color.green;
    public Color triggerColorOutline = new Color(0, 1, 0, 0.25f);

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag(targetTag) && (hasBeenTriggered == false || !triggerOnce))
        {
            Debug.Log($"{other.name} entered the trigger!");

            if (onTrigger != null)
                onTrigger.Invoke();

            if (triggerOnce)
                hasBeenTriggered = true;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = triggerColorSolid;
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Gizmos.DrawWireCube(transform.position, col.bounds.size);
        }

        Gizmos.color = triggerColorOutline;
        Gizmos.DrawCube(transform.position, col.bounds.size);
    }
}
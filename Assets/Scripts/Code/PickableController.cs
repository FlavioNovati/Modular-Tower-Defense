using UnityEngine;

public class PickableController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 m_PickableOffset = Vector3.up;
    [SerializeField] private float m_RaycastDinstance = 100f;
    [SerializeField] private float CheckRadius = 3f;
    
    private TotemSegment m_PickedSegment = null;

    private RaycastHit m_RaycastHit;
    private Ray m_Ray;
    
    private void Update()
    {
        //Update raycast hit
        UpdateRaycastData();

        //Update Picked Position according to raycast
        if (m_PickedSegment != null)
            UpdatePickablePosition();
        
        //Use Raycast hit data to interact
        if (Input.GetMouseButtonDown(0))
            Interact();

        //Use Raycast hit data to relase object
        if (Input.GetMouseButtonDown(1))
            RelasePickedObject();
    }

    private bool UpdateRaycastData()
    {
        //Update Raycast
        m_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Raycast
        return Physics.Raycast(m_Ray, out m_RaycastHit, m_RaycastDinstance, ~(1<<6 | 1<<7));
    }

    private void UpdatePickablePosition()
    {
        //Update pickable position according to raycast hit       
        m_PickedSegment.transform.position = m_RaycastHit.point + m_PickableOffset;
    }

    private void Interact()
    {
        Totem totem = TotemManager.Instance.GetReachableToken(m_RaycastHit.point);

        if(totem)//Totem interaction
        {
            if(m_PickedSegment) //Add item to totem if allowed
            {
                if(totem.CanPlace(m_PickedSegment))
                {
                    totem.AddSegment(m_PickedSegment);
                    //Free picked Object
                    m_PickedSegment = null;
                }
            }
            else //Get item from totem
                m_PickedSegment = totem.GetLastSegment();
            return;
        }
        else //Try pick on item
        {
            if (m_PickedSegment) // Out of totem range
                return;
            else // Try get piece from ground
                TryGetSegment();
        }
    }

    private void TryGetSegment()
    {
        //Try get Segment from gound
        if (m_PickedSegment == null)
        {
            //Get overlapping object
            Collider[] collidingObject = Physics.OverlapSphere(m_RaycastHit.point, CheckRadius);
            //Get first TotemSegment
            for (int i = 0; i < collidingObject.Length; i++)
                if (collidingObject[i].TryGetComponent<TotemSegment>(out TotemSegment segment))
                {
                    m_PickedSegment = segment;
                    return;
                }
        }
    }

    private void RelasePickedObject()
    {
        if (m_PickedSegment == null)
            return;

        //Too close to totem -> Cannot place
        Totem totem = TotemManager.Instance.GetReachableToken(m_RaycastHit.point);
        if (totem != null)
            return;

        //Place it
        if (m_RaycastHit.point != null)
        {
            m_PickedSegment.transform.position = m_RaycastHit.point;
            m_PickedSegment = null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(m_RaycastHit.point != transform.position)
        {
            //Draw Collision point
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(m_RaycastHit.point, CheckRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(m_RaycastHit.point, 0.5f);
            //Draw Turret position
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(m_RaycastHit.point + m_PickableOffset, 0.2f);
        }
    }
#endif

}

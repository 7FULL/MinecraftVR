using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickToMoveOwn : MonoBehaviour
{
    Animal m_Agent;
    RaycastHit m_HitInfo = new RaycastHit();

    void Start()
    {
        m_Agent = GetComponent<Animal>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(2) && !Input.GetKey(KeyCode.LeftShift))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray.origin, ray.direction, out m_HitInfo))
                m_Agent.SetDestination(m_HitInfo.point);
        }
    }
}

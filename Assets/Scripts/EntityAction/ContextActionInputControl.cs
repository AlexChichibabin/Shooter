using System.Collections.Generic;
using UnityEngine;

public class ContextActionInputControl : MonoBehaviour
{
    [SerializeField] private EntityActionCollector m_TargetActionCollector;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            List<EntityContextAction> actionsList = m_TargetActionCollector.GetActionList<EntityContextAction>();

            for (int i = 0; i < actionsList.Count; i++)
            {
                actionsList[i].StartAction();
                actionsList[i].EndAction();
            }
        }
    }
}

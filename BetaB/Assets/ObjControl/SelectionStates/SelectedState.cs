using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class SelectedState : StateMachineBehaviour
{
    private SelectionTracker selectionTracker;
    private SceneObjControl sceneObjControl;

    //public GameObject buttonParent;
    //public RectTransform uiElementprefab;
  
    private GameObject toolbar;

    private bool requestChange;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        requestChange = false;
        selectionTracker = GameObject.FindGameObjectWithTag("SelectionTracker").GetComponent<SelectionTracker>();
        sceneObjControl = selectionTracker.Selection;

        toolbar = CanvasHandeler.instance.toolbar;
        toolbar.SetActive(true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (requestChange)
        {
            animator.SetInteger("SelectionState", 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
             
            if(ClickedOther())
            {
                requestChange = true;
            }

        }
        if (sceneObjControl !=null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(sceneObjControl.transform.position);
            toolbar.transform.position = screenPos;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CanvasHandeler.instance.toolbar.SetActive(false);
    }

    private bool ClickedOther()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        if (Physics.Raycast(ray, out hit, 500))
        {
            if (hit.collider.GetComponent<SceneObjControl>() == sceneObjControl)
            {
                return false;
            }

        }

        return true;
    }

}

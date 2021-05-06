using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.Interaction.Toolkit;

public class Pick : XRGrabInteractable
{
    [Header("곡괭이")]
    [SerializeField] private XRSimpleInteractable secondHandGrabPoints;
    [SerializeField] private BoxCollider pickCollider;

    private XRBaseInteractor secondInteractor;
    private Quaternion attachInitialRotation;
    private Quaternion initialRotationOffset;

    public Vector3 leftAttachPos;
    public Vector3 leftAttachrotation;
    public Vector3 rightAttachPos;
    public Vector3 rightAttachrotation;
    public int damage = 0;
    public bool isInHolster;

    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        Debug.Log("First Grab Enter");
        pickCollider.isTrigger = true;
        if (args.interactor.CompareTag(Constant.handRight))
        {
            isInHolster = false;
            attachTransform.localPosition = rightAttachPos;
            attachTransform.localRotation = Quaternion.Euler(rightAttachrotation.x, rightAttachrotation.y, rightAttachrotation.z);
        }
        else if (args.interactor.CompareTag(Constant.handLeft))
        {
            isInHolster = false;
            attachTransform.localPosition = leftAttachPos;
            attachTransform.localRotation = Quaternion.Euler(leftAttachrotation.x, leftAttachrotation.y, leftAttachrotation.z);
        }
        else
        {
            isInHolster = true;
        }
        //attachInitialRotation = args.interactor.attachTransform.localRotation;
        base.OnSelectEntering(args);
    }

    void Start()
    {
        secondHandGrabPoints.selectEntered.AddListener(OnSecondHandGrab);
        secondHandGrabPoints.selectExited.AddListener(OnSecondHandRelease);
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && selectingInteractor && !isInHolster)
        {
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();
        }
        base.ProcessInteractable(updatePhase);
    }


    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;
        targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - selectingInteractor.attachTransform.position);
        return targetRotation;
    }


    public void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        if (args.interactor != null)
        {
            secondInteractor = args.interactor;
            if (selectingInteractor)
            {
                initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation()) * selectingInteractor.attachTransform.rotation;
            }
        }
    }

    public void OnSecondHandRelease(SelectExitEventArgs args)
    {
        Debug.Log("SECOND HAND RELEASE");
        secondInteractor = null;
    }


    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        Debug.Log("First Grab Exit");
        base.OnSelectExiting(args);
        secondInteractor = null;
        pickCollider.isTrigger = false;
        args.interactor.attachTransform.localRotation = attachInitialRotation;
    }


    public override bool IsSelectableBy(XRBaseInteractor interactor)
    {
        if (isInHolster)
        {
            return base.IsSelectableBy(interactor);
        }
        else
        {
            bool isalreadygrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
            return base.IsSelectableBy(interactor) && !isalreadygrabbed;
        }
    }
}

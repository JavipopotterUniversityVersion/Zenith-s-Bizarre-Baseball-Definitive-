using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Mission", menuName = "Dialogue/Mission")]
public class Mission : ScriptableObject
{
    [SerializeField] DialogueSequence sequence;
    [Space(30)]
    [SerializeField] DialogueContainer missionStart;
    [Space(20)]
    [SerializeField] DialogueContainer missionSuccess;
    [SerializeField] string successCondition;
    [Space(20)]
    [SerializeField] DialogueContainer missionWait;

    // #if UNITY_EDITOR
    // [ContextMenu("SetRemover")]
    // void SetRemover()
    // {
    //     Type[] types = new Type[1] {typeof(DialogueContainer)};
    //     var targetInfo = UnityEvent.GetValidMethodInfo(sequence, "RemoveDialogue", types);
    //     UnityAction methodDelegate = System.Delegate.CreateDelegate(typeof(UnityAction), sequence, targetInfo) as UnityAction;
    //     UnityEventTools.AddPersistentListener(missionSuccess.OnTriggerDialogue, methodDelegate);
    // }
    // #endif

    public void AddMission()
    {
        sequence.AddToFirst(new SequenceElement(missionWait, false));
        sequence.AddToFirst(new SequenceElement(missionSuccess, true, successCondition));
        sequence.AddToFirst(new SequenceElement(missionStart, true));
    }
}

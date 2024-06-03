using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParameterByProcessor : StateMachineBehaviour
{
    [SerializeField] string parameterName;
    [SerializeField] Processor processor;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.SetFloat(parameterName, processor.Result(1));
    }
}

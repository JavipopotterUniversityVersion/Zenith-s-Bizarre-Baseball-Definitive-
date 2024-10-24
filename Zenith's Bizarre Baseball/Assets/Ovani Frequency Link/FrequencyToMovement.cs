using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public abstract class FrequencyToMovement : MonoBehaviour
{
    public FrequencyLink FrequencyLink;
#if UNITY_EDITOR
    public static List<FrequencyToMovement> FreqMoves = new List<FrequencyToMovement>();
    private void Start() => FreqMoves.Add(this);
    private void OnDestroy() => FreqMoves.Remove(this);
    private void OnDisable()
    {
        FreqMoves.Remove(this);
    }
    private void OnEnable()
    {
        FreqMoves.Add(this);
    }
#endif
    private void Update()
    {
#if UNITY_EDITOR
#else
        Move();
#endif
    }
    public abstract void Move();
}

[ExecuteAlways]
public abstract class FrequencyToMovement<T> : FrequencyToMovement
{
    public T A;
    public T B;
    protected virtual T Lerp(T t1, T t2, float ammount) 
    {
        return t1;
    }
    [Header("Preview movement via activating \"Editor and Runtime\" mode!")]
    public UnityEvent<T> Targets = new UnityEvent<T>();

    public override void Move()
    {
        Targets.Invoke(Lerp(A, B, FrequencyLink != null ? FrequencyLink.GetStrength() : 0));
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserGadget : MonoBehaviour
{
    [SerializeField] Identifiable _target;
    [SerializeField] Identifiable _foe;
    [SerializeField] ObjectProcessor _damage;
    [SerializeField] float _damageInterval = 0.3f;
    LineRenderer _lineRenderer;

    [SerializeField] LayerMask _targetLayer;

    List<Transform> _lasersPoints = new List<Transform>();

    private void Awake() 
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetLasers()
    {
        StopCoroutine(UseLaser());
        _lasersPoints = SearchManager.Instance.GetAllSearchables<Transform>(_target).ToList();
        StartCoroutine(UseLaser());
    }

    IEnumerator UseLaser()
    {
        while(true)
        {
            List<Transform> tempLasersPoints = new List<Transform>(_lasersPoints);

            _lineRenderer.positionCount = tempLasersPoints.Count;
            for(int i = 0; i < tempLasersPoints.Count; i++)
            {
                if(tempLasersPoints[i] == null)
                {
                    _lasersPoints.Remove(tempLasersPoints[i]);
                    continue;
                }

                _lineRenderer.SetPosition(i, tempLasersPoints[i].position);

                Transform nextPoint = null;

                int c = i;
                while(nextPoint == null && c < tempLasersPoints.Count - 1)
                {
                    c++;
                    nextPoint = tempLasersPoints[c];
                }

                if(nextPoint == null) continue;

                var hit = Physics2D.Linecast(tempLasersPoints[i].position, nextPoint.position, _targetLayer);
                if(hit.collider != null)
                {
                    print(hit.collider.name);
                    if(hit.collider.TryGetComponent(out Searchable searchable))
                    {
                        if(searchable.IdentifiableType.DerivesFrom(_foe))
                        {
                            if(searchable.TryGetComponent(out HealthHandler healthHandler))
                            {
                                print("Damage");
                                healthHandler.TakeDamage(_damage.Result());
                            }
                        }
                    }
                }
            }

            yield return new WaitForSeconds(_damageInterval);
        }
    }
}

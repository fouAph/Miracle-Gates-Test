using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  [SerializeField] EnemyState enemyState;
  [SerializeField] float cautionDistance;
  [SerializeField] float angryDistance;
  [SerializeField] TargetScanner targetScanner;

  [SerializeField] LayerMask friendMask;

  bool hasReset;

  private Renderer enemyRenderer;
  private Color normalColor;
  private PlayerController target;

  private void Start()
  {
    enemyRenderer = GetComponent<Renderer>();
    normalColor = enemyRenderer.material.color;
  }
  private void Update()
  {
    PlayerController detected = targetScanner.DetectPlayer(transform);
    if (target != detected)
    {
      target = detected;
    }
    NotifyNearbyFriend();

    if (target)
    {
      HandleEnemyState();
      hasReset = false;
    }
    else
    {
      if (hasReset == false)
      {
        StartCoroutine(ResetToNormal());
      }
    }
  }

  private void HandleEnemyState(Enemy enemy = null)
  {
    if (enemy == null)
    {
      if (Vector3.Distance(target.transform.position, transform.position) < angryDistance)
      {
        enemyState = EnemyState.Angry;
      }
      else if (Vector3.Distance(target.transform.position, transform.position) < cautionDistance)
      {
        enemyState = EnemyState.Cautious;
      }
      else if (Vector3.Distance(target.transform.position, transform.position) > cautionDistance)
      {
        enemyState = EnemyState.Angry;
      }
    }
    else
    {
      enemy.enemyState = enemyState;
    }
    HandleEnemyColor();
  }

  private void HandleEnemyColor()
  {
    enemyRenderer.material.color = enemyState switch
    {
      EnemyState.Angry => Color.red,
      EnemyState.Cautious => Color.yellow,
      _ => normalColor,
    };
  }

  private IEnumerator ResetToNormal()
  {
    yield return new WaitForSeconds(1);
    enemyState = EnemyState.Normal;
    hasReset = true;
  }

  private void NotifyNearbyFriend()
  {
    var nearbyFriend = Physics.OverlapSphere(transform.position, 10, friendMask);
    if (nearbyFriend == null) return;
    foreach (var item in nearbyFriend)
    {
      item.GetComponent<Enemy>().HandleEnemyState(this);

      print(item.name);
    }
  }
  private void OnDrawGizmosSelected()
  {
    if (targetScanner != null)
    {
      targetScanner.EditorGizmo(transform);
    }
  }
}

public enum EnemyState { Normal, Cautious, Angry }
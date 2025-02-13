﻿using System.Collections;
using DefaultNamespace;
using UnityEngine;

public class Enemy : MonoBehaviour
{
  public GameObject drop;
  private void OnCollisionEnter2D(Collision2D other)
  {
    doDamage(other);
  }

  private void OnCollisionStay2D(Collision2D other)
  {
    doDamage(other);
  }

  public IEnumerator Death()
  {
    if (drop != null)
    {
      Instantiate(drop, transform.position, Quaternion.identity);
    }
    GetComponent<Animator>().SetBool("dead", true);
    GetComponent<Collider2D>().enabled = false;
    transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
    yield return new WaitForSeconds(2);
    Destroy(gameObject);
  }

  public void StartDeath()
  {
    StartCoroutine(Death());
  }

  void doDamage(Collision2D other)
  {
    if (other.gameObject.CompareTag(Constants.Player) 
        && !other.gameObject.GetComponent<Player>().GetImmune())
    {
      other.gameObject.GetComponent<Player>().ImmuneOn();
        other.gameObject.GetComponent<Player>().RecountHp(-1);
        other.gameObject.GetComponent<Player>().ImmuneOff();
        // other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * 8f, ForceMode2D.Impulse);
    }
  }
}

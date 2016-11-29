using UnityEngine;
using System.Collections;

public class ProjectileCreator : MonoBehaviour {
  public GameObject projectile;
  public float size1 = 1f;
  public float size2 = 2f;
  public void Update() {
    if (Input.GetButtonDown ("Fire1")) {
    	GameObject newProjectile = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
      newProjectile.transform.localScale = new Vector2(
            ((transform.parent.transform.localScale.x<0) ?
                (-newProjectile.transform.localScale.x)
                : (newProjectile.transform.localScale.x)),
             newProjectile.transform.localScale.y) * size1;
    }
    if (Input.GetButtonDown ("Fire2")) {
    	GameObject newProjectile = Instantiate (projectile, transform.position, transform.rotation) as GameObject;
      newProjectile.transform.localScale = new Vector2(
            ((transform.parent.transform.localScale.x<0) ?
                (-newProjectile.transform.localScale.x)
                : (newProjectile.transform.localScale.x)),
             newProjectile.transform.localScale.y) * size2;
    }
  }

}

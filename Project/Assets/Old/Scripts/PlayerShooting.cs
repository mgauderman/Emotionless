using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzlePoint;
    public float shotsPerSecond;

    private bool isFiring = false;
    private bool canFire = true;
    private float secondsPerShot;
    public float shotSpeed;

    void Update()
    {
        //if (Input.GetButtonDown("Fire1")) StartFiring();
        //if (Input.GetButtonUp("Fire1")) StopFiring();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ButtonDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ButtonUp();
        }
    }

    public void ButtonDown()
    {
        StartFiring();
    }

    public void ButtonUp()
    {
        StopFiring();
    }

    public virtual void Start()
    {
        secondsPerShot = 1.0f / shotsPerSecond;
    }

    public virtual void StartFiring()
    {
        isFiring = true;
        if (canFire)
            StartCoroutine(Firing());
    }

    public virtual void StopFiring()
    {
        isFiring = false;
    }

    public void Fire()
    {
        Vector3 pos = muzzlePoint.transform.position;
        GameObject go = Instantiate(bulletPrefab, new Vector3(pos.x, pos.y, 0), muzzlePoint.rotation) as GameObject;
        SendMessage("Numb");
        go.GetComponent<Rigidbody2D>().AddForce(transform.right * shotSpeed);
    }

    public IEnumerator Firing()
    {
        canFire = false;
        Fire();
        yield return new WaitForSeconds(secondsPerShot);
        canFire = true;
        if (isFiring)
            StartCoroutine(Firing());
    }

    public void Respawn()
    {
        StopAllCoroutines();
        isFiring = false;
        canFire = true;
    }

}

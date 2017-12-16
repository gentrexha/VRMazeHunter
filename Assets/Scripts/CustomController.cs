using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Assets.Scripts {
    public class CustomController : MonoBehaviour {

        // Gun
        public float damage = 10f;
        public float range = 100f;
        public float impactForce = 30f;
        public float fireRate = 15f;
        public GameObject impactEffect;
        private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);    // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible
        public AudioSource gunAudio;                                       // Reference to the audio source which will play our shooting sound effect
        public AudioSource explosionAudio;                                       // Reference to the audio source which will play our shooting sound effect
        private LineRenderer laserLine;                                     // Reference to the LineRenderer component which will display our laserline
        public Transform lineRenderTransform;
        private float _nextTimeToFire;
        private Transform _playerTransform;

        private void Start () {
            laserLine = GetComponentInChildren<LineRenderer>();
            _playerTransform = Camera.main.transform;
        }

        private void Update () {
            if (Input.GetButtonUp("Fire1"))
                if (Time.time >= _nextTimeToFire) {
                    _nextTimeToFire = Time.time + 1f / fireRate;
                    Shoot();
                }
        }

        private void Shoot() {
            StartCoroutine(ShotEffect());
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hitInfo;
            laserLine.SetPosition(0, lineRenderTransform.position);
            if (Physics.Raycast(_playerTransform.transform.position, _playerTransform.transform.forward, out hitInfo, range)) {
                laserLine.SetPosition(1, hitInfo.point);
                var _enemy = hitInfo.transform.GetComponent<EnemyController>();
                if (_enemy != null) _enemy.TakeDamage(damage);
                explosionAudio.Play();
                var impactGameObject = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                Destroy(impactGameObject,2f);
            }
            else {
                laserLine.SetPosition(1, rayOrigin + (Camera.main.transform.forward * range));
            }
        }

        private IEnumerator ShotEffect()
        {
            // Play the shooting sound effect
            gunAudio.Play();

            // Turn on our line renderer
            laserLine.enabled = true;

            //Wait for .07 seconds
            yield return shotDuration;

            // Deactivate our line renderer after waiting
            laserLine.enabled = false;
        }
    }
}
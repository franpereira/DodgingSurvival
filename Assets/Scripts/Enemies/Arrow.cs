using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class Arrow : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;
        [SerializeField] float launchForce = 5f;
        [SerializeField] Warning warningPrefab;
        
        public void LaunchTowards(Vector2 target)
        {
            Vector2 direction = target - rb.position;
            rb.AddForce(direction.normalized * launchForce, ForceMode2D.Impulse);
        }

        // This function is used to launch the projectile towards a target in a random parabolic trajectory.
        public void LaunchTowardsInParabola(Vector2 target)
        {
           
            // Direction from the projectile to the target.
            Vector2 direction = target - rb.position;

            // The vertical component (height) of the direction.
            float h = direction.y;

            // The vertical component of the direction is set to 0, so the magnitude is now the horizontal distance to the target.
            direction.y = 0;
            float distance = direction.magnitude;

            // Random launch angle
            float launchAngle = Random.Range(40, 89) * Mathf.Deg2Rad;
            
            // Calculates the height at which the projectile needs to be aimed initially, considering the launch angle.
            direction.y = distance * Mathf.Tan(launchAngle);

            // The total distance the projectile needs to travel is calculated by adding the horizontal distance and the difference in height, 
            // divided by the tangent of the launch angle. This gives the total trajectory distance considering the launch angle.
            distance += h / Mathf.Tan(launchAngle);

            // The launch velocity is calculated using the physics equation for projectile motion.
            // This equation gives the required initial velocity for the projectile to hit the target considering the specified launch angle and gravitational force.
            float velocity = Mathf.Sqrt(distance * Physics2D.gravity.magnitude / Mathf.Sin(2 * launchAngle));

            // The projectile is given an impulse in the calculated direction with the calculated velocity.
            rb.isKinematic = false;
            rb.AddForce(direction.normalized * velocity, ForceMode2D.Impulse);
            
            // Time it should take to reach the target.
            float timeOfFlight = 2 * velocity * Mathf.Sin(launchAngle) / Physics2D.gravity.magnitude;
            StartCoroutine(ShowWarning(target, timeOfFlight));
            
        }

        IEnumerator ShowWarning(Vector2 targetPos, float timeOfFlight)
        {
            // Show the warning some seconds before the impact.
            float secondsBeforeImpact = 2f;
            yield return new WaitForSeconds(timeOfFlight - secondsBeforeImpact);

            Vector2 warningPos = new Vector2(targetPos.x, 1f);
            Warning warning = Instantiate(warningPrefab, warningPos, Quaternion.identity);

            // Scale the warning based on the time remaining.
            float startTime = Time.time;
            float endTime = startTime + secondsBeforeImpact; 
            while (Time.time < endTime)
            {
                float t = (Time.time - startTime) / secondsBeforeImpact;
                warning.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t*t*t);
                yield return null;
            }

            Destroy(warning.gameObject);
        }


        //void OnCollisionEnter2D(Collision2D _) => GetComponent<Collider2D>().enabled = false;
    }
}
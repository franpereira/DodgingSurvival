using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerCharacter : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;

        [SerializeField] float acceleration = 1f;
        [SerializeField] float maxSpeed = 1f;
    
        [SerializeField] float jumpForce = 5f;
        [SerializeField] float fallMultiplier = 2.5f;
        [SerializeField] float lowJumpMultiplier = 2f;

        bool _isJumping;
        bool _isGrounded;
    
        float _horizontalInput;
    
        [SerializeField] Animator animator;
        [SerializeField] AudioSource deathSound;
        public bool IsAlive { get; private set; } = true;

        void Start() => Core.Events.InvokeBegin();

        void OnEnable() => Core.Events.Restart += Resurrect;
        void OnDisable() => Core.Events.Restart -= Resurrect;
    
        void Resurrect()
        {
            IsAlive = true;
            animator.SetTrigger("Resurrect");
        }
    
        public void OnAxis(InputAction.CallbackContext context) => _horizontalInput = context.ReadValue<Vector2>().x;
        public void OnRestart(InputAction.CallbackContext context)
        {
            if (IsAlive is false && context.started) Core.Events.InvokeRestart();
        }
    
        public void OnJump(InputAction.CallbackContext context)
        {
            // When the jump button is initially pressed.
            if (context.started && _isGrounded && IsAlive)
            {
                _isJumping = true;
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
            // When the jump button is released.
            else if (context.canceled)
                _isJumping = false;
        }

        void FixedUpdate()
        {
            Vector2 velocity = rb.velocity;
        
            // Check if the player is grounded.
            _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, LayerMask.GetMask("Ground"));

            if (IsAlive is false) return;
        
            // Jumping:
            switch (velocity.y)
            {
                // Check if the player is jumping.
                case < 0:
                    // This makes the character fall faster than it jumps up, giving a more satisfying feel to the jump.
                    rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
                    break;
                case > 0 when _isJumping is false:
                    // This makes the character start falling immediately after the jump button is released, 
                    // rather than continuing to rise at a decreasing rate.
                    rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
                    break;
            }
        
            // Horizontal movement:
            Vector3 defaultScale = Vector3.one;
            Vector3 flippedScale = new(-1, 1, 1);
            switch (_horizontalInput)
            {
                case > 0f: // Moving right.
                    velocity.x += acceleration;
                    transform.localScale = defaultScale;
                    break;
                case < 0f: // Moving left.
                    velocity.x -= acceleration;
                    transform.localScale = flippedScale;
                    break;
            }
            // Cap the player's speed.
            velocity.x = Mathf.Clamp(velocity.x, -maxSpeed, maxSpeed);
        
            // Apply the velocity to the rigidbody.
            rb.velocity = velocity;
        
            // Update the animator.
            animator.SetFloat("AbsVelX", Mathf.Abs(velocity.x));
            animator.SetFloat("VelY", velocity.y);
            animator.SetFloat("AbsVelY", Mathf.Abs(velocity.y));
            animator.SetBool("IsGrounded", _isGrounded);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Damage") && IsAlive) 
                Die();
        }

        void Die()
        {
            IsAlive = false;
            animator.SetTrigger("Die");
            deathSound.Play();
            Core.Events.InvokeEnd();
        }
    }
}
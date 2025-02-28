using UnityEngine;

public class FPController : MonoBehaviour
{
    [Header("Stats")]

    [SerializeField]
    private float _moveVelocity;

    [SerializeField]
    private float _gravityAccel;

    [SerializeField]
    private float _jumpHeight;

    [SerializeField]
    private float _acceleration;

    [SerializeField]
    private float _coyoteTime;

    [SerializeField]
    private float _groundStickForce;

    [Header("Dependencies")]

    [SerializeField]
    private CharacterController _characterController;

    [SerializeField]
    private Transform _cameraTransform;

    private Vector3 _velocity;

    private float _forceUngroundTime;
    private float _ungroundedTime;
    private bool _wasGrounded;

    private void Update()
    {
        GetInputs(out Vector2 horizontalInput, out bool jumpPressed);

        UpdateTiming();

        Vector3 targetHorizontalVelocity = new Vector3(horizontalInput.x, 0, horizontalInput.y)
                                           * _moveVelocity;

        Vector3 currentHorizontalVelocity = _velocity;
        currentHorizontalVelocity.y = 0;

        Vector3 newHorizontalVelocity = Vector3.MoveTowards(currentHorizontalVelocity, targetHorizontalVelocity,
            _acceleration * Time.deltaTime);

        Vector3 adjustVector = newHorizontalVelocity - currentHorizontalVelocity;

        _velocity += adjustVector;

        bool canJump = _ungroundedTime < _coyoteTime && _forceUngroundTime <= 0;
        if (jumpPressed && canJump)
        {
            Launch(_jumpHeight, Vector3.zero);
        }

        if (!_characterController.isGrounded)
        {
            _velocity.y += _gravityAccel * Time.deltaTime;
        }
        else if (_forceUngroundTime <= 0)
        {
            _velocity.y = -_groundStickForce;
        }

        if (!_characterController.isGrounded && _wasGrounded)
        {
            _velocity.y = Mathf.Max(_velocity.y, 0);
        }

        Vector3 step = _velocity * Time.deltaTime;
        _wasGrounded = _characterController.isGrounded;

        _characterController.Move(step);

        float oldY = _velocity.y;
        _velocity = _characterController.velocity;
        _velocity.y = oldY;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_characterController.transform.position - Vector3.up + Vector3.forward,
            _jumpHeight * Vector3.up);
    }

    private void GetInputs(out Vector2 horizontalInput, out bool jump)
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 rotatedInput = _cameraTransform.TransformDirection(new Vector3(horizontal, 0, vertical));

        horizontalInput = new Vector2(rotatedInput.x, rotatedInput.z).normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
    }

    private void UpdateTiming()
    {
        if (_forceUngroundTime > 0)
        {
            _forceUngroundTime -= Time.deltaTime;
        }

        if (_characterController.isGrounded)
        {
            _ungroundedTime = 0;
        }
        else
        {
            _ungroundedTime += Time.deltaTime;
        }
    }

    public void Launch(float height, Vector3 addVelocity)
    {
        _velocity.y = Mathf.Sqrt(-2 * _gravityAccel * height);
        _velocity += addVelocity;
        _forceUngroundTime = _coyoteTime + 0.1f;
    }
}
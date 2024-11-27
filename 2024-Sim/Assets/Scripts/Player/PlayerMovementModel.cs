using System.Threading;
using NaughtyAttributes;
using UnityEngine;

namespace Player
{
    public class PlayerMovementModel : MonoBehaviour
    {
        [Header("Player Status")]
        [SerializeField] private float firstSpeed = 16.0f;
        [SerializeField] private float baseSpeed = 16.0f;
        [SerializeField] private float acceleration = 10.0f;
        [SerializeField] private float deceleration = 10.0f;
        [SerializeField] private float gravity = 120.0f;
        [SerializeField] private float jumpLowerLimit = 0.03f;
        
        [Header("Player System")]
        [ShowNonSerializedField] private PlayerHorizontalMovementState horizontalMovementState;
        [ShowNonSerializedField] private PlayerVerticalMovementState verticalMovementState;
        [ShowNonSerializedField] private float time;
        [ShowNonSerializedField] private bool isRight;
        [ShowNonSerializedField] private bool isLeft;
        [ShowNonSerializedField] private bool isJump;
        [ShowNonSerializedField] private bool isKeyLock;
        private Rigidbody2D playerRigidbody2D;
        
        /* Player status Get */
        public float FirstSpeed { get { return firstSpeed; } }
        public float BaseSpeed { get { return baseSpeed; } }
        public float Acceleration { get { return acceleration; } }
        public float Deceleration { get { return deceleration; } }
        public float Gravity { get { return gravity; } }
        public float JumpLowerLimit { get { return jumpLowerLimit; } }
        
        /* Player system Get and Set*/
        public PlayerHorizontalMovementState HorizontalMovementState { get { return horizontalMovementState; } set { horizontalMovementState = value; } }
        public PlayerVerticalMovementState VerticalMovementState { get { return verticalMovementState; } set{ verticalMovementState = value; } }
        public float Time { get { return time; } set { time = value; } }
        public bool IsRight { get { return isRight; } set { isRight = value; } }
        public bool IsLeft { get { return isLeft; } set { isLeft = value; } }
        public bool IsJump { get { return isJump; } set { isJump = value; } }
        public bool IsKeyLock { get { return isKeyLock; } set { isKeyLock = value; } }
        public Rigidbody2D playerRB2D { get { return playerRigidbody2D; } }

        public void Start()
        {
            playerRigidbody2D = GetComponent<Rigidbody2D>();
            horizontalMovementState = PlayerHorizontalMovementState.STOP;
            verticalMovementState = PlayerVerticalMovementState.FALL;
        }

        // @brief プレイヤーの移動処理
        public void OnCollisionStay2D(Collision2D collision)
        {
            if( (verticalMovementState == PlayerVerticalMovementState.FALL || verticalMovementState == PlayerVerticalMovementState.DOWN) && collision.gameObject.CompareTag("Ground"))
            {
                verticalMovementState = PlayerVerticalMovementState.GROUND;
                time = 0.0f;
                isKeyLock = true; // キー操作をロック
            }
        }
        
        public void OnCollisionExit2D(Collision2D collision)
        {
            if (verticalMovementState != PlayerVerticalMovementState.JUMP && collision.gameObject.CompareTag("Ground"))
            {
                verticalMovementState = PlayerVerticalMovementState.DOWN;
                time = 0.0f;
                isKeyLock = false; // キー操作を解除
            }
        }
    }
}
using System;
using UnityEditor;
using UnityEngine;
using VContainer.Unity;

namespace Player
{
    public class PlayerMover : ITickable, IFixedTickable
    {
        
        private PlayerMovementModel playerMovementModel;

        public PlayerMover(PlayerMovementModel playerMovementModel)
        {
            this.playerMovementModel = playerMovementModel;
        }
        
        public void Tick()
        {
            /* 水平 */
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                playerMovementModel.IsRight = true;
                playerMovementModel.HorizontalMovementState = PlayerHorizontalMovementState.RIGHT;
            }
            else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                playerMovementModel.IsLeft = true;
                playerMovementModel.HorizontalMovementState = PlayerHorizontalMovementState.LEFT;
            }
            else
            {
                playerMovementModel.IsRight = false;
                playerMovementModel.IsLeft = false;
                playerMovementModel.HorizontalMovementState = PlayerHorizontalMovementState.STOP;
            }
            
            /* 垂直 */
            if (Input.GetKey(KeyCode.C))
            {
                playerMovementModel.IsJump = !playerMovementModel.IsKeyLock;
            }
            else
            {
                playerMovementModel.IsJump = false;
                playerMovementModel.IsKeyLock = false;
            }
        }
        
        public void FixedTick()
        {
            Vector2 nextPos = Vector2.zero;

            // 水平方向の移動
            switch (playerMovementModel.HorizontalMovementState)
            {
                // 停止状態
                case PlayerHorizontalMovementState.STOP:
                    nextPos.x = Mathf.MoveTowards(playerMovementModel.playerRB2D.linearVelocity.x, 0f, playerMovementModel.Deceleration * Time.fixedDeltaTime);
                    break;
                
                // 右移動状態
                case PlayerHorizontalMovementState.RIGHT:
                    if (playerMovementModel.IsRight)
                    {
                        // 加速して目標速度に近づく
                        nextPos.x = Mathf.MoveTowards(playerMovementModel.playerRB2D.linearVelocity.x, playerMovementModel.BaseSpeed, playerMovementModel.Acceleration * Time.fixedDeltaTime);
                    }
                    else
                    {
                        // 入力がない場合は減速
                        nextPos.x = Mathf.MoveTowards(playerMovementModel.playerRB2D.linearVelocity.x, 0f, playerMovementModel.Deceleration * Time.fixedDeltaTime);
                    }

                    // 左入力で状態を変更
                    if (playerMovementModel.IsLeft)
                    {
                        playerMovementModel.HorizontalMovementState = PlayerHorizontalMovementState.LEFT;
                    }
                    break;
                
                // 左移動状態
                case PlayerHorizontalMovementState.LEFT:
                    if (playerMovementModel.IsLeft)
                    {
                        // 加速して目標速度に近づく
                        nextPos.x = Mathf.MoveTowards(playerMovementModel.playerRB2D.linearVelocity.x, -playerMovementModel.BaseSpeed, playerMovementModel.Acceleration * Time.fixedDeltaTime);
                    }
                    else
                    {
                        // 入力がない場合は減速
                        nextPos.x = Mathf.MoveTowards(playerMovementModel.playerRB2D.linearVelocity.x, 0f, playerMovementModel.Deceleration * Time.fixedDeltaTime);
                    }

                    // 右入力で状態を変更
                    if (playerMovementModel.IsRight)
                    {
                        playerMovementModel.HorizontalMovementState = PlayerHorizontalMovementState.RIGHT;
                    }
                    break;
            }
            
            // 垂直方向の移動
            switch (playerMovementModel.VerticalMovementState)
            {
                // 地面にいる状態
                case PlayerVerticalMovementState.GROUND:
                    if (playerMovementModel.IsJump)
                    {
                        playerMovementModel.VerticalMovementState = PlayerVerticalMovementState.JUMP;
                    }

                    break;
                
                // ジャンプ状態
                case PlayerVerticalMovementState.JUMP:
                    playerMovementModel.Time += Time.fixedDeltaTime;

                    if (playerMovementModel.IsJump || playerMovementModel.JumpLowerLimit > playerMovementModel.Time)
                    {
                        nextPos.y = playerMovementModel.FirstSpeed;
                        nextPos.y -= playerMovementModel.Gravity * Mathf.Pow(playerMovementModel.Time, 2);
                    }
                    else
                    {
                        playerMovementModel.Time += Time.fixedDeltaTime;
                        nextPos.y = playerMovementModel.FirstSpeed;
                        nextPos.y -= playerMovementModel.Gravity * Mathf.Pow(playerMovementModel.Time, 2);
                    }

                    if (nextPos.y < 0f)
                    {
                        playerMovementModel.VerticalMovementState = PlayerVerticalMovementState.FALL;
                        nextPos.y = 0f;
                        playerMovementModel.Time = 0f;
                    }

                    break;
                
                // ジャンプ後に落下状態
                case PlayerVerticalMovementState.FALL:
                    playerMovementModel.Time += Time.fixedDeltaTime;

                    nextPos.y = 0f;
                    nextPos.y = -(playerMovementModel.Gravity * Mathf.Pow(playerMovementModel.Time, 2));
                    break;
                
                // 落下状態
                case PlayerVerticalMovementState.DOWN:
                    playerMovementModel.Time += Time.fixedDeltaTime;

                    nextPos.y = 0f;
                    nextPos.y = -(playerMovementModel.Gravity * Mathf.Pow(playerMovementModel.Time, 2));
                    break;
                
                default:
                    throw new Exception("PlayerVerticalMovementState is not defined");
            }

            Debug.Log(nextPos);
            playerMovementModel.playerRB2D.linearVelocity = nextPos;
        }
    }
}
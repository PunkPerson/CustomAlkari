using GameNetcodeStuff;
using UnityEngine;

namespace CustomAlkari
{
    public class ValeraAI : EnemyAI
    {
        enum State {
            LookingForPlayer,
            WalkingToPlayer,
            WalkingFromPlayer
        }

        private int currentState = 0;
        private float timeSinceCollideWithPlayer = 0f;
        private float timeSincePlayedSound = 300.0f;

        private const float SPEED = 5.0f;

        public override void Start()
        {
            base.Start();
            currentState = (int) State.LookingForPlayer;
            creatureVoice.PlayOneShot(enemyType.audioClips[0]);
        }

        private bool HasValidTarget()
        {
            return targetPlayer != null && !targetPlayer.isInsideFactory && !targetPlayer.isPlayerDead;
        }

        public override void Update()
        {
            base.Update();

            switch (currentState)
            {
                case (int) State.LookingForPlayer:
                    agent.speed = 0;
                    if (!HasValidTarget()) 
                    {
                        targetPlayer = GetClosestPlayer();
                        currentState = (int) State.WalkingToPlayer;
                    }
                    break;
                
                case (int) State.WalkingToPlayer:
                    if (timeSinceCollideWithPlayer < 1.0f) return;

                    if (targetPlayer) {
                        SetDestinationToPosition(targetPlayer.transform.position);
                        agent.speed = SPEED;
                        SyncPositionToClients();
                    }
                    else {
                        currentState = (int) State.LookingForPlayer;
                    }
                    break;
                
                case (int) State.WalkingFromPlayer:
                    if (targetPlayer) {
                        if (timeSinceCollideWithPlayer < 5.0f) {
                            SetDestinationToPosition(targetPlayer.transform.position);
                            agent.speed = -SPEED;
                            SyncPositionToClients();
                        }
                        else {
                            timeSinceCollideWithPlayer = 0f;
                            currentState = (int) State.WalkingToPlayer;
                        }
                    }
                    else {
                        currentState = (int) State.LookingForPlayer;
                    }
                    break;
            }
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (timeSinceCollideWithPlayer < 5.0f) timeSinceCollideWithPlayer += 0.2f;
            timeSincePlayedSound += 0.2f;
        }

        public override void HitEnemy(int force = 1, PlayerControllerB? playerWhoHit = null, bool playHitSFX = false, int hitID = -1)
        {
            base.HitEnemy(force, playerWhoHit, playHitSFX, hitID);

            if(isEnemyDead){
                return;
            }

            enemyHP -= force;
            if (enemyHP <= 0 && !isEnemyDead) {
                KillEnemy(true);
                KillEnemyOnOwnerClient();
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            if (timeSinceCollideWithPlayer < 5.0f) {
                currentState = (int) State.WalkingFromPlayer;
                return;
            }

            if (timeSincePlayedSound > 5.0f * 60.0f) {
                creatureVoice.PlayOneShot(enemyType.audioClips[0]);
                timeSincePlayedSound = 0f;
            }
            timeSinceCollideWithPlayer = 0f;
        }
    }
}
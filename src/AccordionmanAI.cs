using System.Collections;
using GameNetcodeStuff;
using UnityEngine;

namespace CustomAlkari
{
    public class AccordionmanAI : EnemyAI
    {
        enum State {
            LookingForPlayer,
            WalkingToPlayer
        }

        [SerializeField] private SkinnedMeshRenderer head;

        private int currentState = 0;
        private float timeSinceCollideWithPlayer = 0f;
        private float timeSincePlayedSound = 0f;
        private bool isAttackHead = false;

        private const float SPEED = 6.0f;

        public override void Start()
        {
            base.Start();
            currentState = (int) State.LookingForPlayer;
            head.SetBlendShapeWeight(0, 0);
        }

        private bool HasValidTarget()
        {
            return targetPlayer != null && !targetPlayer.isInsideFactory && !targetPlayer.isPlayerDead;
        }

        public override void Update()
        {
            base.Update();

            creatureAnimator.SetFloat("speed", agent.speed);

            if (agent.speed < 0.5) {
                creatureVoice.Stop();
            }

            switch (currentState)
            {
                case (int) State.LookingForPlayer:
                    agent.speed = 0;

                    if (!HasValidTarget())
                    {
                        TargetClosestPlayer();
                        currentState = (int) State.WalkingToPlayer;
                    }
                    break;
                
                case (int) State.WalkingToPlayer:
                    if (timeSinceCollideWithPlayer < 2.0f) {
                        agent.speed = 0;
                        return;
                    }

                    if (Vector3.Distance(transform.position, targetPlayer.transform.position) < 2) {
                        if (isAttackHead == false) {
                            isAttackHead = true;
                            StartCoroutine(AttackHead());
                        }
                    }
                    else {
                        if (isAttackHead == true) {
                            isAttackHead = false;
                            StartCoroutine(IdleHead());
                        }
                    }

                    SetDestinationToPosition(targetPlayer.transform.position);
                    agent.speed = SPEED;
                    SyncPositionToClients();
                    break;
            }
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();
            if (timeSinceCollideWithPlayer < 5.0f) timeSinceCollideWithPlayer += 0.2f;

            if (timeSincePlayedSound < 7.0f) timeSincePlayedSound += 0.2f;

            if (timeSincePlayedSound >= 7.0f) {
                if (currentState == (int) State.WalkingToPlayer && agent.speed > 0.5) {
                    creatureVoice.PlayOneShot(enemyType.audioClips[0]);
                }
                timeSincePlayedSound = 0f;
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            if (timeSinceCollideWithPlayer < 5.0f) return;

            PlayerControllerB playerControllerB = MeetsStandardPlayerCollisionConditions(other);
            if (playerControllerB != null) {
                playerControllerB.DamagePlayer(20);
                timeSinceCollideWithPlayer = 0f;
            }
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

        private IEnumerator AttackHead()
        {
            while (head.GetBlendShapeWeight(0) < 100.0f)
            {
                head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) + 0.01f);

                yield return new WaitForSeconds(0.01f);
            }
        }

        private IEnumerator IdleHead()
        {
            while (head.GetBlendShapeWeight(0) > 0)
            {
                head.SetBlendShapeWeight(0, head.GetBlendShapeWeight(0) - 0.01f);

                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
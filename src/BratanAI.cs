using UnityEngine;

namespace CustomAlkari
{
    public class BratanAI : EnemyAI
    {
        //Plugin plugin;

        enum State {
            Idle,
            LookingForPlayer,
            WalkingToPlayer,
            WalkingFromPlayer
        }

        int currentState = 0;
        float timeSinceCollideWithPlayer = 0f;
        float timeSincePlayedSound = 0f;

        private const float speed = 7f;
        private const float rotationSpeed = 7f;

        private bool HasValidTarget()
        {
            return targetPlayer != null && targetPlayer.isInsideFactory && !targetPlayer.isPlayerDead;
        }

        private void WalkToPlayer()
        {
            var direction = (targetPlayer.playerGlobalHead.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += direction * (Time.deltaTime * speed);
        }

        private void WalkFromPlayer()
        {
            var direction = -(targetPlayer.playerGlobalHead.position - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
            transform.position += direction * (Time.deltaTime * speed);
        }
        
        public override void Start()
        {
            base.Start();
            //plugin = new Plugin();
            currentState = (int) State.LookingForPlayer;
            //plugin.Log("BratanEnemy");
        }

        public override void Update()
        {
            base.Update();

            switch (currentState)
            {
                case (int) State.Idle:
                    creatureAnimator.SetTrigger("stopWalk");
                    if (timeSinceCollideWithPlayer > 10.0f) break;
                    else {
                        currentState = (int) State.WalkingFromPlayer;
                    }
                    //plugin.Log("BratanEnemy");
                    break;

                case (int) State.LookingForPlayer:
                    if (!HasValidTarget())
                    {
                        targetPlayer = GetClosestPlayer();
                        currentState = (int) State.WalkingToPlayer;
                    }
                    //plugin.Log("BratanEnemy");
                    break;
                
                case (int) State.WalkingToPlayer:
                    creatureAnimator.SetTrigger("startWalk");
                    WalkToPlayer();
                    //plugin.Log("BratanEnemy");
                    break;
                
                case (int) State.WalkingFromPlayer:
                    if (timeSinceCollideWithPlayer > 10.0f) {
                        currentState = (int) State.LookingForPlayer;
                        break;
                    }
                    creatureAnimator.SetTrigger("startWalk");
                    WalkFromPlayer();
                    //plugin.Log("BratanEnemy");
                    break;
            }
        }

        public override void DoAIInterval()
        {
            base.DoAIInterval();

            if (timeSincePlayedSound < 6.6f) timeSincePlayedSound += 0.2f;

            if (timeSinceCollideWithPlayer < 10.0f) timeSinceCollideWithPlayer += 0.2f;

            //DEBUG VALUE IS 6.6f , DEFAULT IS 390.0f
            if (timeSincePlayedSound >= 390.0f) {
                creatureVoice.PlayOneShot(enemyType.audioClips[UnityEngine.Random.Range(0, enemyType.audioClips.Length)]);
                timeSincePlayedSound = 0f;
            }
        }

        public override void OnCollideWithPlayer(Collider other)
        {
            //plugin.Log("BratanEnemy");
            timeSinceCollideWithPlayer = 0f;
            currentState = (int) State.Idle;
        }
    }
}
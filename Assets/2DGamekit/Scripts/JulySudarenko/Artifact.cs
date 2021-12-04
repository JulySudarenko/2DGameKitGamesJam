using UnityEngine;

namespace Gamekit2D
{
    internal sealed class Artifact : IArtifact
    {
        private IGravity _gravity;
        private ArtifactSettings _settings;
        private CharacterController2D _characterController2D;
        private RandomAudioPlayer _crushSound;
        private float _normalGravityForce;
        private float _charge;
        private bool _isActivate;

        public Artifact(ArtifactSettings settings, CharacterController2D characterController2D,
            float normalGravityForce, IGravity gravity, RandomAudioPlayer crushSound)
        {
            _settings = settings;
            _normalGravityForce = normalGravityForce;
            _characterController2D = characterController2D;
            _gravity = gravity;
            _crushSound = crushSound;
            _charge = _settings.Charge;
        }

        public void Init()
        {
            _settings.JumpArtefactDamager.DisableDamage();
        }

        public void ApplyArtifactAbility()
        {
            if (_charge > 0)
            {
                CheckInput();
                
                if (_isActivate)
                {
                    _charge -= Time.deltaTime;
                }
            }
            else
            {
                _gravity.SetGravity(_settings.NormalGravity, _normalGravityForce);
                _isActivate = false;
            }
        }

        private void CheckInput()
        {
            if (PlayerInput.Instance.Artifact.Down)
            {
                if (_isActivate)
                {
                    _gravity.SetGravity(_settings.NormalGravity, _normalGravityForce);
                    _isActivate = false;
                }
                else
                {
                    _gravity.SetGravity(_settings.ArtefactGravity, _settings.ArtefactForceGravity);
                    _isActivate = true;
                }
            }
        }

        public void ApplyCrushAbility()
        {
            if (PlayerInput.Instance.Artifact.Up && !_isActivate)
            {
                _settings.JumpArtefactDamager.EnableDamage();
                _settings.JumpArtefactDamager.disableDamageAfterHit = true;
                _crushSound.PlayRandomSound();
            }
        }
    }
}

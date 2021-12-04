using System;
using UnityEngine;

namespace Gamekit2D
{
    internal sealed class Artifact : IArtifact, IDisposable
    {
        private readonly IGravity _gravity;
        private readonly ArtifactSettings _settings;
        private CharacterController2D _characterController2D;
        private readonly RandomAudioPlayer _crushSound;
        private readonly float _normalGravityForce;
        private float _charge;
        private float _maxCharge;
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
            _maxCharge = _settings.Charge;
        }

        public void Init()
        {
            _settings.JumpArtifactDamager.DisableDamage();
            _settings.OnImprovedCharge += ImproveCharge;
        }

        public void ApplyArtifactAbility()
        {
            if (_charge > 0)
            {
                CheckInput();

                if (_isActivate)
                {
                    _charge -= Time.deltaTime;
                    _settings.ArtifactView.ArtifactActivate(_maxCharge, _charge);
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
                    _gravity.SetGravity(_settings.ArtifactGravity, _settings.ArtifactForceGravity);
                    _isActivate = true;
                }
            }
        }

        public void ApplyCrushAbility()
        {
            if (PlayerInput.Instance.Artifact.Up && !_isActivate)
            {
                _settings.JumpArtifactDamager.EnableDamage();
                _settings.JumpArtifactDamager.disableDamageAfterHit = true;
                _crushSound.PlayRandomSound();
            }
        }

        private void ImproveCharge()
        {
            _charge += _settings.ImprovedCharge;
            _maxCharge = _settings.MaxCharge;
            if (_charge > _maxCharge)
            {
                _charge = _maxCharge;
            }

            _settings.ArtifactView.ArtifactActivate(_maxCharge, _charge);
        }

        public void Dispose()
        {
            _settings.OnImprovedCharge -= ImproveCharge;
        }
    }
}

using System;
using UnityEngine;
using Action = System.Action;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Gamekit2D
{
    internal sealed class Artifact : IDisposable
    {
        public Action<bool> IsArtifactApply;
        private readonly IGravity _gravity;
        private readonly ArtifactSettings _settings;
        private readonly CharacterController2D _characterController2D;
        private readonly RandomAudioPlayer _crushSound;
        private Quaternion _quaternion;
        private Quaternion _quaternionRotation;
        private Quaternion _quaternionRotationZ;
        private Quaternion _quaternionRotationY;
        private readonly float _normalGravityForce;
        private float _charge;
        private float _maxCharge;
        private float _timer = 0.0f;
        private float _maxPushTime = 0.2f;
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
            _quaternion = Quaternion.Euler(0f, 0f, 0f);
            _quaternionRotation = Quaternion.Euler(0f, 180f, 180f);
            _quaternionRotationZ = Quaternion.Euler(0f, 0f, 180f);
            _quaternionRotationY = Quaternion.Euler(0f, 180f, 0f);
            _isActivate = false;
        }

        public void Init()
        {
            _settings.JumpArtifactDamager.DisableDamage();
            _settings.OnImprovedCharge += ImproveCharge;
        }

        public void ApplyArtifactAbility()
        {
            if (PlayerInput.Instance.Artifact.Held && _charge > 0)
            {
                _settings.ArtifactView.ArtifactActivate(_maxCharge, _charge);
                _gravity.SetGravity(_settings.ArtifactGravity, _settings.ArtifactForceGravity);
                _isActivate = true;
                IsArtifactApply.Invoke(_isActivate);
                _timer += Time.deltaTime;

                _charge -= Time.deltaTime;
                if (_charge <= 0)
                {
                    _charge = 0;
                    Land();
                    _timer = 0.0f;
                }
            }

            if (PlayerInput.Instance.Artifact.Up && _charge > 0)
            {
                Land();
                _timer = 0.0f;
            }

            Rotate();
        }

        private void ApplyCrushAbility()
        {
            _settings.JumpArtifactDamager.EnableDamage();
            _settings.JumpArtifactDamager.disableDamageAfterHit = true;
            _crushSound.PlayRandomSound();
        }

        private void Rotate()
        {
            if (_isActivate && _characterController2D.transform.rotation.z != _quaternionRotation.z)
            {
                _characterController2D.transform.rotation =
                    Quaternion.Lerp(_characterController2D.transform.rotation, _quaternionRotation,
                        4f * Time.deltaTime);
            }

            if (!_isActivate && _characterController2D.transform.rotation.z != _quaternion.z)
            {
                _characterController2D.transform.rotation =
                    Quaternion.Lerp(_quaternion, _quaternion, 4f * Time.deltaTime);
            }
        }

        private void Land()
        {
            ApplyCrushAbility();
            _gravity.SetGravity(_settings.NormalGravity, _normalGravityForce);
            _isActivate = false;
            IsArtifactApply.Invoke(_isActivate);

            if (_timer > _maxPushTime)
            {
                _characterController2D.transform.position += Vector3.down * 2.5f;
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

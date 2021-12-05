using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Gamekit2D
{
    internal sealed class Artifact : IDisposable
    {
        public Action<bool> IsArtifactApply;
        private readonly Gravitation _gravity;
        private readonly ArtifactSettings _settings;
        private readonly CharacterController2D _characterController2D;
        private readonly RandomAudioPlayer _crushSound;
        private readonly Quaternion _quaternion;
        private readonly Quaternion _quaternionRotation;
        private readonly float _normalGravityForce;
        private Quaternion _characterRotation;
        private float _charge;
        private float _maxCharge;
        private float _push = 2.5f;
        private float _rotationSpeed = 4.0f;
        private float _timer = 0.0f;
        private float _maxPushTime = 0.3f;
        private bool _isActivate;


        public Artifact(ArtifactSettings settings, CharacterController2D characterController2D,
            float normalGravityForce, Gravitation gravity, RandomAudioPlayer crushSound)
        {
            _settings = settings;
            _normalGravityForce = normalGravityForce;
            _characterController2D = characterController2D;
            _gravity = gravity;
            _crushSound = crushSound;
            _maxCharge = _settings.Charge;
            _quaternion = Quaternion.Euler(0f, 0f, 0f);
            _quaternionRotation = Quaternion.Euler(0f, 180f, 180f);
            _isActivate = false;
        }

        public void Init()
        {
            _settings.JumpArtifactDamager.DisableDamage();
            _settings.OnImprovedCharge += ImproveCharge;
            _settings.OnGetArtifact += HasArtifact;

            InitIconView();
        }

        public void ApplyArtifactAbility()
        {
            if (PlayerInput.Instance.Artifact.Held && _charge > 0)
            {
                _settings.ArtifactView.ShowPower(_maxCharge, _charge);
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
            _characterRotation = _characterController2D.transform.rotation;

            if (_isActivate && _characterRotation.z != _quaternionRotation.z)
            {
                _characterController2D.transform.rotation =
                    Quaternion.Lerp(_characterController2D.transform.rotation, _quaternionRotation,
                        _rotationSpeed * Time.deltaTime);
            }

            if (!_isActivate && _characterRotation.z != _quaternion.z)
            {
                _characterController2D.transform.rotation =
                    Quaternion.Lerp(_quaternion, _quaternion, _rotationSpeed * Time.deltaTime);
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
                _characterController2D.transform.position += Vector3.down * _push;
            }
        }

        private void InitIconView()
        {
            if (_charge > 0)
            {
                _settings.ArtifactView.ActivateDeactivate(true);
            }
            else
            {
                _settings.ArtifactView.ActivateDeactivate(false);
            }
        }

        private void HasArtifact()
        {
            _charge = _settings.Charge;
            _maxCharge = _settings.Charge;

            InitIconView();
            _settings.ArtifactView.ShowPower(_maxCharge, _charge);
        }

        private void ImproveCharge()
        {
            _charge += _settings.ImprovedCharge;
            _maxCharge = _settings.MaxCharge;
            if (_charge > _maxCharge)
            {
                _charge = _maxCharge;
            }

            InitIconView();
            _settings.ArtifactView.ShowPower(_maxCharge, _charge);
        }

        public void Dispose()
        {
            _settings.OnImprovedCharge -= ImproveCharge;
            _settings.OnGetArtifact -= InitIconView;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HealthManager : MonoBehaviour
{
    [Header("Feedback References")]
    [SerializeField] private AudioClip _impactSound;
    [SerializeField] private float _flashDuration = 0.5f;
    public int _flashDelay = 1;
    private MeshRenderer _meshRef;
    private Color _defaultMeshColor;

    [Header("Health Values")]
    [SerializeField] public int startingHealth;
    [SerializeField] public int lowHealthThreshold;
    [SerializeField] public int healthRestoreRate;
    private int _currentHealth;

    public int currentHealth
    {
        get { return _currentHealth; }
        set { _currentHealth = Mathf.Clamp(value, 0, startingHealth); }
    }

    private void Awake()
    {
        _meshRef = GetComponent<MeshRenderer>();
        _defaultMeshColor = _meshRef.material.color;
    }

    private void Start()
    {
        SetUserHealth();
    }

    private void Update()
    {

    }

    public void SetUserHealth()
    {
        _currentHealth = startingHealth;
    }


    public void DeductHealth(int value)
    {
        _currentHealth -= value;
        Debug.Log(_currentHealth);
        StartCoroutine(DamageFlash());

        if (_currentHealth <= 0)
        {
            StartCoroutine(UserDeath());

        }
    }

    public void IncreaseHealth(int value)
    {
        _currentHealth += value;
    }

    private IEnumerator UserDeath()
    {
        //this.gameObject.SetActive(false);
        yield return new WaitForSeconds(_flashDelay);
    }

    private IEnumerator DamageFlash()
    {
        _meshRef.material.SetColor("_Color", Color.white);
        yield return new WaitForSeconds(_flashDuration);
        FlashReset();
    }

    private void FlashReset()
    {
        _meshRef.material.SetColor("_Color", _defaultMeshColor);
    }

    private void ImpactFeedbackSound()
    {

    }
}
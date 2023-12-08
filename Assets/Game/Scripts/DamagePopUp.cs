using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{


    private TextMeshPro _textMesh;
    private Color _textColor;
    private float _disapearTimer = 1f;
    private float _disapearSpeed = 3f;
    [SerializeField] private Color _baseColor;
    [SerializeField] private Color _criticalColor;

    public static DamagePopUp Create(Vector3 pos, int damage, bool critical) {
        Transform damagePopUpTransform = Instantiate(GameAssets.i.pfDamagePopUp, pos, Quaternion.identity);
        DamagePopUp damagePopUp = damagePopUpTransform.GetComponent<DamagePopUp>();
        damagePopUp.Setup(damage, critical);
        return damagePopUp;
    }

    private void Awake() {
        _textMesh = GetComponent<TextMeshPro>();
        _textColor = _textMesh.color;
    }

    public void Setup(int damage, bool critical) {
        _textMesh.text = damage.ToString();
        if (critical)
        {
            _textMesh.color = _criticalColor;
        }
        else
        {
            _textMesh.color = _baseColor;
        }
    }

    private void Update() {
        float moveYSpeed = 1f;
        transform.position += new Vector3(0 ,moveYSpeed) * Time.deltaTime;
        _disapearTimer -= Time.deltaTime;
        
        
        if (_disapearTimer<0)
        {
            _textColor.a -= _disapearSpeed * Time.deltaTime;
            _textMesh.color = _textColor;
            if (_textColor.a <=0)
            {
                Destroy(gameObject);
            }
        }
    }
}

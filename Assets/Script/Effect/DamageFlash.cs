using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float _flashTime = 0.25f;

    private SpriteRenderer _spriteRenderers;
    private Material _mat;

    private Coroutine _damageFlashCourtine;

    private void Awake()
    {
        _spriteRenderers = GetComponent<SpriteRenderer>();
        Init();
    }

    // Start is called before the first frame update
    void Init()
    {
        _mat = _spriteRenderers.material;
    }

    public void CallDamageFlash()
    {
        _damageFlashCourtine = StartCoroutine(DamageFlasher());
    }

    IEnumerator DamageFlasher()
    {
        SetFlashColor();

        float currentFlashAmount;
        float elapsedTime = 0f;

        while (elapsedTime < _flashTime)
        {
            elapsedTime += Time.deltaTime;

            currentFlashAmount = Mathf.Lerp(1f, 0f, (elapsedTime / _flashTime));
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }

    void SetFlashColor()
    {

        _mat.SetColor("_Color", _flashColor);

    }

    void SetFlashAmount(float amount)
    {
        _mat.SetFloat("_Amount", amount);
    }
}

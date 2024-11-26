using System;
using System.Collections;
using BNG;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Button = UnityEngine.UI.Button;

public class KeyboardController : MonoBehaviour
{
    private const float InputDelay = 0.2f;
    private const float InputThreshold  = 0.5f;
    
    [SerializeField] private GameObject keysMatrix;

    private GameObject[] _lines;
    private int _currentLine = 0; 
    private int _currentKey = 0;
    private GameObject _currentSelectedKey;

    private void Start()
    {
        _lines = GetChildren(keysMatrix);
        HighlightKey();
        StartCoroutine(UpdateWithTimer(InputDelay));
    }
    
    // Задержка перед использованием
    private IEnumerator UpdateWithTimer(float delay)
    {
        while (true)
        {
            HandleNavigation();
            if (InputBridge.Instance.AButton)
            {
                PressKey();
                yield return WaitUntilButtonReleased();
            }
            yield return new WaitForSeconds(delay); 
        }
    }
    
    // Функция для разового нажатия кнопки подтверждения
    private IEnumerator WaitUntilButtonReleased()
    {
        while (InputBridge.Instance.AButton) 
        {
            yield return null; 
        }
    }
    
    // Навигация при помощи джостика
    private void HandleNavigation()
    {
        Vector2 navigationInput = InputBridge.Instance.GetInputAxisValue(InputAxis.RightThumbStickAxis);
        if (navigationInput.y > InputThreshold) MoveVertical(-1); 
        if (navigationInput.y < -InputThreshold) MoveVertical(1); 
        if (navigationInput.x > InputThreshold) MoveHorizontal(1); 
        if (navigationInput.x < -InputThreshold) MoveHorizontal(-1); 
    }

    private void MoveHorizontal(int direction)
    {
        GameObject[] currentKeys = GetChildren(_lines[_currentLine]);
        if (currentKeys == null) return;

        _currentKey = Mathf.Clamp(_currentKey + direction, 0, currentKeys.Length - 1);
        HighlightKey();
    }

    private void MoveVertical(int direction)
    {
        _currentLine = Mathf.Clamp(_currentLine + direction, 0, _lines.Length - 1);
        GameObject[] currentKeys = GetChildren(_lines[_currentLine]);
        if (currentKeys != null)
        {
            _currentKey = Mathf.Clamp(_currentKey, 0, currentKeys.Length - 1);
        }
        HighlightKey();
    }
    
    // Подтвердить выбор
    private void PressKey()
    {
        if (_currentSelectedKey == null) return;

        Button button = _currentSelectedKey.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke();
        }
    }

    // Обновление цвета клавиш
    private void HighlightKey()
    {
        if (_currentSelectedKey != null)
        {
            Image oldButtonImage = _currentSelectedKey.GetComponent<Image>();
            if (oldButtonImage != null)
            {
                oldButtonImage.color = Color.white;
            }
        }

        GameObject[] currentKeys = GetChildren(_lines[_currentLine]);
        if (currentKeys != null && currentKeys.Length > _currentKey)
        {
            _currentSelectedKey = currentKeys[_currentKey];
            Image buttonImage = _currentSelectedKey.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.cyan; 
            }
        }
    }

    // Функция для получения всех линий и кнопок
    private GameObject[] GetChildren(GameObject parentObject)
    {
        if (parentObject == null) return null;

        Transform parentTransform = parentObject.transform;
        GameObject[] children = new GameObject[parentTransform.childCount];
    
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            children[i] = parentTransform.GetChild(i).gameObject;
        }

        return children;
    }
}
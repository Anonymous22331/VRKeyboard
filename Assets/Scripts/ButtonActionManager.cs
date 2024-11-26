using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionManager : MonoBehaviour
{
    /// <summary>
    /// Класс для управления действиями клавиш
    /// </summary>
     
    [SerializeField] private Text displayText;
    [SerializeField] private Image capsImage;
    
    private string _inputText = "";
    private bool _isCapsActive = false;
    
    public void AddCharacter(string character)
    {
        _inputText += _isCapsActive ? character.ToUpper() : character;
        UpdateDisplay();
    }

    public void ToggleCaps()
    {
        _isCapsActive = !_isCapsActive;
        capsImage.color = _isCapsActive ? Color.green : Color.red;
        
    }

    public void Backspace()
    {
        if (_inputText.Length > 0)
        {
            _inputText = _inputText.Substring(0, _inputText.Length - 1);
            UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        if (displayText != null)
        {
            displayText.text = _inputText;
        }
    }
}

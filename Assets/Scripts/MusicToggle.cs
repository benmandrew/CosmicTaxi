using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour {
    public Sprite onImage;
    public Sprite offImage;
    private Image image;
    private bool state = true;

    void Start() {
        image = GetComponent<Image>();
    }

    public void Toggle() {
        if (state) {
            image.sprite = offImage;
        } else {
            image.sprite = onImage;
        }
        state = !state;
    }
}

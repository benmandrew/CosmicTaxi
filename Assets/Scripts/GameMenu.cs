using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
    public GameObject menu;
    public GameObject deathMenu;
    public GameObject winMenu;
    private bool showingMenu = false;

    private readonly float deathTimer = 2.0f;
    private bool dead = false;
    private readonly float winTimer = 8.0f;
    private bool win = false;
    private float score;
    private float menuCooldown = 0.0f;

    void Start() {
        menu.SetActive(showingMenu);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            ToggleMenu();
        }

        if (dead || win) {
            menuCooldown -= Time.deltaTime;
            if (menuCooldown < 0.0f) {
                if (dead) {
                    ShowDeathMenu();
                } else if (win) {
                    ShowWinMenu();
                }

                // reset conditions
                dead = false;
                win = false;
            }
        }
    }

    public void ToggleMenu() {
        showingMenu = !showingMenu;
        menu.SetActive(showingMenu);
        if (showingMenu) {
            Time.timeScale = 0.0f;
        } else {
            Time.timeScale = 1.0f;
        }
    }

    public void NotifyDeath() {
        dead = true;
        menuCooldown = deathTimer;
    }

    public void NotifyWin(float score) {
        // Notify of a win twice (e.g. by closing the overlay) overrides the timer
        if (win) { menuCooldown = 0.0f; }
        win = true;
        menuCooldown = winTimer;
        this.score = score;
    }

    private void ShowDeathMenu() {
        deathMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    private void ShowWinMenu() {
        winMenu.GetComponentsInChildren<Text>()[2].text = "Average journey speed: " + Math.Round(score, 3, MidpointRounding.AwayFromZero);
        winMenu.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void Restart() {
        Time.timeScale = 1.0f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Quit() {
        Time.timeScale = 1.0f;
        // Change to scene switch to main menu.
        SceneManager.LoadScene("MainMenuScene");
    }
}

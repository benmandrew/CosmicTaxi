using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    private GameObject monitor;

    private Image alienSprite;

    private GameObject newPassenger;
    private GameObject passengerName;
    private GameObject origin;

    private GameObject pickUp;
    private GameObject pickUpText;
    private GameObject destination;

    private GameObject dropOff;
    private GameObject dropOffText;

    public Vector3 closedPos;
    public Vector3 openPos;

    private GameObject closeMonitorSign;

    public Passenger testPassenger;
    public Planet testOrigin;
    public Planet testDestination;

    public float openCloseSpeed;

    public float openCoolDown;

    private bool opening;
    private bool closing;

    private bool Isopen;

    private PassengerManager passengerManager;

    private int globalID;

    public float offset = 12f;

    public float screenSize;

    // Start is called before the first frame update
    void Awake()
    {
        monitor = GameObject.Find("Monitor");
        origin = GameObject.Find("Origin");
        passengerName = GameObject.Find("Passenger Name");
        alienSprite = GameObject.Find("Alien Sprite").GetComponent<Image>();
        newPassenger = GameObject.Find("New Passenger");
        pickUp = GameObject.Find("Passenger Pick Up");
        dropOff = GameObject.Find("Passenger Drop Off");
        pickUpText = GameObject.Find("Pick Up Text");
        dropOffText = GameObject.Find("Drop Off Text");
        destination = GameObject.Find("Destination");
        closeMonitorSign = GameObject.Find("Close Cross");
        passengerManager = FindObjectOfType<PassengerManager>();

        closeMonitorSign.SetActive(false);
        newPassenger.SetActive(false);
        pickUp.SetActive(false);
        dropOff.SetActive(false);
        alienSprite.color = new Color(alienSprite.color.r, alienSprite.color.g, alienSprite.color.b, 0f);
        Isopen = false;
        globalID = 0;

        UpdateScreenSize();

        //SetPassenger(testPassenger, testOrigin, testDestination);
    }

    // Update is called once per frame
    void Update()
    {
        if (opening) 
        {
            // Move the monitor on screen
            monitor.transform.localPosition = Vector3.MoveTowards(monitor.transform.localPosition, openPos, openCloseSpeed * Time.deltaTime);
            if ((openPos - monitor.transform.localPosition).magnitude <= 0.01f) 
            {
                // Monitor is fully on screen
                opening = false;
                Isopen = true;
                closeMonitorSign.SetActive(true);
                StartCoroutine(OpenCooldown(globalID));
            }
        }
        if (closing)
        {
            // Move the monitor
            monitor.transform.localPosition = Vector3.MoveTowards(monitor.transform.localPosition, closedPos, openCloseSpeed * Time.deltaTime);
            if ((closedPos - monitor.transform.localPosition).magnitude <= 0.01f)
            {
                closing = false;
                Isopen = false;
                closeMonitorSign.SetActive(false);
                passengerManager.MonitorClosed();
            }
        }
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.RightControl)) 
        {
            toggleSwitch();
        }
        if (Screen.width != screenSize)
        {
            // Screen size has been changed
            UpdateScreenSize();
        }
    }

    private void UpdateScreenSize() 
    {
        float scale = Screen.width / 1928f;
        monitor.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, 1);
        float xPos = -Screen.width / 2f + offset;
        float yPos = Screen.height / 2f - offset;
        openPos = new Vector3(xPos, yPos, 0);
        float width = Screen.width / 3.2f;
        closedPos = new Vector3(xPos - offset - 15 - width, yPos, 0);
        if (Isopen) 
        {
            monitor.transform.localPosition = openPos;
        }
        else
        {
            monitor.transform.localPosition = closedPos;
        }
        screenSize = Screen.width;
    }

    public void OpenMonitor()
    {
        opening = true;
        closing = false;
        globalID++;
    }

    private void CloseMonitor()
    {
        closing = true;
        opening = false;
    }

    public void toggleSwitch()
    {
        if (Isopen && !opening)
        {
            CloseMonitor();
        }
        else if(!closing)
        {
            OpenMonitor();
        }
    }

    public void SetPassenger(Passenger passenger, Planet origin, Planet destination)
    {
        PassengerName(passenger.passengerName);
        SetOriginText(origin.name);
        SetDestinationText(destination.name);
        SetSprite(passenger.passengerSprite);
        SetPickUpText(passenger.pickUpText);
        SetDropOffText(passenger.dropOffText);
        NotifyNewPassenger();
    }

    public void NotifyNewPassenger()
    {
        newPassenger.SetActive(true);
        pickUp.SetActive(false);
        dropOff.SetActive(false);
        OpenMonitor();
    }

    public void NotifyPickUp()
    {
        newPassenger.SetActive(false);
        pickUp.SetActive(true);
        dropOff.SetActive(false);
        OpenMonitor();
    }

    public void NotifyDropOff()
    {
        newPassenger.SetActive(false);
        pickUp.SetActive(false);
        dropOff.SetActive(true);
        OpenMonitor();
    }

    private void SetOriginText(string text) 
    {
        origin.GetComponent<Text>().text = "Origin: " + text;
    }

    private void SetDestinationText(string text) 
    {
        destination.GetComponent<Text>().text = "Destination: " + text;
    }

    private void SetSprite(Sprite sprite) 
    {
        alienSprite.sprite = sprite;
        alienSprite.color = new Color(alienSprite.color.r, alienSprite.color.g, alienSprite.color.b, 1f);
    }

    private void PassengerName(string name) 
    {
        pickUp.GetComponent<Text>().text = name;
        dropOff.GetComponent<Text>().text = name;
        passengerName.GetComponent<Text>().text = "Passenger: " + name;
    }

    private void SetPickUpText(string text) 
    {
        pickUpText.GetComponent<Text>().text = '"' + text + '"';
    }

    private void SetDropOffText(string text)
    {
        dropOffText.GetComponent<Text>().text = '"' + text + '"';
    }

    private IEnumerator OpenCooldown(int openID) 
    {
        yield return new WaitForSeconds(openCoolDown);
        if (openID == globalID) 
        {
            CloseMonitor();
        }
    }
}

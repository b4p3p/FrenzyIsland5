using UnityEngine;
using System.Collections;

public class cmdHideMenu : MonoBehaviour {

    public GameObject TopMenu;
    private UISprite sprite;
    private float MaxTop = 50;

    private enum States
    {
        Hidden , 
        Visible
    }
    private States State;

    public cmdHideMenu()
    {
        State = States.Visible;
    }

	void Start () {
        sprite = TopMenu.GetComponent<UISprite>();
	}
	
	void Update () 
    {
        float y = TopMenu.transform.localPosition.y;
        float step = 0;
        switch (State)
        {
            case States.Hidden:
                if ( y < sprite.height)
                {
                    step = Time.deltaTime;
                    TopMenu.transform.Translate(Vector3.up * step, Space.Self);
                    if ( TopMenu.transform.localPosition.y > sprite.height )
                    {
                        TopMenu.transform.localPosition = new Vector3(0, sprite.height, 0);
                    }
                }
                break;
            case States.Visible:
                
                if (y > 0)
                {
                    step = Mathf.Min ( Time.deltaTime , y );
                    TopMenu.transform.Translate( Vector3.down * step , Space.Self);
                    if (TopMenu.transform.localPosition.y < 0)
                    {
                        TopMenu.transform.localPosition = new Vector3(0, 0 , 0);
                    }
                }
                break;
        }
	}

    void OnClick()
    {
        switch (State)
        {
            case States.Hidden:
                State = States.Visible;
                break;
            case States.Visible:
                State = States.Hidden;
                break;
        }
    }
}

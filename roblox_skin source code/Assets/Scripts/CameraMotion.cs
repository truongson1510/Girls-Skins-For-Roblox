using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.iOS;

public class CameraMotion : MonoBehaviour
{
    bool rotate;
    public Transform target;
    public GameObject center;
    public GameObject[] canvas;
    public GameObject mapsCanv;
    public GameObject CapturCanv;
    // Start is called before the first frame update
    void Start()
    {
        //moveCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (rotate)
        {
            transform.RotateAround(center.transform.position, Vector3.up, 90 * Time.deltaTime);
        }
    }

    public void moveCamera()
    {
        //LeanTween.move(this.gameObject, target.position, 1);
        //LeanTween.rotate(this.gameObject, target.eulerAngles, 1);
        StartCoroutine(StartMove());

        Debug.Log("SHOW INTER HERE: ");
    }
    IEnumerator StartMove()
    {
        canvas[PlayerPrefs.GetInt("player")].transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(false);
        canvas[PlayerPrefs.GetInt("player")].transform.parent.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        yield return new WaitForSeconds(0.7f);
        //rotate = true;
        //LeanTween.moveY(canvas[PlayerPrefs.GetInt("player")], -1000, 1);
        yield return new WaitForSeconds(1.7f);
        canvas[PlayerPrefs.GetInt("player")].transform.parent.gameObject.SetActive(false);
      //  Device.RequestStoreReview();
        mapsCanv.SetActive(true);
    }

    public void rotateCamera()
    {
        rotate = true;
        mapsCanv.SetActive(false);
        StartCoroutine(StopRotation());
    }

    IEnumerator StopRotation()
    {
        yield return new WaitForSeconds(4f);
        rotate = false;
        CapturCanv.SetActive(true);
        Debug.Log("SHOW INTER HERE: ");
    }
}

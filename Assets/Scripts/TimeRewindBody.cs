using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeRewindBody : MonoBehaviour {

    public float recordLength = 5f;

    private bool isRewinding = false;
    private List<PointInTime> rewindList;
	
    void Start() {
        rewindList = new List<PointInTime>();
    }

	void Update () {
        if (Input.GetButtonDown("Fire2")) {
            StartRewinding();
        } else if (Input.GetButtonUp("Fire2")) {
            StopRewinding();
        }
	}

    void FixedUpdate() {
        if (isRewinding) {
            rewind();
        } else {
            record();
        }
    }

    void StartRewinding() {
        isRewinding = true;
    }

    void StopRewinding() {
        isRewinding = false;
    }

    void record() {
        if (rewindList.Count > Mathf.Round(recordLength / Time.fixedDeltaTime)) {
            rewindList.RemoveAt(rewindList.Count - 1);
        }
        rewindList.Insert(0, new PointInTime(transform.position, transform.rotation));
    }

    void rewind() {
        if (rewindList.Count > 0) {
            PointInTime pointInTime = rewindList[0];
            transform.position = pointInTime.position;
            transform.rotation = pointInTime.rotation;
            rewindList.RemoveAt(0);
        } else {
            StopRewinding();
        }
    }
}

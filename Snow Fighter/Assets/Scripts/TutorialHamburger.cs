using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialHamburger : MonoBehaviour
{
    [SerializeField] TutorialUI tutorialUI;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialUI.isHamburgerDestroyed = true;
        }
    }
}

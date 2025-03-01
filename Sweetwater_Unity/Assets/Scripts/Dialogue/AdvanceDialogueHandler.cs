using System;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class AdvanceDialogueHandler : MonoBehaviour
    {
        [SerializeField]
        private LineView _dialogueRunner;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                _dialogueRunner.UserRequestedViewAdvancement();
            }
        }
    }
}

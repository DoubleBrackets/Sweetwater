using System;
using Base;
using Events;
using UnityEngine;
using Yarn.Unity;

namespace Dialogue
{
    public class PlayDialogueHandler : DescriptionMono
    {
        [SerializeField]
        private StringEvent _dialogue;
        
        [SerializeField]
        private DialogueRunner _dialogueRunner;

        private void Awake()
        {
            _dialogue.AddListener(PlayDialogue);
        }

        private void OnDestroy()
        {
            _dialogue.RemoveListener(PlayDialogue);
        }
        
        private void PlayDialogue(string dialogue)
        {
            _dialogueRunner.StartDialogue(dialogue);
        }
    }
}

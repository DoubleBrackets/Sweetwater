using Base;
using Interaction;
using TMPro;
using UnityEngine;

namespace Protag
{
    public class CursorUI : DescriptionMono
    {
        [SerializeField]
        private TMP_Text _interactionPreviewText;

        [SerializeField]
        private GameObject _defaultCursor;

        [SerializeField]
        private GameObject _interactCursor;

        public void UpdateInteractionPreview(CanInteractCheckResult check)
        {
            if (check.CanInteract)
            {
                _interactionPreviewText.text = check.CursorHint;
                _defaultCursor.SetActive(false);
                _interactCursor.SetActive(true);
            }
            else
            {
                _interactionPreviewText.text = "";
                _defaultCursor.SetActive(true);
                _interactCursor.SetActive(false);
            }
        }
    }
}
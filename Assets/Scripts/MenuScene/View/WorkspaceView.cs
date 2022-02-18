using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace EAR.View
{
    public class WorkspaceView : MonoBehaviour
    {
        [SerializeField]
        private Image avatar;

        [SerializeField]
        private TMP_Text title;

        [SerializeField]
        private TMP_Text role;

        [SerializeField]
        private TMP_Text owner;

        [SerializeField]
        private Button button;

        private Action<int> workspaceClickEvent;
        private int id;

        public void PopulateData(WorkspaceData data)
        {
            id = data.id;
            title.text = data.name;
            role.text = data.role;
            owner.text = data.owner;
            workspaceClickEvent = data.workspaceClickEvent;
        }

        public void PopulateData(Sprite sprite)
        {
            avatar.sprite = sprite;
        }

        private void ButtonClickEventSubscriber()
        {
            workspaceClickEvent?.Invoke(id);
        }

        void Start()
        {
            button.onClick.AddListener(ButtonClickEventSubscriber);
        }
    }
}


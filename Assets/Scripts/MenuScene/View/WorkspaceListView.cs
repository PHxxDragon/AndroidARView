using UnityEngine;
using System;
using System.Collections.Generic;

namespace EAR.View
{
    public class WorkspaceListView : ViewInterface
    {
        public event Action WorkspaceRefreshEvent;

        [SerializeField]
        private GameObject Container;
        [SerializeField]
        private GameObject WorkspacePrefabs;

        private Dictionary<int, WorkspaceView> workspaceViews = new Dictionary<int, WorkspaceView>();

        public void PopulateData(List<WorkspaceData> datas)
        {
            foreach (Transform transform in Container.transform)
            {
                Destroy(transform.gameObject);
                workspaceViews.Clear();
            }
            foreach (WorkspaceData data in datas)
            {
                WorkspaceView workspaceView = Instantiate(WorkspacePrefabs, Container.transform).GetComponent<WorkspaceView>();
                workspaceView.PopulateData(data);
                workspaceViews.Add(data.id, workspaceView);
            }
        }

        public void PopulateData(Sprite sprite, int id)
        {
            workspaceViews[id].PopulateData(sprite);
        }

        public override void Refresh(object args = null)
        {
            WorkspaceRefreshEvent?.Invoke();
        }
    }
}


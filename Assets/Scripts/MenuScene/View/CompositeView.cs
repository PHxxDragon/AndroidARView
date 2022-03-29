using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EAR.View
{
    public class CompositeView : ListItemView<object>
    {
        [SerializeField]
        private ModuleView modulePrefab;
        [SerializeField]
        private SectionView sectionPrefab;

        //private GameObject view;

        public override void PopulateData(object data)
        {
            if (data is ModuleData)
            {
                ModuleView moduleView = Instantiate(modulePrefab, transform);
                //view = moduleView.gameObject;
                moduleView.PopulateData(data as ModuleData);
            } 
            if (data is SectionData)
            {
                SectionView sectionView = Instantiate(sectionPrefab, transform);
                //view = sectionView.gameObject;
                sectionView.PopulateData(data as SectionData);
            }
        }
    }
}


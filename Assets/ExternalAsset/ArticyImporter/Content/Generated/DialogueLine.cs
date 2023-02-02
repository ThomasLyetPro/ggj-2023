//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Dracula_1stStepsDemoProject.Features;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Dracula_1stStepsDemoProject
{
    
    
    public class DialogueLine : DialogueFragment, IDialogueFragment, IPropertyProvider, IObjectWithFeatureDialogueFragment
    {
        
        [SerializeField()]
        private ArticyValueDialogueLineTemplate mTemplate = new ArticyValueDialogueLineTemplate();
        
        private static Articy.Dracula_1stStepsDemoProject.Templates.DialogueLineTemplateConstraint mConstraints = new Articy.Dracula_1stStepsDemoProject.Templates.DialogueLineTemplateConstraint();
        
        public Articy.Dracula_1stStepsDemoProject.Templates.DialogueLineTemplate Template
        {
            get
            {
                return mTemplate.GetValue();
            }
            set
            {
                mTemplate.SetValue(value);
            }
        }
        
        public static Articy.Dracula_1stStepsDemoProject.Templates.DialogueLineTemplateConstraint Constraints
        {
            get
            {
                return mConstraints;
            }
        }
        
        public DialogueFragmentFeature GetFeatureDialogueFragment()
        {
            return Template.DialogueFragment;
        }
        
        protected override void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            DialogueLine newClone = ((DialogueLine)(aClone));
            if ((Template != null))
            {
                newClone.Template = ((Articy.Dracula_1stStepsDemoProject.Templates.DialogueLineTemplate)(Template.CloneObject(newClone, aFirstClassParent)));
            }
            base.CloneProperties(newClone, aFirstClassParent);
        }
        
        public override bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return base.IsLocalizedPropertyOverwritten(aProperty);
        }
        
        #region property provider interface
        public override void setProp(string aProperty, object aValue)
        {
            if (aProperty.Contains("."))
            {
                Template.setProp(aProperty, aValue);
                return;
            }
            base.setProp(aProperty, aValue);
        }
        
        public override Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if (aProperty.Contains("."))
            {
                return Template.getProp(aProperty);
            }
            return base.getProp(aProperty);
        }
        #endregion
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using UnityEngine;


namespace Articy.Dracula_1stStepsDemoProject.GlobalVariables
{
    
    
    [Serializable()]
    [CreateAssetMenu(fileName="ArticyGlobalVariables", menuName="Create GlobalVariables", order=620)]
    public class ArticyGlobalVariables : BaseGlobalVariables
    {
        
        [SerializeField()]
        [HideInInspector()]
        private DialogueChapter1_final mDialogueChapter1_final = new DialogueChapter1_final();
        
        [SerializeField()]
        [HideInInspector()]
        private Inventory_final mInventory_final = new Inventory_final();
        
        #region Initialize static VariableName set
        static ArticyGlobalVariables()
        {
            variableNames.Add("DialogueChapter1_final.firstQuestion");
            variableNames.Add("DialogueChapter1_final.nameRevealed");
            variableNames.Add("Inventory_final.stake");
        }
        #endregion
        
        public DialogueChapter1_final DialogueChapter1_final
        {
            get
            {
                return mDialogueChapter1_final;
            }
        }
        
        public Inventory_final Inventory_final
        {
            get
            {
                return mInventory_final;
            }
        }
        
        public static ArticyGlobalVariables Default
        {
            get
            {
                return ((ArticyGlobalVariables)(Articy.Unity.ArticyDatabase.DefaultGlobalVariables));
            }
        }
        
        public override void Init()
        {
            DialogueChapter1_final.RegisterVariables(this);
            Inventory_final.RegisterVariables(this);
        }
        
        public static ArticyGlobalVariables CreateGlobalVariablesClone()
        {
            return Articy.Unity.BaseGlobalVariables.CreateGlobalVariablesClone<ArticyGlobalVariables>();
        }
    }
}
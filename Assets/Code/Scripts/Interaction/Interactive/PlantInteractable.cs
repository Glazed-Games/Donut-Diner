using DonutDiner.InteractionModule.Interactive.Devices;
using DonutDiner.ItemModule;
using DonutDiner.ItemModule.Items;
using System.Collections.Generic;
using UnityEngine;

public enum PlantResponse { nothing,grow,shrink,attack}

namespace DonutDiner.InteractionModule.Environment
{
    public class PlantInteractable : ItemSpot
    {
        #region Fields

        [SerializeField] private InteractiveDevice _linkedDevice;

        [SerializeField,Min(0)] private int currentStage;

        [SerializeField] private Transform plantBody;

        [SerializeField] private List<PlantStage> stages;
        private List<ItemObject> stageItems;



        [SerializeField] private string attackTriggerParameter = "attack";
        [SerializeField] private string eatTriggerParameter = "eat";

        [SerializeField] private Animator animator;
        [SerializeField] private bool animating;
        private Vector3 oldSize;
        private Vector3 newSize;

        [SerializeField, Min(0.1f)] private float animationLength = 3;
        private float timer;

        #endregion

        #region Public Methods

        private void OnEnable()
        {
            if (plantBody == null) { plantBody = transform; }

            oldSize = plantBody.localScale;
            newSize = plantBody.localScale;

            if (stages == null || stages.Count == 0)
            {
                return; 
            }

            foreach (PlantStage el in stages)
            {
                CheckItemListForStage(el);
            }

            if (currentStage < stages.Count)
            {
                newSize = stages[currentStage].size;
        
            }
            timer = animationLength;
            animating = true;
        }

        private void Update()
        {
            if (animating)
            { 
                timer -= Time.deltaTime;

                plantBody.localScale = Vector3.Lerp(oldSize,newSize,(animationLength - timer)/animationLength);

                if (timer <= 0)
                { animating = false; }
            }
        }


        protected override bool CanPlaceItem(Transform transform, out ItemToCarry item)
        {

            return transform.TryGetComponent(out item)  && (IsAllowed(item) || stageItems.Contains(item.GetComponent<ItemObject>()));
        }

        public override bool TryPlaceItem(Transform transform)
        {
            if (!CanPlaceItem(transform, out ItemToCarry item)) return false;

            transform.gameObject.SetActive(false);
            //_linkedDevice.CanInteract = true;

            SetTrigger(eatTriggerParameter);
            

            CheckStage(transform.GetComponent<IItem>());

            return true;
        }

        public  void Take(ItemToCarry item)
        {
            item.Take();


        }


        public void CheckStage(IItem _item)
        {
            if (_item == null || _item.Root == null) { return; }
            if (stages == null || stages.Count <= currentStage) { return; }

            SetBool(attackTriggerParameter,false);
            oldSize = plantBody.localScale;
            newSize = plantBody.localScale;
            if (stages[currentStage].Response(_item.Root) == PlantResponse.grow)
            {
                
                currentStage++;
                if (stages.Count > currentStage)
                {
                    newSize = stages[currentStage].size;

                }

            }
            else if (stages[currentStage].Response(_item.Root) == PlantResponse.shrink)
            {
                currentStage--;
                if (currentStage >= 0)
                {
                    newSize = stages[currentStage].size;
                }
            }
            else if (stages[currentStage].Response(_item.Root) == PlantResponse.attack)
            {
                SetBool(attackTriggerParameter, true);
            }

            currentStage = Mathf.Clamp(currentStage,0,stages.Count );

            if (_linkedDevice != null)
            {
                if (currentStage == 0)
                { _linkedDevice.CanInteract = true; }
                else
                {
                    _linkedDevice.CanInteract = false;
                }
            }
        }

        public void CheckItemListForStage(PlantStage _stage)
        {
            //in the case where the items needed for a plant stage were not included in the allowed item list this will add them
            //to a secondary list the plant will check when eating a donut

            if (stageItems == null) { stageItems = new List<ItemObject>(); }
            if (stageItems.Contains(_stage.growObject) == false)
            { stageItems.Add(_stage.growObject); }
            if (stageItems.Contains(_stage.shrinkObject) == false)
            { stageItems.Add(_stage.shrinkObject); }
            if (stageItems.Contains(_stage.attackObject) == false)
            { stageItems.Add(_stage.attackObject); }

        }



        public bool HasKey()
        {
            if (Item) { return true; }
            return false;
        }

        #endregion

        #region Protected Methods

        protected override void RemoveItem()
        {
        
        }

        #endregion

        public void StartAnimating()
        {
            //called from a event on the  animator
            animating = true;
            timer = animationLength;
        }

        public void SetTrigger(string _ParamName)
        {
            if (animator == null) { animator = GetComponent<Animator>(); }
            if (animator == null) { return; }

            if (ContainsParam(_ParamName))
            { animator.SetTrigger(_ParamName); }

        }

        public void SetBool(string _ParamName,bool _value)
        {
            if (animator == null) { animator = GetComponent<Animator>(); }
            if (animator == null) { return; }

            if (ContainsParam(_ParamName))
            { animator.SetBool(_ParamName,_value); }

        }

        public bool ContainsParam(string _ParamName)
        {
            if (animator == null) { return false; }

            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name.ToLower() == _ParamName.ToLower()) return true;
            }
            return false;
        }

    }
}
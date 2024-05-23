using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem
{
    public class ExpressionHandler : MonoBehaviour
    {
        [SerializeField]
        private DialogueDisplayHandler _dHandler;

        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        private void Start()
        {
            // Get List of PresetData     
            EntityData presetdata = Resources.Load<EntityData>("EntityData");

            //Setup the event on Start Line to change/enable the name display
          
            _dHandler.onStartLine += (NodeData data) =>
            {            
                int expressionId = data.ExpressionId;

                if (data.PresetName != 0)
                {
                    _image.sprite = presetdata.data[data.PresetName - 1].Expressions.Emotions[expressionId].Image;
                    _image.enabled = true;
                }
                else
                {
                    _image.enabled = false;
                }
            };

            _dHandler.onEndDialogue += (e) =>
            {
                _image.enabled = false;
            };
        }
    }

}
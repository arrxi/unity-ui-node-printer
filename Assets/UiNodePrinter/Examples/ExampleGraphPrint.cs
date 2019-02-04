using UnityEngine;
using UnityEngine.UI;

namespace CleverCrow.UiNodeBuilder {
    public class ExampleGraphPrint : MonoBehaviour {
        private NodeGraph _graph;
        
        public NodeGraphPrinter printer;
        public NodeData root;
        public NodeContext context;
        
        [Header("Skill Points")]
        
        [SerializeField]
        private int _skillPoints = 2;
        public Text skillPointText;

        private void Start () {
            var graphBuilder = new NodeGraphBuilder();
            root.children.ForEach(child => NodeRecursiveAdd(graphBuilder, child));

            _graph = graphBuilder.Build();
            printer.Build(_graph);

            UpdateSkillPoints();
        }

        private void NodeRecursiveAdd (NodeGraphBuilder builder, NodeData data) {
            builder.Add(data.displayName, data.description, data.graphic)
                .IsPurchasable((node) => !node.Purchased && _skillPoints > 0)
                .OnPurchase((node) => {
                    _skillPoints -= 1;
                    UpdateSkillPoints();
                })
                .OnClickNode((node) => context.Open(node));
            data.children.ForEach(child => NodeRecursiveAdd(builder, child));
            builder.End();
        }

        private void UpdateSkillPoints () {
            skillPointText.text = $"Skill Points: {_skillPoints}";
            
            if (_skillPoints == 0) {
                _graph.Nodes.ForEach(n => n.Disable());
            }
        }
    }
}
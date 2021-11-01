using MonstersDataManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerControl {
    [Serializable]
    public class Skeletone : Tree<Bone> {
        Bone _root;
        public List<Bone> ListOfBones { get; private set; }
        public Skeletone(Bone root) : base(root) {
            _root = root;
            ListOfBones = new List<Bone>();
            ListOfBones.Add(_root);

            SetChilderen(Root);
           // DetachBones(Root);
        }

        
        private TreeNode<Bone> FindNode(TreeNode<Bone> node, Bone bone) {
            if (node.value == bone) return node;
            else
                for (int i = 0; i < node.Children.Count; i++) {
                    var n = FindNode(node.Children[i], bone);
                    if (n != null) return n;
                }
            return null;
        }

        private void SetChilderen(TreeNode<Bone> node) {
            var childeren = node.value.GetComponentsInChildren<Bone>();
            if (childeren is null || childeren.Length < 1) return;
            for (int i = 0; i < childeren.Length; i++) {
                if (childeren[i] == node.value) continue;
                if (childeren[i].transform.parent != node.value.transform) continue;
                node.value.length = Vector3.Distance(node.value.transform.position, childeren[i].transform.position);
                var newNode = node.AddChild(childeren[i]);
                ListOfBones.Add(childeren[i]);
                SetChilderen(newNode);
            }
        }
        
        private void DetachBones(TreeNode<Bone> node) {
            if (node.value.transform.parent is null) return;

            for (int i = 0; i < node.Children.Count; i++) {
                if (node.Children[i].value.transform.parent != _root.transform)
                    node.Children[i].value.transform.SetParent(_root.transform);
                DetachBones(node.Children[i]);
            }

        }
    }
}



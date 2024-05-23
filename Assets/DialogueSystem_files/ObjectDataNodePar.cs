using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DialogueSystem
{
    public struct ObjectDataNodePar
    {
        private GameObject obj;
        private NodeData data;

        public ObjectDataNodePar(GameObject obj, NodeData data)
        {
            this.obj = obj;
            this.data = data;
        }

        public NodeData Data { get => data; set => data = value; }
        public GameObject Obj { get => obj; set => obj = value; }
    }
}

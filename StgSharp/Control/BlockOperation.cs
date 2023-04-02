using System;
using System.Collections.Generic;
using System.Text;

namespace StgSharp.Control
{
    public abstract class BlockOperation : INode<BlockOperation>
    {
        
        private ContainerNode<BlockOperation> _id;


        ContainerNode<BlockOperation> INode<BlockOperation>.ID => _id;

        void INode<BlockOperation>.setID(ContainerNode<BlockOperation> id)
        {
            if (_id == default)
            {
                _id = id;
            }
            else
            {
                throw new InvalidOperationException("Cannot set ID twice!");
            }
        }


        public abstract void OnUpdating();

    }
}


using System;

public class DoubleCircularList
{
    private DoubleNode head;
    public DoubleCircularList()
    {
        this.head = null;
    }

    public bool IsEmpty()
    {
        return this.head == null;
    }

    public void Append(object data)
    {
        if(this.IsEmpty())
        {
            this.head = new DoubleNode(data);
        } else
        {
            DoubleNode node = this.head;
            while(node.getNext() != null)
            {
                node = node.getNext();
            }

            DoubleNode newNode = new DoubleNode(data);
            newNode.setPrev(node);

            node.setNext(newNode);
        }
    }

    public DoubleNode getHead()
    {
        return this.head;
    }
}

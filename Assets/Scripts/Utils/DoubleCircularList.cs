
public class DoubleCircularList
{
    private DoubleNode head, tail, pointer;
    private int length = 0, pos = 0;
    public DoubleCircularList()
    {
        this.head = null;
        this.pointer = null;
        this.tail = null;
    }

    public bool IsEmpty()
    {
        return this.head == null;
    }

    public void Append(object data)
    {
        if(this.IsEmpty())
        {
            DoubleNode newNode = new DoubleNode(data);
            newNode.setNext(newNode);
            newNode.setPrev(newNode);

            this.head = newNode;
            this.tail = newNode;
        } else
        {
            // Poniendolo de ultimo
            DoubleNode node = this.tail;
            DoubleNode newNode = new DoubleNode(data);
            newNode.setNext(this.head);
            node.setNext(newNode);
            newNode.setPrev(node);

            this.tail = newNode;
            this.head.setPrev(this.tail);
        }

        this.length++;
    }

    public void GoTo(int pos)
    {
        int _pos = pos % this.length;

        if(!this.IsEmpty())
        {
            int actual = 0;
            DoubleNode node = this.head;
            while(_pos != actual)
            {
                actual++;
                node = node.getNext();
            }

            this.pos = actual;
            this.pointer = node;
        }
    }

    public void PointHead()
    {
        this.pos = 0;
        this.pointer = this.head;
    }

    public void PointTail()
    {
        this.pos = this.length - 1;
        this.pointer = this.tail;
    }

    public void Next()
    {
        this.pointer = this.pointer.getNext();
        this.pos = (this.pos + 1) % this.length;
    }
    public void Prev()
    {
        this.pointer = this.pointer.getPrev();
        this.pos = this.pos - 1;
        if(this.pos < 0) { this.pos = this.length - 1; }
    }

    public DoubleNode getHead()
    {
        return this.head;
    }

    public DoubleNode getTail()
    {
        return this.tail;
    }

    public DoubleNode getPointer()
    {
        if(pointer == null) this.pointer = this.head;
        return this.pointer;
    }

    public int Length()
    {
        return this.length;
    }

    public int Pos()
    {
        return this.pos;
    }
}

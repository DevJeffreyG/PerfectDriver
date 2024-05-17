

public class DoubleNode
{
    private object data;
    private DoubleNode prev;
    private DoubleNode next;

    public DoubleNode(object data)
    {
        this.data = data;
    }

    public DoubleNode getPrev() { return prev; }
    public DoubleNode getNext() { return next; }
    public void setPrev(DoubleNode prev)
    {
        this.prev = prev;
    }

    public void setNext(DoubleNode next)
    {
        this.next = next;
    }


    public object getData() { return data; }
}

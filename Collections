using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collections
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }

    public class Node<T>
    {
        public Node(T data)
        {
            Data = data;
        }
        public T Data { get; set; }
        public Node<T> Next { get; set; }
    }

    abstract class Collect <T>: IEnumerable<T>,IEnumerable
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    class MyArray<T> : Collect<T>
    {
        int index;
        Node<T> node;
        int count;
        public MyArray()
        {
            count = 0;
            node = null;
        }

        //public MyArray(int size)
        //{

        //}

        public MyArray(T[] array)
        {

        }

    }

    class MyQueue<T> : Collect <T>,IEnumerable<T>
    {
        int count;
        Node<T> head; // головной/первый элемент
        Node<T> tail; // последний/хвостовой элемент

        public MyQueue()
        {
            count = 0;
            head = null;
            tail = null;
        }

        //public MyQueue(int size)
        //{

        //}

        public MyQueue(IEnumerable<T> collection)
        {
            foreach(var elem in collection)
            {
                Node<T> node = new Node<T>(elem);
                Node<T> tempNode = tail;
                tail = node;
                if (count == 0)
                    head = tail;
                else
                    tempNode.Next = tail;
                count++;
            }
        }
       
        // добавление в очередь
        public void Enqueue(T data)
        {
            Node<T> node = new Node<T>(data);
            Node<T> tempNode = tail;
            tail = node;
            if (count == 0)
                head = tail;
            else
                tempNode.Next = tail;
            count++;
        }
        // удаление из очереди
        public T Dequeue()
        {
            if (count == 0)
                throw new InvalidOperationException();
            T output = head.Data;
            head = head.Next;
            count--;
            return output;
        }
        // получаем первый элемент
        public T First
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException();
                return head.Data;
            }
        }
        // получаем последний элемент
        public T Last
        {
            get
            {
                if (IsEmpty)
                    throw new InvalidOperationException();
                return tail.Data;
            }
        }
        public int Count => count;
        public bool IsEmpty => count == 0;

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public bool Contains(T data)
        {
            Node<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(data))
                    return true;
                current = current.Next;
            }
            return false;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }
    }

    class MyStack<T> : Collect<T>,IEnumerable<T>
    {
        int count;
        Node<T> head;
        public MyStack()
        {
            count = 0;
            head = null;
        }

        //public MyStack(int capacity)
        //{
            
        //}

        public MyStack(IEnumerable<T> collection)
        {
            foreach(var elem in collection)
            {
                Node<T> node = new Node<T>(elem);
                node.Next = head;
                head = node;
                count++;
            }
        }

        public bool IsEmpty
        {
            get { return count == 0; }
        }

        public bool Contains()
        {
            return true;
            //Метод Contains() проверяет наличие элемента в стеке и возвращает true в случае нахождения его там.
        }

        public void Push(T item)
        {
            Node<T> node = new Node<T>(item);
            node.Next = head;
            head = node;
            count++;
        }


        public T Pop()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            Node<T> temp = head;
            head = head.Next;
            count--;
            return temp.Data;
        }

        public T Peek()
        {
            if (IsEmpty)
                throw new InvalidOperationException("Стек пуст");
            return head.Data;
        }

        public void Clear()
        {
            head = null;
            count = 0;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            Node<T> current = head;
            while (current != null)
            {
                yield return current.Data;
                current = current.Next;
            }
        }

    }
}

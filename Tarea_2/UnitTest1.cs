namespace Tarea_2
{
    public enum SortDirection
    {
        Ascending,
        Descending
    }

    //=============================================Definicion Nodos=============================================
    public class DoubleNode
    {
        public int Value { get; set; }
        public DoubleNode? Next { get; set; }
        public DoubleNode? Prev { get; set; }

        public DoubleNode(int value)
        {
            Value = value;
            Next = null;
            Prev = null;
        }
    }

    //=============================================Definicion Lista doblemente enlazada=============================================
    public class DoubleLinkedList
    {
        public DoubleNode? Head { get; set; }
        public DoubleNode? Tail { get; set; }
        private DoubleNode? Middle { get; set; } //para ejercicio 3
        private int Count { get; set; } //para ejercicio 3

        public DoubleLinkedList()
        {
            Head = null;
            Tail = null;
            Middle = null; //para ejercicio 3
            Count = 0; //para ejercicio 3
        }

        public void BasicInsert(int value)
        {
            DoubleNode newNode = new DoubleNode(value);
            if (Head == null)
            {
                Head = newNode;
            }
            else
            {
                DoubleNode current = Head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
                newNode.Prev = current;
            }
        }


        //=============================================PROBLEMA #1=============================================

        public void Insert(int value, SortDirection direction)
        {
            DoubleNode newNode = new DoubleNode(value);
            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
                Middle = newNode;
            }
            else if ((direction == SortDirection.Ascending && Head.Value >= value) ||
                     (direction == SortDirection.Descending && Head.Value <= value))
            {
                newNode.Next = Head;
                Head.Prev = newNode;
                Head = newNode;
            }
            else
            {
                DoubleNode current = Head;
                while (current.Next != null &&
                       ((direction == SortDirection.Ascending && current.Next.Value < value) ||
                        (direction == SortDirection.Descending && current.Next.Value > value)))
                {
                    current = current.Next;
                }
                newNode.Next = current.Next;
                if (current.Next != null)
                {
                    current.Next.Prev = newNode;
                }
                else
                {
                    Tail = newNode;
                }
                current.Next = newNode;
                newNode.Prev = current;
            }
        }

        public static void MergeSorted(DoubleLinkedList listA, DoubleLinkedList listB, SortDirection direction)
        {
            if (listA == null)
            {
                throw new ArgumentNullException(nameof(listA), "ListA must be non-null.");
            }

            if (listB == null)
            {
                throw new ArgumentNullException(nameof(listB), "ListB must be non-null.");
            }

            DoubleNode? currentA = listA.Head;
            DoubleNode? currentB = listB.Head;
            DoubleNode? mergedTail = null;

            // Inicializar la lista fusionada con la cabeza de listA
            listA.Head = null;
            listA.Tail = null;

            while (currentA != null && currentB != null)
            {
                DoubleNode? nextNode;
                if ((direction == SortDirection.Ascending && currentA.Value <= currentB.Value) ||
                    (direction == SortDirection.Descending && currentA.Value >= currentB.Value))
                {
                    nextNode = currentA;
                    currentA = currentA.Next;
                }
                else
                {
                    nextNode = currentB;
                    currentB = currentB.Next;
                }

                if (listA.Head == null)
                {
                    listA.Head = nextNode;
                    mergedTail = nextNode;
                }
                else
                {
                    if (mergedTail != null)
                    {
                        mergedTail.Next = nextNode;
                        if (nextNode != null)
                        {
                            nextNode.Prev = mergedTail;
                        }
                    }
                    mergedTail = nextNode;
                }
            }

            while (currentA != null)
            {
                if (listA.Head == null)
                {
                    listA.Head = currentA;
                    mergedTail = currentA;
                }
                else
                {
                    if (mergedTail != null)
                    {
                        mergedTail.Next = currentA;
                    }
                    currentA.Prev = mergedTail;
                    mergedTail = currentA;
                }
                currentA = currentA.Next;
            }

            while (currentB != null)
            {
                if (listA.Head == null)
                {
                    listA.Head = currentB;
                    mergedTail = currentB;
                }
                else
                {
                    if (mergedTail != null)
                    {
                        mergedTail.Next = currentB;
                    }
                    currentB.Prev = mergedTail;
                    mergedTail = currentB;
                }
                currentB = currentB.Next;
            }

            listA.Tail = mergedTail;
        }

        //=============================================PROBLEMA #2=============================================
        public void Invert(DoubleLinkedList? list)
        {
            if (list == null)
            {
                throw new ArgumentNullException(nameof(list), "List cannot be null.");
            }

            if (list.Head == null)
            {
                throw new InvalidOperationException("List is empty.");
            }

            DoubleNode? current = list.Head;
            DoubleNode? temp = null;

            while (current != null)
            {
                temp = current.Prev;
                current.Prev = current.Next;
                current.Next = temp;
                current = current.Prev;
            }

            if (temp != null)
            {
                list.Head = temp.Prev;
            }
        }

        //=============================================PROBLEMA #3=============================================
        public void InsertInOrder(int value)
        {
            DoubleNode newNode = new DoubleNode(value);

            if (Head == null)
            {
                Head = newNode;
                Tail = newNode;
                Middle = newNode;
            }
            else if (value <= Head.Value)
            {
                newNode.Next = Head;
                Head.Prev = newNode;
                Head = newNode;
            }
            else if (value >= Tail.Value)
            {
                newNode.Prev = Tail;
                Tail.Next = newNode;
                Tail = newNode;
            }
            else
            {
                DoubleNode? current = Head;
                while (current != null && current.Value < value)
                {
                    current = current.Next;
                }

                newNode.Next = current;
                newNode.Prev = current?.Prev;
                if (current?.Prev != null)
                {
                    current.Prev.Next = newNode;
                }
                if (current != null)
                {
                    current.Prev = newNode;
                }
            }

            Count++;
            UpdateMiddleOnInsert();
        }

        private void UpdateMiddleOnInsert()
        {
            if (Count == 1)
            {
                Middle = Head;
            }
            else if (Count % 2 == 0)
            {
                Middle = Middle?.Next;
            }
        }

        public int GetMiddle()
        {
            if (Middle == null)
            {
                throw new InvalidOperationException("List is empty.");
            }
            return Middle.Value;
        }

        public void Remove(DoubleNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }

            if (node == Head)
            {
                Head = Head?.Next;
                if (Head != null)
                {
                    Head.Prev = null;
                }
            }
            else if (node == Tail)
            {
                Tail = Tail?.Prev;
                if (Tail != null)
                {
                    Tail.Next = null;
                }
            }
            else
            {
                if (node.Prev != null)
                {
                    node.Prev.Next = node.Next;
                }
                if (node.Next != null)
                {
                    node.Next.Prev = node.Prev;
                }
            }

            Count--;
            UpdateMiddleOnRemove();
        }

        private void UpdateMiddleOnRemove()
        {
            if (Count == 0)
            {
                Middle = null;
            }
            else if (Count % 2 != 0)
            {
                Middle = Middle?.Prev;
            }
        }
    }

//=============================================PRUEBAS UNITARIAS=============================================
[TestClass]
public class UnitTest1
{
    
//=============================================PRUEBA PROBLEMA #1=============================================

    //=============================================Pruebas Excepciones=============================================
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestMergeSorted_NullListA()
    {
        DoubleLinkedList? listA = null;
        DoubleLinkedList listB = new DoubleLinkedList();
        listB.Insert(1, SortDirection.Ascending);

        DoubleLinkedList.MergeSorted(listA, listB, SortDirection.Ascending);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestMergeSorted_NullListB()
    {
        DoubleLinkedList listA = new DoubleLinkedList();
        listA.Insert(1, SortDirection.Ascending);
        DoubleLinkedList? listB = null;

        DoubleLinkedList.MergeSorted(listA, listB, SortDirection.Ascending);
    }

    [TestMethod]
    //=============================================Mezcla con lista ascendiente=============================================
        public void TesteMergeSortedAscending()
        {
        DoubleLinkedList listA = new DoubleLinkedList();
        listA.Insert(0, SortDirection.Ascending);
        listA.Insert(2, SortDirection.Ascending);
        listA.Insert(6, SortDirection.Ascending);
        listA.Insert(10, SortDirection.Ascending);
        listA.Insert(25, SortDirection.Ascending);

        DoubleLinkedList listB = new DoubleLinkedList();
        listB.Insert(3, SortDirection.Ascending);
        listB.Insert(7, SortDirection.Ascending);
        listB.Insert(11, SortDirection.Ascending);
        listB.Insert(40, SortDirection.Ascending);
        listB.Insert(50, SortDirection.Ascending);

        DoubleLinkedList.MergeSorted(listA, listB, SortDirection.Ascending);

        int[] expected = { 0, 2, 3, 6, 7, 10, 11, 25, 40, 50 };
        DoubleNode? current = listA.Head;
        foreach (int value in expected)
        {
            Assert.AreEqual(value, current?.Value);
            current = current?.Next;
        }
        }

    [TestMethod]
    //=============================================Mezcla con lista descendiente=============================================
        public void TesteMergeSortedDescending()
        {
        DoubleLinkedList listA = new DoubleLinkedList();
        listA.Insert(10, SortDirection.Descending);
        listA.Insert(15, SortDirection.Descending);

        DoubleLinkedList listB = new DoubleLinkedList();
        listB.Insert(9, SortDirection.Descending);
        listB.Insert(40, SortDirection.Descending);
        listB.Insert(50, SortDirection.Descending);

        DoubleLinkedList.MergeSorted(listA, listB, SortDirection.Descending);

        int[] expected = { 50, 40, 15, 10, 9 };
        DoubleNode? current = listA.Head;
        foreach (int value in expected)
        {
            Assert.AreEqual(value, current?.Value);
            current = current?.Next;
        }
        }
//=============================================PRUEBA PROBLEMA #2=============================================
        //=============================================Pruebas Excepciones=============================================
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestInvert_NullList()
        {
            DoubleLinkedList? list = null;
            DoubleLinkedList listtype = new DoubleLinkedList();
            listtype.Invert(list);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestInvert_EmptyList()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.Invert(list);
        }

        //=============================================Pruebas normales=============================================

        [TestMethod]
        public void TestInvert_MultipleElements()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.BasicInsert(1);
            list.BasicInsert(0);
            list.BasicInsert(30);
            list.BasicInsert(50);
            list.BasicInsert(2);

            list.Invert(list);

            int[] expected = {2,50,30,0,1};
            DoubleNode? current = list.Head;
            foreach (int value in expected)
            {
                Assert.AreEqual(value, current?.Value);
                current = current?.Next;
            }
        }


        [TestMethod]
        public void TestInvert_SingleElement()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.Insert(2, SortDirection.Ascending);

            list.Invert(list);

            int[] expected = { 2 };
            DoubleNode? current = list.Head;
            foreach (int value in expected)
            {
                Assert.AreEqual(value, current?.Value);
                current = current?.Next;
            }
        }

    //=============================================PRUEBA PROBLEMA #3=============================================
        //=============================================Pruebas Excepciones=============================================

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGetMiddle_NullList()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.GetMiddle();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGetMiddle_EmptyList()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.GetMiddle();
        }

        //=============================================Pruebas normales=============================================

        [TestMethod]
        public void TestInsertInOrder_SingleElement()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.InsertInOrder(1);

            Assert.AreEqual(1, list.GetMiddle());
        }

        [TestMethod]
        public void TestInsertInOrder_TwoElements()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.InsertInOrder(1);
            list.InsertInOrder(2);

            Assert.AreEqual(2, list.GetMiddle());
        }

        [TestMethod]
        public void TestInsertInOrder_ThreeElements()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.InsertInOrder(0);
            list.InsertInOrder(1);
            list.InsertInOrder(2);

            Assert.AreEqual(1, list.GetMiddle());
        }

        [TestMethod]
        public void TestInsertInOrder_FourElements()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.InsertInOrder(0);
            list.InsertInOrder(1);
            list.InsertInOrder(2);
            list.InsertInOrder(3);

            Assert.AreEqual(2, list.GetMiddle());
        }


        [TestMethod]
        public void TestInsertInOrder_And_GetMiddle_AfterRemovals()
        {
            DoubleLinkedList list = new DoubleLinkedList();
            list.InsertInOrder(0);
            list.InsertInOrder(1);
            list.InsertInOrder(2);

            DoubleNode? nodeToRemove = list.Head?.Next;
            if (nodeToRemove != null)
            {
                list.Remove(nodeToRemove);
            }

            Assert.AreEqual(1, list.GetMiddle());
        }

}
}
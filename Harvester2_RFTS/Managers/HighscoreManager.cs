using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Harvester
{
    /// <summary>
    /// This class takes care of creating the highscore, loading and saving
    /// </summary>
    public class HighscoreManager
    {
        //Attributes
        private Node rootNode;

        #region Set up the Singleton
        //Here is how this works

        //First make a static variable of type SpawnManager
        private static HighscoreManager _instance;
        

        //Then make a property so you can access it
        public static HighscoreManager Instance
        {
            get
            {
                //if it hasn't been initilized
                if (_instance == null)
                {
                    //Make a new one
                    _instance = new HighscoreManager();
                }

                //Otherwise return the instance of
                return _instance;
            }
        }
        
        /// <summary>
        /// Private contructor ensures nothing can make a new Asset Manager
        /// </summary>
        
        private HighscoreManager()
        {
            rootNode = null;
        }
        #endregion
        
        public List<string> GenerateScoreList()
        {
            return null;
        }

        private void Insert(int data, Node current)
        {
            if (data >= current.Data)
            {
                if (current.ChildRight != null)
                {
                    Insert(data, current.ChildRight);   
                }
                else
                 {
                current.ChildRight = new Node(data);
               }
            }

            if (data < current.Data)
            {
                if (current.ChildLeft != null)
                {

                    
                        Insert(data, current.ChildLeft);
                    
                }
                else
                {
                current.ChildLeft = new Node(data);
                }
            }
        }

        private void Print(Node current, int level)
        {
            for(int i = 0; i < level; i++)
            {
                Console.Write("|");
            }
            Console.WriteLine(current.Data.ToString());

            level++;
            if(current.ChildLeft !=null)
            {
                Print(current.ChildLeft, level);
            }

            if(current.ChildRight != null)
            {
                Print(current.ChildRight, level);
            }
        }

        public void Insert(int data)
        {
            if (rootNode == null)
            {
                rootNode = new Node(data);
            }
            else
            {
                Insert(data, rootNode);
            }
        }
        public void Print()
        {
            Print(rootNode, 0);
        }
    }
    public class Node
    {
        //Attributes
        private int data;

        private Node childLeft;
        private Node childRight;

        //Properties
        public int Data { get { return data; } set { data = value; } }

        public Node ChildLeft { get { return childLeft; } set { childLeft = value; } }
        public Node ChildRight { get { return childRight; } set { childRight = value; } }

        public Node(int data)
        {
            this.data = data;
        }
    }
}

// See https://aka.ms/new-console-template for more information

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace hikingpath
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //creating Lists
            //List<Node> nodes = new List<Node>();
            //List<Element> elements = new List<Element>();
            //List<Value> values = new List<Value>();
            List<Value> resultValues = new List<Value>(); //copy path into this file
            //check if arguments are correctly given
            //Map m = JsonConvert.DeserializeObject<Map>(File.ReadAllText(args[0]));
            //int n = int.Parse(args[1]);
            string filePathofMesh =
                "/Users/felixbotscher/Documents/Xibix/xibixcodingchallenge/xibixcodingchallenge/mesh_x_sin_cos_10000[82][1].json"; // won't be needed later
            int n = 50;
            //if(n < 1 || n > m.elements.Count) printResult(null);
            /*-----------------------Tests----------------------------
            //creating Objects and add them to the List
            Node node1 = new Node(0, 0, 0);
            Node node2 = new Node(1, 0, 1);
            Node node3 = new Node(2, 0, 2);
            int nodeNr1 = 0, nodeNr2 = 1, nodeNr3 = 2;
            List<int> intListe = new List<int>();
            intListe.Add(nodeNr1);
            intListe.Add(nodeNr2);
            intListe.Add(nodeNr3);
            nodes.Add(node1);
            nodes.Add(node2);
            nodes.Add(node3);
            Element element1 = new Element(0, intListe);
            elements.Add(element1);
            Value value1 = new Value(0, 2.4);
            values.Add(value1);
            Console.WriteLine(node1);
            Console.WriteLine(element1);
            Console.WriteLine(value1);
            // works great as expected 
            //adding Lists to the Map Object
            Map map = new Map(nodes, elements, values);
            string filePath =
                "/Users/felixbotscher/Documents/Xibix/xibixcodingchallenge/xibixcodingchallenge/test.json";

            //test serialization of List Objects first
            string convert = JsonConvert.SerializeObject(nodes, Formatting.Indented);
            Console.WriteLine(convert);
            convert = JsonConvert.SerializeObject(element1, Formatting.Indented);
            Console.WriteLine(convert);
            convert = JsonConvert.SerializeObject(value1, Formatting.Indented);
            Console.WriteLine(convert);
            convert = JsonConvert.SerializeObject(map, Formatting.Indented);
            Console.WriteLine(convert);
            //string convert = JsonConvert.SerializeObject(map/values, Formatting.Indented);
            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, map);
            }

            Console.WriteLine("-----------------------------------");
            // read file into a string and deserialize JSON to a type
            Map testMap = JsonConvert.DeserializeObject<Map>(File.ReadAllText(filePath));
            Console.WriteLine(testMap); // funktioniert
            */
            //----------------------Program--------------------------------------
            Map meshMap = JsonConvert.DeserializeObject<Map>(File.ReadAllText(filePathofMesh));
            Value highestPoint = findHighestPoint(meshMap.values);
            //Console.WriteLine("Der höchste Punkt auf der Karte ist auf dem Element mit der ID {0} und dem Wert {1}.", highestPoint.element_id, highestPoint.value);
            resultValues.Add(highestPoint);
            //printResult(resultValues);
            Element elementStart = meshMap.elements.Find(x => x.id == highestPoint.element_id);
            List<Element> neighbours = new List<Element>();
            List<Value> valuesOfNeighbours = new List<Value>();
            List<Value> sortedNeighbours = new List<Value>();
            for (int y = 0; y < n; y++)
            {
                //find the neighbours
                neighbours = findNeighbours(meshMap.nodes, elementStart, meshMap.elements);
                //change List<Elements> to List<Values> because we need to compare the value of Value
                valuesOfNeighbours = returnValue(neighbours, meshMap.values); //alternative: meshMap.values.Find(x => x.element_id == i.id)
                /*
                Console.WriteLine("Values of neighbours:");
                foreach (var i in valuesOfNeighbours)
                {
                    Console.WriteLine(i.ToString());
                }
                */
                //loop proves if we have visited the element already
                foreach (var ivalue in valuesOfNeighbours.ToList())
                {
                    if (resultValues.Contains(ivalue))
                    {
                        var itemToRemove = valuesOfNeighbours.Single(r => r.element_id == ivalue.element_id);
                        valuesOfNeighbours.Remove(itemToRemove);
                    }
                }
                //check for null if we are in a corner 
                if (!valuesOfNeighbours.Any())
                {
                    break;
                }
                sortedNeighbours = valuesOfNeighbours.OrderByDescending(val=>val.value).ToList();
                /*
                Console.WriteLine("Sorted neighbours:");
                foreach (var i in sortedNeighbours)
                {
                    Console.WriteLine(i.ToString());
                }
                */
                resultValues.Add(sortedNeighbours.First());
                //Console.WriteLine("\nOldElement:" + sortedNeighbours.First());
            }
            printResult(resultValues);
        }
        
        /**
         * function that returns the highest point of the map by using th max and first function on lists
         */
        public static Value findHighestPoint(List<Value> values)
        {
            double greatesVal = values.Max(val => val.value);
            return values.First(x => x.value == greatesVal);
        }

        /**
         * function that finds the three neighbours of a element 
         */
        public static List<Element> findNeighbours(List<Node> nodes, Element elementStart, List<Element> elements)
        {
            var nodeId1 = elementStart.nodes[0]; // first Value of List in Element of elementStart
            var nodeId2 = elementStart.nodes[1]; // second Value of List in Element of elementStart
            var nodeId3 = elementStart.nodes[2]; // third Value of List in Element of elementStart
            List<Node> elementNodesList =
                nodes.FindAll(lambda => lambda.id == nodeId1 || lambda.id == nodeId2 || lambda.id == nodeId3); //retrieve a List of Nodes of the elementStart
            List<Element> oneElements = containsID(nodeId1, elements);
            List<Element> oneElementsIntersectionTwo = containsID(nodeId2, oneElements);
            List<Element> twoElements = containsID(nodeId2, elements);
            List<Element> twoElementsIntersectionThree = containsID(nodeId3, twoElements);
            List<Element> threeElements = containsID(nodeId3, elements);
            List<Element> threeElementsIntersectionOne = containsID(nodeId1, threeElements);

            List<Element> result = oneElementsIntersectionTwo.Concat(twoElementsIntersectionThree)
                .Concat(threeElementsIntersectionOne).ToList();
            result = result.Where(x => x.id != elementStart.id).ToList();
            return result;
        }
        
        /**
         * Betrachtet eine Liste von Elementen und prüft ob die gegebene nodeId gleich einer von den NodeIds des Elementes ist
         * Rückgabeparameter ist dann eine Liste an Elementen vom Typ Element
         */
        public static List<Element> containsID(int nodeId, List<Element> elements)
        {
            return elements.FindAll(elem => elem.nodes[0] == nodeId || elem.nodes[1] == nodeId || elem.nodes[2] == nodeId);
        }
        /**
         * returnValue finds all value elements which have the same id as in the elements list
         */
        public static List<Value> returnValue(List<Element> elements, List<Value> values)
        {
            return values.FindAll(lambda => lambda.element_id == elements[0].id || lambda.element_id ==elements[1].id || lambda.element_id == elements[2].id);
        }
        /**
         * returns the highest neighbour Value element of the given list
         */
        public static void printResult(List<Value> values)
        {
            Console.WriteLine("[");
            foreach (var value in values)
            {
                Console.WriteLine("  {" + value + "},");
            }
            Console.Write("]");
        }
    }
}
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
            List<Value> resultValues = new List<Value>(); //add peaks of the path into this file
            //check if arguments are correctly given
            //Map m = JsonConvert.DeserializeObject<Map>(File.ReadAllText(args[0]));
            //int n = int.Parse(args[1]);
            string filePathofMesh = //won't be needed later
                "/Users/felixbotscher/Documents/Xibix/xibixcodingchallenge/xibixcodingchallenge/mesh_x_sin_cos_20000[1].json"; 
                //"/Users/felixbotscher/Documents/Xibix/xibixcodingchallenge/xibixcodingchallenge/mesh_x_sin_cos_10000[82][1].json";
                //"/Users/felixbotscher/Documents/Xibix/xibixcodingchallenge/xibixcodingchallenge/mesh[1].json"; 
            int n = 20;
            //if(n < 1 || n > m.elements.Count) printResult(null);
            /* ---------Code for serialization-------------------------------------
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
            
            Value highestPoint = FindHighestPoint(meshMap.values);
            //Console.WriteLine("Der höchste Punkt auf der Karte ist auf dem Element mit der ID {0} und dem Wert {1}.", highestPoint.element_id, highestPoint.value);
            resultValues.Add(highestPoint);
            //printResult(resultValues);
            Element elementStart = meshMap.elements.Find(x => x.id == highestPoint.element_id);
            Value oldValue = highestPoint;
            List<Element> neighbours = new List<Element>();
            List<Value> valuesOfNeighbours = new List<Value>();
            List<Value> sortedNeighbours = new List<Value>();
            for (var y = 2; y<=n ; y++)
            {
                //set new elementStart from the Value before
                oldValue = resultValues.Last();
                elementStart = meshMap.elements.Find(x => x.id == oldValue.element_id);
                //find the neighbours
                neighbours = FindNeighbours(meshMap.nodes, elementStart, meshMap.elements);
                //change List<Elements> to List<Values> because we need to compare the value of Value
                valuesOfNeighbours = ReturnValue(neighbours, meshMap.values); //alternative: meshMap.values.Find(x => x.element_id == i.id)
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
                resultValues.Add(sortedNeighbours.First());
            }
            PrintResult(resultValues);
        }
        
        /**
         * function that returns the highest point of the map by using th max and first function on lists
         */
        private static Value FindHighestPoint(List<Value> values)
        {
            double greatesVal = values.Max(val => val.value);
            return values.First(x => x.value == greatesVal);
        }

        /**
         * function that finds the three neighbours of a element 
         */
        private static List<Element> FindNeighbours(List<Node> nodes, Element elementStart, List<Element> elements)
        {
            var nodeId1 = elementStart.nodes[0]; // first Value of List in Element of elementStart
            var nodeId2 = elementStart.nodes[1]; // second Value of List in Element of elementStart
            var nodeId3 = elementStart.nodes[2]; // third Value of List in Element of elementStart
            List<Node> elementNodesList =
                nodes.FindAll(lambda => lambda.id == nodeId1 || lambda.id == nodeId2 || lambda.id == nodeId3); //retrieve a List of Nodes of the elementStart
            List<Element> oneElements = ContainsID(nodeId1, elements);
            List<Element> oneElementsIntersectionTwo = ContainsID(nodeId2, oneElements);
            List<Element> twoElements = ContainsID(nodeId2, elements);
            List<Element> twoElementsIntersectionThree = ContainsID(nodeId3, twoElements);
            List<Element> threeElements = ContainsID(nodeId3, elements);
            List<Element> threeElementsIntersectionOne = ContainsID(nodeId1, threeElements);

            List<Element> result = oneElementsIntersectionTwo.Concat(twoElementsIntersectionThree)
                .Concat(threeElementsIntersectionOne).ToList();
            result = result.Where(x => x.id != elementStart.id).ToList();
            return result;
        }
        
        /**
         * Betrachtet eine Liste von Elementen und prüft ob die gegebene nodeId gleich einer von den NodeIds des Elementes ist
         * Rückgabeparameter ist dann eine Liste an Elementen vom Typ Element
         */
        private static List<Element> ContainsID(int nodeId, List<Element> elements)
        {
            return elements.FindAll(elem => elem.nodes[0] == nodeId || elem.nodes[1] == nodeId || elem.nodes[2] == nodeId);
        }
        /**
         * returnValue finds all value elements which have the same id as in the elements list
         */
        private static List<Value> ReturnValue(List<Element> elements, List<Value> values)
        {
            List<Value> result = new List<Value>();
            foreach (var element in elements)
            {
                result.Add(values.First(x => x.element_id == element.id));
            }
            return result;
            //return values.FindAll(lambda => lambda.element_id == elements[0].id || lambda.element_id ==elements[1].id || lambda.element_id == elements[2].id);
        }
        /**
         * returns the highest neighbour Value element of the given list
         */
        private static void PrintResult(List<Value> values)
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
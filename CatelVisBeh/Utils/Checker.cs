
namespace CatelVisBeh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    public static class Checker
    {
        /// <summary>
        /// Checks if string parameter is not null,empty or whitespace throws ArgumentException.
        /// </summary>
        /// <param name="value"></param>
        public static void Required(string value)
        {
            if (!IsOkay(value))
                throw new ArgumentException();
        }

        /// <summary>
        /// Checks if string parameter is not null,empty or whitespace.
        /// </summary>
        /// <param name="value"></param>
        /// <returns> If not valid value then false else true.</returns>
        public static bool IsOkay(string value)
        {
            return (!String.IsNullOrEmpty(value) && !String.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// Generic function that takes only parameters that implement IComparable interface. Function does compare according to option 
        /// {0}: open interval, default option
        /// {1}: closed inteval from left side
        /// {2}: closed inteval from right side
        /// {3}: closed inteval from both side
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testedItem"></param>
        /// <param name="beginInterval"></param>
        /// <param name="endInterval"></param>
        /// <param name="optionCompare"></param>
        /// <returns></returns>
        public static bool IsValueInInterval<T>(T testedItem, T beginInterval, T endInterval, int optionCompare = 0) where T : IComparable
        {
            if (optionCompare == 1) return (beginInterval.CompareTo(testedItem) <= 0) && (endInterval.CompareTo(testedItem) > 0);
            else if (optionCompare == 2) return (beginInterval.CompareTo(testedItem) < 0) && (endInterval.CompareTo(testedItem) >= 0);
            else if (optionCompare == 3) return (beginInterval.CompareTo(testedItem) <= 0) && (endInterval.CompareTo(testedItem) >= 0);

            return (beginInterval.CompareTo(testedItem) < 0) && (endInterval.CompareTo(testedItem) > 0);
        }

        /// <summary>
        /// Finds for possible match of params given among the collection of entries.
        /// Compares (collocation) from the begining step by step.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"> Entries.</param>
        /// <param name="fromFirst">if true fromFirst, then if match found the string from first parameter(collection) will be added, otherwise from second param .</param>
        /// <param name="toFind"> Params that should be match with some entries</param>
        /// <returns>Collection of strings that matches (whole string from toFind array).</returns>
        public static ICollection<string> PossibleMatchStrict(ICollection<string> collection, bool fromFirst, params string[] toFind)
        {
            ICollection<string> x = null;
            if (collection.Count > 0 && toFind.Length > 0)
                foreach (var paramsItem in toFind)
                    foreach (var collectionItem in collection)
                        if (FoundMatch(paramsItem, collectionItem))
                        {
                            if (x == null) x = new List<string>();
                            if (fromFirst)
                            {
                                if (!x.Contains(collectionItem))
                                    x.Add(collectionItem);
                            }
                            else
                            {
                                if (!x.Contains(paramsItem))
                                    x.Add(paramsItem);
                            }
                        }
            return x;
        }

        /// <summary>
        /// Finds for possible match of params given among the collection of entries.
        /// Compares (collocation) from the begining step by step.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"> Entries.</param>
        /// <param name="toFind"> Params that should be match with some entries</param>
        /// <param name="longer">Fills collection if true with the longer, otherwise with shorter.</param>
        /// <returns>Collection of strings that matches ().</returns>
        public static ICollection<string> PossibleMatchFlexible(ICollection<string> collection, bool longer, params string[] toFind)
        {
            ICollection<string> x = null;
            if (collection.Count > 0 && toFind.Length > 0)
                foreach (var paramsItem in toFind)
                    foreach (var collectionItem in collection)
                            if (FoundMatch(collectionItem, paramsItem, out bool longStr))
                            {
                                if (x == null) x = new List<string>();
                                if (longer && longStr || !longer && !longStr)
                                {
                                    if (!x.Contains(collectionItem))
                                        x.Add(collectionItem);
                                }
                                if (!longer && longStr || longer && !longStr)
                                {
                                    if (!x.Contains(paramsItem))
                                        x.Add(paramsItem);
                                }
                            }
            return x;
        }

        /// <summary>
        /// Finds for possible match of params given among the collection of entries. Compares first param with second array of params.
        /// Use if the param in toFind array contains shorter strings as toCompare param.
        /// </summary>
        /// <param name="collection"> Entries.</param>
        /// <param name="toFind"> Params that should be match with some entries</param>
        /// <returns>Null if no match, otherwise returns string from params array toFind.</returns>
        public static string PossibleMatch(string toCompare, params string[] toFind)
        {
            if (IsOkay(toCompare) && toFind != null && toFind.Length > 0)
                foreach (var paramsItem in toFind)
                    if (FoundMatch(paramsItem, toCompare))
                        return paramsItem;
            return null;
        }

        /// <summary>
        /// Try to split given string and compare their substrings acc. to spliter. 
        /// It omit all white spaces in given parameters. In first not null(white Space)  comparison returns true or false. 
        /// Aims to compare all words from both sources if possible. The result directly depends on the shorter combination of string.
        /// </summary>
        /// <param name="paramsItem"></param>
        /// <param name="collectionItem"></param>
        /// <returns>Even if only one (major first) substring match return true, otherwise false</returns>
        private static bool FoundMatch(string paramsItem, string collectionItem)
        {
            paramsItem = paramsItem.ToLower().Trim();
            collectionItem = collectionItem.ToLower().Trim();

            string[] splitParam = paramsItem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] splitColl = collectionItem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var indexer = splitParam.Length <= splitColl.Length ? splitParam.Length : splitColl.Length;

            if (IsOkay(paramsItem) && IsOkay(collectionItem))
            {
                for (int i = 0; i < indexer; i++)
                    if (!splitParam[i].Equals(splitColl[i]))
                        return false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Finds for possible match of params given among the collection of entries.
        /// </summary>
        /// <param name="collection"> Entries.</param>
        /// <param name="FindingOrComparing"> If set as true and was found any match it will return the longer string.</param>
        /// <param name="toFind"> Params that should be match with some entries</param>
        public static string PossibleMatch(string toCompare, bool FindingOrComparing = false, params string[] toFind)
        {
            if (IsOkay(toCompare) && toFind != null && toFind.Length > 0)
                foreach (var paramsItem in toFind)
                    if (FoundMatch(paramsItem, toCompare, out bool longer))
                    {
                        if (FindingOrComparing && longer || !FindingOrComparing && !longer)
                            return paramsItem;
                        if (!FindingOrComparing && longer || FindingOrComparing && !longer)
                            return toCompare;
                    }
            return null;
        }

        /// <summary>
        /// Try to split given string and compare their substrings acc. to spliter. 
        /// It omit all white spaces in given parameters. In first not null(white Space)  comparison returns true or false. 
        /// Aims to compare all words from both sources if possible. The result directly depends on the shorter combination of string.
        /// </summary>
        /// <param name="paramsItem"></param>
        /// <param name="collectionItem"></param>
        /// <param name="longer">If found match , to get info if first param was longer as second string parameter. Longer:  {True} = first param is longer or equal; 
        /// {False} = second is longer. </param>
        /// <returns>Even if only one (major first) substring match return true, otherwise false</returns>
        private static bool FoundMatch(string paramsItem, string collectionItem, out bool longer)
        {
            longer = false;

            paramsItem = paramsItem.ToLower().Trim();
            collectionItem = collectionItem.ToLower().Trim();

            string[] splitParam = paramsItem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string[] splitColl = collectionItem.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            var indexer = splitParam.Length <= splitColl.Length ? splitParam.Length : splitColl.Length;

            if (IsOkay(paramsItem) && IsOkay(collectionItem))
            {
                for (int i = 0; i < indexer; i++)
                    if (!splitParam[i].Equals(splitColl[i]))
                        return false;
                longer = splitParam.Length >= splitColl.Length ? true : false;
                return true;
            }
            return false;
        }

        /// <summary>
        /// It takes anonymous action to execute so many times as reps indicate. And makes summary of action.
        /// For the first we warm up our code because first creatation is always very slow.
        /// </summary>
        /// <param name="what"> What is our goal : description</param>
        /// <param name="reps"> How many times we want to repeate the action to get the precise summary.</param>
        /// <param name="action">Ation to be executed to measure what time it takes.</param>
        public static void Measure(string what, int reps, Action action)
        {
            action(); // warm up

            double[] results = new double[reps];
            for (int i = 0; i < reps; i++)
            {
                Stopwatch watch = Stopwatch.StartNew();
                action();
                results[i] = watch.Elapsed.TotalMilliseconds;
            }
            Console.WriteLine($"{what} - AVG = {results.Average()} Min = {results.Min()} Max = {results.Max()}");
        }
    }
}

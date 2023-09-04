/* NameSpaces */
using System;
using System.Collections.Generic;
/* NameSpaces */
namespace FlamesBotV2
{
    /* CUSTOM TOOLS THAT HELPED ME */
    public class Tools
    {
        /* Array Sortrer */
        public static void ArraySorter()
        {
            Array.Sort(Vars.legendary_fish);    // Load The Array (To Sort It Alphabetically)

            string formattedFishList = string.Join("\",\n\"", Vars.legendary_fish);     // Load The Array (To Place Quotes And Commas)

            string finalFormattedList = $"\"{formattedFishList}\"";

            Console.WriteLine(finalFormattedList);

            int fishCount = Vars.legendary_fish.Length;             // Load The Array (To Count The Items)
            Console.WriteLine($"Number of items: {fishCount}");
        }
        /* Array Sortrer */

        /* Single Array Duplicate Finder */
        public static void SingleArrayDuplicateFinder()
        {
            List<string> duplicates = FindDuplicates(Vars.legendary_fish);   // Load The Array That You Want To Check For Duplicates

            if (duplicates.Count > 0)
            {
                Console.WriteLine("Duplicate items found:");
                foreach (string duplicate in duplicates)
                {
                    Console.WriteLine(duplicate);
                }
            }
            else
            {
                Console.WriteLine("No duplicate items found.");
            }

            List<string> FindDuplicates(string[] array)
            {
                var seen = new HashSet<string>();
                var fishduplicates = new List<string>();

                foreach (string item in array)
                {
                    if (!seen.Add(item))
                    {
                        fishduplicates.Add(item);
                    }
                }

                return fishduplicates;
            }
        }
        /* Single Array Duplicate Finder */

        /* Multiple Array Duplicate Finder */
        public static void MultipleArrayDuplicateDFinder()
        {
            /* Load The Arrays That You Want To Check For Duplicates */
            Dictionary<string, List<string>> fishArrays = new Dictionary<string, List<string>>
            {
            { "common_fish", new List<string>(Vars.common_fish) },
            { "uncommon_fish", new List<string>(Vars.uncommon_fish) },
            { "rare_fish", new List<string>(Vars.rare_fish) },
            { "epic_fish", new List<string>(Vars.epic_fish) },
            { "legendary_fish", new List<string>(Vars.legendary_fish) },
            { "mythical_fish", new List<string>(Vars.mythical_fish) }
            };
            /* Load The Arrays That You Want To Check For Duplicates */
            Dictionary<string, List<string>> fishInArrays = new Dictionary<string, List<string>>();
            bool duplicatesFound = false;

            void CheckAndPrintDuplicates(string[] fishArray, string arrayName)
            {
                foreach (string fish in fishArray)
                {
                    if (!fishInArrays.ContainsKey(fish))
                    {
                        fishInArrays[fish] = new List<string> { arrayName };
                    }
                    else
                    {
                        fishInArrays[fish].Add(arrayName);
                        duplicatesFound = true;
                    }
                }
            }

            /* Load The Arrays That You Want To Check For Duplicates */
            CheckAndPrintDuplicates(Vars.common_fish, "common_fish");
            CheckAndPrintDuplicates(Vars.uncommon_fish, "uncommon_fish");
            CheckAndPrintDuplicates(Vars.rare_fish, "rare_fish");
            CheckAndPrintDuplicates(Vars.epic_fish, "epic_fish");
            CheckAndPrintDuplicates(Vars.legendary_fish, "legendary_fish");
            CheckAndPrintDuplicates(Vars.mythical_fish, "mythical_fish");
            /* Load The Arrays That You Want To Check For Duplicates */

            foreach (var kvp in fishInArrays)
            {
                if (kvp.Value.Count > 1)
                {
                    Console.WriteLine($"[Duplicate] item: {kvp.Key} ({string.Join(", ", kvp.Value)})");
                }
            }

            if (!duplicatesFound)
            {
                Console.WriteLine("No duplicates found in the arrays.");
            }
        }
        /* Multiple Array Duplicate Finder */
    }
}

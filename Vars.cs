namespace FlamesBotV2
{
    public class Vars
    {
        /* Positions */
        public static string the_docs = "**[+]** Isn't open-field\n**[+]** There are many resources\n**[+]** There is lot of storage\n\n**[-]** Is crowded\n**[-]** It stands out";
        
        public static string the_suburbs = "**[+]** Doesn't stands out\n**[+]** Can be protected fairly easy\n**[+]** Is Surrounded by forest\n\n**[-]** Is open-field\n**[-]** Easy to raid";

        public static string the_underground = "**[+]** Is Hidden\n**[+]** Is maze-like\n**[+]** Can be used to travel across the city\n\n**[-]** Is Dark\n**[-]** Hard to defend everything";

        public static string west_side_factories = "**[+]** Can be used to craft west components\n**[+]** Rich on armour\n**[+]** Have a lot of towers and buildings\n\n**[-]** Needs electricity to power the machines\n**[-]** Movement can be spotted easily";

        public static string south_side_factories = "**[+]** Can be used to craft south components\n**[+]** Rich on ammo and weapons\n**[+]** Hard to raid\n\n**[-]** Needs coal to power the machines\n**[-]** Easy to spy on";
        /* Positions */



        /* Armory */
        public static string CD = "\nDamage: 25\nDurability: 600\nReload Speed: 0.5s\n\n**Requirements**\nReceiver: 1\nBarrel: 1\nScope: 1\nMagazine: 1";       // Hawk Eye Text

        public static string CP = "\nDamage: 15\nDurability: 400\nReload speed: 1s\n\n**Requirements**\nFrame: 1\nSlide: 1\nBarrel: 1\nMagazine: 1";            // Glock 17

        public static string CS = "\nDamage: 40\nDurability: 800\nReload speed: 2s\n\n**Requirements**\nReceiver: 1\nStock: 1\nBarrel: 1\nMagazine Tube: 1";    // SPAS-12

        public static string CSN = "\nDamage: 60\nDurability: 1000\nReload speed: 3s\n\n**Requirements**\nReceiver: 1\nBarrel: 1\nScope: 1\nBolt: 1";           // M40A5

        public static string CR = "\nDamage: 35\nDurability: 800\nReload speed: 1.5s\n\n**Requirements**\nReceiver: 1\nStock: 1\nBarrel: 1\nMagazine: 1";       // AK-47
        /* Armory */



        /* Melee Weapons Array */
        public static string[] melee_weapons = { "Scythe", "Katana", "Machete", "Combat Axe" };
        /* Melee Weapons Array */



        /* Riot Shield Perk Components */
        public static string[] riot_shield_components = { "Titanium Plates", "Iron Spikes", "Camouflage Component" };
        /* Riot Shield Perk Components */



        /* 8ball Responses Array */
        public static string[] eight_ball_responses = new string[]
        {
            "Not at all.",
            "My reply is no.",
            "My sources say no.",
            "Absolutely no.",
            "Very doubtful.",
            "As I see it, no.",
            "Very unlikely.",
            "It is not certain.",
            "I dont't know.",
            "Ask again later.",
            "Better not tell you now.",
            "Cannot predict now.",
            "It is certain.",
            "It is decidedly so.",
            "Without a doubt.",
            "Yes, definitely.",
            "You may rely on it.",
            "As I see it, yes.",
            "Most likely.",
            "Absolutely yes."
        };
        /* 8ball Responses Array */



        /* Common Fish Array */
        public static string[] common_fish = new string[]
        {
            "Anchovy",
            "Angelfish",
            "Atlantic Cod",
            "Barb",
            "Barfish",
            "Barracuda",
            "Barreleye",
            "Basslet",
            "Bat Ray",
            "Batfish",
            "Beardfish",
            "Beluga Sturgeon",
            "Betta",
            "Bichir",
            "Bitterling",
            "Black Bass",
            "Black Skirt Tetra",
            "Carp",
            "Catfish",
            "Clownfish",
            "Cod",
            "Coelacanth",
            "Corydoras Catfish",
            "Crappie",
            "Cusk",
            "Darter",
            "Dory",
            "Eel",
            "Goldfish",
            "Grouper",
            "Guppy",
            "Haddock",
            "Herring",
            "Lemon Tetra",
            "Lionfish",
            "Mackerel",
            "Marlin",
            "Molly",
            "Perch",
            "Pipefish",
            "Platy",
            "Pollock",
            "Pufferfish",
            "Rainbow Trout",
            "Rainbowfish",
            "Rasbora",
            "Rosy Barb",
            "Salmon",
            "Snapper",
            "Sole",
            "Stonefish",
            "Sunbleak",
            "Sunfish",
            "Swordfish",
            "Swordtail",
            "Tetra",
            "Trout",
            "Tuna",
            "Tiger Barb",
            "Bala Shark"
        };
        /* Common Fish Array */



        /* Uncommon Fish Array */
        public static string[] uncommon_fish = new string[]
        {
            "Anglerfish",
            "Axolotl",
            "Blobfish",
            "Boxfish",
            "Cookiecutter Shark",
            "Diamond Tetra",
            "Dragonfish",
            "Electric Eel",
            "Fangtooth",
            "Flying Gurnard",
            "Glass Catfish",
            "Goblin Shark",
            "Hagfish",
            "Hammerhead Shark",
            "Icefish",
            "Jackknife Fish",
            "Kissing Gourami",
            "Lanternfish",
            "Mantis Shrimp",
            "Mola Mola",
            "Moonfish",
            "Narwhal",
            "Oarfish",
            "Panda Garra",
            "Parrotfish",
            "Pineapplefish",
            "Razorfish",
            "Sawfish",
            "Scorpionfish",
            "Siamese Algae Eater",
            "Silver Dollar",
            "Threadfin Rainbowfish",
            "Three-spot Gourami",
            "Triggerfish",
            "Unicorn Fish",
            "Vampire Squid",
            "Wobbegong",
            "X-ray Tetra",
            "Yellow Tang",
            "Zebra Shark"
        };
        /* Uncommon Fish Array */



        /* Rare Fish Array */
        public static string[] rare_fish = new string[]
        {
            "Albino Bichir",
            "Amazon Puffer",
            "Black Neon Tetra",
            "Bloodfin Tetra",
            "Blue Tetra",
            "Bristlenose Pleco",
            "Celestial Pearl Danio",
            "Denison Barb",
            "Discus",
            "Dwarf Gourami",
            "Electric Blue Acara",
            "Electric Blue Jack Dempsey",
            "Emperor Tetra",
            "Fahaka Puffer",
            "Firemouth Cichlid",
            "Flowerhorn Cichlid",
            "Giant Freshwater Stingray",
            "Golden Mahseer",
            "Green Terror",
            "Green Tiger Barb",
            "Hillstream Loach",
            "Kamfa Flowerhorn",
            "Koi",
            "Lake Kutubu Rainbowfish",
            "Mandarinfish",
            "Mbu Puffer",
            "Mekong Giant Catfish",
            "Pearl Gourami",
            "Pearsei Cichlid",
            "Pirarucu",
            "Royal Gramma",
            "Sailfin Tang",
            "Shovelnose Catfish",
            "Synodontis Angelicus",
            "Threadfin Butterflyfish",
            "Tiger Shovelnose Catfish",
            "Upside-Down Catfish",
            "Violet Goby",
            "Wolf Cichlid",
            "Zebra Pleco"
        };
        /* Rare Fish Array */



        /* Epic Fish Array */
        public static string[] epic_fish = new string[]
        {
            "Asian Glass Catfish",
            "Asian Leaffish",
            "Banded Leporinus",
            "Black Phantom Tetra",
            "Black Widow Tetra",
            "Blood Parrot Cichlid",
            "Blue Acara",
            "Chocolate Cichlid",
            "Clown Knifefish",
            "Congo Tetra",
            "Convict Cichlid",
            "Green Spotted Puffer",
            "Jardini Arowana",
            "Kuhli Loach",
            "Oscar Cichlid",
            "Phoenix",
            "Rainbow Cichlid",
            "Red Rainbowfish",
            "Tropheus Cichlid",
            "Zebra Danio"
        };
        /* Epic Fish Array */



        /* Legendary Fish Array */
        public static string[] legendary_fish = new string[]
        {
            "African Butterflyfish",
            "Blue Diamond Discus",
            "Dovii Cichlid",
            "Empress Cichlid",
            "Frontosa Cichlid",
            "Jaguar Cichlid",
            "Red Tail Barracuda",
            "Royal Farlowella Catfish",
            "Red Phantom Tetra",
            "Glass Bloodfin Tetra",
            "Lancer Catfish",
            "Giant Snakehead",
            "Arowana Loach",
            "Asian Redtail Catfish",
            "Silver Arowana",
            "Golden Nugget Pleco",
            "Vampire Tetra",
            "Black Diamond Stingray",
            "Redtail Catfish",
            "Golden Dorado"
        };
        /* Legendary Fish Array */



        /* Mythical Fish Array */
        public static string[] mythical_fish = new string[]
        {
            "Siren's Melodyfish",
            "Phoenixfin",
            "Kraken's Kin",
            "Leviathan Serpent",
            "Nymph's Kissfish",
            "Celestial Angler",
            "Mystic Moonfish",
            "Ethereal Eel",
            "Abyssal Enigma",
            "Frostfire Serpent"
        };
        /* Mythical Fish Array */



        /* Void Fish Array */
        public static string[] void_fish = new string[]
        {
            "Grim Reaper",
            "Toxic Spiker",
            "Plague Angel",
            "Reality Folder",
            "Mind Trapper"
        };
        /* Void Fish Array */
    }
}
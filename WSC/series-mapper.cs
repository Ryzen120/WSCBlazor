using System.Collections.Generic;

namespace WSC.Services
{
    /// <summary>
    /// Maps card series codes to full series names
    /// </summary>
    public class SeriesMapper
    {
        private readonly Dictionary<string, string> _seriesMap;

        public SeriesMapper()
        {
            _seriesMap = new Dictionary<string, string>
            {
                // Filled with the series mapping from the original WinForms app
                { "AW", "Accel World" },
                { "AB", "Angel Beats!" },
                { "KW", "Angel Beats! / Kud" },
                { "Kab", "Angel Beats! / Kud" },
                { "ARI", "Arifureta: From Commonplace to World's Strongest" },
                { "ALL", "Assault Lily" },
                { "AOT", "Attack on Titan" },
                { "AZL", "Azur Lane" },
                { "BD", "Bang Dream" },
                { "BR", "Black Rock Shooter" },
                { "BFR", "Bofuri" },
                { "CN", "CANAAN" },
                { "CCS", "Cardcaptor Sakura" },
                { "CC", "Chain Chronicle" },
                { "CHA", "Charlotte" },
                { "Kch", "Charlotte" },
                { "CL", "CLANNAD" },
                { "Kcl", "CLANNAD" },
                { "CS", "Crayon Shin-Chan" },
                { "DC", "Da Capo/Dal Segno" },
                { "DC2", "Da Capo/Dal Segno" },
                { "DC3", "Da Capo/Dal Segno" },
                { "DS", "Da Capo/Dal Segno" },
                { "DC4", "Da Capo/Dal Segno" },
                { "FXX", "Darling in the Franxx" },
                { "DAL", "Date A Live" },
                { "GT", "Day Break Illusion" },
                { "DS2", "Devil Survivor 2" },
                { "DG", "Disgaea" },
                { "DD", "Dog Days" },
                { "EV", "Evangelion" },
                { "FT", "FAIRY TAIL" },
                { "ZM", "Familiar of Zero" },
                { "FS", "Fate" },
                { "FU", "Fate" },
                { "FH", "Fate" },
                { "APO", "Fate/Apocrypha" },
                { "FGO", "Fate/Grand Order" },
                { "FZ", "Fate/Zero" },
                { "Fab", "Fujimi Fantasia Bunko" },
                { "Foy", "Fujimi Fantasia Bunko" },
                { "Fii", "Fujimi Fantasia Bunko" },
                { "Fks", "Fujimi Fantasia Bunko" },
                { "Fkm", "Fujimi Fantasia Bunko" },
                { "Fkz", "Fujimi Fantasia Bunko" },
                { "Fsl", "Fujimi Fantasia Bunko" },
                { "Fsi", "Fujimi Fantasia Bunko" },
                { "F35", "Fujimi Fantasia Bunko" },
                { "Fos", "Fujimi Fantasia Bunko" },
                { "Fdl", "Fujimi Fantasia Bunko" },
                { "Fdy", "Fujimi Fantasia Bunko" },
                { "Ftr", "Fujimi Fantasia Bunko" },
                { "Fdd", "Fujimi Fantasia Bunko" },
                { "Fhc", "Fujimi Fantasia Bunko" },
                { "Ffp", "Fujimi Fantasia Bunko" },
                { "Fmr", "Fujimi Fantasia Bunko" },
                { "Fra", "Fujimi Fantasia Bunko" },
                { "GG", "Gargantia" },
                { "GST", "Gigant Shooter Tsukasa" },
                { "GF", "Girl Friend Beta" },
                { "GBS", "Goblin Slayer" },
                { "GZL", "Godzilla" },
                { "GC", "Guilty Crown" },
                { "GGST", "Guilty Gear" },
                { "GL", "Gurren Lagann" },
                { "SY", "Haruhi Suzumiya" },
                { "Ssy", "Haruhi Suzumiya" },
                { "HLL", "Hina Logic" },
                { "IM", "Idolmaster" },
                { "IAS", "Idolmaster" },
                { "IMC", "Imas Cinderella Girls" },
                { "IMS", "Imas Million Live" },
                { "ID", "Index / Railgun" },
                { "RG", "Index / Railgun" },
                { "GU", "Is the Order a Rabbit??" },
                { "JJ", "Jojo's Bizarre Adventure" },
                { "Snk", "Kadokawa Sneaker Bunko" },
                { "Sks", "Kadokawa Sneaker Bunko" },
                { "Sst", "Kadokawa Sneaker Bunko" },
                { "Snw", "Kadokawa Sneaker Bunko" },
                { "Ssw", "Kadokawa Sneaker Bunko" },
                { "Shh", "Kadokawa Sneaker Bunko" },
                { "Smi", "Kadokawa Sneaker Bunko" },
                { "Sls", "Kadokawa Sneaker Bunko" },
                { "Seo", "Kadokawa Sneaker Bunko" },
                { "Sky", "Kadokawa Sneaker Bunko" },
                { "Ssk", "Kadokawa Sneaker Bunko" },
                { "Ssh", "Kadokawa Sneaker Bunko" },
                { "Shg", "Kadokawa Sneaker Bunko" },
                { "Sfl", "Kadokawa Sneaker Bunko" },
                { "Smc", "Kadokawa Sneaker Bunko" },
                { "Smu", "Kadokawa Sneaker Bunko" },
                { "Sle", "Kadokawa Sneaker Bunko" },
                { "Srm", "Kadokawa Sneaker Bunko" },
                { "KC", "KanColle" },
                { "KG", "Katanagatari" },
                { "KMN", "Kemono Friends" },
                { "Kab", "Key 20th Anniversary" },
                { "Kch", "Key 20th Anniversary" },
                { "Kcl", "Key 20th Anniversary" },
                { "Klb", "Key 20th Anniversary" },
                { "Krw", "Key 20th Anniversary" },
                { "KLK", "Kill La Kill" },
                { "KI", "Kiznaiver" },
                { "KS", "Konosuba" },
                { "Sks", "Konosuba" },
                { "SK", "Leapt thr. Space/My-Hime/Otome" },
                { "MH", "Leapt thr. Space/My-Hime/Otome" },
                { "LB", "Little Busters!" },
                { "Klb", "Little Busters!" },
                { "LH", "Log Horizon" },
                { "LOD", "Lost Decade" },
                { "LL", "Love Live!" },
                { "LNJ", "Love Live Nijigasaki" },
                { "LSS", "Love Live! Sunshine!!" },
                { "LS", "Lucky Star" },
                { "MF", "Macross Frontier" },
                { "MM", "Madoka Magica" },
                { "MR", "Madoka Magica" },
                { "MB", "MELTY BLOOD / Kyoukai" },
                { "KK", "MELTY BLOOD / Kyoukai" },
                { "PD", "Miku" },
                { "MK", "Milky Holmes" },
                { "MK2", "Milky Holmes" },
                { "BM", "Monogatari Series" },
                { "NM", "Monogatari Series" },
                { "MG", "Monogatari Series" },
                { "MTI", "Mushoku Tensei: Jobless Reincarnation" },
                { "NS", "Nanoha" },
                { "N1", "Nanoha" },
                { "NV", "Nanoha" },
                { "NA", "Nanoha" },
                { "N2", "Nanoha" },
                { "NR", "Nanoha" },
                { "ND", "Nanoha" },
                { "NJ", "Nichijou" },
                { "NK", "Nisekoi" },
                { "NGL", "No Game No Life" },
                { "OMS", "Osomatsu-san" },
                { "OVL", "Overlord" },
                { "P3", "Persona" },
                { "P4", "Persona" },
                { "PQ", "Persona" },
                { "P5", "Persona" },
                { "PT", "Phantom" },
                { "PRD", "Princess Connect" },
                { "PI", "Prisma Illya" },
                { "PP", "Psycho-Pass" },
                { "PY", "Puyo Puyo" },
                { "5HY", "Quintessential Quintuples" },
                { "SBY", "Rascal Does Not Dream of Bunny Girl Senpai" },
                { "KNK", "Rent-A-Girlfriend" },
                { "RSL", "Revue Starlight" },
                { "RW", "Rewrite" },
                { "Krw", "Rewrite" },
                { "RZ", "Re:Zero" },
                { "RN", "Robotics;Notes" },
                { "SHS", "Saekano" },
                { "SKR", "Sakura Wars" },
                { "SGS", "Schoolgirl Strikers" },
                { "SB", "Sengoku BASARA" },
                { "SS", "Shana" },
                { "SE", "Shining Force" },
                { "SF", "Shining Force" },
                { "SPY", "Spy Family" },
                { "SW", "Star Wars" },
                { "STG", "Steins Gate" },
                { "SMP", "Summer Pockets" },
                { "GGO", "Sword Art Gun Gale" },
                { "SAO", "Sword Art Online" },
                { "SG", "Symphogear" },
                { "TF", "Terra Formars" },
                { "TSK", "That Time I Got Reincarnated as a Slime" },
                { "GRI", "The Fruit of Grisaia" },
                { "KF", "THE KING OF FIGHTERS" },
                { "TL", "To Love-Ru" },
                { "VA", "Visual Arts" },
                { "VR", "Vividred Operation" },
                { "VS", "ViVid Strike!" },
                { "Woo", "Wooser" },
                { "YYS", "Yuuna" }
            };
        }

        /// <summary>
        /// Gets the full series name from a series code
        /// </summary>
        public string GetSeriesName(string seriesCode)
        {
            if (string.IsNullOrEmpty(seriesCode))
                return "Unknown";

            if (_seriesMap.TryGetValue(seriesCode, out string seriesName))
                return seriesName;

            return seriesCode; // Return the code if no mapping exists
        }

        /// <summary>
        /// Gets all available series as a dictionary of code to name
        /// </summary>
        public Dictionary<string, string> GetAllSeries()
        {
            return new Dictionary<string, string>(_seriesMap);
        }
        
        /// <summary>
        /// Gets all unique series names
        /// </summary>
        public IEnumerable<string> GetAllSeriesNames()
        {
            return new HashSet<string>(_seriesMap.Values);
        }
    }
}

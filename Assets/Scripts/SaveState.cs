using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Provides functionality to load and save the game's save state, containing information about the best times levels have been completed in.
/// </summary>
[XmlRoot]
public class SaveState {

    /// <summary>
    /// Contains data on the best time a level has been completed at, and for convenience, the amount of stars the player received.
    /// </summary>
    public class LevelRecord {

        [XmlAttribute]
        public int id;

        [XmlAttribute]
        public float bestTime;

        [XmlAttribute]
        public int stars;

        public LevelRecord() {
        }

        public LevelRecord(int id) {
            this.id = id;
        }

    }

    [XmlArray("LevelRecords")]
    [XmlArrayItem("LevelRecord")]
    public List<LevelRecord> levelRecords = new List<LevelRecord>();

    /// <summary>
    /// Returns a level record stored for a given level id.
    /// </summary>
    /// <param name="level">the id for the level to lookup</param>
    /// <returns>a level record containing best time data or null if none has been saved yet</returns>
    public LevelRecord GetLevelRecord(int level) {
        return levelRecords.FirstOrDefault(t => t.id == level);
    }

    /// <summary>
    /// Updates a record stored for a level or creates it if it doesn't exist yet.
    /// This will only affect the save state if a previous best time has been beaten.
    /// The save state will automatically be saved to file when this is called.
    /// </summary>
    /// <param name="level"></param>
    /// <param name="bestTime"></param>
    /// <param name="stars"></param>
    public void UpdateLevelRecord(int level, float bestTime, int stars) {
        var record = GetLevelRecord(level);
        if (record == null) {
            record = new LevelRecord(level);
            levelRecords.Add(record);
        }

        if (record.bestTime <= 0f || bestTime < record.bestTime) {
            record.bestTime = bestTime;
            record.stars = stars;

            SaveToFile();
        }
    }

    /// <summary>
    /// Saves the game's save state to the savegame.xml file.
    /// </summary>
    public void SaveToFile() {
        using (var stream = new FileStream("savegame.xml", FileMode.Create)) {
            var serializer = new XmlSerializer(typeof(SaveState));
            serializer.Serialize(stream, this);
        }
    }

    /// <summary>
    /// Loads the game's save state from the savegame.xml file, or creates it if it doesn't exit.
    /// </summary>
    /// <returns>the game's save state data</returns>
    public static SaveState LoadFromFile() {
        try {
            using (var stream = new FileStream("savegame.xml", FileMode.OpenOrCreate)) {
                var serializer = new XmlSerializer(typeof(SaveState));
                return (SaveState) serializer.Deserialize(stream);
            }
        } catch (System.Exception) {
            // We don't care, it's probably a new empty file, or it's corrupt
            return new SaveState();
        }
    }

}

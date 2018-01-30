using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

[XmlRoot]
public class SaveState {

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

    public bool HasUnlockedLevel(int level) {
        // Unlock all levels by default. It's better that way for the demo and locked levels aren't really necessary for only three levels.
        return true;

        /*if (level == 1) {
            return true;
        }

        return GetLevelRecord(level) != null;*/
    }

    public void UnlockLevel(int level) {
        if (!HasUnlockedLevel(level)) {
            levelRecords.Add(new LevelRecord(level));

            SaveToFile();
        }
    }

    public LevelRecord GetLevelRecord(int level) {
        return levelRecords.FirstOrDefault(t => t.id == level);
    }

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

    public void SaveToFile() {
        using (var stream = new FileStream("savegame.xml", FileMode.Create)) {
            var serializer = new XmlSerializer(typeof(SaveState));
            serializer.Serialize(stream, this);
        }
    }

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

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
        return levelRecords.Any(t => t.id == level);
    }

    public void UnlockLevel(int level) {
        if (!HasUnlockedLevel(level)) {
            levelRecords.Add(new LevelRecord(level));

            SaveToFile();
        }
    }

    public void UpdateLevelRecord(int level, float bestTime, int stars) {
        var record = levelRecords.FirstOrDefault(t => t.id == level);
        if (record == null) {
            record = new LevelRecord(level);
            levelRecords.Add(record);
        }

        if (bestTime < record.bestTime) {
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
        } catch (XmlException) {
            // We don't care, it's probably a new empty file, or it's corrupt
            return new SaveState();
        }
    }

}

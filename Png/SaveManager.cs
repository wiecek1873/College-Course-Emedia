using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EmediaWPF
{
    public class SaveManager : Singleton<SaveManager>
    {
        private const string FILE_NAME = "save.txt";

        public EncryptionSave LoadKeys()
        {
            var file = new StreamReader(FILE_NAME);
            var json = file.ReadToEnd();
            return JsonSerializer.Deserialize<EncryptionSave>(json);
        }

        public void SaveKeys(EncryptionSave keys)
        {
            var json = JsonSerializer.Serialize<EncryptionSave>(keys);
            var file = new StreamWriter(FILE_NAME);
            file.Write(json);
        }
    }
}
